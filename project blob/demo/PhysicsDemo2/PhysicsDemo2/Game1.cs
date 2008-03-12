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

namespace PhysicsDemo2
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;


		Texture2D text = null;

		//graphics stuff
		Matrix worldMatrix;
		Matrix viewMatrix;
		Matrix projectionMatrix;
		//
		VertexPositionColor[] cubeVertices;
		VertexDeclaration basicEffectVertexDeclarationColor;
		VertexDeclaration basicEffectVertexDeclarationTexture;
		VertexBuffer vertexBuffer;
		BasicEffect basicEffect;
#if TEXTURE
		VertexPositionNormalTexture[] triVertices;
#else
		VertexPositionColor[] triVertices;
#endif
		VertexBuffer triVertexBuffer;

		bool paused = false;
		bool triggermode = false;
		bool follow = false;

		DemoCube testCube;
		Vector3 cubeStartPosition = new Vector3(2, 8, -2);
		List<Plane> collisionPlanes = new List<Plane>();

		VertexPositionColor[] planeVertices;
		VertexBuffer planeVertexBuffer;


		SpriteFont font;


		//fps
		float time = 0f;
		float update = 1f;
		int frames = 0;
		string fps = "";

		// physics stuff

		Vector3 gravity = new Vector3(0f, -9.8f, 0f);
		float friction = 6.0f;


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


			testCube = new DemoCube(cubeStartPosition, 1);
			collisionPlanes.Add(new Plane(new Vector3(-5, 0, -5), new Vector3(-5, 0, 5), new Vector3(5, 0, 5)));
			collisionPlanes.Add(new Plane(new Vector3(5, 0, 5), new Vector3(5, 10, -5), new Vector3(-5, 0, -5)));

			planeVertices = new VertexPositionColor[15];

			planeVertices[0] = new VertexPositionColor(new Vector3(-5, 0, -5), Color.Red);
			planeVertices[1] = new VertexPositionColor(new Vector3(-5, 0, 5), Color.Red);
			planeVertices[2] = new VertexPositionColor(new Vector3(5, 0, 5), Color.Red);

			planeVertices[3] = new VertexPositionColor(new Vector3(5, 0, 5), Color.Blue);
			planeVertices[4] = new VertexPositionColor(new Vector3(5, 10, -5), Color.Blue);
			planeVertices[5] = new VertexPositionColor(new Vector3(-5, 0, -5), Color.Blue);

			planeVertices[6] = new VertexPositionColor(new Vector3(-5, 0, -5), Color.Yellow);
			planeVertices[7] = new VertexPositionColor(new Vector3(-15, 0, 5), Color.Yellow);
			planeVertices[8] = new VertexPositionColor(new Vector3(-5, 0, 5), Color.Yellow);

			planeVertices[9] = new VertexPositionColor(new Vector3(-5, 0, 5), Color.White);
			planeVertices[11] = new VertexPositionColor(new Vector3(-5, 0, 15), Color.White);
			planeVertices[10] = new VertexPositionColor(new Vector3(-15, 0, 5), Color.White);

			planeVertices[14] = new VertexPositionColor(new Vector3(5, 0, 5), Color.Orange);
			planeVertices[13] = new VertexPositionColor(new Vector3(-5, 0, 15), Color.Orange);
			planeVertices[12] = new VertexPositionColor(new Vector3(-5, 0, 5), Color.Orange);


			planeVertexBuffer = new VertexBuffer(
				graphics.GraphicsDevice,
				VertexPositionColor.SizeInBytes * planeVertices.Length, BufferUsage.None);

			planeVertexBuffer.SetData<VertexPositionColor>(planeVertices);

			text = Content.Load<Texture2D>(@"test");

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
			// graphics stuff?
			InitializeTransform();
			InitializeEffect();
			InitializeCube();

			graphics.GraphicsDevice.RenderState.PointSize = 5;

			font = Content.Load<SpriteFont>(@"Courier New");
		}

		/// <summary>
		/// Initializes the transforms used for the 3D model.
		/// </summary>
		private void InitializeTransform()
		{
			worldMatrix = Matrix.Identity;

			viewMatrix = Matrix.CreateLookAt(new Vector3(0, 5, 20), new Vector3(0, 4, 0),
				Vector3.Up);

			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.ToRadians(45),  // 45 degree angle
				(float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height,
				1.0f, 100.0f);
		}

		/// <summary>
		/// Initializes the basic effect (parameter setting and technique selection)
		/// used for the 3D model.
		/// </summary>
		private void InitializeEffect()
		{

			basicEffectVertexDeclarationTexture = new VertexDeclaration(
				graphics.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

			basicEffectVertexDeclarationColor = new VertexDeclaration(
				graphics.GraphicsDevice, VertexPositionColor.VertexElements);


			basicEffect = new BasicEffect(graphics.GraphicsDevice, null);

			basicEffect.Alpha = 1.0f;
			basicEffect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
			basicEffect.SpecularColor = new Vector3(0.75f, 0.75f, 0.75f);
			basicEffect.SpecularPower = 5.0f;
			basicEffect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);

			basicEffect.DirectionalLight0.Enabled = true;
			basicEffect.DirectionalLight0.DiffuseColor = Vector3.One;
			basicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
			basicEffect.DirectionalLight0.SpecularColor = Vector3.One;

			basicEffect.DirectionalLight1.Enabled = true;
			basicEffect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
			basicEffect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-1.0f, -1.0f, 1.0f));
			basicEffect.DirectionalLight1.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);

			basicEffect.LightingEnabled = true;
			basicEffect.TextureEnabled = true;

			if (text == null)
			{
				throw new Exception();
			}
			basicEffect.Texture = text;

			basicEffect.World = worldMatrix;
			basicEffect.View = viewMatrix;
			basicEffect.Projection = projectionMatrix;
		}

		/// <summary>
		/// Initializes the vertices and indices of the 3D model.
		/// </summary>
		private void InitializeCube()
		{

			cubeVertices = new VertexPositionColor[8];
#if TEXTURE
			triVertices = new VertexPositionNormalTexture[36];
#else
			triVertices = new VertexPositionColor[36];
#endif

			for (int i = 0; i < 8; i++)
			{
				cubeVertices[i] = new VertexPositionColor(testCube.points[i].Position, Color.White);
			}

			vertexBuffer = new VertexBuffer(graphics.GraphicsDevice, VertexPositionColor.SizeInBytes * cubeVertices.Length, BufferUsage.None);

#if TEXTURE
			triVertexBuffer = new VertexBuffer(graphics.GraphicsDevice, VertexPositionNormalTexture.SizeInBytes * triVertices.Length, BufferUsage.None);
#else
			triVertexBuffer = new VertexBuffer(graphics.GraphicsDevice, VertexPositionColor.SizeInBytes * triVertices.Length, BufferUsage.None);
#endif

			vertexBuffer.SetData<VertexPositionColor>(cubeVertices);
#if TEXTURE
			triVertexBuffer.SetData<VertexPositionNormalTexture>(triVertices);
#else
			triVertexBuffer.SetData<VertexPositionColor>(triVertices);
#endif


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
			// Allows the default game to exit on Xbox 360 and Windows
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputHandler.IsKeyPressed(Keys.Escape))
			{
				this.Exit();
			}

			if (InputHandler.IsKeyPressed(Keys.Space))
			{
				testCube = new DemoCube(cubeStartPosition, 1);
			}
			if (InputHandler.IsKeyPressed(Keys.P))
			{
				paused = !paused;
			}

			if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
			{
				paused = true;
			}
			if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
			{
				paused = false;
			}
			if (InputHandler.IsKeyPressed(Keys.S))
			{
				DemoCube.springVal = 92.5f;
				testCube.setSpringForce(92.5f);
			}
			if (InputHandler.IsKeyPressed(Keys.D))
			{
				DemoCube.springVal = 62.5f;
				testCube.setSpringForce(62.5f);
			}
			if (InputHandler.IsKeyPressed(Keys.W))
			{
				DemoCube.springVal = 12.5f;
				testCube.setSpringForce(12.5f);
			}
			if (InputHandler.IsKeyPressed(Keys.Up))
			{
				float newVal = DemoCube.springVal + 1.0f;
				DemoCube.springVal = newVal;
				testCube.setSpringForce(newVal);
				Console.WriteLine("Force set to " + newVal);
			}
			if (InputHandler.IsKeyPressed(Keys.Down))
			{
				float newVal = DemoCube.springVal - 1.0f;
				DemoCube.springVal = newVal;
				testCube.setSpringForce(newVal);
				Console.WriteLine("Force set to " + newVal);
			}
			if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
			{
				triggermode = true;
			}
			if (triggermode)
			{
				float newVal = 10f + (GamePad.GetState(PlayerIndex.One).Triggers.Right * 90f);
				DemoCube.springVal = newVal;
				testCube.setSpringForce(newVal);
			}
			if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
			{
				testCube = new DemoCube(cubeStartPosition, 1);
			}

			if (!paused)
			{
				doFakePhysics((float)gameTime.ElapsedGameTime.TotalSeconds);
			}


			// update position vert
			cubeVertices = new VertexPositionColor[8];
			triVertices = testCube.getTriangleVertexes();

			for (int i = 0; i < 8; i++)
			{
				cubeVertices[i] = new VertexPositionColor(testCube.points[i].Position, Color.Black);
			}



			vertexBuffer.SetData<VertexPositionColor>(cubeVertices);
