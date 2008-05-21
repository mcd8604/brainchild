#define TIMED

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Collections;
using Engine;
using Physics2;
using Audio;

namespace Project_blob.GameState {
	public class GameplayScreen : GameScreen {
		SpriteBatch spriteBatch;

		public bool WinFlag = false;

		float lastClimbCollision = 0;

		Model blobModel;
		//Model skyBox;

		//Texture2D skyTexture;
		//TextureInfo ti;
		//StaticModel sky;
		private Blob theBlob;
		internal Blob Player { get { return theBlob; } }

		Texture2D firm, soft, slick, sticky;

		bool default_sticky = false;
		bool default_firm = false;

		private const float TEXT_EVENT_TIME = 5.0f;

		public static bool TextEventHit = false;

		Texture2D blobTexture;
		Texture2D distortMapText;

		Texture2D gaugeLine;
		Texture2D gaugeMark;

		Texture2D backdrop;

		//Effect effect;
		//Effect celEffect;
		//Effect blobEffect;
		Effect cartoonEffect;
		Effect postprocessEffect;
		Effect distortEffect;
		Effect distorterEffect;

		public bool blob_Climbing = false;
		//bool startCameraFollow = false;

		RenderTarget2D sceneRenderTarget;
		RenderTarget2D normalDepthRenderTarget;
		RenderTarget2D distortionMap;
		ResolveTexture2D tempRenderTarget;

		Matrix worldMatrix;
		//Matrix viewMatrix;
		//Matrix projectionMatrix;

		VertexDeclaration VertexDeclarationColor;
		VertexDeclaration VertexDeclarationTexture;

		private float deadTimer = 0f;
		public static bool deadSet = false;

		List<Drawable> drawables = new List<Drawable>();

		Vector4 lightPosition;

		public static bool ChangeBlobColor = false;
		private static Color m_BlobColor = new Color(new Vector3(0, 0.5f, 0));
        public static Color BlobColor
        {
            get { return m_BlobColor; }
            set { m_BlobColor = value; }
        }

		public static PhysicsManager physics;

		internal static HighScores HighScoreManager = new HighScores();

		//Vector2 cameraOffset = new Vector2();

#if DEBUG
		private static bool DEBUG_GodMode = false;
#endif
#if DEBUG
		internal static bool FPS = true;
#else
		internal static bool FPS = false;
#endif

		public enum CameraType {
			Follow,
			Cinema,
			Chase
		}

		private static string m_TextEventstring = string.Empty;
		public static string TextEvent {
			get { return m_TextEventstring; }
			set { m_TextEventstring = value; }
		}

		public static float m_lastTextEvent = 0;

		public static CameraType CurCamera = CameraType.Follow;
		//bool cinema = false;
#if DEBUG
		bool paused = false;
        public bool Paused
        {
            get
            {
                return paused;
            }
            set
            {
                paused = value;
            }
        }
		bool step = false;
#endif
		//bool follow = true;
		//bool chase = false;

		//bool points = false;

		public SpriteFont font;
		//fps
		private float time = 0f;
		private const float update = 1f;
		private int frames = 0;
		private string fps = string.Empty;

		public static GameplayScreen game;

		static Vector3 defaultCameraPosition = new Vector3(0, 15, 10);
		//Vector3 cameraPosition = defaultCameraPosition;
		Vector2 cameraAngle = new Vector2(1f, 0.4f);
		float cameraLength = 20f;
		float playerCamMulti = 0.05f;

		public static bool cameraInvert = false;
		int invertVert = 1;
		bool OrientCamera = false;

		public static Area currentArea;

#if TIMED
		System.Diagnostics.Stopwatch physicsTime = new System.Diagnostics.Stopwatch();
		System.Diagnostics.Stopwatch drawTime = new System.Diagnostics.Stopwatch();
#endif

		CameraBody CameraBody;

		private Vector3 blobStartPosition = new Vector3(0, 10, 0);

#if DEBUG && TIMED
		float DEBUG_MaxPhys = -1;
		float DEBUG_MinPhys = -1;
		float DEBUG_MaxDraw = -1;
		float DEBUG_MinDraw = -1;
#endif

		public GameplayScreen() {
			game = this;
			TransitionOnTime = TimeSpan.FromSeconds(1.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);

			lightPosition = new Vector4(5, 5, 5, 0);
		}

