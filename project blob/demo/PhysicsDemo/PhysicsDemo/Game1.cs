using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace PhysicsDemo
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		ContentManager content;


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

		//text
		SpriteBatch sprites;
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
			content = new ContentManager(Services);
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
				VertexPositionColor.SizeInBytes * planeVertices.Length, ResourceUsage.None);

			planeVertexBuffer.SetData<VertexPositionColor>(planeVertices);

			text = content.Load<Texture2D>(@"test");


			base.Initialize();
		}


		/// <summary>
		/// Load your graphics content.  If loadAllContent is true, you should
		/// load content from both ResourceManagementMode pools.  Otherwise, just
		/// load ResourceManagementMode.Manual content.
		/// </summary>
		/// <param name="loadAllContent">Which type of content to load.</param>
		protected override void LoadGraphicsContent(bool loadAllContent)
		{
			if (loadAllContent)
			{
				// TODO: Load any ResourceManagementMode.Automatic content	
			}

			text = content.Load<Texture2D>(@"test");
			// graphics stuff?
			InitializeTransform();
			InitializeEffect();
			InitializeCube();

			graphics.GraphicsDevice.RenderState.PointSize = 5;

			sprites = new SpriteBatch(graphics.GraphicsDevice);
			font = content.Load<SpriteFont>(@"Courier New");


			// TODO: Load any ResourceManagementMode.Manual content
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

			basicEffect.EnableDefaultLighting();

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

			for ( int i = 0; i < 8; i++ ) {
				cubeVertices[i] = new VertexPositionColor(testCube.points[i].Position, Color.White);
			}

			vertexBuffer = new VertexBuffer(
				graphics.GraphicsDevice,
				VertexPositionColor.SizeInBytes * cubeVertices.Length, ResourceUsage.None);

#if TEXTURE
			triVertexBuffer = new VertexBuffer(
				graphics.GraphicsDevice,
				VertexPositionNormalTexture.SizeInBytes * triVertices.Length, ResourceUsage.None);
#else
			triVertexBuffer = new VertexBuffer(
				graphics.GraphicsDevice,
				VertexPositionColor.SizeInBytes * triVertices.Length, ResourceUsage.None);
#endif

			vertexBuffer.SetData<VertexPositionColor>(cubeVertices);
#if TEXTURE
			triVertexBuffer.SetData<VertexPositionNormalTexture>(triVertices);
#else
			triVertexBuffer.SetData<VertexPositionColor>(triVertices);
#endif

		}


		/// <summary>
		/// Unload your graphics content.  If unloadAllContent is true, you should
		/// unload content from both ResourceManagementMode pools.  Otherwise, just
		/// unload ResourceManagementMode.Manual content.  Manual content will get
		/// Disposed by the GraphicsDevice during a Reset.
		/// </summary>
		/// <param name="unloadAllContent">Which type of content to unload.</param>
		protected override void UnloadGraphicsContent(bool unloadAllContent)
		{
			if (unloadAllContent)
			{
				// TODO: Unload any ResourceManagementMode.Automatic content
				content.Unload();
			}

			// TODO: Unload any ResourceManagementMode.Manual content
		}


		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			InputHandler.Update();
			// Allows the default game to exit on Xbox 360 and Windows
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				this.Exit();
			}

			if (InputHandler.IsKeyPressed(Keys.Space))
			{
				testCube = new DemoCube(cubeStartPosition, 1);
			}
			if (InputHandler.IsKeyPressed(Keys.P) )
			{
				paused = !paused;
			}

			if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
			{
				paused = true;
			}
			if ( GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
			{
				paused = false;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.S))
			{
				DemoCube.springVal = 92.5f;
				testCube.setSpringForce(92.5f);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				DemoCube.springVal = 62.5f;
				testCube.setSpringForce(62.5f);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.W))
			{
				DemoCube.springVal = 12.5f;
				testCube.setSpringForce(12.5f);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Up))
			{
				float newVal = DemoCube.springVal + 0.1f;
				DemoCube.springVal = newVal;
				testCube.setSpringForce(newVal);
				Console.WriteLine("Force set to " + newVal);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Down))
			{
				float newVal = DemoCube.springVal - 0.1f;
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

			vertexBuffer = new VertexBuffer(
				graphics.GraphicsDevice,
				VertexPositionColor.SizeInBytes * cubeVertices.Length, ResourceUsage.None);
#if TEXTURE
			triVertexBuffer = new VertexBuffer(
				graphics.GraphicsDevice,
				VertexPositionNormalTexture.SizeInBytes * triVertices.Length, ResourceUsage.None);
#else
			triVertexBuffer = new VertexBuffer(
				graphics.GraphicsDevice,
				VertexPositionColor.SizeInBytes * triVertices.Length, ResourceUsage.None);
#endif

			vertexBuffer.SetData<VertexPositionColor>(cubeVertices);
#if TEXTURE
			triVertexBuffer.SetData<VertexPositionNormalTexture>(triVertices);
#else
			triVertexBuffer.SetData<VertexPositionColor>(triVertices);
#endif



			if ( InputHandler.IsKeyPressed(Keys.F))
			{
				follow = !follow;
			}
			//camera follow
			if (follow)
			{
				viewMatrix = Matrix.CreateLookAt(new Vector3(0, 5, 20), testCube.getCenter(), Vector3.Up);
				basicEffect.View = viewMatrix;
			}
			else
			{
				viewMatrix = Matrix.CreateLookAt(new Vector3(0, 5, 20), new Vector3(0, 4, 0),
				Vector3.Up);
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
				foreach (Plane c in collisionPlanes) {
					float lastVal = c.DotNormal(lastPos);
					float thisVal = c.DotNormal(p.Position);
					if (lastVal > 0 && thisVal < 0) // we were 'above' now 'behind'
					{
						//float u = lastVal / (lastVal - thisVal);
						//Vector3 newPos = (lastPos * (1 - u)) + (p.Position * u);
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
			if (float.IsNaN(rev.X) || float.IsNaN(rev.Y) || float.IsNaN(rev.Z)){
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
			graphics.GraphicsDevice.VertexDeclaration = basicEffectVertexDeclarationColor;

			// background (hill + flat)
			basicEffect.TextureEnabled = false;
			basicEffect.VertexColorEnabled = true;
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
			graphics.GraphicsDevice.VertexDeclaration = basicEffectVertexDeclarationTexture;
			basicEffect.VertexColorEnabled = false;
			basicEffect.TextureEnabled = true;
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
			sprites.Begin();
			sprites.DrawString(font, fps, Vector2.Zero, Color.White);
			if ( paused ) {
				sprites.DrawString(font, "Paused", new Vector2((graphics.GraphicsDevice.Viewport.Width - font.MeasureString("Paused").X) * 0.5f, (graphics.GraphicsDevice.Viewport.Height - font.MeasureString("Paused").Y) * 0.5f), Color.White);
			}
			sprites.End();

			base.Draw(gameTime);
			++frames;
		}
	}
}
