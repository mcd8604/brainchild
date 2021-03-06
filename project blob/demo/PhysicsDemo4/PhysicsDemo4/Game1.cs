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

namespace PhysicsDemo4
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

        //shadow stuff
        Matrix lightViewProjectionMatrix;
        RenderTarget2D renderTarget;
        Texture2D texturedRenderedTo;

		bool paused = false;
		bool controllermode = false;
		bool follow = true;


		DemoCube testCube;
		Vector3 cubeStartPosition = new Vector3(0, 10, 0);
		List<Collidable> collision = new List<Collidable>();



		//VertexBuffer triVertexBuffer;
		VertexBuffer cubeVertexBuffer;

		VertexDeclaration VertexDeclarationColor;
		VertexDeclaration VertexDeclarationTexture;



		static Vector3 defaultCameraPosition = new Vector3(0, 15, 10);
		Vector3 cameraPosition = defaultCameraPosition;
		Vector2 cameraAngle = new Vector2(1f, 0.4f);
		float cameraLength = 20f;
		float playerCamMulti = 0.1f;
		float playerMouseLookMulti = 0.005f;

		float vb;

		bool drawMode = true;



		//fps
		float time = 0f;
		float update = 1f;
		int frames = 0;
		string fps = "";


		// physics stuff
		float friction = 12.0f;
		float airfriction = 1f;
		float playerMoveMulti = 12 / 60f;

		bool OrientCamera = false;

		Vector3 gravityOrigin = Vector3.Zero;

		Vector3 getGravity(Vector3 from)
		{
			//return new Vector3(0f, -9.8f, 0f);
			//return Vector3.Normalize(gravityOrigin - from) * ((9.8f * 25f) / Vector3.DistanceSquared(from, gravityOrigin) );
			return Vector3.Normalize(gravityOrigin - from) * 9.8f;
		}
		Vector3 getUp(Vector3 from)
		{
			bool Collision = false;
			Collidable CollisionTri = null;
			float CollisionU = 1;
			foreach (Collidable c in collision)
			{

				float u = c.didIntersect(from, gravityOrigin);
				if (u < 1)
				{
					if (!Collision)
					{
						CollisionTri = c;
						CollisionU = u;
						Collision = true;
					}
					else
					{
						if (u < CollisionU)
						{
							CollisionTri = c;
							CollisionU = u;
						}
					}


				}

			}

			if (Collision)
			{
				return CollisionTri.Normal();
			}
			else
			{
				return Vector3.Up;
			}
		}


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
			testCube = new DemoCube(cubeStartPosition, 1);



			// 'world' cube

			collision.Add(new CollisionQuad(new Vector3(-5, 5, 5), new Vector3(5, 5, 5), new Vector3(5, 5, -5), new Vector3(-5, 5, -5), Color.Orange)); // 'top'
			collision.Add(new CollisionQuad(new Vector3(-5, -5, 5), new Vector3(-5, -5, -5), new Vector3(5, -5, -5), new Vector3(5, -5, 5), Color.Blue));


			collision.Add(new CollisionQuad(new Vector3(-5, -5, 5), new Vector3(5, -5, 5), new Vector3(5, 5, 5), new Vector3(-5, 5, 5), Color.Red));
			collision.Add(new CollisionQuad(new Vector3(-5, -5, -5), new Vector3(-5, 5, -5), new Vector3(5, 5, -5), new Vector3(5, -5, -5), Color.Green));

			collision.Add(new CollisionQuad(new Vector3(5, -5, -5), new Vector3(5, 5, -5), new Vector3(5, 5, 5), new Vector3(5, -5, 5), Color.Yellow));
			collision.Add(new CollisionQuad(new Vector3(-5, -5, -5), new Vector3(-5, -5, 5), new Vector3(-5, 5, 5), new Vector3(-5, 5, -5), Color.Purple));




			cubeVertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionNormalTexture.SizeInBytes * 16, BufferUsage.None);

			InputHandler.LoadDefaultBindings();


			base.Initialize();
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

            renderTarget = new RenderTarget2D(graphics.GraphicsDevice, 512, 512, 1, SurfaceFormat.Color);
		}


		/// <summary>
		/// Initializes the basic effect (parameter setting and technique selection)
		/// used for the 3D model.
		/// </summary>
		private void InitializeEffect()
		{
			worldMatrix = Matrix.Identity;

			viewMatrix = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 4, 0), Vector3.Up);

			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.ToRadians(45),  // 45 degree angle
				(float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height,
				1.0f, 50.0f);

			VertexDeclarationTexture = new VertexDeclaration(GraphicsDevice, VertexPositionNormalTexture.VertexElements);

			VertexDeclarationColor = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

			effect = Content.Load<Effect>("effects");

			effect.Parameters["xView"].SetValue(viewMatrix);
			effect.Parameters["xProjection"].SetValue(projectionMatrix);
			effect.Parameters["xWorld"].SetValue(worldMatrix);

			effect.Parameters["xTexture"].SetValue(text);
			effect.Parameters["xEnableLighting"].SetValue(true);
			//effect.Parameters["xShowNormals"].SetValue(true);
			effect.Parameters["xLightDirection"].SetValue(Vector3.Down);
            effect.Parameters["xLightPos"].SetValue(new Vector4(5, 5, 5, 0));
            effect.Parameters["xMaxDepth"].SetValue(0.7f);
			effect.Parameters["xAmbient"].SetValue(0.25f);

            Vector4 lightPos = effect.Parameters["xLightPos"].GetValueVector4();
            lightViewProjectionMatrix = Matrix.CreateLookAt(new Vector3(lightPos.X, lightPos.Y, lightPos.Z), testCube.getCenter(), new Vector3(0, 0, 1)) * Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, (float)this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height, 1f, 100f);
            effect.Parameters["xLightViewProjection"].SetValue(lightViewProjectionMatrix);

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
				testCube = new DemoCube(cubeStartPosition, 1);
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
				testCube.setSpringForce(92.5f);
			}
			if (InputHandler.IsKeyPressed(Keys.D))
			{
				testCube.setSpringForce(62.5f);
			}
			if (InputHandler.IsKeyPressed(Keys.W))
			{
				testCube.setSpringForce(12.5f);
			}
			if (InputHandler.IsKeyPressed(Keys.Q))
			{
				friction = 24f;
			}
			if (InputHandler.IsKeyPressed(Keys.E))
			{
				friction = 0f;
			}
			if (InputHandler.IsKeyPressed(Keys.A))
			{
				friction = 12f;
			}
			if (InputHandler.IsKeyPressed(Keys.Z))
			{
				drawMode = !drawMode;
			}
			if (InputHandler.IsKeyPressed(Keys.O))
			{
				OrientCamera = !OrientCamera;
			}

			// Xbox
			if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
			{
				controllermode = true;
			}
			if (controllermode)
			{
				testCube.setSpringForce(12.5f + (GamePad.GetState(PlayerIndex.One).Triggers.Right * 80f));
				friction = GamePad.GetState(PlayerIndex.One).Triggers.Left * 24f;
				vb = MathHelper.Clamp(vb - 0.1f, 0f, 1f);
				InputHandler.SetVibration(vb, vb * 0.25f);
			}

			// Quick Torque
			Vector2 move = InputHandler.GetAnalogAction(AnalogActions.Movement);
			if (move != Vector2.Zero)
			{
				Vector3 Up;
				if (OrientCamera)
				{
					Up = getUp(testCube.getCenter());
				}
				else
				{
					Up = Vector3.Up;
				}
				Vector3 Horizontal = Vector3.Cross(testCube.getCenter() - cameraPosition, Up);
				Vector3 Run = Vector3.Cross(Horizontal, Up);
				foreach (Point p in testCube.points)
				{
					p.Force += Vector3.Cross(p.Position - testCube.getCenter(), Horizontal) * (move.Y * playerMoveMulti);
					p.Force += Vector3.Cross(p.Position - testCube.getCenter(), Run) * (move.X * playerMoveMulti);
				}
			}




			if (!paused)
			{
				doSomePhysics((float)gameTime.ElapsedGameTime.TotalSeconds);
			}

			if (follow)
			{

				cameraAngle += InputHandler.GetAnalogAction(AnalogActions.MouseLook) * playerMouseLookMulti;
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
				Vector3 Offset = new Vector3((float)Math.Cos(cameraAngle.X) * cameraLength, (float)Math.Sin(cameraAngle.Y) * cameraLength, (float)Math.Sin(cameraAngle.X) * cameraLength);
				cameraPosition = testCube.getCenter() + Offset;

				// new Vector3(10, 10, 20)
				if (OrientCamera)
				{
					viewMatrix = Matrix.CreateLookAt(cameraPosition, testCube.getCenter(), getUp(testCube.getCenter()));
				}
				else
				{
					viewMatrix = Matrix.CreateLookAt(cameraPosition, testCube.getCenter(), Vector3.Up);
				}
				effect.Parameters["xView"].SetValue(viewMatrix);

				//effect.Parameters["xLightPos"].SetValue(new Vector4(cameraPosition.X * 0.5f, cameraPosition.Y * 0.5f, cameraPosition.Z * 0.5f, 0));
				effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
			}

			cubeVertexBuffer.SetData<VertexPositionNormalTexture>(testCube.getTriangleVertexes());



            Vector4 lightPos = effect.Parameters["xLightPos"].GetValueVector4();
            lightViewProjectionMatrix = Matrix.CreateLookAt(new Vector3(lightPos.X, lightPos.Y, lightPos.Z), testCube.getCenter(), new Vector3(0, 0, 1)) * Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, (float)this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height, 1f, 100f);
            effect.Parameters["xLightViewProjection"].SetValue(lightViewProjectionMatrix);


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


		#region Some Physics
		private List<Collidable> CollisionChain = new List<Collidable>();
		private void doSomePhysics(float TotalElapsedSeconds)
		{

			foreach (Point p in testCube.points)
			{
				CollisionChain.Clear();
				// Apply Freefall Forces
				Vector3 Force = p.Force;
				// forces
				Force += getGravity(p.Position) * p.mass;
				foreach (Spring s in testCube.springs)
				{
					if (p == s.A)
					{
						Force += s.getForceVectorOnA();
					}
					else if (p == s.B)
					{
						Force += s.getForceVectorOnB();
					}
				}

				// Air Friction
				Force += Vector3.Negate(p.Velocity) * airfriction;

				// Calc Acceleration
				Vector3 Acceleration = p.Acceleration;
				Acceleration = Force / p.mass;

				// Calc Velocity
				Vector3 Velocity = p.Velocity;
				Velocity += Acceleration * TotalElapsedSeconds;

				// Calc New Position
				Vector3 originalPosition = p.Position;
				Vector3 finalPosition = p.Position;
				finalPosition += Velocity * TotalElapsedSeconds;


				// Check for Collision
				bool Collision = false;
				Collidable CollisionTri = null;
				float CollisionU = float.MaxValue;
				foreach (Collidable c in collision)
				{
					float u = c.didIntersect(originalPosition, finalPosition);
					// If Collision ( u < 1 ) - Split Time and redo
					if (u < 1)
					{

						if (Collision)
						{

							//Console.WriteLine("Secondary Collision!");

						}


						if (!Collision)
						{
							CollisionTri = c;
							CollisionU = u;
							Collision = true;
						}
						else
						{
							if (u < CollisionU)
							{
								CollisionTri = c;
								CollisionU = u;
							}
						}


					}

				}

				if (Collision)
				{
					// freefallPhysics first half
					freefallPhysics(p, TotalElapsedSeconds * CollisionU);

					// sliding physics second half
					slidingPhysics(p, TotalElapsedSeconds * (1 - CollisionU), CollisionTri);
				}

				// If No Collision, apply values Calc'd Above
				if (!Collision)
				{
					p.Force = Vector3.Zero;
					p.Acceleration = Vector3.Zero;
					p.Velocity = Velocity;
					p.Position = finalPosition;
				}

			}

		}
		private void freefallPhysics(Point p, float time)
		{

			// forces
			p.Force += getGravity(p.Position) * p.mass;
			foreach (Spring s in testCube.springs)
			{
				if (p == s.A)
				{
					p.Force += s.getForceVectorOnA();
				}
				else if (p == s.B)
				{
					p.Force += s.getForceVectorOnB();
				}
			}

			// air friction
			p.Force += Vector3.Negate(p.Velocity) * airfriction;

			// acceleration
			p.Acceleration = p.Force / p.mass;

			// velocity - by euler-cromer
			p.Velocity += p.Acceleration * time;

			// position
			p.Position += p.Velocity * time;


			//done
			p.Force = Vector3.Zero;
			p.Acceleration = Vector3.Zero;

		}
		private void slidingPhysics(Point p, float time, Collidable s)
		{

			// nudge it out just enough to be above the plane to keep floating point error from falling through
			while (s.DotNormal(p.Position) <= 0)
			{
				p.Position += (s.Normal() * 0.001f);
			}


			// Stop Velocity in direction of the wall
			Vector3 NormalEffect = (s.Normal() * (p.Velocity.Length() * (float)Math.Cos(Math.Atan2(Vector3.Cross(p.Velocity, s.Normal()).Length(), Vector3.Dot(p.Velocity, s.Normal())))));
			p.Velocity = (p.Velocity - NormalEffect);

			vb += NormalEffect.Length() * 0.1f;

			// forces
			Vector3 Force = p.Force;
			Force += getGravity(p.Position) * p.mass;
			foreach (Spring sp in testCube.springs)
			{
				if (p == sp.A)
				{
					Force += sp.getForceVectorOnA();
				}
				else if (p == sp.B)
				{
					Force += sp.getForceVectorOnB();
				}
			}

			// normal force
			Vector3 NormalForce = (s.Normal() * (Force.Length() * (float)Math.Cos(Math.Atan2(Vector3.Cross(Force, s.Normal()).Length(), Vector3.Dot(Force, s.Normal())))));
			Force = (Force - NormalForce);

			// air friction
			Force += Vector3.Negate(p.Velocity) * airfriction;

			// surface friction !
			Force += Vector3.Negate(p.Velocity) * friction;


			// acceleration
			Vector3 Acceleration = p.Acceleration;
			Acceleration = Force / p.mass;

			// velocity
			Vector3 Velocity = p.Velocity;
			Velocity += Acceleration * time;

			// position
			Vector3 originalPosition = p.Position;
			Vector3 finalPosition = p.Position;
			finalPosition += Velocity * time;


			bool Collision = false;
			Collidable CollisionTri = null;
			float CollisionU = float.MaxValue;
			foreach (Collidable c in collision)
			{
				float u = c.didIntersect(originalPosition, finalPosition);
				// If Collision ( u < 1 ) - Split Time and redo
				if (u < 1)
				{

					if (s == c)
					{
						Console.WriteLine("Duplicate Collision!");
						//throw new Exception();
						continue;
					}

					if (CollisionChain.Contains(s))
					{
						Console.WriteLine("Duplicate Sliding Collision! Ignoring - This probably means a point is going to fall through the world.");
						//throw new Exception();
						continue;
					}

					//Console.WriteLine("Sliding Collision!");

					if (Collision)
					{

						//Console.WriteLine("Secondary Sliding Collision!");

					}


					if (!Collision)
					{
						CollisionTri = c;
						CollisionU = u;
						Collision = true;
					}
					else
					{
						if (u < CollisionU)
						{
							CollisionTri = c;
							CollisionU = u;
						}
					}

				}

			}

			if (Collision)
			{
				CollisionChain.Add(s);

				// freefallPhysics first half
				slidingPhysics(p, time * CollisionU, s);

				// sliding physics second half
				slidingPhysics(p, time * (1 - CollisionU), CollisionTri);
			}

			// If No Collision, apply values Calc'd Above
			if (!Collision)
			{
				p.Force = Vector3.Zero;
				p.Acceleration = Vector3.Zero;
				p.Velocity = Velocity;
				p.Position = finalPosition;
			}



			// done
			p.Force = Vector3.Zero;
			p.Acceleration = Vector3.Zero;

		}
		#endregion


		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.SetRenderTarget(0, renderTarget);

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
			foreach (Collidable c in collision)
			{
				VertexPositionColor[] temp = c.getTriangleVertexes();
				VertexBuffer tempVertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * temp.Length, BufferUsage.None);
				tempVertexBuffer.SetData<VertexPositionColor>(temp);
				GraphicsDevice.Vertices[0].SetSource(tempVertexBuffer, 0, VertexPositionColor.SizeInBytes);
				effect.Begin();
				foreach (EffectPass pass in effect.CurrentTechnique.Passes)
				{
					pass.Begin();
					c.DrawMe(GraphicsDevice);
					pass.End();
				}
				effect.End();
			}

            // Collision Tris ShadowMap
            effect.CurrentTechnique = effect.Techniques["ShadowMap"];
            foreach (Collidable c in collision)
            {
                effect.Begin();
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    c.DrawMe(GraphicsDevice);
                    pass.End();
                }
                effect.End();
            }

			// Box
			effect.CurrentTechnique = effect.Techniques["Textured"];
			GraphicsDevice.VertexDeclaration = VertexDeclarationTexture;
			cubeVertexBuffer.SetData<VertexPositionNormalTexture>(testCube.getTriangleVertexes());
			GraphicsDevice.Vertices[0].SetSource(cubeVertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
			effect.Begin();
			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Begin();
				testCube.DrawMe(GraphicsDevice);
				pass.End();
			}
			effect.End();

            // Box ShadowMap
            effect.CurrentTechnique = effect.Techniques["ShadowMap"];
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                testCube.DrawMe(GraphicsDevice);
                pass.End();
            }
            effect.End();

			// Corner Dots -
			effect.CurrentTechnique = effect.Techniques["Colored"];
			GraphicsDevice.VertexDeclaration = VertexDeclarationColor;
			GraphicsDevice.RenderState.DepthBufferEnable = false;
			VertexPositionColor[] dotVertices = new VertexPositionColor[8];
			for (int i = 0; i < 8; ++i)
			{
				dotVertices[i] = new VertexPositionColor(testCube.points[i].Position, Color.Black);
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

            graphics.GraphicsDevice.ResolveRenderTarget(0);
            texturedRenderedTo = renderTarget.GetTexture();

            graphics.GraphicsDevice.SetRenderTarget(0, null);
            graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            using (SpriteBatch sprite = new SpriteBatch(graphics.GraphicsDevice))
            {
                sprite.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                sprite.Draw(texturedRenderedTo, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 1);
                sprite.End();
            }


			// GUI
			GraphicsDevice.RenderState.FillMode = FillMode.Solid;
			spriteBatch.Begin();
			spriteBatch.DrawString(font, fps, Vector2.Zero, Color.White);
			if (DemoCube.springVal < 40)
			{
				spriteBatch.DrawString(font, "Soft", new Vector2(150, 0), Color.White);
			}
			else if (DemoCube.springVal > 70)
			{
				spriteBatch.DrawString(font, "Firm", new Vector2(150, 0), Color.White);
			}
			else
			{
				spriteBatch.DrawString(font, "Normal", new Vector2(150, 0), Color.White);
			}
			if (friction < 8)
			{
				spriteBatch.DrawString(font, "Slick", new Vector2(350, 0), Color.White);
			}
			else if (friction > 16)
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