		private void reset() {
			if (physics != null) {
				physics.stop();
			}
			physics = PhysicsManager.getInstance();

			// Configurable physics parameters:
			// All of these are more or less arbitrary, and can be tweaked within reason for different effects.

			if (currentArea != null)
			{
				PhysicsManager.AirFriction = currentArea.AirFriction ?? PhysicsManager.AirFriction;
			}

			physics.Player.Traction.Minimum = 0.1f;
			physics.Player.Traction.Origin = 1f;
			physics.Player.Traction.Maximum = 5f;

			physics.Player.Cling.Minimum = 0f;
			physics.Player.Cling.Origin = 50f;
			physics.Player.Cling.Maximum = 100f;

			physics.Player.Resilience.Minimum = 10f;
			physics.Player.Resilience.Origin = 20f;
			physics.Player.Resilience.Maximum = 40f;

			physics.Player.Volume.Minimum = 40f;
			physics.Player.Volume.Origin = 80f;
			physics.Player.Volume.Maximum = 120f;

			physics.Player.Twist = 0.2f;
			physics.Player.AirTwist = 0.05f;
			physics.Player.Drift = 0.01f;
			physics.Player.AirDrift = 0.1f;

			physics.Player.MaxJumpWork = 20;
			physics.Player.AirJumpWork = 0;

#if DEBUG
			physics.Player.AirJumpWork = 5;
#endif

			// --------------------

			theBlob = new Blob(blobModel, blobStartPosition);
			theBlob.text = blobTexture;
			theBlob.DisplacementText = distortMapText;
			theBlob.setGraphicsDevice(ScreenManager.GraphicsDevice);

			physics.Player.PlayerBody = theBlob;
			physics.AddBody(theBlob);

			physics.Player.PlayerBody.addTask(new GravityVector(10f, new Vector3(0f, -1.0f, 0f)));

			CameraBody = new CameraBody(theBlob);
			physics.AddBody(CameraBody);

			if (currentArea != null) {
				physics.AddBodys(currentArea.getBodies());
			}

			ChaseCamera cam = CameraManager.getSingleton.ActiveCamera as ChaseCamera;
			if (cam != null) {
				cam.Climbing = false;
				cam.DesiredPosition = theBlob.getCenter();
				cam.Reset();
			}
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		public override void LoadContent() {
			backdrop = ScreenManager.Content.Load<Texture2D>(@"Textures\\backdrop");

			gaugeLine = ScreenManager.Content.Load<Texture2D>(@"UI Sprites\\GaugeLine");
			gaugeMark = ScreenManager.Content.Load<Texture2D>(@"UI Sprites\\GaugeMark");

			firm = ScreenManager.Content.Load<Texture2D>(@"UI Sprites\\Firm");
			soft = ScreenManager.Content.Load<Texture2D>(@"UI Sprites\\Soft");

			slick = ScreenManager.Content.Load<Texture2D>(@"UI Sprites\\SlickWord");
			sticky = ScreenManager.Content.Load<Texture2D>(@"UI Sprites\\Sticky");

			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

			//load fonts
			font = ScreenManager.Content.Load<SpriteFont>(@"Fonts\\Courier New");

			//load shaders
			//celEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\Cel");

			blobModel = ScreenManager.Content.Load<Model>(@"Models\\blob");

			blobTexture = ScreenManager.Content.Load<Texture2D>(@"Textures\\transparancy_png");
			distortMapText = ScreenManager.Content.Load<Texture2D>(@"Textures\\PrivacyGlass");

			//blobEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\Blobs");

			cartoonEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\CartoonEffect");
			EffectManager.getSingleton.AddEffect("cartoonEffect", cartoonEffect);

			postprocessEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\PostprocessEffect");
			EffectManager.getSingleton.AddEffect("postprocessEffect", postprocessEffect);

			distortEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\Distort");
            distortEffect.Parameters["blobColor"].SetValue(m_BlobColor.ToVector4());
			EffectManager.getSingleton.AddEffect("distort", distortEffect);

			distorterEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\Distorters");
			EffectManager.getSingleton.AddEffect("distorter", distorterEffect);

			//cartoonEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\DepthBuffer");

			//load skybox
			//skyBox = ScreenManager.Content.Load<Model>(@"Models\\skyBox");
			//skyTexture = ScreenManager.Content.Load<Texture2D>(@"Textures\\point_text");

			reset();

			//load default level
			Level.LoadLevel("FinalLevel", "effects");

			//List of Static Drawables to add to Scene
			//List<Drawable> staticDrawables = new List<Drawable>();
			//List<Portal> portals = new List<Portal>();

			//load first area
			//if (Level.Areas.Count > 0)
			//{
			//    IEnumerator e = Level.Areas.Values.GetEnumerator();
			//    e.MoveNext();
			//    //e.MoveNext();
			//    //e.MoveNext();

			//    currentArea = (Area)e.Current;
			//    currentArea.LoadAreaGameplay(ScreenManager);
			//    currentArea.Display.EffectName = "cartoonEffect";
			//    currentArea.Display.WorldParameterName = "World";
			//    currentArea.Display.TextureParameterName = "Texture";
			//    currentArea.Display.TechniqueName = "Lambert";
			//    staticDrawables = currentArea.getDrawableList();
			//    portals = currentArea.Portals;
			//    physics.AddBodys(currentArea.getBodies());
			//}
			//else
			//{
			//    //empty level
			//}

			//Initialize the camera
			BasicCamera camera = new BasicCamera();
			camera.FieldOfView = MathHelper.ToRadians(45.0f);
			camera.AspectRatio = (float)ScreenManager.GraphicsDevice.Viewport.Width / (float)ScreenManager.GraphicsDevice.Viewport.Height;
			camera.NearPlane = 1.0f;
			camera.FarPlane = 1000.0f;

			camera.Position = new Vector3(0, 0, -10);
			camera.Target = Vector3.Zero;
			camera.Up = Vector3.Up;

			CameraManager.getSingleton.AddCamera("default", camera);
			CameraManager.getSingleton.SetActiveCamera("default");

			CinematicCamera cinematicCamera = new CinematicCamera();

			cinematicCamera.FieldOfView = MathHelper.ToRadians(45.0f);
			cinematicCamera.AspectRatio = (float)ScreenManager.GraphicsDevice.Viewport.Width / (float)ScreenManager.GraphicsDevice.Viewport.Height;
			cinematicCamera.NearPlane = 1.0f;
			cinematicCamera.FarPlane = 1000.0f;

			cinematicCamera.Position = new Vector3(0, 0, -10);
			cinematicCamera.Target = Vector3.Zero;
			cinematicCamera.Up = Vector3.Up;

			CameraManager.getSingleton.AddCamera("cinematic", cinematicCamera);

			ChaseCamera chaseCamera = new ChaseCamera();
			chaseCamera.FieldOfView = MathHelper.ToRadians(45.0f);
			chaseCamera.AspectRatio = (float)ScreenManager.GraphicsDevice.Viewport.Width / (float)ScreenManager.GraphicsDevice.Viewport.Height;
			chaseCamera.NearPlane = 1.0f;
			chaseCamera.FarPlane = 1000.0f;

			chaseCamera.Up = Vector3.Up;
			chaseCamera.ChasePosition = theBlob.getCenter();

			chaseCamera.ChaseDirection = Vector3.Forward;

			chaseCamera.Reset();

			CameraManager.getSingleton.AddCamera("chase", chaseCamera);
			//CameraManager.getSingleton.SetActiveCamera("chase");


#if DEBUG
			string[] tempArray = new string[Level.Areas.Keys.Count];
			Level.Areas.Keys.CopyTo(tempArray, 0);
			using (SelectForm f = new SelectForm(tempArray)) {
				if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					nextAreaName = f.getSelected();

					ChangeArea();
				} else {
#endif
					nextAreaName = "HubWorld";
					ChangeArea();
#if DEBUG
				}
			}
#endif




			//theDisplay = new Display(worldMatrix, viewMatrix, projectionMatrix);
			//theDisplay.DrawnList.Add(

			//camera.Postiion = defaultCameraPosition;
		}

