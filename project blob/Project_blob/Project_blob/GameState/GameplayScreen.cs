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

namespace Project_blob.GameState
{
	public class GameplayScreen : GameScreen
	{
		SpriteBatch spriteBatch;

		Model blobModel;
		//Model skyBox;

		//Texture2D skyTexture;
		TextureInfo ti;
		StaticModel sky;
		private Blob theBlob;
		public Blob Player { get { return theBlob; } }

		Texture2D blobTexture;
		Texture2D distortMapText;

		Effect effect;
		Effect celEffect;
		Effect blobEffect;
		Effect cartoonEffect;
		Effect postprocessEffect;
		Effect distortEffect;
		Effect distorterEffect;

		RenderTarget2D sceneRenderTarget;
		RenderTarget2D normalDepthRenderTarget;
		RenderTarget2D distortionMap;
		ResolveTexture2D tempRenderTarget;
		RenderTarget2D depthBufferRenderTarget;

		Matrix worldMatrix;
		//Matrix viewMatrix;
		//Matrix projectionMatrix;

		VertexDeclaration VertexDeclarationColor;
		VertexDeclaration VertexDeclarationTexture;

		List<Drawable> drawables = new List<Drawable>();

		Vector4 lightPosition;

		PhysicsManager physics;

		bool cinema = false;
		bool paused = false;
        bool step = false;
		bool controllermode = false;
		bool follow = true;

		bool points = false;

		SpriteFont font;
		//fps
		float time = 0f;
		float update = 1f;
		int frames = 0;
		string fps = "";

		public static GameplayScreen game;

		static Vector3 defaultCameraPosition = new Vector3(0, 15, 10);
		//Vector3 cameraPosition = defaultCameraPosition;
		Vector2 cameraAngle = new Vector2(1f, 0.4f);
		float cameraLengthMulti = 1f;
		float cameraLength = 20f;
		float playerCamMulti = 0.1f;

		bool OrientCamera = false;

		public static Area currentArea;

		System.Diagnostics.Stopwatch physicsTime = new System.Diagnostics.Stopwatch();
		System.Diagnostics.Stopwatch drawTime = new System.Diagnostics.Stopwatch();

		public GameplayScreen()
		{
			game = this;
			TransitionOnTime = TimeSpan.FromSeconds(1.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);

			lightPosition = new Vector4(5, 5, 5, 0);
		}

		private Vector3 blobStartPosition = new Vector3(0, 0, 0);

