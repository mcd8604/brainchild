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

namespace BlobImport
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
        Texture text;

        Effect effect;

        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;

        VertexDeclaration VertexDeclarationColor;
        VertexDeclaration VertexDeclarationTexture;

        List<Drawable> drawables = new List<Drawable>();

        Physics.PhysicsManager physics;

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

            physics.Player.Traction.Minimum = 0.5f;
            physics.Player.Traction.Origin = 1f;
            physics.Player.Traction.Maximum = 2f;

            physics.Player.Cling.Minimum = 0f;
            physics.Player.Cling.Origin = 0f;
            physics.Player.Cling.Maximum = 0f;

            physics.Player.Resilience.Minimum = 12.5f;
            physics.Player.Resilience.Origin = 62.5f;
            physics.Player.Resilience.Maximum = 92.5f;
            physics.Player.Resilience.Delta = 10;

            physics.Player.Volume.Minimum = 0f;
            physics.Player.Volume.Origin = 5f;
            physics.Player.Volume.Maximum = 10f;
            physics.Player.Volume.Delta = 5;

            //drawables.Clear();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            blobModel = this.Content.Load<Model>(@"ball");
            text = this.Content.Load<Texture2D>(@"test");

            theBlob = new Blob(blobModel);
            physics.AddPoints(theBlob.points);
            physics.AddSprings(theBlob.springs);

            physics.AddCollidables(theBlob.getCollidables());

            physics.Player.PlayerBody = theBlob;
                
            addToPhysicsAndDraw(new StaticTri(new Vector3(-5, 0, 5), new Vector3(-5, 0, -5), new Vector3(-10, 5, -5), Color.Red));
            addToPhysicsAndDraw(new StaticTri(new Vector3(-5, 0, 5), new Vector3(-10, 5, -5), new Vector3(-10, 5, 5), Color.Pink));

            addToPhysicsAndDraw(new StaticTri(new Vector3(5, 0, -5), new Vector3(5, 0, 5), new Vector3(10, 5, -5), Color.Pink));
            addToPhysicsAndDraw(new StaticTri(new Vector3(5, 0, 5), new Vector3(10, 5, 5), new Vector3(10, 5, -5), Color.Red));

            addToPhysicsAndDraw(new StaticTri(new Vector3(-10, 15, 5), new Vector3(-10, 15, -5), new Vector3(-5, 20, -5), Color.Red));
            addToPhysicsAndDraw(new StaticTri(new Vector3(-10, 15, 5), new Vector3(-5, 20, -5), new Vector3(-5, 20, 5), Color.Pink));

            addToPhysicsAndDraw(new StaticTri(new Vector3(10, 15, -5), new Vector3(10, 15, 5), new Vector3(5, 20, -5), Color.Pink));
            addToPhysicsAndDraw(new StaticTri(new Vector3(10, 15, 5), new Vector3(5, 20, 5), new Vector3(5, 20, -5), Color.Red));

            addToPhysicsAndDraw(new StaticTri(new Vector3(-10, 5, 5), new Vector3(-10, 5, -5), new Vector3(-10, 15, -5), Color.Red));
            addToPhysicsAndDraw(new StaticTri(new Vector3(-10, 5, 5), new Vector3(-10, 15, -5), new Vector3(-10, 15, 5), Color.Pink));

            addToPhysicsAndDraw(new StaticTri(new Vector3(10, 5, -5), new Vector3(10, 5, 5), new Vector3(10, 15, -5), Color.Pink));
            addToPhysicsAndDraw(new StaticTri(new Vector3(10, 5, 5), new Vector3(10, 15, 5), new Vector3(10, 15, -5), Color.Red));

            addToPhysicsAndDraw(new StaticTri(new Vector3(5, 20, -5), new Vector3(5, 20, 5), new Vector3(-5, 20, -5), Color.Pink));
            addToPhysicsAndDraw(new StaticTri(new Vector3(5, 20, 5), new Vector3(-5, 20, 5), new Vector3(-5, 20, -5), Color.Red));

			addToPhysicsAndDraw(new StaticTri(new Vector3(-5, -2, -5), new Vector3(-5, -2, 5), new Vector3(5, -2, 5), Color.White));
			addToPhysicsAndDraw(new StaticTri(new Vector3(5, -2, 5), new Vector3(5, -2, -5), new Vector3(-5, -2, -5), Color.White));



            physics.Gravity = new Physics.GravityVector(0.5f, new Vector3(0f, -1.0f, 0f));

            theBlob.setGraphicsDevice(GraphicsDevice);
            foreach (Drawable d in drawables)
            {
                d.setGraphicsDevice(GraphicsDevice);
            }
            //cameraLengthMulti = 1f;

            InitializeEffect();
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

            effect = Content.Load<Effect>("effects");

            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            effect.Parameters["xTexture"].SetValue(text);
            effect.Parameters["xEnableLighting"].SetValue(true);
            //effect.Parameters["xShowNormals"].SetValue(true);
            //effect.Parameters["xLightDirection"].SetValue(Vector3.Down);
            effect.Parameters["xLightPos"].SetValue(new Vector4(5, 5, 5, 0));
            effect.Parameters["xAmbient"].SetValue(0.25f);

            effect.Parameters["xCameraPos"].SetValue(new Vector4(0,0,-5, 0));
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

			InputHandler.Update();



			viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -10), theBlob.getCenter(), Vector3.Up);
			effect.Parameters["xView"].SetValue(viewMatrix);


			// Quick Torque
			Vector2 move = InputHandler.GetAnalogAction(AnalogActions.Movement);
			if (move != Vector2.Zero)
			{
				Vector3 Up;
				//if (OrientCamera)
				//{
				//	Up = physics.getUp(theBlob.getCenter());
				//}
				//else
				//{
					Up = Vector3.Up;
				//}
					Vector3 Horizontal = Vector3.Normalize(Vector3.Cross(theBlob.getCenter() - new Vector3(0, 0, -20), Up));
				Vector3 Run = Vector3.Normalize(Vector3.Cross(Horizontal, Up));

				physics.Player.applyTorque(move.Y * 10f, Horizontal);
				physics.Player.applyTorque(move.X * 10f, Run);

				//foreach (Physics.Point p in playerCube.points)
				//{
				//    p.CurrentForce += Vector3.Normalize(Vector3.Cross(p.Position - playerCube.getCenter(), Horizontal)) * (move.Y * playerMoveMulti);
				//    p.CurrentForce += Vector3.Normalize(Vector3.Cross(p.Position - playerCube.getCenter(), Run)) * (move.X * playerMoveMulti);
				//}
			}




            physics.update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);


            GraphicsDevice.RenderState.CullMode = CullMode.None;
            //GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
            //GraphicsDevice.RenderState.DepthBufferEnable = false;

            // Box
            effect.CurrentTechnique = effect.Techniques["Textured"];
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                theBlob.DrawMe();
                pass.End();
            }
            effect.End();

            // Collision Tris
            effect.CurrentTechnique = effect.Techniques["Colored"];
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
			}

			//base.Draw(gameTime);
        }
    }
}
