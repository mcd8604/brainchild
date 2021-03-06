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

namespace PhysicsDemo5
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		//graphics stuff
		Effect effect;
		Matrix worldMatrix;
		//Matrix viewMatrix;
		//Matrix projectionMatrix;
		Texture2D text = null;
		SpriteFont font;


		bool paused = false;
		bool controllermode = false;
		bool follow = true;


		SoftCube playerCube;
		Vector3 cubeStartPosition = new Vector3(0, 10, 0);

		VertexBuffer cubeVertexBuffer;

		VertexDeclaration VertexDeclarationColor;
		VertexDeclaration VertexDeclarationTexture;

        //Camera
        Camera camera;

		static Vector3 defaultCameraPosition = new Vector3(0, 15, 10);
		//Vector3 cameraPosition = defaultCameraPosition;
		//Vector2 cameraAngle = new Vector2(1f, 0.4f);
		//float cameraLength = 20f;
		//float playerCamMulti = 0.1f;

		bool drawMode = true;
		int gameMode = 0;
        bool points = false;

		List<Drawable> drawables = new List<Drawable>();


		Vector4 lightPosition;


		//fps
		float time = 0f;
		float update = 1f;
		int frames = 0;
		string fps = "";


		bool OrientCamera = false;



		// physics - input
		float playerMoveMulti = 7.5f;



        World world;



		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.PreferMultiSampling = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
            initCamera();
			initGlobe();
			Physics.Physics.AirFriction = 0.5f;

			cubeVertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionNormalTexture.SizeInBytes * 16, BufferUsage.None);

			InputHandler.LoadDefaultBindings();

			base.Initialize();
		}

        private void initCamera()
        {
            camera = new Camera();

            camera.Position = defaultCameraPosition;
            camera.CameraAngle = new Vector2(1.0f, 0.4f);
            camera.CameraZoom = 20.0f;
            camera.CamMulti = 0.1f;
            camera.NearPlaneDistance = 1.0f;
            camera.FarPlaneDistance = 100.0f;
            camera.FieldOfView = 45.0f;
        }

		private void initCube()
		{

			cubeStartPosition = new Vector3(0, 10, 0);
			//playerCube = new DemoCube(cubeStartPosition, 1);
			//Physics.Physics.AddPoints(playerCube.points);
			//Physics.Physics.AddSprings(playerCube.springs);
            playerCube = new SoftCube(cubeStartPosition, 1);
            Physics.Physics.AddBody(playerCube);

			// 'world' 'cube'

            world = new World();

			StaticQuad temp = new StaticQuad(new Vector3(-5, 5, 5), new Vector3(5, 5, 5), new Vector3(5, 5, -5), new Vector3(-5, 5, -5), Color.Orange); // 'top'
            world.collidables.Add(temp);
			drawables.Add(temp);
			temp = new StaticQuad(new Vector3(-5, -5, 5), new Vector3(-5, -5, -5), new Vector3(5, -5, -5), new Vector3(5, -5, 5), Color.Blue);
            world.collidables.Add(temp);
			drawables.Add(temp);

			temp = new StaticQuad(new Vector3(-5, -5, 5), new Vector3(5, -5, 5), new Vector3(5, 5, 5), new Vector3(-5, 5, 5), Color.Red);
            world.collidables.Add(temp);
			drawables.Add(temp);
			temp = new StaticQuad(new Vector3(-5, -5, -5), new Vector3(-5, 5, -5), new Vector3(5, 5, -5), new Vector3(5, -5, -5), Color.Green);
            world.collidables.Add(temp);
			drawables.Add(temp);

			temp = new StaticQuad(new Vector3(5, -5, -5), new Vector3(5, 5, -5), new Vector3(5, 5, 5), new Vector3(5, -5, 5), Color.Yellow);
            world.collidables.Add(temp);
			drawables.Add(temp);
			temp = new StaticQuad(new Vector3(-5, -5, -5), new Vector3(-5, -5, 5), new Vector3(-5, 5, 5), new Vector3(-5, 5, -5), Color.Purple);
            world.collidables.Add(temp);
			drawables.Add(temp);


            Physics.Physics.AddBody(world);


			Physics.Physics.setGravityMode(true);


			lightPosition = new Vector4(5, 5, 5, 0);


		}


		private void initGlobe()
		{

			cubeStartPosition = new Vector3(0, 12, 0);
            //playerCube = new DemoCube(cubeStartPosition, 1);
            //Physics.Physics.AddPoints(playerCube.points);
            //Physics.Physics.AddSprings(playerCube.springs);
            playerCube = new SoftCube(cubeStartPosition, 1);
            Physics.Physics.AddBody(playerCube);

            world = new World();



			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 10, 0), new Vector3(-7, 7, 7), new Vector3(7, 7, 7), Color.Orange));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 10, 0), new Vector3(7, 7, 7), new Vector3(7, 7, -7), Color.Orange));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 10, 0), new Vector3(7, 7, -7), new Vector3(-7, 7, -7), Color.Orange));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 10, 0), new Vector3(-7, 7, -7), new Vector3(-7, 7, 7), Color.Orange));


			addToPhysicsAndDraw(new StaticTri(new Vector3(0, -10, 0), new Vector3(-7, -7, 7), new Vector3(-7, -7, -7), Color.Blue));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, -10, 0), new Vector3(-7, -7, -7), new Vector3(7, -7, -7), Color.Blue));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, -10, 0), new Vector3(7, -7, -7), new Vector3(7, -7, 7), Color.Blue));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, -10, 0), new Vector3(7, -7, 7), new Vector3(-7, -7, 7), Color.Blue));


			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 0, 10), new Vector3(-7, -7, 7), new Vector3(7, -7, 7), Color.Red));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 0, 10), new Vector3(7, -7, 7), new Vector3(7, 7, 7), Color.Red));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 0, 10), new Vector3(7, 7, 7), new Vector3(-7, 7, 7), Color.Red));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 0, 10), new Vector3(-7, 7, 7), new Vector3(-7, -7, 7), Color.Red));


			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 0, -10), new Vector3(-7, -7, -7), new Vector3(-7, 7, -7), Color.Green));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 0, -10), new Vector3(-7, 7, -7), new Vector3(7, 7, -7), Color.Green));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 0, -10), new Vector3(7, 7, -7), new Vector3(7, -7, -7), Color.Green));
			addToPhysicsAndDraw(new StaticTri(new Vector3(0, 0, -10), new Vector3(7, -7, -7), new Vector3(-7, -7, -7), Color.Green));


			addToPhysicsAndDraw(new StaticTri(new Vector3(10, 0, 0), new Vector3(7, -7, -7), new Vector3(7, 7, -7), Color.Yellow));
			addToPhysicsAndDraw(new StaticTri(new Vector3(10, 0, 0), new Vector3(7, 7, -7), new Vector3(7, 7, 7), Color.Yellow));
			addToPhysicsAndDraw(new StaticTri(new Vector3(10, 0, 0), new Vector3(7, 7, 7), new Vector3(7, -7, 7), Color.Yellow));
			addToPhysicsAndDraw(new StaticTri(new Vector3(10, 0, 0), new Vector3(7, -7, 7), new Vector3(7, -7, -7), Color.Yellow));


			addToPhysicsAndDraw(new StaticTri(new Vector3(-10, 0, 0), new Vector3(-7, -7, -7), new Vector3(-7, -7, 7), Color.Purple));
			addToPhysicsAndDraw(new StaticTri(new Vector3(-10, 0, 0), new Vector3(-7, -7, 7), new Vector3(-7, 7, 7), Color.Purple));
			addToPhysicsAndDraw(new StaticTri(new Vector3(-10, 0, 0), new Vector3(-7, 7, 7), new Vector3(-7, 7, -7), Color.Purple));
			addToPhysicsAndDraw(new StaticTri(new Vector3(-10, 0, 0), new Vector3(-7, 7, -7), new Vector3(-7, -7, -7), Color.Purple));


            Physics.Physics.AddBody(world);

			Physics.Physics.setGravityMode(true);

			lightPosition = new Vector4(7, 7, 7, 0);
		}


		private void initThree()
		{

			cubeStartPosition = new Vector3(2, 12, -2);

            //playerCube = new DemoCube(cubeStartPosition, 1);
            //Physics.Physics.AddPoints(playerCube.points);
            //Physics.Physics.AddSprings(playerCube.springs);
            playerCube = new SoftCube(cubeStartPosition, 1);
            Physics.Physics.AddBody(playerCube);

            SoftCube test = new SoftCube(new Vector3(0, 5, 0), 1);
            Physics.Physics.AddBody(test);
            drawables.AddRange(test.getDrawables());

            world = new World();


			addToPhysicsAndDraw(new StaticTri(new Vector3(-5, 0, -5), new Vector3(-5, 0, 5), new Vector3(5, 0, 5), Color.Red));
			addToPhysicsAndDraw(new StaticTri(new Vector3(5, 0, 5), new Vector3(5, 8, -5), new Vector3(-5, 0, -5), Color.Blue)); //
			addToPhysicsAndDraw(new StaticTri(new Vector3(-5, 0, -5), new Vector3(-15, 0, 5), new Vector3(-5, 0, 5), Color.Yellow));
			addToPhysicsAndDraw(new StaticTri(new Vector3(-15, 0, 5), new Vector3(-5, 0, 15), new Vector3(-5, 0, 5), Color.White));
			addToPhysicsAndDraw(new StaticTri(new Vector3(-5, 0, 5), new Vector3(-5, 0, 15), new Vector3(5, 0, 5), Color.Orange));
			addToPhysicsAndDraw(new StaticTri(new Vector3(-15, 8, 15), new Vector3(-5, 0, 15), new Vector3(-15, 0, 5), Color.Purple));
			addToPhysicsAndDraw(new StaticTri(new Vector3(-5, 0, -5), new Vector3(-15, 0, -5), new Vector3(-15, 0, 5), Color.Yellow));
			addToPhysicsAndDraw(new StaticTri(new Vector3(-5, 0, 15), new Vector3(5, 0, 15), new Vector3(5, 0, 5), Color.Orange));


            Physics.Physics.AddBody(world);


			Physics.Physics.setGravityMode(false);


			lightPosition = new Vector4(-5, 5, 5, 0);

		}


		private void addToPhysicsAndDraw(T t)
		{

            world.collidables.Add(t);
			drawables.Add(t);

		}


		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			text = Content.Load<Texture2D>(@"test");
			font = Content.Load<SpriteFont>(@"Courier New");

			GraphicsDevice.RenderState.PointSize = 5;

			// graphics stuff?
			InitializeEffect();
		}


		/// <summary>
		/// Initializes the basic effect (parameter setting and technique selection)
		/// used for the 3D model.
		/// </summary>
		private void InitializeEffect()
		{
			worldMatrix = Matrix.Identity;

            //viewMatrix = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 4, 0), Vector3.Up);
            camera.View = Matrix.CreateLookAt(camera.Position, new Vector3(0, 4, 0), Vector3.Up);

            //projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            //    MathHelper.ToRadians(45),  // 45 degree angle
            //    (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height,
            //    1.0f, 100.0f);
            camera.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(camera.FieldOfView),  // 45 degree angle
                (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height,
                camera.NearPlaneDistance, camera.FarPlaneDistance);

			VertexDeclarationTexture = new VertexDeclaration(GraphicsDevice, VertexPositionNormalTexture.VertexElements);

			VertexDeclarationColor = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

			effect = Content.Load<Effect>("effects");

            //effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xView"].SetValue(camera.View);
            //effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xProjection"].SetValue(camera.Projection);
			effect.Parameters["xWorld"].SetValue(worldMatrix);

			effect.Parameters["xTexture"].SetValue(text);
			effect.Parameters["xEnableLighting"].SetValue(true);
			//effect.Parameters["xShowNormals"].SetValue(true);
			effect.Parameters["xLightDirection"].SetValue(Vector3.Down);
			effect.Parameters["xLightPos"].SetValue(new Vector4(5, 5, 5, 0));
			effect.Parameters["xAmbient"].SetValue(0.25f);

            //effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
            effect.Parameters["xCameraPos"].SetValue(new Vector4(camera.Position.X, camera.Position.Y, camera.Position.Z, 0));
		}


		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
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
			if (InputHandler.IsActionPressed(Actions.Reset))
			{
				if (gameMode == 2)
				{
					Physics.Physics.Clear();
					drawables.Clear();
					initCube();
				}
				else if (gameMode == 1)
				{
					Physics.Physics.Clear();
					drawables.Clear();
					initThree();
				}
				else if (gameMode == 0)
				{
					Physics.Physics.Clear();
					drawables.Clear();
					initGlobe();
				}
			}
			if (InputHandler.IsKeyPressed(Keys.P))
			{
				paused = !paused;
			}
			if (InputHandler.IsKeyPressed(Keys.T))
			{
				graphics.ToggleFullScreen();
			}
			if (InputHandler.IsKeyPressed(Keys.F))
			{
				follow = !follow;
				//camera follow
				if (!follow)
				{
                    //cameraPosition = defaultCameraPosition;
                    camera.Position = defaultCameraPosition;
                    //viewMatrix = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 4, 0), Vector3.Up);
                    camera.View = Matrix.CreateLookAt(camera.Position, new Vector3(0, 4, 0), Vector3.Up);
                    //effect.Parameters["xView"].SetValue(viewMatrix);
                    effect.Parameters["xView"].SetValue(camera.View);

                    //effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
                    effect.Parameters["xCameraPos"].SetValue(new Vector4(camera.Position.X, camera.Position.Y, camera.Position.Z, 0));
				}
			}
			if (InputHandler.IsKeyPressed(Keys.S))
			{
				playerCube.setSpringForce(92.5f);
			}
			if (InputHandler.IsKeyPressed(Keys.D))
			{
				playerCube.setSpringForce(62.5f);
			}
			if (InputHandler.IsKeyPressed(Keys.W))
			{
				playerCube.setSpringForce(12.5f);
			}
			if (InputHandler.IsKeyPressed(Keys.Q))
			{
				Physics.Physics.TEMP_SurfaceFriction = 48f;
			}
			if (InputHandler.IsKeyPressed(Keys.E))
			{
				Physics.Physics.TEMP_SurfaceFriction = 0f;
			}
			if (InputHandler.IsKeyPressed(Keys.A))
			{
				Physics.Physics.TEMP_SurfaceFriction = 12f;
			}
			if (InputHandler.IsKeyPressed(Keys.Z))
			{
				drawMode = !drawMode;
			}
			if (InputHandler.IsKeyPressed(Keys.O))
			{
				OrientCamera = !OrientCamera;
			}
			if (InputHandler.IsKeyPressed(Keys.M))
			{
				gameMode = (gameMode + 1) % 3;
				if (gameMode == 2)
				{
					Physics.Physics.Clear();
					drawables.Clear();
					initCube();
				}
				else if (gameMode == 1)
				{
					Physics.Physics.Clear();
					drawables.Clear();
					initThree();
				}
				else if (gameMode == 0)
				{
					Physics.Physics.Clear();
					drawables.Clear();
					initGlobe();
				}
			}
            if (InputHandler.IsKeyPressed(Keys.OemPeriod))
            {
                points = !points;
            }

			// Xbox
			if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
			{
				controllermode = true;
			}
			if (controllermode)
			{
				playerCube.setSpringForce(12.5f + (GamePad.GetState(PlayerIndex.One).Triggers.Right * 80f));
				Physics.Physics.TEMP_SurfaceFriction = GamePad.GetState(PlayerIndex.One).Triggers.Left * 24f;
				float vb = MathHelper.Clamp(Physics.Physics.ImpactThisFrame - 0.1f, 0f, 1f);
				InputHandler.SetVibration(vb, 0f);
			}

			// Quick Torque
			Vector2 move = InputHandler.GetAnalogAction(AnalogActions.Movement);
			if (move != Vector2.Zero)
			{
				Vector3 Up;
				if (OrientCamera)
				{
					Up = Physics.Physics.getUp(playerCube.getCenter());
				}
				else
				{
					Up = Vector3.Up;
				}
                //Vector3 Horizontal = Vector3.Normalize(Vector3.Cross(playerCube.getCenter() - cameraPosition, Up));
                Vector3 Horizontal = Vector3.Normalize(Vector3.Cross(playerCube.getCenter() - camera.Position, Up));
				Vector3 Run = Vector3.Normalize(Vector3.Cross(Horizontal, Up));
				foreach (Physics.Point p in playerCube.points)
				{
					p.Force += Vector3.Normalize(Vector3.Cross(p.Position - playerCube.getCenter(), Horizontal)) * (move.Y * playerMoveMulti);
					p.Force += Vector3.Normalize(Vector3.Cross(p.Position - playerCube.getCenter(), Run)) * (move.X * playerMoveMulti);
				}
			}




			if (!paused)
			{
				Physics.Physics.update((float)gameTime.ElapsedGameTime.TotalSeconds);
			}

			if (follow)
			{
                camera.CameraAngle += InputHandler.GetAnalogAction(AnalogActions.Camera) * camera.CamMulti;
                if (camera.CameraAngleX < -MathHelper.Pi)
                {
                    camera.CameraAngleX += MathHelper.TwoPi;
                }
                else if (camera.CameraAngleX > MathHelper.Pi)
                {
                    camera.CameraAngleX -= MathHelper.TwoPi;
                }

                camera.CameraAngle = Vector2.Clamp(camera.CameraAngle, new Vector2(-MathHelper.TwoPi, -MathHelper.PiOver2), new Vector2(MathHelper.TwoPi, MathHelper.PiOver2));

                // following camera
                camera.CameraZoom = MathHelper.Clamp(camera.CameraZoom + (InputHandler.getMouseWheelDelta() * -0.01f), 10, 40);
                Vector3 Offset = new Vector3((float)Math.Cos(camera.CameraAngle.X) * camera.CameraZoom, (float)Math.Sin(camera.CameraAngle.Y) * camera.CameraZoom, (float)Math.Sin(camera.CameraAngle.X) * camera.CameraZoom);
                camera.Position = playerCube.getCenter() + Offset;

                /*
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
                Vector3 Offset = new Vector3((float)Math.Cos(cameraAngle.X) * cameraLength, (float)Math.Sin(cameraAngle.Y) * cameraLength, (float)Math.Sin(cameraAngle.X) * cameraLength);
                cameraPosition = playerCube.getCenter() + Offset;
                */
 

				// new Vector3(10, 10, 20)
				if (OrientCamera)
				{
                    //viewMatrix = Matrix.CreateLookAt(cameraPosition, playerCube.getCenter(), Physics.Physics.getUp(playerCube.getCenter()));
                    camera.View = Matrix.CreateLookAt(camera.Position, playerCube.getCenter(), Physics.Physics.getUp(playerCube.getCenter()));
				}
				else
				{
                    //viewMatrix = Matrix.CreateLookAt(cameraPosition, playerCube.getCenter(), Vector3.Up);
                    camera.View = Matrix.CreateLookAt(camera.Position, playerCube.getCenter(), Vector3.Up);
				}
                //effect.Parameters["xView"].SetValue(viewMatrix);
                effect.Parameters["xView"].SetValue(camera.View);

				//effect.Parameters["xLightPos"].SetValue(new Vector4(cameraPosition.X * 0.5f, cameraPosition.Y * 0.5f, cameraPosition.Z * 0.5f, 0));
				
                //effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
                effect.Parameters["xCameraPos"].SetValue(new Vector4(camera.Position.X, camera.Position.Y, camera.Position.Z, 0));
			}

			cubeVertexBuffer.SetData<VertexPositionNormalTexture>(playerCube.getTriangleVertexes());



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

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			if (drawMode)
			{
				GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
				GraphicsDevice.RenderState.FillMode = FillMode.Solid;
				GraphicsDevice.RenderState.DepthBufferEnable = true;
			}
			else
			{
				GraphicsDevice.RenderState.CullMode = CullMode.None;
				GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
				GraphicsDevice.RenderState.DepthBufferEnable = false;
			}
			GraphicsDevice.RenderState.AlphaBlendEnable = false;
			GraphicsDevice.RenderState.AlphaTestEnable = false;

			// Collision Tris
			effect.CurrentTechnique = effect.Techniques["Colored"];
			GraphicsDevice.VertexDeclaration = VertexDeclarationColor;
			foreach (Drawable d in drawables)
			{
				VertexPositionColor[] temp = d.getTriangleVertexes();
				VertexBuffer tempVertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * temp.Length, BufferUsage.None);
				tempVertexBuffer.SetData<VertexPositionColor>(temp);
				GraphicsDevice.Vertices[0].SetSource(tempVertexBuffer, 0, VertexPositionColor.SizeInBytes);
				effect.Begin();
				foreach (EffectPass pass in effect.CurrentTechnique.Passes)
				{
					pass.Begin();
					d.DrawMe(GraphicsDevice);
					pass.End();
				}
				effect.End();
			}

			// Box
			effect.CurrentTechnique = effect.Techniques["Textured"];
			GraphicsDevice.VertexDeclaration = VertexDeclarationTexture;
			cubeVertexBuffer.SetData<VertexPositionNormalTexture>(playerCube.getTriangleVertexes());
			GraphicsDevice.Vertices[0].SetSource(cubeVertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
			effect.Begin();
			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Begin();
				playerCube.DrawMe(GraphicsDevice);
				pass.End();
			}
			effect.End();

            if (points)
            {
                // Corner Dots -
                effect.CurrentTechnique = effect.Techniques["Colored"];
                GraphicsDevice.VertexDeclaration = VertexDeclarationColor;
                GraphicsDevice.RenderState.DepthBufferEnable = false;
                VertexPositionColor[] dotVertices = new VertexPositionColor[8];
                for (int i = 0; i < 8; ++i)
                {
                    dotVertices[i] = new VertexPositionColor(playerCube.points[i].Position, Color.Black);
                }
                VertexBuffer dotvertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * 8, BufferUsage.None);
                dotvertexBuffer.SetData<VertexPositionColor>(dotVertices);
                GraphicsDevice.Vertices[0].SetSource(dotvertexBuffer, 0, VertexPositionColor.SizeInBytes);
                effect.Begin();
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, 8);
                    pass.End();
                }
                effect.End();
            }


			// GUI
			GraphicsDevice.RenderState.FillMode = FillMode.Solid;
			spriteBatch.Begin();
			spriteBatch.DrawString(font, fps, Vector2.Zero, Color.White);
			if (SoftCube.springVal < 40)
			{
				spriteBatch.DrawString(font, "Soft", new Vector2(150, 0), Color.White);
			}
			else if (SoftCube.springVal > 70)
			{
				spriteBatch.DrawString(font, "Firm", new Vector2(150, 0), Color.White);
			}
			else
			{
				spriteBatch.DrawString(font, "Normal", new Vector2(150, 0), Color.White);
			}
			if (Physics.Physics.TEMP_SurfaceFriction < 8)
			{
				spriteBatch.DrawString(font, "Slick", new Vector2(350, 0), Color.White);
			}
			else if (Physics.Physics.TEMP_SurfaceFriction > 16)
			{
				spriteBatch.DrawString(font, "Sticky", new Vector2(350, 0), Color.White);
			}
			else
			{
				spriteBatch.DrawString(font, "Normal", new Vector2(350, 0), Color.White);
			}
			if (paused)
			{
				spriteBatch.DrawString(font, "Paused", new Vector2((GraphicsDevice.Viewport.Width - font.MeasureString("Paused").X) * 0.5f, (GraphicsDevice.Viewport.Height - font.MeasureString("Paused").Y) * 0.5f), Color.White);
			}
			spriteBatch.End();


			//fps
			++frames;

			base.Draw(gameTime);
		}
	}
}
