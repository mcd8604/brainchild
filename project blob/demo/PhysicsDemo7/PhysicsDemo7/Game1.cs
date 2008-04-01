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

namespace PhysicsDemo7
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
        Matrix viewMatrix;
        Matrix projectionMatrix;
        Texture2D text = null;
        SpriteFont font;

        bool paused = false;
        bool controllermode = false;
        bool follow = true;

        DemoCube playerCube;
        Vector3 cubeStartPosition = new Vector3(0, 10, 0);

        VertexDeclaration VertexDeclarationColor;
        VertexDeclaration VertexDeclarationTexture;

        static Vector3 defaultCameraPosition = new Vector3(0, 15, 10);
        Vector3 cameraPosition = defaultCameraPosition;
        Vector2 cameraAngle = new Vector2(1f, 0.4f);
        float cameraLengthMulti = 1f;
        float cameraLength = 20f;
        float playerCamMulti = 0.1f;

        bool drawMode = true;
        int gameMode = 3;
        bool points = true;

        List<Drawable> drawables = new List<Drawable>();

        Vector4 lightPosition;

        //fps
        float time = 0f;
        float update = 1f;
        int frames = 0;
        string fps = "";

        bool OrientCamera = false;

        // physics - input
        //float playerMoveMulti = 7.5f;
        float playerMoveMulti = 50f;

        Physics.PhysicsManager physics;

		List<Physics.Point> test = new List<Physics.Point>();

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
            InputHandler.LoadDefaultBindings();

            reset();
            //initBasic();

            base.Initialize();
        }

		Physics.Point testPoint;

        private void initBasic()
        {

            playerCube = new DemoCube(cubeStartPosition, 1);
			physics.AddPoints(playerCube.points);
			physics.AddSprings(playerCube.springs);
			physics.AddBody(playerCube);
			physics.Player.PlayerBody = playerCube;



			addToPhysicsAndDraw(new StaticQuad(new Vector3(10, 0, 10), new Vector3(10, 0, -10), new Vector3(-10, 0, -10), new Vector3(-10, 0, 10), Color.White));
			addToPhysicsAndDraw(new StaticQuad(new Vector3(-10, 0, -10), new Vector3(-10, 0, 10), new Vector3(10, 0, 10), new Vector3(10, 0, -10), Color.White));


			testPoint = new Physics.Point(new Vector3(0, 10, 25));
			addToPhysicsAndDraw(new TriAA(testPoint, new Physics.Point(new Vector3(10, 0, 10)), new Physics.Point(new Vector3(-10, 0, 10)), Color.Red));
			

			//addToPhysicsAndDraw(new StaticQuad(new Vector3(8, 0, 15), new Vector3(8, 0, 5), new Vector3(-2, 0, 5), new Vector3(-2, 0, 5), Color.White));
			//addToPhysicsAndDraw(new StaticQuad(new Vector3(-12, 0, -12), new Vector3(-8, 0, -4), new Vector3(-4, 0, -4), new Vector3(-4, 0, -4), Color.White));


            physics.AddGravity( new Physics.GravityVector());

            lightPosition = new Vector4(0, 5, 0, 0);

            playerCube.setGraphicsDevice(GraphicsDevice);
            foreach (Drawable d in drawables)
            {
                d.setGraphicsDevice(GraphicsDevice);
            }

        }

        private void addToPhysicsAndDraw(T t)
        {
            physics.AddCollidable(t);
            drawables.Add(t);
        }

        private void reset()
        {
            physics = new Physics.PhysicsManager();

            //physics.AirFriction = 0.5f;
            physics.AirFriction = 2f;

            physics.Player.Traction.Minimum = 0.5f;
            physics.Player.Traction.Origin = 1f;
            physics.Player.Traction.Maximum = 2f;

            physics.Player.Cling.Minimum = 0f;
            physics.Player.Cling.Origin = 0f;
            physics.Player.Cling.Maximum = 0f;

            physics.Player.Resilience.Minimum = 20f;
            physics.Player.Resilience.Origin = 100f;
            physics.Player.Resilience.Maximum = 200f;
            physics.Player.Resilience.Delta = 10;

            physics.Player.Volume.Minimum = 0f;
            physics.Player.Volume.Origin = 100f;
            physics.Player.Volume.Maximum = 20f;
            physics.Player.Volume.Delta = 5;

            drawables.Clear();

            initBasic();
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

            InitializeEffect();
        }

        private void InitializeEffect()
        {
            worldMatrix = Matrix.Identity;

            viewMatrix = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 4, 0), Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),  // 45 degree angle
                (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height,
                1.0f, 100.0f);

            VertexDeclarationTexture = new VertexDeclaration(GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            VertexDeclarationColor = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

            effect = Content.Load<Effect>("effects");

            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            effect.Parameters["xTexture"].SetValue(text);
            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xLightPos"].SetValue(new Vector4(5, 5, 5, 0));
            effect.Parameters["xAmbient"].SetValue(0.25f);

            effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
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
                if (gameMode == 3)
                {
                    reset();
                    //initLoop();
                }
                if (gameMode == 2)
                {
                    reset();
                    //initCube();
                }
                else if (gameMode == 1)
                {
                    reset();
                    //initThree();
                }
                else if (gameMode == 0)
                {
                    reset();
                    //initGlobe();
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
                    cameraPosition = defaultCameraPosition;
                    viewMatrix = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 4, 0), Vector3.Up);
                    effect.Parameters["xView"].SetValue(viewMatrix);

                    effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
                }
            }

            if (InputHandler.IsKeyPressed(Keys.S))
            {
                //playerCube.setSpringForce(92.5f);
                physics.Player.Resilience.Target = 1f;
            }
            if (InputHandler.IsKeyPressed(Keys.D))
            {
                //playerCube.setSpringForce(62.5f);
                physics.Player.Resilience.Target = 0.5f;
            }
            if (InputHandler.IsKeyPressed(Keys.W))
            {
                //playerCube.setSpringForce(12.5f);
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
                drawMode = !drawMode;
            }
            if (InputHandler.IsKeyPressed(Keys.O))
            {
                OrientCamera = !OrientCamera;
            }
            if (InputHandler.IsKeyPressed(Keys.M))
            {
                gameMode = (gameMode + 1) % 4;
                if (gameMode == 3)
                {
                    reset();
                    //initLoop();
                }
                if (gameMode == 2)
                {
                    reset();
                    //initCube();
                }
                else if (gameMode == 1)
                {
                    reset();
                    //initThree();
                }
                else if (gameMode == 0)
                {
                    reset();
                    //initGlobe();
                }
            }
            if (InputHandler.IsKeyPressed(Keys.OemPeriod))
            {
                points = !points;
            }
            if (InputHandler.IsKeyPressed(Keys.N))
            {
                Tri.DEBUG_DrawNormal = !Tri.DEBUG_DrawNormal;
            }

            // Xbox
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                controllermode = true;
            }
            if (controllermode)
            {
                //playerCube.setSpringForce(12.5f + (GamePad.GetState(PlayerIndex.One).Triggers.Right * 80f));
                physics.Player.Resilience.Target = GamePad.GetState(PlayerIndex.One).Triggers.Right;
                //Physics.PhysicsManager.TEMP_SurfaceFriction = GamePad.GetState(PlayerIndex.One).Triggers.Left * 24f;
                physics.Player.Traction.Target = GamePad.GetState(PlayerIndex.One).Triggers.Left;
                //float vb = MathHelper.Clamp(physics.ImpactThisFrame - 0.1f, 0f, 1f);
                //InputHandler.SetVibration(vb, 0f);
            }

            // Quick Torque
            Vector2 move = InputHandler.GetAnalogAction(AnalogActions.Movement);
            if (move != Vector2.Zero)
            {
                Vector3 Up;
                //if (OrientCamera)
                //{
                //    Up = physics.getUp(playerCube.getCenter());
                //}
                //else
                //{
                    Up = Vector3.Up;
                //}
                Vector3 Horizontal = Vector3.Normalize(Vector3.Cross(playerCube.getCenter() - cameraPosition, Up));
                Vector3 Run = Vector3.Normalize(Vector3.Cross(Horizontal, Up));

                physics.Player.applyTorque(move.Y * playerMoveMulti, Horizontal);
                physics.Player.applyTorque(move.X * playerMoveMulti, Run);

                //foreach (Physics.Point p in playerCube.points)
                //{
                //    p.CurrentForce += Vector3.Normalize(Vector3.Cross(p.Position - playerCube.getCenter(), Horizontal)) * (move.Y * playerMoveMulti);
                //    p.CurrentForce += Vector3.Normalize(Vector3.Cross(p.Position - playerCube.getCenter(), Run)) * (move.X * playerMoveMulti);
                //}
            }

            if (InputHandler.IsKeyPressed(Keys.PageUp))
            {
				testPoint.PotientialPosition.Y += 1;
				testPoint.NextPosition.Y += 1;
            }
            else if (InputHandler.IsKeyPressed(Keys.PageDown))
            {
				testPoint.PotientialPosition.Y -= 1;
				testPoint.NextPosition.Y -= 1;
            }

			if (InputHandler.IsKeyPressed(Keys.V))
			{
				if (test.Count == 0)
				{
					for (float x = -20; x < 20; x += 1f)
					{
						for (float z = -20; z < 20; z += 1f)
						{
							test.Add(new Physics.Point(new Vector3(x, 20, z)));
						}
					}
					physics.AddPoints(test);
				}
			}


            if (InputHandler.IsKeyDown(Keys.X))
            {
                //playerCube.idealVolume = playerCube.baseVolume + 50f;
                physics.Player.Volume.Target = 1f;
            }
            else if (InputHandler.IsKeyDown(Keys.C))
            {
                //playerCube.idealVolume = playerCube.baseVolume + 50f;
                physics.Player.Volume.Target = 0f;
            }
            else
            {
                //playerCube.idealVolume = playerCube.baseVolume;
                physics.Player.Volume.Target = 0.5f;
            }
            //Console.WriteLine(playerCube.getVolume());
            //playerCube.update();

            if (!paused)
            {
                physics.update((float)gameTime.ElapsedGameTime.TotalSeconds);
				testPoint.CurrentPosition = testPoint.NextPosition;
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
                cameraLength = MathHelper.Clamp(cameraLength + (InputHandler.getMouseWheelDelta() * -0.01f), 10, 40);
                Vector3 Offset = new Vector3((float)Math.Cos(cameraAngle.X) * cameraLength * cameraLengthMulti, (float)Math.Sin(cameraAngle.Y) * cameraLength * cameraLengthMulti, (float)Math.Sin(cameraAngle.X) * cameraLength * cameraLengthMulti);
                cameraPosition = playerCube.getCenter() + Offset;

                // new Vector3(10, 10, 20)
                //if (OrientCamera)
                //{
                //    viewMatrix = Matrix.CreateLookAt(cameraPosition, playerCube.getCenter(), physics.getUp(playerCube.getCenter()));
                //}
                //else
                //{
                    viewMatrix = Matrix.CreateLookAt(cameraPosition, playerCube.getCenter(), Vector3.Up);
                //}
                effect.Parameters["xView"].SetValue(viewMatrix);

                //effect.Parameters["xLightPos"].SetValue(new Vector4(cameraPosition.X * 0.5f, cameraPosition.Y * 0.5f, cameraPosition.Z * 0.5f, 0));
                effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
            }

            //cubeVertexBuffer.SetData<VertexPositionNormalTexture>(playerCube.getTriangleVertexes());



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
            }

            // Box
            effect.CurrentTechnique = effect.Techniques["Textured"];
            GraphicsDevice.VertexDeclaration = VertexDeclarationTexture;
            //cubeVertexBuffer.SetData<VertexPositionNormalTexture>(playerCube.getTriangleVertexes());
            //GraphicsDevice.Vertices[0].SetSource(cubeVertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
            GraphicsDevice.Vertices[0].SetSource(playerCube.getVertexBuffer(), 0, playerCube.getVertexStride());
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                playerCube.DrawMe();
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
                    dotVertices[i] = new VertexPositionColor(playerCube.points[i].CurrentPosition, Color.Black);
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

			if (test.Count == 1600)
			{
				effect.CurrentTechnique = effect.Techniques["Colored"];
				GraphicsDevice.VertexDeclaration = VertexDeclarationColor;
				GraphicsDevice.RenderState.DepthBufferEnable = true;
				VertexPositionColor[] testVertices = new VertexPositionColor[1600];
				for (int i = 0; i < 1600; ++i)
				{
					testVertices[i] = new VertexPositionColor(test[i].CurrentPosition, Color.Pink);
				}
				VertexBuffer testvertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * 1600, BufferUsage.None);
				testvertexBuffer.SetData<VertexPositionColor>(testVertices);
				GraphicsDevice.Vertices[0].SetSource(testvertexBuffer, 0, VertexPositionColor.SizeInBytes);
				effect.Begin();
				foreach (EffectPass pass in effect.CurrentTechnique.Passes)
				{
					pass.Begin();
					GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, 1600);
					pass.End();
				}
				effect.End();
			}


            // GUI
            GraphicsDevice.RenderState.FillMode = FillMode.Solid;
            spriteBatch.Begin();
            spriteBatch.DrawString(font, fps, Vector2.Zero, Color.White);
            if (physics.Player.Resilience.Target < 0.5)
            {
                spriteBatch.DrawString(font, "Soft", new Vector2(150, 0), Color.White);
            }
            else if (physics.Player.Resilience.Target > 0.5)
            {
                spriteBatch.DrawString(font, "Firm", new Vector2(150, 0), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, "Normal", new Vector2(150, 0), Color.White);
            }
            if (physics.Player.Traction.Target < 0.5)
            {
                spriteBatch.DrawString(font, "Slick", new Vector2(350, 0), Color.White);
            }
            else if (physics.Player.Traction.Target > 0.5)
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
            //spriteBatch.DrawString(font, physics.DEBUG_BumpLoops.ToString(), new Vector2(550, 0), Color.White);
            spriteBatch.DrawString(font, playerCube.getVolume().ToString(), new Vector2(600, 0), Color.White);
            spriteBatch.End();


            //fps
            ++frames;

            base.Draw(gameTime);
        }
    }
}
