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

namespace Project_blob
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model blobModel;

        Blob theBlob;
        Vector3 blobStartPosition = new Vector3(0, 10, 0);
        Texture text;

        Effect effect;
        Effect celEffect;

        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;

        VertexDeclaration VertexDeclarationColor;
        VertexDeclaration VertexDeclarationTexture;

        List<Drawable> drawables = new List<Drawable>();

        Vector4 lightPosition;

        Physics.PhysicsManager physics;

        bool paused = false;
        bool controllermode = false;
        bool follow = true;

        bool drawMode = true;
        bool points = false;

		SpriteFont font;
		//fps
		float time = 0f;
		float update = 1f;
		int frames = 0;
		string fps = "";

        static Vector3 defaultCameraPosition = new Vector3(0, 15, 10);
        Vector3 cameraPosition = defaultCameraPosition;
        Vector2 cameraAngle = new Vector2(1f, 0.4f);
        float cameraLengthMulti = 1f;
        float cameraLength = 20f;
        float playerCamMulti = 0.1f;

        bool OrientCamera = false;

        float playerMoveMulti = 50f;

        Area currentArea;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            reset();
            physics.AirFriction = 2f;

			InputHandler.LoadDefaultBindings();

            base.Initialize();
        }

        private void reset()
        {
            physics = new Physics.PhysicsManager();

            physics.Player.Traction.Minimum = 0f;
            physics.Player.Traction.Origin = 50f;
            physics.Player.Traction.Maximum = 100f;

            physics.Player.Cling.Minimum = 0f;
            physics.Player.Cling.Origin = 100f;
            physics.Player.Cling.Maximum = 400f;

            physics.Player.Resilience.Minimum = 10f;
            physics.Player.Resilience.Origin = 40f;
            physics.Player.Resilience.Maximum = 120f;
            physics.Player.Resilience.Delta = 10;

            physics.Player.Volume.Minimum = 0f;
			physics.Player.Volume.Origin = 10f;
            physics.Player.Volume.Maximum = 50f;
            physics.Player.Volume.Delta = 1;
        }

        private void resetBlob()
        {
            blobStartPosition = new Vector3(0.1f, 1.0001f, 0.1f);
            theBlob = new Blob(blobModel, blobStartPosition);
            theBlob.setGraphicsDevice(GraphicsDevice);

            physics.Player.PlayerBody = theBlob;
            physics.AddPoints(theBlob.points);
            physics.AddSprings(theBlob.springs);
            physics.AddGravity(new Physics.GravityVector(9.8f, new Vector3(0f, -1.0f, 0f)));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			font = Content.Load<SpriteFont>(@"Fonts\\Courier New");

            celEffect = Content.Load<Effect>(@"Shaders\\Cel");

            // TODO: use this.Content to load your game content here
            blobModel = this.Content.Load<Model>(@"Models\\soccerball");
            text = this.Content.Load<Texture2D>(@"Textures\\test");

            resetBlob();

            /*foreach (Drawable d in drawables)
            {
                d.setGraphicsDevice(GraphicsDevice);
            }*/
			//cameraLengthMulti = 1f;

			lightPosition = new Vector4(5, 5, 5, 0);

            GraphicsDevice.RenderState.PointSize = 5;

            InitializeEffect();

            Level.LoadLevel("ground", "effects");

            //load first area
            if (Level.Areas.Count > 0)
            {
                IEnumerator e = Level.Areas.Values.GetEnumerator();
                e.MoveNext();
                currentArea = (Area)e.Current;
            }
            else
            {
                //empty level
            }        
        }

        private void addToPhysicsAndDraw(T t)
        {
            physics.AddCollidable(t);
            drawables.Add(t);
        }

        /// <summary>
        /// Initializes the basic effect (parameter setting and technique selection)
        /// used for the 3D model.
        /// </summary>
        private void InitializeEffect()
        {
            worldMatrix = Matrix.Identity;

            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -10), new Vector3(0, 0, 0), Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),  // 45 degree angle
                (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height,
                1.0f, 100.0f);

            VertexDeclarationTexture = new VertexDeclaration(GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            VertexDeclarationColor = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

            effect = Content.Load<Effect>("Shaders\\effects");

            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            effect.Parameters["xTexture"].SetValue(text);
            effect.Parameters["xEnableLighting"].SetValue(true);
            //effect.Parameters["xShowNormals"].SetValue(true);
            //effect.Parameters["xLightDirection"].SetValue(Vector3.Down);
            effect.Parameters["xLightPos"].SetValue(new Vector4(0, 5, 0, 0));
            effect.Parameters["xAmbient"].SetValue(0.25f);

            effect.Parameters["xCameraPos"].SetValue(new Vector4(0,0,-5, 0));

            EffectManager.getSingleton.AddEffect("effects", effect);

            celEffect.Parameters["World"].SetValue(worldMatrix);
            celEffect.Parameters["View"].SetValue(viewMatrix);
            celEffect.Parameters["Projection"].SetValue(projectionMatrix);

            celEffect.Parameters["DiffuseLightColor"].SetValue(new Vector4(0.75f, 0.75f, 0.75f, 1.0f));
            celEffect.Parameters["LightPosition"].SetValue(new Vector3(1.0f, 600.0f, 600.0f));
            celEffect.Parameters["LayerOneSharp"].SetValue(.3f);
            celEffect.Parameters["LayerOneRough"].SetValue(0.15f);
            celEffect.Parameters["LayerOneContrib"].SetValue(0.15f);
            celEffect.Parameters["LayerTwoSharp"].SetValue(0.10f);
            celEffect.Parameters["LayerTwoRough"].SetValue(4.0f);
            celEffect.Parameters["LayerTwoContrib"].SetValue(0.3f);
            celEffect.Parameters["EdgeOffset"].SetValue(0.05f);

            celEffect.Parameters["EyePosition"].SetValue(cameraPosition);

            EffectManager.getSingleton.AddEffect("celEffect", celEffect);
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
                reset();
                resetBlob();
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

                    celEffect.Parameters["EyePosition"].SetValue(cameraPosition);
                    celEffect.Parameters["View"].SetValue(viewMatrix);
                    effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
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
                drawMode = !drawMode;
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
                /*float vb = MathHelper.Clamp(physics.ImpactThisFrame - 0.1f, 0f, 1f);
                InputHandler.SetVibration(vb, 0f);*/
            }

            // Quick Torque
            Vector2 move = InputHandler.GetAnalogAction(AnalogActions.Movement);
            if (move != Vector2.Zero)
            {
                Vector3 Up;
                /*if (OrientCamera)
                {
                    Up = physics.getUp(theBlob.getCenter());
                }
                else
                {*/
                    Up = Vector3.Up;
                //}
                Vector3 Horizontal = Vector3.Normalize(Vector3.Cross(theBlob.getCenter() - cameraPosition, Up));
                Vector3 Run = Vector3.Normalize(Vector3.Cross(Horizontal, Up));

                physics.Player.applyTorque(move.Y * playerMoveMulti, Horizontal);
                physics.Player.applyTorque(move.X * playerMoveMulti, Run);

                //foreach (Physics.Point p in theBlob.points)
                //{
                //    p.CurrentForce += Vector3.Normalize(Vector3.Cross(p.Position - theBlob.getCenter(), Horizontal)) * (move.Y * playerMoveMulti);
                //    p.CurrentForce += Vector3.Normalize(Vector3.Cross(p.Position - theBlob.getCenter(), Run)) * (move.X * playerMoveMulti);
                //}
            }

            if (InputHandler.IsKeyPressed(Keys.PageUp))
            {
                theBlob.setSpringLength(0.1f);
                cameraLengthMulti *= 1.015f;
            }
            else if (InputHandler.IsKeyPressed(Keys.PageDown))
            {
                theBlob.setSpringLength(-0.1f);
                cameraLengthMulti *= 0.985f;
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
            else
            {
                //theBlob.idealVolume = theBlob.baseVolume;
                physics.Player.Volume.Target = 0.5f;
            }
            //Console.WriteLine(theBlob.getVolume());
            //theBlob.update();

            if (!paused)
            {
                physics.update((float)gameTime.ElapsedGameTime.TotalSeconds);
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
                cameraPosition = theBlob.getCenter() + Offset;

                // new Vector3(10, 10, 20)
                /*if (OrientCamera)
                {
                    viewMatrix = Matrix.CreateLookAt(cameraPosition, theBlob.getCenter(), physics.getUp(theBlob.getCenter()));
                }
                else
                {*/
                    viewMatrix = Matrix.CreateLookAt(cameraPosition, theBlob.getCenter(), Vector3.Up);
                //}
                effect.Parameters["xView"].SetValue(viewMatrix);
                celEffect.Parameters["View"].SetValue(viewMatrix);

                //effect.Parameters["xLightPos"].SetValue(new Vector4(cameraPosition.X * 0.5f, cameraPosition.Y * 0.5f, cameraPosition.Z * 0.5f, 0));
                effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
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

            // Box
            effect.CurrentTechnique = effect.Techniques["Textured"];
            GraphicsDevice.Textures[0] = text;
            celEffect.Begin();
            foreach (EffectPass pass in celEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                theBlob.DrawMe();
                pass.End();
            }
            celEffect.End();

            // Collision Tris
           /* effect.CurrentTechnique = effect.Techniques["Colored"];
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

            //Level Models
            currentArea.Display.WorldParameterName = "xWorld";
            currentArea.Display.Draw(GraphicsDevice);

            if (points)
            {
                // Corner Dots -
                effect.CurrentTechnique = effect.Techniques["Colored"];
                GraphicsDevice.VertexDeclaration = VertexDeclarationColor;
                GraphicsDevice.RenderState.DepthBufferEnable = false;
                VertexPositionColor[] dotVertices = new VertexPositionColor[theBlob.points.Count];
                for (int i = 0; i < theBlob.points.Count; ++i)
                {
                    dotVertices[i] = new VertexPositionColor(theBlob.points[i].CurrentPosition, Color.Black);
                }
                VertexBuffer dotvertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * theBlob.points.Count, BufferUsage.None);
                dotvertexBuffer.SetData<VertexPositionColor>(dotVertices);
                GraphicsDevice.Vertices[0].SetSource(dotvertexBuffer, 0, VertexPositionColor.SizeInBytes);
                effect.Begin();
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, theBlob.points.Count);
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
            spriteBatch.DrawString(font, "Orig Vol: " + theBlob.getVolume().ToString(), new Vector2(450, 30), Color.White);
            spriteBatch.DrawString(font, "New Vol: " + theBlob.getNewVolume().ToString(), new Vector2(450, 60), Color.White);
            //spriteBatch.DrawString(font, theBlob.getNewVolume().ToString(), new Vector2(675, 0), Color.White);
            spriteBatch.End();

			//fps
			++frames;

			//base.Draw(gameTime);
        }
    }
}
