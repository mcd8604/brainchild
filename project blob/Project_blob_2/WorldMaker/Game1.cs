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
using Project_blob;
using Engine;

namespace WorldMaker
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		ContentManager content;

		public bool draw = false;

		Vector3 lightPos = new Vector3(0, 20, 0);

		private Drawable _activeDrawable;
		public Drawable ActiveDrawable
		{
			get { return _activeDrawable; }
			set
			{
				_activeDrawable = value;
				modelEditor.UpdateValues();
			}
		}

		Area _activeArea;
		public Area ActiveArea
		{
			get { return _activeArea; }
			set { _activeArea = value; }
		}

		public Area nextArea = null;

		public readonly string EFFECT_TYPE = "CartoonEffect";

		private string _effectName;
		public string EffectName
		{
			get { return _effectName; }
			set { _effectName = value; }
		}

		//static Matrix tempView, tempProj;

		//Effect celshader;
		Matrix worldMatrix;
		Matrix viewMatrix;
		Matrix projectionMatrix;
		public Matrix WorldMatrix
		{
			get { return worldMatrix; }
		}
		public Matrix ViewMatrix
		{
			get { return viewMatrix; }
			set { viewMatrix = value; }
		}
		public Matrix ProjectionMatrix
		{
			get { return projectionMatrix; }
		}
		//SpriteFont font;

		const string POINT_TEXT = "point_text";

		//VertexDeclaration VertexDeclarationColor;
		VertexDeclaration VertexDeclarationTexture;

		public static bool cinema = false;
		public static bool follow = true;
		public Vector3 focusPoint = new Vector3(0, 0, 0);
		Vector3 Up = Vector3.Up;
		Vector3 Horizontal = new Vector3();
		Vector3 RunVector = new Vector3();

		static Vector3 defaultCameraPosition = new Vector3(0, 15, 10);
		Vector3 cameraPosition = defaultCameraPosition;
		Vector2 cameraAngle = new Vector2(1f, 0.4f);
		float cameraLength = 20f;
		float playerCamMulti = 0.1f;

		static ModelEditor modelEditor;
		static LevelEditor levelEditor;

		public Game1()
		{

			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			content = new ContentManager(Services);
		}

		protected override void Dispose(bool disposing)
		{
			if (!modelEditor.IsDisposed)
			{
				modelEditor.Invoke(new ModelEditor.Callback(modelEditor.Close));
			}
			if (!levelEditor.IsDisposed)
			{
				levelEditor.Invoke(new LevelEditor.Callback(levelEditor.Close));
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			_effectName = EFFECT_TYPE;
			spriteBatch = new SpriteBatch(GraphicsDevice);

			GraphicsDevice.RenderState.PointSize = 5;

			InputHandler.LoadDefaultBindings();

			//gui
			System.Windows.Forms.Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);

			modelEditor = new ModelEditor(this);
			new System.Threading.Thread(delegate()
			{
				System.Windows.Forms.Application.Run(modelEditor);
			}).Start();

			levelEditor = new LevelEditor(this);
			new System.Threading.Thread(delegate()
			{
				System.Windows.Forms.Application.Run(levelEditor);
			}).Start();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{


			if (EFFECT_TYPE != "basic")
				EffectManager.getSingleton.AddEffect(EFFECT_TYPE, Content.Load<Effect>(@"Shaders\\" + EFFECT_TYPE));
			else
				EffectManager.getSingleton.AddEffect(EFFECT_TYPE, new BasicEffect(GraphicsDevice, null));

			EffectManager.getSingleton.AddEffect("cartoonEffect", Content.Load<Effect>(@"Shaders\\CartoonEffect"));
			EffectManager.getSingleton.AddEffect("postprocessEffect", Content.Load<Effect>(@"Shaders\\PostprocessEffect"));
			EffectManager.getSingleton.AddEffect("distort", Content.Load<Effect>(@"Shaders\\Distort"));
			EffectManager.getSingleton.AddEffect("distorter", Content.Load<Effect>(@"Shaders\\Distorters"));

			//TextureManager.getSingleton.AddTexture("grass", Content.Load<Texture2D>(@"Models\\free-grass-texture"));
			//TextureManager.getSingleton.AddTexture("test", Content.Load<Texture2D>(@"Textures\\test"));
			//TextureManager.getSingleton.AddTexture("point_text", Content.Load<Texture2D>(@"Textures\\point_text"));

			if (System.IO.Directory.Exists(@"Content\Textures"))
			{
				string[] texturePaths = System.IO.Directory.GetFiles(@"Content\Textures");
				foreach (string s in texturePaths)
				{
					string textureFile = s.Substring(s.LastIndexOf("\\") + 1);
					string textureName = textureFile.Remove(textureFile.LastIndexOf('.'));
					//TextureManager.getSingleton.AddTexture(textureName, content.Load<Texture2D>(@"Content\\Textures\\" + textureName));
                    Texture2D t = content.Load<Texture2D>(@"Content\\Textures\\" + textureName);
                    t.Name = textureName;
                    TextureManager.AddTexture(t);
				}
			}

			//ModelManager.getSingleton.AddModel("cube", content.Load<Model>(System.Environment.CurrentDirectory + "/Content/Models/cube"));
			//ModelManager.getSingleton.AddModel("ball", content.Load<Model>(System.Environment.CurrentDirectory + "/Content/Models/ball"));
			//ModelManager.getSingleton.AddModel("ground", content.Load<Model>(System.Environment.CurrentDirectory + "/Content/Models/ground"));

			if (System.IO.Directory.Exists(@"Content\Models"))
			{
				string[] modelPaths = System.IO.Directory.GetFiles(@"Content\Models");
				foreach (string s in modelPaths)
				{
					string modelFile = s.Substring(s.LastIndexOf("\\") + 1);
					if (modelFile.EndsWith(".xnb"))
					{
						string modelName = modelFile.Remove(modelFile.LastIndexOf('.'));
						ModelManager.getSingleton.AddModel(modelName, content.Load<Model>(@"Content\\Models\\" + modelName));
					}
				}
			}
			graphics.GraphicsDevice.VertexDeclaration = new VertexDeclaration(
				graphics.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

			worldMatrix = Matrix.Identity;

			viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero,
				Vector3.Up);

			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.ToRadians(45),  // 45 degree angle
				(float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height,
				1.0f, 1000.0f);

			if (EFFECT_TYPE == "basic")
			{
				Level.AddArea("testArea", new Area(worldMatrix, viewMatrix, projectionMatrix));
			}
			else if (EFFECT_TYPE == "effects")
			{
				EffectManager.getSingleton.GetEffect(_effectName).Parameters["xView"].SetValue(viewMatrix);
				EffectManager.getSingleton.GetEffect(_effectName).Parameters["xProjection"].SetValue(projectionMatrix);
				EffectManager.getSingleton.GetEffect(_effectName).Parameters["xWorld"].SetValue(worldMatrix);

				EffectManager.getSingleton.GetEffect(_effectName).Parameters["xEnableLighting"].SetValue(true);

				EffectManager.getSingleton.GetEffect(_effectName).Parameters["xLightPos"].SetValue(new Vector4(5, 5, 5, 0));
				EffectManager.getSingleton.GetEffect(_effectName).Parameters["xAmbient"].SetValue(0.5f);

				Level.AddArea("testArea", new Area(worldMatrix, _effectName, "xWorld", "xTexture", "Textured"));
			}
			else if (EFFECT_TYPE == "Cel")
			{
				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["World"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["World"].SetValue(worldMatrix);

				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["Projection"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["Projection"].SetValue(projectionMatrix);

				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["DiffuseLightColor"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["DiffuseLightColor"].SetValue(new Vector4(0.75f, 0.75f, 0.75f, 1.0f));

				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LightPosition"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["LightPosition"].SetValue(lightPos);

				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneSharp"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneSharp"].SetValue(.9f);

				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneRough"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneRough"].SetValue(0.15f);

				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneContrib"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneContrib"].SetValue(0.08f);

				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoSharp"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoSharp"].SetValue(0.05f);

				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoRough"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoRough"].SetValue(2.0f);

				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoContrib"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoContrib"].SetValue(0.4f);

				if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["EdgeOffset"] != null)
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["EdgeOffset"].SetValue(0.03f);

				Level.AddArea("testArea", new Area(worldMatrix, _effectName, "World", "NONE", null));
			}
			else if (EFFECT_TYPE == "CartoonEffect")
			{
				EffectManager.getSingleton.GetEffect(_effectName).Parameters["View"].SetValue(viewMatrix);
				EffectManager.getSingleton.GetEffect(_effectName).Parameters["Projection"].SetValue(projectionMatrix);
				EffectManager.getSingleton.GetEffect(_effectName).Parameters["World"].SetValue(worldMatrix);
				EffectManager.getSingleton.GetEffect(_effectName).Parameters["TextureEnabled"].SetValue(true);

				Level.AddArea("testArea", new Area(worldMatrix, _effectName, "World", "NONE", null));
			}

			CinematicCamera camera = new CinematicCamera();
			camera.FieldOfView = MathHelper.ToRadians(45.0f);
			camera.AspectRatio = (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height;
			camera.NearPlane = 1.0f;
			camera.FarPlane = 1000.0f;

			camera.Position = new Vector3(0, 0, -10);
			camera.Target = Vector3.Zero;
			camera.Up = Vector3.Up;

			CameraManager.getSingleton.AddCamera("cinematic", camera);
			CameraManager.getSingleton.SetActiveCamera("cinematic");

			_activeArea = Level.Areas["testArea"];
			_activeArea.LoadAreaWorldMaker(this);
			//effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
			VertexDeclarationTexture = new VertexDeclaration(GraphicsDevice, VertexPositionNormalTexture.VertexElements);

			_activeArea.Display.createRenderTargets(GraphicsDevice);
		}

        //public void CreateRenderTargets()
        //{
        //    PresentationParameters pp = GraphicsDevice.PresentationParameters;

        //    RenderTarget2D sceneRenderTarget = new RenderTarget2D(GraphicsDevice,
        //        pp.BackBufferWidth, pp.BackBufferHeight, 1,
        //        pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

        //    RenderTarget2D normalDepthRenderTarget = new RenderTarget2D(GraphicsDevice,
        //        pp.BackBufferWidth, pp.BackBufferHeight, 1,
        //        pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

        //    RenderTarget2D distortionMap = new RenderTarget2D(GraphicsDevice,
        //        pp.BackBufferWidth, pp.BackBufferHeight, 1,
        //        pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

        //    ResolveTexture2D tempRenderTarget = new ResolveTexture2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1,
        //        pp.BackBufferFormat);

        //    RenderTarget2D depthBufferRenderTarget = new RenderTarget2D(GraphicsDevice,
        //        pp.BackBufferWidth, pp.BackBufferHeight, 1,
        //        pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

        //    _activeArea.Display.SceneRanderTarget = sceneRenderTarget;
        //    _activeArea.Display.NormalDepthRenderTarget = normalDepthRenderTarget;
        //    _activeArea.Display.DistortionMap = distortionMap;
        //    _activeArea.Display.TempRenderTarget = tempRenderTarget;
        //    _activeArea.Display.DepthMapRenderTarget = depthBufferRenderTarget;

        //    _activeArea.Display.Distort = Content.Load<Effect>(@"Shaders\\Distort");
        //    _activeArea.Display.Distorter = Content.Load<Effect>(@"Shaders\\Distorters");
        //    _activeArea.Display.CartoonEffect = Content.Load<Effect>(@"Shaders\\CartoonEffect");
        //    _activeArea.Display.PostProcessEffect = Content.Load<Effect>(@"Shaders\\PostprocessEffect");
        //}

		public static void SetUpCinematicCamera(List<CameraFrame> cameraFrames)
		{
			CinematicCamera cinematicCamera = (CinematicCamera)CameraManager.getSingleton.GetCamera("cinematic");
			cinematicCamera.Frames = cameraFrames;
			cinematicCamera.Running = true;
			CameraManager.getSingleton.SetActiveCamera("cinematic");
			cinema = true;
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			content.Unload();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			InputHandler.Update();
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputHandler.IsKeyPressed(Keys.Escape))
			{
				this.Exit();
			}
			Horizontal = Vector3.Normalize(Vector3.Cross(focusPoint - cameraPosition, Up));
			RunVector = Vector3.Normalize(Vector3.Cross(Horizontal, Up));

			if (InputHandler.IsKeyPressed(Keys.A))
			{
				//strif to the left
				this.focusPoint += Horizontal;
			}
			if (InputHandler.IsKeyPressed(Keys.D))
			{
				//strif to the right
				this.focusPoint -= Horizontal;
			}
			if (InputHandler.IsKeyPressed(Keys.W))
			{
				//move foward
				this.focusPoint += RunVector;
			}
			if (InputHandler.IsKeyPressed(Keys.S))
			{
				//move backwards
				this.focusPoint -= RunVector;
			}

			if (follow)
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
				cameraLength = MathHelper.Clamp(cameraLength + (InputHandler.getMouseWheelDelta() * -0.1f), 10, 500);
				Vector3 Offset = new Vector3((float)Math.Cos(cameraAngle.X) * cameraLength, (float)Math.Sin(cameraAngle.Y) * cameraLength, (float)Math.Sin(cameraAngle.X) * cameraLength);
				cameraPosition = focusPoint + Offset;

				viewMatrix = Matrix.CreateLookAt(cameraPosition, focusPoint, Vector3.Up);
				try
				{
					EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName).Parameters["View"].SetValue(viewMatrix);
				}
				catch (Exception e) {
					Log.Out.WriteLine(e);
				}

				if (EFFECT_TYPE == "effects")
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
				else if (EFFECT_TYPE == "Cel")
					EffectManager.getSingleton.GetEffect(_effectName).Parameters["EyePosition"].SetValue(new Vector3(cameraPosition.X, cameraPosition.Y, cameraPosition.Z));
			}
			else
			{
				if (cinema)
				{
					CameraManager.getSingleton.Update(gameTime);
					if (EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName) is BasicEffect)
					{
						((BasicEffect)EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName)).View = CameraManager.getSingleton.ActiveCamera.View;
					}
					else
					{
						if (EFFECT_TYPE == "effects")
							EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName).Parameters["xView"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
						else //if (EFFECT_TYPE == "Cel")
							EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName).Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
					}

					if (EFFECT_TYPE == "effects")
						EffectManager.getSingleton.GetEffect(_effectName).Parameters["xCameraPos"].SetValue(new Vector4(CameraManager.getSingleton.ActiveCamera.Position, 0));
					else if (EFFECT_TYPE == "Cel")
						EffectManager.getSingleton.GetEffect(_effectName).Parameters["EyePosition"].SetValue(CameraManager.getSingleton.ActiveCamera.Position);
					if (((CinematicCamera)CameraManager.getSingleton.ActiveCamera).FinishedCinematics)
					{
						cinema = false;
						((CinematicCamera)CameraManager.getSingleton.ActiveCamera).Position = new Vector3(0, 0, -10);
						((CinematicCamera)CameraManager.getSingleton.ActiveCamera).Target = Vector3.Zero;
						((CinematicCamera)CameraManager.getSingleton.ActiveCamera).Up = Vector3.Up;
						((CinematicCamera)CameraManager.getSingleton.ActiveCamera).FinishedCinematics = false;
					}
				}
				else
				{
					//if (CameraPanSetter.ViewMatrix != null)
					//{
					//    viewMatrix = CameraPanSetter.ViewMatrix;
					//    if (EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName) is BasicEffect)
					//    {
					//        ((BasicEffect)EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName)).View = viewMatrix;
					//    }
					//    else
					//    {
					//        if (EFFECT_TYPE == "effects")
					//            EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName).Parameters["xView"].SetValue(viewMatrix);
					//        else //if (EFFECT_TYPE == "Cel")
					//            EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName).Parameters["View"].SetValue(viewMatrix);
					//    }

					//    if (EFFECT_TYPE == "effects")
					//        EffectManager.getSingleton.GetEffect(_effectName).Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
					//    else if (EFFECT_TYPE == "Cel")
					//        EffectManager.getSingleton.GetEffect(_effectName).Parameters["EyePosition"].SetValue(new Vector3(cameraPosition.X, cameraPosition.Y, cameraPosition.Z));
					//}
				}
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			if (nextArea != null)
			{
				_activeArea = nextArea;
				_activeArea.LoadAreaWorldMaker(this);
				Level.CurrentArea = _activeArea;
				nextArea = null;
				levelEditor.Invoke(new LevelEditor.Callback(levelEditor.UpdateLists));
			} 
			else 
			//if (draw)
			{
				foreach (string str in LevelEditor.DrawablesToDelete)
				{
					_activeArea.RemoveDrawable(str);
					//_activeArea.RemoveEvent(str);
				}
				LevelEditor.DrawablesToDelete.Clear();

				foreach (DrawableInfo drawableInfo in LevelEditor.DrawablesToAdd)
				{
                    drawableInfo.drawable.Drawn = true;
					_activeArea.AddDrawable(drawableInfo.name, drawableInfo.textureID, drawableInfo.drawable);
				}
				LevelEditor.DrawablesToAdd.Clear();

				/*foreach (EventInfo eventInfo in LevelEditor.EventsToAdd)
				{
					_activeArea.AddEvent(eventInfo.name, eventInfo.eventTrigger);
				}
				LevelEditor.EventsToAdd.Clear();*/

				graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
				if (_activeArea.Display.CartoonEffect != null)
				{
					_activeArea.Display.Draw(graphics.GraphicsDevice);
				}

				base.Draw(gameTime);
			}
		}
	}
}