#if TEXTURE
			triVertexBuffer.SetData<VertexPositionNormalTexture>(triVertices);
#else
			triVertexBuffer.SetData<VertexPositionColor>(triVertices);
#endif



			if (InputHandler.IsKeyPressed(Keys.F))
			{
				follow = !follow;
				//camera follow

				if (!follow)
				{
					viewMatrix = Matrix.CreateLookAt(new Vector3(0, 5, 20), new Vector3(0, 4, 0),
					Vector3.Up);
					basicEffect.View = viewMatrix;
				}
			}

			if (follow)
			{
				viewMatrix = Matrix.CreateLookAt(new Vector3(0, 5, 20), testCube.getCenter(), Vector3.Up);
				basicEffect.View = viewMatrix;
			}


			//fps
			if (time > update)
			{
				fps = Convert.ToInt32(frames / time).ToString();
				time = 0;
				frames = 0;
			}


			base.Update(gameTime);
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;
		}


		private void doFakePhysics(float TotalElapsedSeconds)
		{
			foreach (Point p in testCube.points)
			{
				// forces
				p.Force += gravity;
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

				// acceleration = f / m
				p.Acceleration = p.Force / p.mass;


				// velocity - by euler-cromer
				p.Velocity += p.Acceleration * TotalElapsedSeconds;


				// friction (?)
				p.Velocity = inhibit(p.Velocity, friction * TotalElapsedSeconds);

				// position
				Vector3 lastPos = p.Position;
				p.Position += p.Velocity * TotalElapsedSeconds;

				// collision
				foreach (Plane c in collisionPlanes)
				{
					float lastVal = c.DotNormal(lastPos);
					float thisVal = c.DotNormal(p.Position);
					if (lastVal > 0 && thisVal < 0) // we were 'above' now 'behind'
					{
						//float u = lastVal / (lastVal - thisVal);
						//Vector3 newPos2 = (lastPos * (1 - u)) + (p.Position * u);
						//Console.WriteLine(c.DotNormal(newPos2));


						Vector3 newPos = lastPos;

						p.Velocity = Vector3.Zero;
						p.Position = newPos;
					}
				}

				// done
				p.Force = Vector3.Zero;
				p.Acceleration = Vector3.Zero;
			}

		}

		private Vector3 inhibit(Vector3 baseVal, float reduceBy)
		{
			Vector3 rev = Vector3.Negate(baseVal);
			rev.Normalize();
			if (float.IsNaN(rev.X) || float.IsNaN(rev.Y) || float.IsNaN(rev.Z))
			{
				return Vector3.Zero;
			}
			Vector3 friction = rev * reduceBy;
			return baseVal + friction;
		}




		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;



			// background (hill + flat)
			basicEffect.TextureEnabled = false;
			basicEffect.VertexColorEnabled = true;
			graphics.GraphicsDevice.VertexDeclaration = basicEffectVertexDeclarationColor;
			graphics.GraphicsDevice.Vertices[0].SetSource(planeVertexBuffer, 0, VertexPositionColor.SizeInBytes);
			basicEffect.Begin();
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Begin();
				graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 5);
				pass.End();
			}
			basicEffect.End();



			// box