		private void reset()
		{
			if (physics != null)
			{
				physics.stop();
			}
			physics = PhysicsManager.getInstance();

			physics.AirFriction = 1f;

			physics.Player.Traction.Minimum = 0.1f;
			physics.Player.Traction.Origin = 1f;
			physics.Player.Traction.Maximum = 5f;

			physics.Player.Cling.Minimum = 0f;
			physics.Player.Cling.Origin = 5f;
			physics.Player.Cling.Maximum = 10f;

			physics.Player.Resilience.Minimum = 20f;
			physics.Player.Resilience.Origin = 40f;
			physics.Player.Resilience.Maximum = 50f;

			physics.Player.Volume.Minimum = 50f;
			physics.Player.Volume.Origin = 100f;
			physics.Player.Volume.Maximum = 200f;

			theBlob = new Blob(blobModel, blobStartPosition);
			theBlob.text = blobTexture;
			theBlob.DisplacementText = distortMapText;
			theBlob.setGraphicsDevice(ScreenManager.GraphicsDevice);

			physics.Player.PlayerBody = theBlob;
			physics.AddBody(theBlob);

			physics.Player.PlayerBody.tasks.Add(new GravityVector(10f, new Vector3(0f, -1.0f, 0f)));

			if (currentArea != null)
			{
				physics.AddBodys(currentArea.getBodies());
			}
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		public override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

			//load fonts
			font = ScreenManager.Content.Load<SpriteFont>(@"Fonts\\Courier New");

			//load shaders
			celEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\Cel");

			blobModel = ScreenManager.Content.Load<Model>(@"Models\\soccerball");

			blobTexture = ScreenManager.Content.Load<Texture2D>(@"Textures\\transparancy_png");
			distortMapText = ScreenManager.Content.Load<Texture2D>(@"Textures\\PrivacyGlass");

			blobEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\Blobs");

			cartoonEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\CartoonEffect");

			postprocessEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\PostprocessEffect");

			distortEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\Distort");
			distorterEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\Distorters");

			//cartoonEffect = ScreenManager.Content.Load<Effect>(@"Shaders\\DepthBuffer");

			//load skybox
			//skyBox = ScreenManager.Content.Load<Model>(@"Models\\skyBox");
			//skyTexture = ScreenManager.Content.Load<Texture2D>(@"Textures\\point_text");

			reset();

			//load default level
            Level.LoadLevel("FinalLevel", "effects");

			//List of Static Drawables to add to Scene
			List<Drawable> staticDrawables = new List<Drawable>();
			List<Portal> portals = new List<Portal>();

			//load first area
			if (Level.Areas.Count > 0)
			{
				IEnumerator e = Level.Areas.Values.GetEnumerator();
				e.MoveNext();
				//e.MoveNext();
				//e.MoveNext();

				currentArea = (Area)e.Current;
				currentArea.LoadAreaGameplay(ScreenManager);
				currentArea.Display.EffectName = "cartoonEffect";
				currentArea.Display.WorldParameterName = "World";
				currentArea.Display.TextureParameterName = "Texture";
				currentArea.Display.TechniqueName = "Lambert";
				staticDrawables = currentArea.getDrawableList();
				portals = currentArea.Portals;
				physics.AddBodys(currentArea.getBodies());
			}
			else
			{
				//empty level
			}

			ti = new TextureInfo("cloudsky", 0);
			sky = new StaticModel("sky", "skyBox", "none", ti, new List<short>());
			sky.initialize();
			sky.TextureName = "cloudsky";
			sky.Scale = Matrix.CreateScale(750f);

			currentArea.Display.SkyBox = sky;


			//Add the Static Drawables to the Octree
			List<Drawable> temp = new List<Drawable>(staticDrawables);
            //SceneManager.getSingleton.BuildOctree(ref temp);
            SceneManager.getSingleton.GraphType = SceneManager.SceneGraphType.Portal;
			//SceneManager.getSingleton.BuildPortalScene(temp);
			foreach (Portal p in portals)
			{
				p.CreateBoundingBox();
			}
			SceneManager.getSingleton.BuildPortalScene(temp, portals);
            SceneManager.getSingleton.PortalScene.CurrSector = 1;


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

			InitializeEffect();

            currentArea.Display.SetBlurEffectParameters(1f / (float)ScreenManager.GraphicsDevice.Viewport.Width, 1f / (float)ScreenManager.GraphicsDevice.Viewport.Height);

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
		private void InitializeEffect()
		{
			worldMatrix = Matrix.Identity;

			//viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -10), new Vector3(0, 0, 0), Vector3.Up);
			//camera.View = Matrix.CreateLookAt(new Vector3(0, 0, -10), new Vector3(0, 0, 0), Vector3.Up);

			//projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
			//    MathHelper.ToRadians(45),  // 45 degree angle
			//    (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height,
			//    1.0f, 1000.0f);
			//camera.Projection = Matrix.CreatePerspectiveFieldOfView(camera.FieldOfView,  // 45 degree angle
			//    camera.AspectRatio, camera.NearPlane, camera.FarPlane);

			VertexDeclarationTexture = new VertexDeclaration(ScreenManager.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

			VertexDeclarationColor = new VertexDeclaration(ScreenManager.GraphicsDevice, VertexPositionColor.VertexElements);

			effect = ScreenManager.Content.Load<Effect>("Shaders\\effects");

			//effect.Parameters["xView"].SetValue(viewMatrix);
			//effect.Parameters["xProjection"].SetValue(projectionMatrix);
			effect.Parameters["xView"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
			effect.Parameters["xProjection"].SetValue(CameraManager.getSingleton.ActiveCamera.Projection);
			effect.Parameters["xWorld"].SetValue(worldMatrix);

			effect.Parameters["xTexture"].SetValue(blobTexture);
			effect.Parameters["xEnableLighting"].SetValue(true);
			//effect.Parameters["xShowNormals"].SetValue(true);
			//effect.Parameters["xLightDirection"].SetValue(Vector3.Down);
			effect.Parameters["xLightPos"].SetValue(new Vector4(0, 5, 0, 0));
			effect.Parameters["xAmbient"].SetValue(0.25f);

			effect.Parameters["xCameraPos"].SetValue(new Vector4(0, 0, -5, 0));

			EffectManager.getSingleton.AddEffect("effects", effect);

			celEffect.Parameters["World"].SetValue(worldMatrix);
			//celEffect.Parameters["View"].SetValue(viewMatrix);
			//celEffect.Parameters["Projection"].SetValue(projectionMatrix);
			celEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
			celEffect.Parameters["Projection"].SetValue(CameraManager.getSingleton.ActiveCamera.Projection);

			celEffect.Parameters["DiffuseLightColor"].SetValue(new Vector4(0.75f, 0.75f, 0.75f, 1.0f));
			celEffect.Parameters["LightPosition"].SetValue(new Vector3(1.0f, 600.0f, 600.0f));
			celEffect.Parameters["LayerOneSharp"].SetValue(.3f);
			celEffect.Parameters["LayerOneRough"].SetValue(10.0f);
			celEffect.Parameters["LayerOneContrib"].SetValue(0.08f);
			celEffect.Parameters["LayerTwoSharp"].SetValue(0.10f);
			celEffect.Parameters["LayerTwoRough"].SetValue(1.0f);
			celEffect.Parameters["LayerTwoContrib"].SetValue(0.2f);
			celEffect.Parameters["EdgeOffset"].SetValue(0.009f);

			//celEffect.Parameters["EyePosition"].SetValue(cameraPosition);
			celEffect.Parameters["EyePosition"].SetValue(CameraManager.getSingleton.ActiveCamera.Position);

			EffectManager.getSingleton.AddEffect("celEffect", celEffect);

			BasicEffect be = new BasicEffect(ScreenManager.GraphicsDevice, null);

			be.Alpha = 1.0f;
			be.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
			be.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
			be.SpecularPower = 5.0f;
			be.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

			be.DirectionalLight0.Enabled = true;
			be.DirectionalLight0.DiffuseColor = Vector3.One;
			be.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
			be.DirectionalLight0.SpecularColor = Vector3.One;

			be.LightingEnabled = true;
			be.TextureEnabled = true;

			be.World = worldMatrix;
			//be.View = viewMatrix;
			//be.Projection = projectionMatrix;
			be.View = CameraManager.getSingleton.ActiveCamera.View;
			be.Projection = CameraManager.getSingleton.ActiveCamera.Projection;

			EffectManager.getSingleton.AddEffect("basic", be);

			cartoonEffect.Parameters["World"].SetValue(worldMatrix);
            cartoonEffect.Parameters["Projection"].SetValue(CameraManager.getSingleton.ActiveCamera.Projection);
			cartoonEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
			cartoonEffect.Parameters["TextureEnabled"].SetValue(true);

			distorterEffect.Parameters["WorldViewProjection"].SetValue(worldMatrix * CameraManager.getSingleton.ActiveCamera.View * CameraManager.getSingleton.ActiveCamera.Projection);
			distorterEffect.Parameters["WorldView"].SetValue(worldMatrix * CameraManager.getSingleton.ActiveCamera.View);

			CreateRenderTargets();

			EffectManager.getSingleton.AddEffect("postprocessEffect", postprocessEffect);
			EffectManager.getSingleton.AddEffect("cartoonEffect", cartoonEffect);

			EffectManager.getSingleton.AddEffect("Distorter", distorterEffect);
			EffectManager.getSingleton.AddEffect("Distort", distortEffect);

			cartoonEffect.Parameters["MaxDepth"].SetValue(60);

			//EffectManager.getSingleton.AddEffect("DepthBuffer", depthBufferEffect);


		}

		public void CreateRenderTargets()
		{
			PresentationParameters pp = ScreenManager.GraphicsDevice.PresentationParameters;

			sceneRenderTarget = new RenderTarget2D(ScreenManager.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

			normalDepthRenderTarget = new RenderTarget2D(ScreenManager.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

			distortionMap = new RenderTarget2D(ScreenManager.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

			tempRenderTarget = new ResolveTexture2D(ScreenManager.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat);

			depthBufferRenderTarget = new RenderTarget2D(ScreenManager.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);


			currentArea.Display.SceneRanderTarget = sceneRenderTarget;
			currentArea.Display.NormalDepthRenderTarget = normalDepthRenderTarget;
			currentArea.Display.DistortionMap = distortionMap;
			currentArea.Display.TempRenderTarget = tempRenderTarget;
			currentArea.Display.DepthMapRenderTarget = depthBufferRenderTarget;

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		public override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		public void ChangeArea(String area, Vector3 position)
		{
			currentArea = Level.Areas[area];
			currentArea.Display.WorldParameterName = "xWorld";
			currentArea.Display.TextureParameterName = "xTexture";
			currentArea.LoadAreaGameplay(ScreenManager);
			blobStartPosition = position;
			List<Drawable> temp = currentArea.getDrawableList();
			List<Portal> portals = currentArea.Portals;
            //SceneManager.getSingleton.BuildOctree(ref temp);
			//SceneManager.getSingleton.BuildPortalScene(temp);
			foreach (Portal p in portals)
			{
				p.CreateBoundingBox();
			}
			SceneManager.getSingleton.BuildPortalScene(temp, portals);
			reset();
		}

		public void SetUpCinematicCamera(List<Vector3> cameraPos, List<Vector3> cameraLooks, List<Vector3> cameraUps)
		{
			CinematicCamera cinematicCamera = (CinematicCamera)CameraManager.getSingleton.GetCamera("cinematic");
			cinematicCamera.Ups = cameraUps;
			cinematicCamera.Positions = cameraPos;
			cinematicCamera.LookAts = cameraLooks;
			cinematicCamera.Running = true;
			CameraManager.getSingleton.SetActiveCamera("cinematic");
			cinema = true;
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime, bool otherScreenHasFocus,
													   bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
			currentArea.Display.AddToBeDrawn(sky);
			if (IsActive)
			{
				physicsTime.Reset();
				physicsTime.Start();
				if (!paused)
				{
					physics.update((float)gameTime.ElapsedGameTime.TotalSeconds);

					EffectManager.getSingleton.GetEffect("cartoonEffect").Parameters["blobCenter"].SetValue(theBlob.getCenter());
                    EffectManager.getSingleton.GetEffect("Distort").Parameters["blobCenter"].SetValue(new Vector2(theBlob.getCenter().X, theBlob.getCenter().Y));

                    Vector4 tempPos = new Vector4(theBlob.getCenter(), 0);
                    tempPos.Y += 10;
                    cartoonEffect.Parameters["LightPos"].SetValue(tempPos);
                    Matrix lightViewProjectionMatrix = Matrix.CreateLookAt(new Vector3(tempPos.X, tempPos.Y, tempPos.Z), theBlob.getCenter(), new Vector3(0,0,1)) *
                        Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, (float)ScreenManager.CurrentResolution.Width / (float)ScreenManager.CurrentResolution.Height, CameraManager.getSingleton.ActiveCamera.NearPlane, CameraManager.getSingleton.ActiveCamera.FarPlane);
                    cartoonEffect.Parameters["LightWorldViewProjection"].SetValue(worldMatrix * lightViewProjectionMatrix);

				}
				physicsTime.Stop();
                if (step)
                {
                    paused = true;
                    step = false;
                }

                if (InputHandler.IsKeyPressed(Keys.L))
                {
                    step = true;
                    paused = false;
                }

                if (InputHandler.IsKeyPressed(Keys.PageUp))
                {
                    physics.physicsMultiplier *= 1.1f;
                } else if (InputHandler.IsKeyPressed(Keys.PageDown))
                {
                    physics.physicsMultiplier *= 0.9f;
                }

				// actually, shouldn't the skybox be centered around /the camera/ instead of the blob?
				if (currentArea.Display.SkyBox != null)
					currentArea.Display.SkyBox.Position = Matrix.CreateTranslation(theBlob.getCenter());

				//Update Camera
				//camera.Update(gameTime);
				CameraManager.getSingleton.Update(gameTime);

				if (InputHandler.IsActionPressed(Actions.Pause))
				{
					ScreenManager.AddScreen(new PauseMenuScreen());
				}
				if (InputHandler.IsKeyPressed(Keys.OemTilde))
				{
					if (PhysicsManager.enableParallel != PhysicsManager.ParallelSetting.Never)
					{
						PhysicsManager.enableParallel = PhysicsManager.ParallelSetting.Never;
					}
					else
					{
						PhysicsManager.enableParallel = PhysicsManager.ParallelSetting.Always;
					}
					reset();
					DEBUG_MaxPhys = -1;
					DEBUG_MinPhys = -1;
					DEBUG_MaxDraw = -1;
					DEBUG_MinDraw = -1;
				}
				if (InputHandler.IsActionPressed(Actions.Reset))
				{
					reset();
					DEBUG_MaxPhys = -1;
					DEBUG_MinPhys = -1;
					DEBUG_MaxDraw = -1;
					DEBUG_MinDraw = -1;
				}
				if (InputHandler.IsKeyPressed(Keys.T))
				{
					ScreenManager.ToggleFullScreen();
				}
				if (InputHandler.IsKeyPressed(Keys.P))
				{
					paused = !paused;
				}
				if (InputHandler.IsKeyPressed(Keys.F))
				{
					follow = !follow;
					//camera follow
					if (!follow)
					{
						//cameraPosition = defaultCameraPosition;
						CameraManager.getSingleton.ActiveCamera.Position = defaultCameraPosition;
						//viewMatrix = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 4, 0), Vector3.Up);
						//effect.Parameters["xView"].SetValue(viewMatrix);
						//camera.View = Matrix.CreateLookAt(camera.Postiion, new Vector3(0, 4, 0), Vector3.Up);
						CameraManager.getSingleton.ActiveCamera.Target = new Vector3(0, 4, 0);
						CameraManager.getSingleton.ActiveCamera.Up = Vector3.Up;


						cartoonEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
						distorterEffect.Parameters["WorldViewProjection"].SetValue(worldMatrix * CameraManager.getSingleton.ActiveCamera.View * CameraManager.getSingleton.ActiveCamera.Projection);
						distorterEffect.Parameters["WorldView"].SetValue(worldMatrix * CameraManager.getSingleton.ActiveCamera.View);

						effect.Parameters["xView"].SetValue(CameraManager.getSingleton.ActiveCamera.View);

						//celEffect.Parameters["EyePosition"].SetValue(cameraPosition);
						celEffect.Parameters["EyePosition"].SetValue(CameraManager.getSingleton.ActiveCamera.Position);
						//celEffect.Parameters["View"].SetValue(viewMatrix);
						celEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
						//effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
						effect.Parameters["xCameraPos"].SetValue(new Vector4(CameraManager.getSingleton.ActiveCamera.Position, 0));
					}
				}

				if (InputHandler.IsKeyPressed(Keys.S))
				{
					//theBlob.setSpringForce(92.5f);
					physics.Player.Resilience.Target = 1f;
				}
				if (InputHandler.IsKeyPressed(Keys.D))
				{
					//theBlob.setSpringForce(62.5f);
					physics.Player.Resilience.Target = 0.5f;
				}
				if (InputHandler.IsKeyPressed(Keys.W))
				{
					//theBlob.setSpringForce(12.5f);
					physics.Player.Resilience.Target = 0f;
				}
				if (InputHandler.IsKeyPressed(Keys.Q))
				{
					//Physics.PhysicsManager.TEMP_SurfaceFriction = 2f;
					physics.Player.Traction.Target = 1f;
					physics.Player.Cling.Target = 1f;
				}
				if (InputHandler.IsKeyPressed(Keys.E))
				{
					//Physics.PhysicsManager.TEMP_SurfaceFriction = 0.5f;
					physics.Player.Traction.Target = 0f;
					physics.Player.Cling.Target = 0f;
				}
				if (InputHandler.IsKeyPressed(Keys.A))
				{
					//Physics.PhysicsManager.TEMP_SurfaceFriction = 1f;
					physics.Player.Traction.Target = 0.5f;
					physics.Player.Cling.Target = 0.5f;
				}
				if (InputHandler.IsKeyPressed(Keys.Z))
				{
					currentArea.Display.DEBUG_WireframeMode = !currentArea.Display.DEBUG_WireframeMode;
				}
				if (InputHandler.IsKeyPressed(Keys.O))
				{
					OrientCamera = !OrientCamera;
				}
				if (InputHandler.IsKeyPressed(Keys.OemPeriod))
				{
					points = !points;
				}
				if (InputHandler.IsKeyPressed(Keys.N))
				{
					Tri.DEBUG_DrawNormal = !Tri.DEBUG_DrawNormal;
				}

				if (InputHandler.IsKeyPressed(Keys.B))
				{
					DEBUG_MaxPhys = -1;
					DEBUG_MinPhys = -1;
					DEBUG_MaxDraw = -1;
					DEBUG_MinDraw = -1;
				}

                if(InputHandler.IsKeyPressed(Keys.OemPlus))
                {
                    currentArea.Display.saveOut = true;
                }

				// Xbox
				if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
				{
					controllermode = true;
				}
				if (controllermode)
				{
					//theBlob.setSpringForce(12.5f + (GamePad.GetState(PlayerIndex.One).Triggers.Right * 80f));
					physics.Player.Resilience.Target = GamePad.GetState(PlayerIndex.One).Triggers.Right;
					//Physics.PhysicsManager.TEMP_SurfaceFriction = GamePad.GetState(PlayerIndex.One).Triggers.Left * 24f;
					physics.Player.Traction.Target = GamePad.GetState(PlayerIndex.One).Triggers.Left;
					physics.Player.Cling.Target = 0.25f + (GamePad.GetState(PlayerIndex.One).Triggers.Left * 0.5f);
					/*float vb = MathHelper.Clamp(physics.ImpactThisFrame - 0.1f, 0f, 1f);
					InputHandler.SetVibration(vb, 0f);*/
				}

				// Quick Torque

				Vector2 move = InputHandler.GetAnalogAction(AnalogActions.Movement);
				if (move != Vector2.Zero)
				{
                    physics.Player.move(move, CameraManager.getSingleton.ActiveCamera.Position);
				}

                if (InputHandler.IsKeyPressed(Keys.J) || InputHandler.IsButtonPressed(Buttons.A))
                {
                    physics.Player.jump();
                }

				if (InputHandler.IsKeyDown(Keys.Home))
				{
					theBlob.setSpringLength(-0.001f);
					//cameraLengthMulti *= 1.015f;
				}
				else if (InputHandler.IsKeyDown(Keys.End))
				{
					theBlob.setSpringLength(0.001f);
					//cameraLengthMulti *= 0.985f;
				}


				if (InputHandler.IsKeyDown(Keys.X))
				{
					//theBlob.idealVolume = theBlob.baseVolume + 50f;
					physics.Player.Volume.Target = 1f;
				}
				else if (InputHandler.IsKeyDown(Keys.C))
				{
					//theBlob.idealVolume = theBlob.baseVolume + 50f;
					physics.Player.Volume.Target = 0f;
				}
				else if (InputHandler.IsKeyDown(Keys.V))
				{
					//theBlob.idealVolume = theBlob.baseVolume;
					physics.Player.Volume.Target = 0.5f;
				}
				//Console.WriteLine(theBlob.getVolume());
				//theBlob.update();

				if (cinema)
				{
					distorterEffect.Parameters["WorldViewProjection"].SetValue(worldMatrix * CameraManager.getSingleton.ActiveCamera.View * CameraManager.getSingleton.ActiveCamera.Projection);
					distorterEffect.Parameters["WorldView"].SetValue(worldMatrix * CameraManager.getSingleton.ActiveCamera.View);
					cartoonEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
					effect.Parameters["xView"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
					celEffect.Parameters["EyePosition"].SetValue(CameraManager.getSingleton.ActiveCamera.Position);
					celEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
					effect.Parameters["xCameraPos"].SetValue(new Vector4(CameraManager.getSingleton.ActiveCamera.Position, 0));
					if (((CinematicCamera)CameraManager.getSingleton.ActiveCamera).FinishedCinematics)
					{
						cinema = false;
						((CinematicCamera)CameraManager.getSingleton.ActiveCamera).Position = new Vector3(0, 0, -10);
						((CinematicCamera)CameraManager.getSingleton.ActiveCamera).Target = Vector3.Zero;
						((CinematicCamera)CameraManager.getSingleton.ActiveCamera).Up = Vector3.Up;
						((CinematicCamera)CameraManager.getSingleton.ActiveCamera).FinishedCinematics = false;
					}
				}
				else if (follow)
				{
					cameraAngle += InputHandler.GetAnalogAction(AnalogActions.Camera) * playerCamMulti;
					if (cameraAngle.X < -MathHelper.Pi)
					{
						cameraAngle.X += MathHelper.TwoPi;
					}
					else if (cameraAngle.X > MathHelper.Pi)
					{
						cameraAngle.X -= MathHelper.TwoPi;
					}

					cameraAngle = Vector2.Clamp(cameraAngle, new Vector2(-MathHelper.TwoPi, -MathHelper.PiOver2), new Vector2(MathHelper.TwoPi, MathHelper.PiOver2));

					// following camera
					cameraLength = MathHelper.Clamp(cameraLength + (InputHandler.getMouseWheelDelta() * -0.01f), 10, 40);
					Vector3 Offset = new Vector3((float)Math.Cos(cameraAngle.X) * cameraLength * cameraLengthMulti, (float)Math.Sin(cameraAngle.Y) * cameraLength * cameraLengthMulti, (float)Math.Sin(cameraAngle.X) * cameraLength * cameraLengthMulti);
					//cameraPosition = theBlob.getCenter() + Offset;
					CameraManager.getSingleton.ActiveCamera.Position = theBlob.getCenter() + Offset;

					// new Vector3(10, 10, 20)
					/*if (OrientCamera)
					{
						viewMatrix = Matrix.CreateLookAt(cameraPosition, theBlob.getCenter(), physics.getUp(theBlob.getCenter()));
					}
					else
					{*/
					//viewMatrix = Matrix.CreateLookAt(cameraPosition, theBlob.getCenter(), Vector3.Up);
					//camera.View = Matrix.CreateLookAt(camera.Postiion, theBlob.getCenter(), Vector3.Up);
					CameraManager.getSingleton.ActiveCamera.Target = theBlob.getCenter();
					//}
					//effect.Parameters["xView"].SetValue(viewMatrix);
					//celEffect.Parameters["View"].SetValue(viewMatrix);
					effect.Parameters["xView"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
					celEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);

					//effect.Parameters["xLightPos"].SetValue(new Vector4(cameraPosition.X * 0.5f, cameraPosition.Y * 0.5f, cameraPosition.Z * 0.5f, 0));
					//effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
					effect.Parameters["xCameraPos"].SetValue(new Vector4(CameraManager.getSingleton.ActiveCamera.Position, 0));
				}

				//cubeVertexBuffer.SetData<VertexPositionNormalTexture>(theBlob.getTriangleVertexes());



				// light
				effect.Parameters["xLightPos"].SetValue(lightPosition);



				//fps
				time += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (time > update)
				{
					fps = Convert.ToInt32(frames / time).ToString();
					time = 0;
					frames = 0;
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			drawTime.Reset();
			drawTime.Start();

			//ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

			//ScreenManager.GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
			ScreenManager.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
			ScreenManager.GraphicsDevice.RenderState.DepthBufferEnable = true;
			ScreenManager.GraphicsDevice.RenderState.AlphaBlendEnable = false;
			ScreenManager.GraphicsDevice.RenderState.AlphaTestEnable = false;

			//effect.CurrentTechnique = effect.Techniques["Textured"];
			//ScreenManager.GraphicsDevice.Textures[0] = blobTexture;

			//blobEffect.CurrentTechnique = blobEffect.Techniques["BlobBlendTwoPasses"];

			//cartoonEffect.Parameters["Texture"].SetValue(blobTexture);

			// Set suitable renderstates for drawing a 3D model.
			//RenderState renderState = ScreenManager.GraphicsDevice.RenderState;

			cartoonEffect.Parameters["World"].SetValue(worldMatrix);
			cartoonEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
			cartoonEffect.Parameters["Projection"].SetValue(CameraManager.getSingleton.ActiveCamera.Projection);

			distorterEffect.Parameters["WorldViewProjection"].SetValue(worldMatrix * CameraManager.getSingleton.ActiveCamera.View * CameraManager.getSingleton.ActiveCamera.Projection);
			distorterEffect.Parameters["WorldView"].SetValue(worldMatrix * CameraManager.getSingleton.ActiveCamera.View);


			//renderState.CullMode = CullMode.CullCounterClockwiseFace;

			//ScreenManager.GraphicsDevice.SetRenderTarget(0, normalDepthRenderTarget);
			//ScreenManager.GraphicsDevice.Clear(Color.Black);
			//cartoonEffect.CurrentTechnique = cartoonEffect.Techniques["NormalDepth"];
			//cartoonEffect.Begin();
			//foreach (EffectPass pass in cartoonEffect.CurrentTechnique.Passes)
			//{
			//    pass.Begin();
			//    theBlob.DrawMe();
			//    pass.End();
			//}
			//cartoonEffect.End();



			//ScreenManager.GraphicsDevice.SetRenderTarget(0, null);
			//ScreenManager.GraphicsDevice.SetRenderTarget(0, null);
			//Vector2 resolution = new Vector2(sceneRenderTarget.Width,
			//                                     sceneRenderTarget.Height);

			//Texture2D normalDepthTexture = normalDepthRenderTarget.GetTexture();

			//EffectParameterCollection parameters = postprocessEffect.Parameters;
			//parameters["EdgeWidth"].SetValue(1.0f);
			//parameters["EdgeIntensity"].SetValue(1.0f);
			//parameters["ScreenResolution"].SetValue(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width,ScreenManager.GraphicsDevice.Viewport.Height));
			//parameters["NormalDepthTexture"].SetValue(normalDepthTexture);

			//postprocessEffect.CurrentTechnique = postprocessEffect.Techniques["EdgeDetect"];

			//// Draw a fullscreen sprite to apply the postprocessing effect.
			//SpriteBatch spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
			//spriteBatch.Begin(SpriteBlendMode.None,
			//                  SpriteSortMode.Immediate,
			//                  SaveStateMode.None);

			//postprocessEffect.Begin();
			//postprocessEffect.CurrentTechnique.Passes[0].Begin();

			////spriteBatch.Draw(sceneRenderTarget.GetTexture(), Vector2.Zero, Color.White);

			//spriteBatch.End();

			//postprocessEffect.CurrentTechnique.Passes[0].End();
			//postprocessEffect.End();

			//renderState.CullMode = CullMode.CullClockwiseFace;

			// Collision Tris
			/*effect.CurrentTechnique = effect.Techniques["Colored"];
			GraphicsDevice.VertexDeclaration = VertexDeclarationColor;
			GraphicsDevice.Indices = null;
			foreach (Drawable d in drawables)
			{
				//VertexPositionColor[] temp = d.getTriangleVertexes();
				//VertexBuffer tempVertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * temp.Length, BufferUsage.None);
				//tempVertexBuffer.SetData<VertexPositionColor>(temp);
				//GraphicsDevice.Vertices[0].SetSource(tempVertexBuffer, 0, VertexPositionColor.SizeInBytes);
				GraphicsDevice.Vertices[0].SetSource(d.getVertexBuffer(), 0, d.getVertexStride());
				effect.Begin();
				foreach (EffectPass pass in effect.CurrentTechnique.Passes)
				{
					pass.Begin();
					d.DrawMe();
					pass.End();
				}
				effect.End();
			}*/

			//Octree Cull the Static Drawables
			foreach (TextureInfo info in currentArea.Display.DrawnList.Keys)
			{
				currentArea.Display.DrawnList[info].Clear();
			}
			if (currentArea.Display.SkyBox != null)
				currentArea.Display.AddToBeDrawn(currentArea.Display.SkyBox);

			SceneManager.getSingleton.UpdateVisibleDrawables(gameTime);

			//Level Models
			currentArea.Display.Draw(ScreenManager.GraphicsDevice, theBlob);

			if (points)
			{
				// Corner Dots -
				effect.CurrentTechnique = effect.Techniques["Colored"];
				ScreenManager.GraphicsDevice.VertexDeclaration = VertexDeclarationColor;
				ScreenManager.GraphicsDevice.RenderState.DepthBufferEnable = false;
				VertexPositionColor[] dotVertices = new VertexPositionColor[theBlob.points.Count];
				for (int i = 0; i < theBlob.points.Count; ++i)
				{
					dotVertices[i] = new VertexPositionColor(theBlob.points[i].ExternalPosition, Color.Black);
				}
				VertexBuffer dotvertexBuffer = new VertexBuffer(ScreenManager.GraphicsDevice, VertexPositionColor.SizeInBytes * theBlob.points.Count, BufferUsage.None);
				dotvertexBuffer.SetData<VertexPositionColor>(dotVertices);
				ScreenManager.GraphicsDevice.Vertices[0].SetSource(dotvertexBuffer, 0, VertexPositionColor.SizeInBytes);
				effect.Begin();
				foreach (EffectPass pass in effect.CurrentTechnique.Passes)
				{
					pass.Begin();
					ScreenManager.GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, theBlob.points.Count);
					pass.End();
				}
				effect.End();

				// Velocity Vectors			
				VertexPositionColor[] vectorVertices = new VertexPositionColor[theBlob.points.Count * 2];

				for (int i = 0; i < theBlob.points.Count; i++)
				{
					vectorVertices[i * 2] = (new VertexPositionColor(theBlob.points[i].ExternalPosition, Color.Red));
					vectorVertices[(i * 2) + 1] = (new VertexPositionColor(theBlob.points[i].ExternalPosition + theBlob.points[i].ExternalVelocity, Color.Pink));
				}
				VertexBuffer vectorVertexBuffer = new VertexBuffer(ScreenManager.GraphicsDevice, VertexPositionColor.SizeInBytes * vectorVertices.Length, BufferUsage.None);
				vectorVertexBuffer.SetData<VertexPositionColor>(vectorVertices);

				ScreenManager.GraphicsDevice.Vertices[0].SetSource(vectorVertexBuffer, 0, VertexPositionColor.SizeInBytes * vectorVertices.Length);

				effect.Begin();
				foreach (EffectPass pass in effect.CurrentTechnique.Passes)
				{
					pass.Begin();
					ScreenManager.GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, theBlob.points.Count);
					pass.End();
				}
				effect.End();

			}

			//ScreenManager.GraphicsDevice.SetRenderTarget(0, normalDepthRenderTarget);

			//renderState.AlphaBlendEnable = false;
			//renderState.AlphaTestEnable = false;
			//renderState.DepthBufferEnable = true;

			//cartoonEffect.CurrentTechnique = cartoonEffect.Techniques["NormalDepth"];

			//cartoonEffect.Begin();
			//foreach (EffectPass pass in cartoonEffect.CurrentTechnique.Passes)
			//{
			//    pass.Begin();
			//    theBlob.DrawMe();
			//    pass.End();
			//}
			//cartoonEffect.End();

			//ScreenManager.GraphicsDevice.SetRenderTarget(0, sceneRenderTarget);

			//renderState.AlphaBlendEnable = false;
			//renderState.AlphaTestEnable = false;
			//renderState.DepthBufferEnable = true;

			//cartoonEffect.CurrentTechnique = cartoonEffect.Techniques["Toon"];

			//cartoonEffect.Begin();
			//foreach (EffectPass pass in cartoonEffect.CurrentTechnique.Passes)
			//{
			//    pass.Begin();
			//    theBlob.DrawMe();
			//    pass.End();
			//}
			//cartoonEffect.End();

			//currentArea.Display.ApplyPostProcessing(ScreenManager.GraphicsDevice);
			drawTime.Stop();



			if (DEBUG_MaxPhys == -1)
			{
				DEBUG_MaxPhys = (float)physicsTime.Elapsed.TotalMilliseconds;
			}
			else
			{
				DEBUG_MaxPhys = Math.Max(DEBUG_MaxPhys, (float)physicsTime.Elapsed.TotalMilliseconds);
			}
			if (DEBUG_MinPhys == -1)
			{
				DEBUG_MinPhys = (float)physicsTime.Elapsed.TotalMilliseconds;
			}
			else
			{
				DEBUG_MinPhys = Math.Min(DEBUG_MinPhys, (float)physicsTime.Elapsed.TotalMilliseconds);
			}
			if (DEBUG_MaxDraw == -1)
			{
				DEBUG_MaxDraw = (float)drawTime.Elapsed.TotalMilliseconds;
			}
			else
			{
				DEBUG_MaxDraw = Math.Max(DEBUG_MaxDraw, (float)drawTime.Elapsed.TotalMilliseconds);
			}
			if (DEBUG_MinDraw == -1)
			{
				DEBUG_MinDraw = (float)drawTime.Elapsed.TotalMilliseconds;
			}
			else
			{
				DEBUG_MinDraw = Math.Min(DEBUG_MinDraw, (float)drawTime.Elapsed.TotalMilliseconds);
			}


			// GUI
			ScreenManager.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
			spriteBatch.Begin();
			spriteBatch.DrawString(font, fps, Vector2.Zero, Color.White);
			spriteBatch.DrawString(font, "Phys: " + physicsTime.Elapsed.TotalMilliseconds, new Vector2(0, 30), Color.White);
			spriteBatch.DrawString(font, "Draw: " + drawTime.Elapsed.TotalMilliseconds, new Vector2(0, 60), Color.White);

			spriteBatch.DrawString(font, "Phys(Max): " + DEBUG_MaxPhys, new Vector2(0, 100), Color.White);
			spriteBatch.DrawString(font, "Draw(Max): " + DEBUG_MaxDraw, new Vector2(0, 130), Color.White);

			spriteBatch.DrawString(font, "Phys(Min): " + DEBUG_MinPhys, new Vector2(320, 100), Color.White);
			spriteBatch.DrawString(font, "Draw(Min): " + DEBUG_MinDraw, new Vector2(320, 130), Color.White);

			if (physics.Player != null)
			{
				if (physics.Player.Resilience.Target < 0.33)
				{
					spriteBatch.DrawString(font, "Soft", new Vector2(150, 0), Color.White);
				}
				else if (physics.Player.Resilience.Target > 0.66)
				{
					spriteBatch.DrawString(font, "Firm", new Vector2(150, 0), Color.White);
				}
				else
				{
					spriteBatch.DrawString(font, "Normal", new Vector2(150, 0), Color.White);
				}
				if (physics.Player.Traction.Target < 0.33)
				{
					spriteBatch.DrawString(font, "Slick", new Vector2(350, 0), Color.White);
				}
				else if (physics.Player.Traction.Target > 0.66)
				{
					spriteBatch.DrawString(font, "Sticky", new Vector2(350, 0), Color.White);
				}
				else
				{
					spriteBatch.DrawString(font, "Normal", new Vector2(350, 0), Color.White);
				}
			}

			if (paused)
			{
				spriteBatch.DrawString(font, "Paused", new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - font.MeasureString("Paused").X) * 0.5f, (ScreenManager.GraphicsDevice.Viewport.Height - font.MeasureString("Paused").Y) * 0.5f), Color.White);
			}
			//spriteBatch.DrawString(font, physics.DEBUG_BumpLoops.ToString(), new Vector2(550, 0), Color.White);
			spriteBatch.DrawString(font, "Vol: " + theBlob.getVolume().ToString(), new Vector2(345, 30), Color.White);
			spriteBatch.DrawString(font, "Next Vol: " + theBlob.getPotentialVolume().ToString(), new Vector2(250, 60), Color.White);
			//spriteBatch.DrawString(font, theBlob.getNewVolume().ToString(), new Vector2(675, 0), Color.White);
			spriteBatch.DrawString(font, "Collidables: " + physics.DEBUG_GetNumCollidables(), new Vector2(500, 0), Color.White);

			spriteBatch.DrawString(font, "Drawn: " + SceneManager.getSingleton.Drawn, new Vector2(600, 30), Color.White);

			spriteBatch.DrawString(font, "PWR: " + physics.PWR, new Vector2(0, 566), Color.White);
            spriteBatch.DrawString(font, "PM: " + physics.physicsMultiplier, new Vector2(300, 566), Color.White);

			spriteBatch.End();

			//fps
			++frames;

			//base.Draw(gameTime);
		}

		float DEBUG_MaxPhys = -1;
		float DEBUG_MinPhys = -1;
		float DEBUG_MaxDraw = -1;
		float DEBUG_MinDraw = -1;
	}
}