		/*private void addToPhysicsAndDraw(T t)
		{
			physics.AddCollidable(t);
			drawables.Add(t);
		}*/

		/// <summary>
		/// Initializes the basic effect (parameter setting and technique selection)
		/// used for the 3D model.
		/// </summary>
		private void InitializeEffect() {
			worldMatrix = Matrix.Identity;

			VertexDeclarationTexture = new VertexDeclaration(ScreenManager.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

			VertexDeclarationColor = new VertexDeclaration(ScreenManager.GraphicsDevice, VertexPositionColor.VertexElements);

			Camera cam = CameraManager.getSingleton.ActiveCamera;

			cartoonEffect.Parameters["World"].SetValue(worldMatrix);
			cartoonEffect.Parameters["Projection"].SetValue(cam.Projection);
			cartoonEffect.Parameters["View"].SetValue(cam.View);
			cartoonEffect.Parameters["TextureEnabled"].SetValue(true);
			cartoonEffect.Parameters["MaxDepth"].SetValue(60);

			currentArea.Display.CartoonEffect = cartoonEffect;

			distorterEffect.Parameters["WorldViewProjection"].SetValue(worldMatrix * cam.View * cam.Projection);
			distorterEffect.Parameters["WorldView"].SetValue(worldMatrix * cam.View);

			CreateRenderTargets();

			currentArea.Display.PostProcessEffect = postprocessEffect;

			currentArea.Display.Distort = distortEffect;
			currentArea.Display.Distorter = distorterEffect;

		}

		public void CreateRenderTargets() {
			PresentationParameters pp = ScreenManager.GraphicsDevice.PresentationParameters;

            if(sceneRenderTarget == null)
			    sceneRenderTarget = new RenderTarget2D(ScreenManager.GraphicsDevice,
				    pp.BackBufferWidth, pp.BackBufferHeight, 1,
				    pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

            if(normalDepthRenderTarget == null)
			    normalDepthRenderTarget = new RenderTarget2D(ScreenManager.GraphicsDevice,
				    pp.BackBufferWidth, pp.BackBufferHeight, 1,
				    pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

            if(distortionMap == null)
			    distortionMap = new RenderTarget2D(ScreenManager.GraphicsDevice,
				    pp.BackBufferWidth, pp.BackBufferHeight, 1,
				    pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

            if(tempRenderTarget == null)
			    tempRenderTarget = new ResolveTexture2D(ScreenManager.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1,
				    pp.BackBufferFormat);

            //depthBufferRenderTarget = new RenderTarget2D(ScreenManager.GraphicsDevice,
            //    pp.BackBufferWidth, pp.BackBufferHeight, 1,
            //    pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);


			currentArea.Display.SceneRanderTarget = sceneRenderTarget;
			currentArea.Display.NormalDepthRenderTarget = normalDepthRenderTarget;
			currentArea.Display.DistortionMap = distortionMap;
			currentArea.Display.TempRenderTarget = tempRenderTarget;
			//currentArea.Display.DepthMapRenderTarget = depthBufferRenderTarget;

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		public override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		public static bool CauseDeath(Body body) {
			if (body != null && body.Equals(game.theBlob)) {
#if DEBUG
				if (!DEBUG_GodMode) {
#endif
					physics.Player.Dead = true;
#if DEBUG
				}
#endif
				return true;
			}
			return false;
		}

		public static void SetCheckPoint(Vector3 position) {
			game.blobStartPosition = position;
		}

		public static Vector3 GetCheckpoint() {
			return game.blobStartPosition;
		}

		public static void SetChangeArea(string area) {
			game.nextAreaName = area;
			game.UseDefaultAreaPos = true;
			game.ChangeAreaFlag = true;
		}

		public static void SetChangeArea(string area, Vector3 position) {
			game.nextAreaName = area;
			game.nextAreaPosition = position;
			game.ChangeAreaFlag = true;
			game.UseDefaultAreaPos = false;
		}

		public static void SetResetArea() {
			game.nextAreaName = Level.GetAreaName(currentArea);
			game.ChangeAreaFlag = true;
			game.UseDefaultAreaPos = true;
		}

		public static void SetLoadCheckpoint() {
			game.LoadCheckpoint = true;
		}

		private bool LoadCheckpoint = false;
		private bool ChangeAreaFlag = false;
		private string nextAreaName;
		private Vector3 nextAreaPosition = Vector3.Zero;
		private bool UseDefaultAreaPos = true;

		private void ChangeArea() {
			currentArea = Level.Areas[nextAreaName];

			if (UseDefaultAreaPos) {
				blobStartPosition = currentArea.StartPosition;
			} else {
				blobStartPosition = nextAreaPosition;
				nextAreaPosition = Vector3.Zero;
			}
			CameraManager.getSingleton.ActiveCamera.Position = currentArea.CameraSpawnPosition;
			CameraManager.getSingleton.ActiveCamera.Target = blobStartPosition;
            CameraManager.getSingleton.ActiveCamera.Target = blobStartPosition;

			TextureManager.ClearTextures();

			currentArea.LoadAreaGameplay(ScreenManager);

			//Give the SceneManager a reference to the display
			SceneManager.getSingleton.Display = currentArea.Display;

			InitializeEffect();

			if (currentArea.CullingStructure == SceneGraphType.Portal) {
				//Add the Static Drawables to the Octree
				List<Drawable> temp = new List<Drawable>(currentArea.getDrawableList());
				List<Portal> portals = currentArea.Portals;
				SceneManager.getSingleton.GraphType = SceneGraphType.Portal;
				//SceneManager.getSingleton.BuildOctree(ref temp);
				//SceneManager.getSingleton.BuildPortalScene(temp);
				foreach (Portal p in portals) {
					p.CreateBoundingBox();
				}
				SceneManager.getSingleton.BuildPortalScene(temp, portals);
				SceneManager.getSingleton.PortalScene.CurrSector = 1;
			} else {
				List<Drawable> temp = new List<Drawable>(currentArea.getDrawableList());
				SceneManager.getSingleton.GraphType = SceneGraphType.Octree;
				SceneManager.getSingleton.BuildOctree(ref temp);
			}

			currentArea.Display.SetBlurEffectParameters(1f / (float)ScreenManager.GraphicsDevice.Viewport.Width, 1f / (float)ScreenManager.GraphicsDevice.Viewport.Height);

			reset();

			AudioManager.LoadAmbientSounds(currentArea.AmbientSounds);
		}

		public void SetUpCinematicCamera(List<CameraFrame> cameraFrames) {
			CinematicCamera cinematicCamera = (CinematicCamera)CameraManager.getSingleton.GetCamera("cinematic");
			cinematicCamera.Frames = cameraFrames;
			cinematicCamera.Running = true;
			CameraManager.getSingleton.SetActiveCamera("cinematic");
			//cinema = true;
			CurCamera = CameraType.Cinema;
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime, bool otherScreenHasFocus,
													   bool coveredByOtherScreen)
		{
														   
			if (ChangeBlobColor)
			{
				currentArea.Display.Distort.Parameters["blobColor"].SetValue(m_BlobColor.ToVector4());
				ChangeBlobColor = false;
			}

			if (LoadCheckpoint) {
				LoadCheckpoint = false;
				reset();
			}

			if (ChangeAreaFlag) {
				ChangeAreaFlag = false;
				ChangeArea();
			}

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
			if (IsActive) {
#if TIMED
				physicsTime.Reset();
				physicsTime.Start();
#endif
#if DEBUG
				if (!paused) {
#endif
					physics.update((float)gameTime.ElapsedGameTime.TotalSeconds);

					if (currentArea.TimeLimit > 0 && currentArea.TimeLimit < physics.Time) {
						physics.Player.Dead = true;
					}

					if (physics.Player.Dead) {
						if (deadSet) {
							if (deadTimer + 1.5f <= (float)gameTime.TotalGameTime.TotalSeconds) {
								ScreenManager.AddScreen(new DeathScreen());
								deadSet = false;
							}
						} else {
							deadTimer = (float)gameTime.TotalGameTime.TotalSeconds;
							deadSet = true;
						}

					}

					cartoonEffect.Parameters["blobCenter"].SetValue(theBlob.getCenter());
					distortEffect.Parameters["blobCenter"].SetValue(new Vector2(theBlob.getCenter().X, theBlob.getCenter().Y));

					if (TextEventHit) {
						TextEventHit = false;
						m_lastTextEvent = gameTime.TotalGameTime.Seconds;
					}

					if (gameTime.TotalGameTime.Seconds - m_lastTextEvent > TEXT_EVENT_TIME) {
						m_TextEventstring = string.Empty;
					}

					//Vector4 tempPos = new Vector4(theBlob.getCenter(), 0);
					//tempPos.Y += 10;
					//cartoonEffect.Parameters["LightPos"].SetValue(tempPos);
					//Matrix lightViewProjectionMatrix = Matrix.CreateLookAt(new Vector3(tempPos.X, tempPos.Y, tempPos.Z), theBlob.getCenter(), new Vector3(0,0,1)) *
					//    Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, (float)ScreenManager.CurrentResolution.Width / (float)ScreenManager.CurrentResolution.Height, CameraManager.getSingleton.ActiveCamera.NearPlane, CameraManager.getSingleton.ActiveCamera.FarPlane);
					//cartoonEffect.Parameters["LightWorldViewProjection"].SetValue(worldMatrix * lightViewProjectionMatrix);

#if DEBUG
				}
#endif
#if TIMED
				physicsTime.Stop();
#endif
#if DEBUG
				if (step) {
					paused = true;
					step = false;
				}
#endif

				//Update Camera
				//camera.Update(gameTime);
				CameraManager.getSingleton.Update(gameTime);

				Camera cam = CameraManager.getSingleton.ActiveCamera;

				if (!(cam is CinematicCamera)) {

					if (InputHandler.IsActionPressed(Actions.Reset)) {
						SetResetArea();
#if DEBUG && TIMED
						DEBUG_MaxPhys = -1;
						DEBUG_MinPhys = -1;
						DEBUG_MaxDraw = -1;
						DEBUG_MinDraw = -1;
#endif
					}


					if (InputHandler.IsActionPressed(Actions.ToggleElasticity)) {
						default_firm = !default_firm;
						if (physics.Player.Resilience.Target <= 0.5f) {
							physics.Player.Resilience.Target = physics.Player.Volume.Target = 1f;
						} else {
							physics.Player.Resilience.Target = physics.Player.Volume.Target = 0f;
						}
					}
					if (InputHandler.IsActionPressed(Actions.ToggleStickiness)) {
						default_sticky = !default_sticky;
						if (physics.Player.Cling.Target <= 0.5f) {
							physics.Player.Cling.Target = physics.Player.Traction.Target = 1f;
						} else {
							physics.Player.Cling.Target = physics.Player.Traction.Target = 0f;
						}
					}

					// Xbox Specific Controls:
					if (InputHandler.ControllerConnected()) {
						if (default_firm) {
							physics.Player.Resilience.Target = physics.Player.Volume.Target = InputHandler.RightTriggerValue;
						} else {
							physics.Player.Resilience.Target = physics.Player.Volume.Target = 1 - InputHandler.RightTriggerValue;
						}
						if (default_sticky) {
							physics.Player.Cling.Target = physics.Player.Traction.Target = InputHandler.LeftTriggerValue;
						} else {
							physics.Player.Cling.Target = physics.Player.Traction.Target = 1 - InputHandler.LeftTriggerValue;
						}
					}

					//InputHandler.SetVibration(MathHelper.Clamp(physics.ImpactThisFrame - 0.1f, 0f, 1f), 0f);
					// Quick Torque

					Vector2 move = InputHandler.GetAnalogAction(AnalogActions.Movement);
					if (move != Vector2.Zero) {
						physics.Player.move(move, cam.Position);
					}

#if !DEBUG
					if (InputHandler.IsActionPressed(Actions.Jump))
					{
						physics.Player.jump();
					}
#endif
				}

				if (InputHandler.IsActionPressed(Actions.Pause)) {
					PauseMenuScreen pauseMenu = new PauseMenuScreen();
					ScreenManager.AddScreen(pauseMenu);

					//PlayTime.Stop();
				}

				if (InputHandler.IsActionPressed(Actions.ChangeCamera)) {
					if (CurCamera == CameraType.Cinema) {
						((CinematicCamera)(cam)).FinishedCinematics = true;
					}
					if (CurCamera == CameraType.Chase) {
						CurCamera = CameraType.Follow;
						CameraManager.getSingleton.SetActiveCamera("default");
					} else if (CurCamera == CameraType.Follow) {
						CurCamera = CameraType.Chase;
						CameraManager.getSingleton.SetActiveCamera("chase");
						ChaseCamera chaseCam = cam as ChaseCamera;
						if (chaseCam != null) {
							chaseCam.DesiredPosition = theBlob.getCenter();
							chaseCam.ChasePosition = theBlob.getCenter();
							chaseCam.Reset();
						}
					}
				}

				cameraLength += (InputHandler.getMouseWheelDelta() * -0.01f);

				if (InputHandler.IsActionDown(Actions.ZoomOut)) {
					cameraLength += 1;
				} else if (InputHandler.IsActionDown(Actions.ZoomIn)) {
					cameraLength -= 1;
				}

				cameraLength = MathHelper.Clamp(cameraLength, 10, 40);

				if (CurCamera == CameraType.Cinema) {
					distorterEffect.Parameters["WorldViewProjection"].SetValue(worldMatrix * cam.View * cam.Projection);
					distorterEffect.Parameters["WorldView"].SetValue(worldMatrix * cam.View);
					cartoonEffect.Parameters["View"].SetValue(cam.View);

					if (cam is CinematicCamera && ((CinematicCamera)cam).FinishedCinematics) {
						((CinematicCamera)cam).currentIndex = 0;
						((CinematicCamera)cam).FinishedCinematics = false;
						CurCamera = CameraType.Follow;
						CameraManager.getSingleton.SetActiveCamera("default");
					}
				} else if (CurCamera == CameraType.Follow) {
					cameraAngle += InputHandler.GetAnalogAction(AnalogActions.Camera) * playerCamMulti;
					if (cameraAngle.X < -MathHelper.Pi) {
						cameraAngle.X += MathHelper.TwoPi;
					} else if (cameraAngle.X > MathHelper.Pi) {
						cameraAngle.X -= MathHelper.TwoPi;
					}

					cameraAngle = Vector2.Clamp(cameraAngle, new Vector2(-MathHelper.TwoPi, -MathHelper.PiOver2), new Vector2(MathHelper.TwoPi, MathHelper.PiOver2));

					// following camera
					if (cameraInvert) {
						invertVert = -1;
					} else {
						invertVert = 1;
					}

					Vector3 Offset = new Vector3((float)Math.Cos(cameraAngle.X) * cameraLength, (float)Math.Sin(cameraAngle.Y) * cameraLength * invertVert, (float)Math.Sin(cameraAngle.X) * cameraLength);

					CameraBody.setCameraOffset(Offset);
					cam.Position = CameraBody.getCameraPosition();

					cam.Target = theBlob.getCenter();
				} else if (cam is ChaseCamera) {

					ChaseCamera chaseCam = ((ChaseCamera)cam) as ChaseCamera;

					chaseCam.UserOffset = InputHandler.GetAnalogAction(AnalogActions.Camera);

					if (physics.Player.Touching) {
						Vector3 AvgNormal = Vector3.Zero;
						foreach (Collidable c in physics.Player.WasTouching) {
							if (c.getMaterial().Friction == MaterialFactory.CLING_STICKY) {
								lastClimbCollision = physics.Time;
								chaseCam.Climbing = true;
								AvgNormal += c.Normal;
								break;
							}
						}
						if (AvgNormal != Vector3.Zero) {
							chaseCam.ClimbNormal = Vector3.Normalize(AvgNormal);
						}
					}

					if (physics.Time - lastClimbCollision > 1f) {
						chaseCam.Climbing = false;
					}

					if (theBlob.getAverageVelocity() != Vector3.Zero)
						chaseCam.ChaseDirection = theBlob.getAverageVelocity();

					chaseCam.ChasePosition = theBlob.getCenter();
					chaseCam.DesiredPositionOffset = cameraLength * 0.5f;

				}

				if (OrientCamera && physics.Player.Touching) {
					cam.Up = physics.Player.Normal;
				}

				if (currentArea.Display.SkyBox != null) {
					currentArea.Display.SkyBox.Position = Matrix.CreateTranslation(cam.Position);
					currentArea.Display.SkyBox.updateVertexBuffer();
				}


				//fps
				if (FPS)
				{
					time += (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (time > update)
					{
						fps = Convert.ToInt32(frames / time).ToString();
						time = 0;
						frames = 0;
					}
				}

#if DEBUG

				if (InputHandler.IsKeyPressed(Keys.U)) {
					physics.Player.Dead = true;
				}

				if (InputHandler.IsKeyPressed(Keys.R)) {
					if (physics.Player.Resilience.Target <= 0.5f) {
						physics.Player.Resilience.Target = physics.Player.Volume.Target = 1f;
					} else {
						physics.Player.Resilience.Target = physics.Player.Volume.Target = 0f;
					}
				}
				if (InputHandler.IsKeyPressed(Keys.T)) {
					if (physics.Player.Cling.Target <= 0.5f) {
						physics.Player.Cling.Target = physics.Player.Traction.Target = 1f;
					} else {
						physics.Player.Cling.Target = physics.Player.Traction.Target = 0f;
					}
				}

				if (InputHandler.IsKeyPressed(Keys.H)) {
					SceneManager.getSingleton.Cull = !SceneManager.getSingleton.Cull;
                    Console.WriteLine(SceneManager.getSingleton.Cull);
				}

				if (InputHandler.IsKeyPressed(Keys.G)) {
					DEBUG_GodMode = !DEBUG_GodMode;
				}

				if (InputHandler.IsKeyPressed(Keys.M)) {
					physics.Player.DEBUG_MoveModeFlag = !physics.Player.DEBUG_MoveModeFlag;
				}

				if (InputHandler.IsKeyPressed(Keys.N)) {
					OrientCamera = !OrientCamera;
				}

				if (InputHandler.IsKeyPressed(Keys.L)) {
					step = true;
					paused = false;
				}

				if (InputHandler.IsKeyPressed(Keys.PageUp)) {
					physics.physicsMultiplier += 0.1f;
				} else if (InputHandler.IsKeyPressed(Keys.PageDown)) {
					physics.physicsMultiplier -= 0.1f;
				}

				if (InputHandler.IsKeyPressed(Keys.OemTilde)) {
					if (PhysicsManager.enableParallel != PhysicsManager.ParallelSetting.Never) {
						PhysicsManager.enableParallel = PhysicsManager.ParallelSetting.Never;
					} else {
						PhysicsManager.enableParallel = PhysicsManager.ParallelSetting.Always;
					}
					SetResetArea();
#if TIMED
					DEBUG_MaxPhys = -1;
					DEBUG_MinPhys = -1;
					DEBUG_MaxDraw = -1;
					DEBUG_MinDraw = -1;
#endif
				}

				if (InputHandler.IsKeyPressed(Keys.P)) {
					paused = !paused;
				}

				if (InputHandler.IsKeyPressed(Keys.S)) {
					physics.Player.Resilience.Target = 1f;
				}
				if (InputHandler.IsKeyPressed(Keys.D)) {
					physics.Player.Resilience.Target = 0.5f;
				}
				if (InputHandler.IsKeyPressed(Keys.W)) {
					physics.Player.Resilience.Target = 0f;
				}
				if (InputHandler.IsKeyPressed(Keys.Q)) {
					physics.Player.Traction.Target = 1f;
					physics.Player.Cling.Target = 1f;
				}
				if (InputHandler.IsKeyPressed(Keys.E)) {
					physics.Player.Traction.Target = 0f;
					physics.Player.Cling.Target = 0f;
				}
				if (InputHandler.IsKeyPressed(Keys.A)) {
					physics.Player.Traction.Target = 0.5f;
					physics.Player.Cling.Target = 0.5f;
				}
				if (InputHandler.IsKeyPressed(Keys.Z)) {
					currentArea.Display.DEBUG_WireframeMode = !currentArea.Display.DEBUG_WireframeMode;
				}

				if (InputHandler.IsKeyPressed(Keys.B)) {
#if TIMED
					DEBUG_MaxPhys = -1;
					DEBUG_MinPhys = -1;
					DEBUG_MaxDraw = -1;
					DEBUG_MinDraw = -1;
#endif
				}

				if (InputHandler.IsKeyPressed(Keys.OemPlus)) {
					currentArea.Display.saveOut = true;
				}

				if (InputHandler.IsKeyPressed(Keys.J) || InputHandler.IsButtonPressed(Buttons.A)) {
					physics.Player.jump();
				}

				if (InputHandler.IsKeyDown(Keys.Home)) {
					foreach (Spring s in theBlob.getSprings()) {
						s.Length *= 1.001f;
					}
					physics.Player.Volume.Minimum *= 1.001f;
					physics.Player.Volume.Origin *= 1.001f;
					physics.Player.Volume.Maximum *= 1.001f;
					cartoonEffect.Parameters["MaxShadowSize"].SetValue(cartoonEffect.Parameters["MaxShadowSize"].GetValueSingle() * 1.001f);
				} else if (InputHandler.IsKeyDown(Keys.End)) {
					foreach (Spring s in theBlob.getSprings()) {
						s.Length *= 0.999f;
					}
					physics.Player.Volume.Minimum *= 0.999f;
					physics.Player.Volume.Origin *= 0.999f;
					physics.Player.Volume.Maximum *= 0.999f;
					cartoonEffect.Parameters["MaxShadowSize"].SetValue(cartoonEffect.Parameters["MaxShadowSize"].GetValueSingle() * 0.999f);
				}


				if (InputHandler.IsKeyDown(Keys.X)) {
					physics.Player.Volume.Target = 1f;
				} else if (InputHandler.IsKeyDown(Keys.C)) {
					physics.Player.Volume.Target = 0f;
				} else if (InputHandler.IsKeyDown(Keys.V)) {
					physics.Player.Volume.Target = 0.5f;
				}
#endif
				if (WinFlag) {
					float playerTime = GameplayScreen.physics.Time;
					WinScreen tempWin = new WinScreen(GameplayScreen.physics.Time);
					GameState.GameScreen.ScreenManager.AddScreen(tempWin);
					tempWin.CheckForNewHighScore();
					WinFlag = false;
				}

			}
		}

		public override void Draw(GameTime gameTime) {
#if TIMED
			drawTime.Reset();
			drawTime.Start();
#endif

			Camera cam = CameraManager.getSingleton.ActiveCamera;

			cartoonEffect.Parameters["World"].SetValue(worldMatrix);
			cartoonEffect.Parameters["View"].SetValue(cam.View);
			cartoonEffect.Parameters["Projection"].SetValue(cam.Projection);

			distorterEffect.Parameters["WorldViewProjection"].SetValue(worldMatrix * cam.View * cam.Projection);
			distorterEffect.Parameters["WorldView"].SetValue(worldMatrix * cam.View);

			//Octree Cull the Static Drawables
			//foreach (List<Drawable> drawables in ) {
                foreach (Drawable d in currentArea.getDrawableList())
                {
                    d.Drawn = false;
                }
			//}

			SceneManager.getSingleton.UpdateVisibleDrawables(gameTime);

			//Level Models
			currentArea.Display.Draw(ScreenManager.GraphicsDevice, theBlob);


#if TIMED
			drawTime.Stop();
#endif

#if DEBUG && TIMED
			if (DEBUG_MaxPhys == -1) {
				DEBUG_MaxPhys = (float)physicsTime.Elapsed.TotalMilliseconds;
			} else {
				DEBUG_MaxPhys = Math.Max(DEBUG_MaxPhys, (float)physicsTime.Elapsed.TotalMilliseconds);
			}
			if (DEBUG_MinPhys == -1) {
				DEBUG_MinPhys = (float)physicsTime.Elapsed.TotalMilliseconds;
			} else {
				DEBUG_MinPhys = Math.Min(DEBUG_MinPhys, (float)physicsTime.Elapsed.TotalMilliseconds);
			}
			if (DEBUG_MaxDraw == -1) {
				DEBUG_MaxDraw = (float)drawTime.Elapsed.TotalMilliseconds;
			} else {
				DEBUG_MaxDraw = Math.Max(DEBUG_MaxDraw, (float)drawTime.Elapsed.TotalMilliseconds);
			}
			if (DEBUG_MinDraw == -1) {
				DEBUG_MinDraw = (float)drawTime.Elapsed.TotalMilliseconds;
			} else {
				DEBUG_MinDraw = Math.Min(DEBUG_MinDraw, (float)drawTime.Elapsed.TotalMilliseconds);
			}
#endif

			// GUI
			ScreenManager.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
			spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
			if (FPS)
			{
				spriteBatch.DrawString(font, fps, Vector2.Zero, Color.White);
			}
#if !DEBUG
            if (currentArea.ShowTime) {
                string t = "Time - " + Format.Time(physics.Time);
                spriteBatch.DrawString(font, t, new Vector2(ScreenManager.graphics.GraphicsDevice.Viewport.Width - (font.MeasureString(t).X + 10), 0), Color.White);
            }

            if (currentArea.TimeLimit > 0) 
            {
				string t = "Time Limit - " + Format.TimeNamed(currentArea.TimeLimit);
                spriteBatch.DrawString(font, t, new Vector2(ScreenManager.graphics.GraphicsDevice.Viewport.Width - (font.MeasureString(t).X + 10), 30), Color.White);
            }
			
#else
			spriteBatch.DrawString(font, "Time - " + Format.Time(physics.Time), new Vector2(0, 175), Color.White);

#if TIMED
			spriteBatch.DrawString(font, "Phys: " + physicsTime.Elapsed.TotalMilliseconds, new Vector2(0, 30), Color.White);
			spriteBatch.DrawString(font, "Draw: " + drawTime.Elapsed.TotalMilliseconds, new Vector2(0, 60), Color.White);

			spriteBatch.DrawString(font, "Phys(Max): " + DEBUG_MaxPhys, new Vector2(0, 100), Color.White);
			spriteBatch.DrawString(font, "Draw(Max): " + DEBUG_MaxDraw, new Vector2(0, 130), Color.White);

			spriteBatch.DrawString(font, "Phys(Min): " + DEBUG_MinPhys, new Vector2(320, 100), Color.White);
			spriteBatch.DrawString(font, "Draw(Min): " + DEBUG_MinDraw, new Vector2(320, 130), Color.White);

			if (physics.Player != null) {
				if (physics.Player.Resilience.Target < 0.33) {
					spriteBatch.DrawString(font, "Soft", new Vector2(150, 0), Color.White);
				} else if (physics.Player.Resilience.Target > 0.66) {
					spriteBatch.DrawString(font, "Firm", new Vector2(150, 0), Color.White);
				} else {
					spriteBatch.DrawString(font, "Normal", new Vector2(150, 0), Color.White);
				}
				if (physics.Player.Traction.Target < 0.33) {
					spriteBatch.DrawString(font, "Slick", new Vector2(350, 0), Color.White);
				} else if (physics.Player.Traction.Target > 0.66) {
					spriteBatch.DrawString(font, "Sticky", new Vector2(350, 0), Color.White);
				} else {
					spriteBatch.DrawString(font, "Normal", new Vector2(350, 0), Color.White);
				}
			}
#endif
#endif

#if DEBUG
			if (paused) {
				spriteBatch.DrawString(font, "Paused", new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - font.MeasureString("Paused").X) * 0.5f, (ScreenManager.GraphicsDevice.Viewport.Height - font.MeasureString("Paused").Y) * 0.5f), Color.White);
			}

			if (DEBUG_GodMode) {
				spriteBatch.DrawString(font, "GOD MODE", new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - font.MeasureString("Paused").X) * 0.5f, (ScreenManager.GraphicsDevice.Viewport.Height - font.MeasureString("Paused").Y) * 0.6f), Color.Yellow);
			}

			spriteBatch.DrawString(font, "Vol: " + theBlob.Volume.ToString(), new Vector2(345, 30), Color.White);
			spriteBatch.DrawString(font, "Next Vol: " + theBlob.PotentialVolume.ToString(), new Vector2(250, 60), Color.White);

			spriteBatch.DrawString(font, "Collidables: " + physics.DEBUG_GetNumCollidables(), new Vector2(500, 0), Color.White);
			spriteBatch.DrawString(font, "PWR: " + physics.PWR, new Vector2(0, 566), Color.White);

			spriteBatch.DrawString(font, "Points: " + physics.DEBUG_GetNumPoints(), new Vector2(592, 30), Color.White);

			spriteBatch.DrawString(font, "Drawn: " + SceneManager.getSingleton.Drawn, new Vector2(600, 60), Color.White);

			spriteBatch.DrawString(font, "PM: " + physics.physicsMultiplier, new Vector2(300, 566), Color.White);




			if (end > DateTime.Now)
			{
				spriteBatch.DrawString(font, (end - DateTime.Now).ToString(), new Vector2(500, 566), Color.White);
			}
#endif
			if (TextMin != TextMax) {
				spriteBatch.Draw(backdrop, new Rectangle((int)(TextMin.X - 10), (int)(TextMin.Y - 5), (int)((TextMax.X - TextMin.X) + 20), (int)((TextMax.Y - TextMin.Y) + 10)), Color.White);
			}
			displayText(m_TextEventstring, new Vector2((int)(ScreenManager.graphics.GraphicsDevice.Viewport.Width * 0.5f), (int)(ScreenManager.graphics.GraphicsDevice.Viewport.Height * 0.2f)), spriteBatch);


			spriteBatch.Draw(gaugeLine, new Rectangle(20, 105, gaugeLine.Width, 400), Color.White);

			spriteBatch.Draw(gaugeLine, new Rectangle(ScreenManager.graphics.GraphicsDevice.Viewport.Width - 30, 105, gaugeLine.Width, 400), Color.White);

			if (default_sticky)
				spriteBatch.Draw(gaugeMark, new Vector2(13, 110 + (physics.Player.Traction.Target * 375)), Color.White);
			else
				spriteBatch.Draw(gaugeMark, new Vector2(13, 110 + ((1 - physics.Player.Traction.Target) * 375)), Color.White);

			if (default_firm)
				spriteBatch.Draw(gaugeMark, new Vector2(ScreenManager.graphics.GraphicsDevice.Viewport.Width - 40, 110 + (physics.Player.Resilience.Target * 375)), Color.White);
			else
				spriteBatch.Draw(gaugeMark, new Vector2(ScreenManager.graphics.GraphicsDevice.Viewport.Width - 40, 110 + ((1 - physics.Player.Resilience.Target) * 375)), Color.White);

			Rectangle topRight = new Rectangle(ScreenManager.graphics.GraphicsDevice.Viewport.Width - 125, 40, 125, 75);
			Rectangle bottomRight = new Rectangle(ScreenManager.graphics.GraphicsDevice.Viewport.Width - 75, 505, 75, 50);
			if (default_firm) {
				spriteBatch.Draw(soft, topRight, Color.White);
				spriteBatch.Draw(firm, bottomRight, Color.White);
			} else {
				spriteBatch.Draw(firm, topRight, Color.White);
				spriteBatch.Draw(soft, bottomRight, Color.White);
			}


			Rectangle topleft = new Rectangle(0, 40, 125, 75);
			Rectangle bottomLeft = new Rectangle(0, 505, 75, 50);
			if (default_sticky) {
				spriteBatch.Draw(slick, topleft, Color.White);
				spriteBatch.Draw(sticky, bottomLeft, Color.White);
			} else {
				spriteBatch.Draw(sticky, topleft, Color.White);
				spriteBatch.Draw(slick, bottomLeft, Color.White);
			}

			spriteBatch.End();

			//fps
			if (FPS)
			{
				++frames;
			}

			//base.Draw(gameTime);
		}

		private void displayText(string text, Vector2 center, SpriteBatch spriteBatch) {
			TextMin = center;
			TextMax = center;
			try {
				float usableScreenWidth = ScreenManager.graphics.GraphicsDevice.Viewport.Width * 0.6f;
				string nextstring = null;
				if (font.MeasureString(text).X > usableScreenWidth) {
					nextstring = string.Empty;
					do {
						nextstring = nextstring.Insert(0, text.Substring(text.Length - 1));
						text = text.Remove(text.Length - 1);
					} while (font.MeasureString(text).X > usableScreenWidth);
					while (!text.EndsWith(" ")) {
						nextstring = nextstring.Insert(0, text.Substring(text.Length - 1));
						text = text.Remove(text.Length - 1);
					}
					text = text.Remove(text.Length - 1);
				}

				spriteBatch.DrawString(font, text, center, Color.White, 0f, font.MeasureString(text) * 0.5f, 1f, SpriteEffects.None, 0f);

				if (nextstring != null) {
					Vector2 newCenter = center;
					newCenter.Y += (int)(font.MeasureString(text).Y * 0.6f);
					displayText(nextstring, newCenter, spriteBatch);
				}
				TextMax = Vector2.Max(TextMax, center + font.MeasureString(text) * 0.5f);
				TextMin = Vector2.Min(TextMin, center - font.MeasureString(text) * 0.5f);
			} catch (Exception e) {
				Log.Out.WriteLine(e);
			}
		}

		private Vector2 TextMin;
		private Vector2 TextMax;

#if DEBUG
		private readonly DateTime end = new DateTime(2008, 5, 21, 20, 0, 0);
#endif
	}
}