#if TEXTURE
			basicEffect.VertexColorEnabled = false;
			basicEffect.TextureEnabled = true;
			graphics.GraphicsDevice.VertexDeclaration = basicEffectVertexDeclarationTexture;
			graphics.GraphicsDevice.Vertices[0].SetSource(triVertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
#else
			graphics.GraphicsDevice.Vertices[0].SetSource(triVertexBuffer, 0, VertexPositionColor.SizeInBytes);
#endif
			//graphics.GraphicsDevice.Textures[0] = text;

			basicEffect.Begin();
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Begin();
				graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);
				pass.End();
			}
			basicEffect.End();


			// corner dots
			basicEffect.TextureEnabled = false;
			basicEffect.VertexColorEnabled = true;
			graphics.GraphicsDevice.VertexDeclaration = basicEffectVertexDeclarationColor;
			graphics.GraphicsDevice.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionColor.SizeInBytes);
			basicEffect.Begin();
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Begin();
				graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, 8);
				pass.End();
			}
			basicEffect.End();



			// GUI
			spriteBatch.Begin();
			spriteBatch.DrawString(font, fps, Vector2.Zero, Color.White);
			if (paused)
			{
				spriteBatch.DrawString(font, "Paused", new Vector2((graphics.GraphicsDevice.Viewport.Width - font.MeasureString("Paused").X) * 0.5f, (graphics.GraphicsDevice.Viewport.Height - font.MeasureString("Paused").Y) * 0.5f), Color.White);
			}
			spriteBatch.End();

			base.Draw(gameTime);
			++frames;
		}
	}
}
