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


		//graphics stuff
		Matrix worldMatrix;
		Matrix viewMatrix;
		Matrix projectionMatrix;
		//
		VertexPositionColor[] cubeVertices;
		VertexDeclaration basicEffectVertexDeclaration;
		VertexBuffer vertexBuffer;
		BasicEffect basicEffect;

		bool paused = false;

		DemoCube testCube;
		List<Plane> collisionPlanes = new List<Plane>();

		// physics stuff

		Vector3 gravity = new Vector3(0f, -9.8f, 0f);
		float friction = 0.1f;

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

			testCube = new DemoCube(new Vector3(2, 5, -2), 1);
			collisionPlanes.Add(new Plane(new Vector3(-5, 0, -5), new Vector3(-5, 0, 5), new Vector3(5, 0, 5)));
			collisionPlanes.Add(new Plane(new Vector3(5, 0, 5), new Vector3(5, 4, -5), new Vector3(-5, 0, -5)));

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

			// graphics stuff?
			InitializeTransform();
			InitializeEffect();
			InitializeCube();

			graphics.GraphicsDevice.RenderState.PointSize = 10;


			// TODO: Load any ResourceManagementMode.Manual content
		}

		/// <summary>
		/// Initializes the transforms used for the 3D model.
		/// </summary>
		private void InitializeTransform()
		{
			//float tilt = MathHelper.ToRadians(22.5f);  // 22.5 degree angle
			// Use the world matrix to tilt the cube along x and y axes.
			//worldMatrix = Matrix.CreateRotationX(tilt) *
			//	Matrix.CreateRotationY(tilt);
			worldMatrix = Matrix.CreateRotationX(0);

			viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero,
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
			basicEffectVertexDeclaration = new VertexDeclaration(
				graphics.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

			basicEffect = new BasicEffect(graphics.GraphicsDevice, null);
			basicEffect.Alpha = 1.0f;
			basicEffect.DiffuseColor = new Vector3(1.0f, 0.0f, 1.0f);
			basicEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
			basicEffect.SpecularPower = 5.0f;
			basicEffect.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

			basicEffect.DirectionalLight0.Enabled = true;
			basicEffect.DirectionalLight0.DiffuseColor = Vector3.One;
			basicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
			basicEffect.DirectionalLight0.SpecularColor = Vector3.One;

			basicEffect.DirectionalLight1.Enabled = true;
			basicEffect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
			basicEffect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-1.0f, -1.0f, 1.0f));
			basicEffect.DirectionalLight1.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);

			basicEffect.LightingEnabled = true;

			basicEffect.World = worldMatrix;
			basicEffect.View = viewMatrix;
			basicEffect.Projection = projectionMatrix;
		}

		/// <summary>
		/// Initializes the vertices and indices of the 3D model.
		/// </summary>
		private void InitializeCube()
		{
			/*
			cubeVertices = new VertexPositionNormalTexture[36];

			Vector3 topLeftFront = new Vector3(-1.0f, 1.0f, 1.0f);
			Vector3 bottomLeftFront = new Vector3(-1.0f, -1.0f, 1.0f);
			Vector3 topRightFront = new Vector3(1.0f, 1.0f, 1.0f);
			Vector3 bottomRightFront = new Vector3(1.0f, -1.0f, 1.0f);
			Vector3 topLeftBack = new Vector3(-1.0f, 1.0f, -1.0f);
			Vector3 topRightBack = new Vector3(1.0f, 1.0f, -1.0f);
			Vector3 bottomLeftBack = new Vector3(-1.0f, -1.0f, -1.0f);
			Vector3 bottomRightBack = new Vector3(1.0f, -1.0f, -1.0f);

			Vector2 textureTopLeft = new Vector2(0.0f, 0.0f);
			Vector2 textureTopRight = new Vector2(1.0f, 0.0f);
			Vector2 textureBottomLeft = new Vector2(0.0f, 1.0f);
			Vector2 textureBottomRight = new Vector2(1.0f, 1.0f);

			Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f);
			Vector3 backNormal = new Vector3(0.0f, 0.0f, -1.0f);
			Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);
			Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
			Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
			Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f);

			// Front face.
			cubeVertices[0] =
				new VertexPositionNormalTexture(
				topLeftFront, frontNormal, textureTopLeft);
			cubeVertices[1] =
				new VertexPositionNormalTexture(
				bottomLeftFront, frontNormal, textureBottomLeft);
			cubeVertices[2] =
				new VertexPositionNormalTexture(
				topRightFront, frontNormal, textureTopRight);
			cubeVertices[3] =
				new VertexPositionNormalTexture(
				bottomLeftFront, frontNormal, textureBottomLeft);
			cubeVertices[4] =
				new VertexPositionNormalTexture(
				bottomRightFront, frontNormal, textureBottomRight);
			cubeVertices[5] =
				new VertexPositionNormalTexture(
				topRightFront, frontNormal, textureTopRight);

			// Back face.
			cubeVertices[6] =
				new VertexPositionNormalTexture(
				topLeftBack, backNormal, textureTopRight);
			cubeVertices[7] =
				new VertexPositionNormalTexture(
				topRightBack, backNormal, textureTopLeft);
			cubeVertices[8] =
				new VertexPositionNormalTexture(
				bottomLeftBack, backNormal, textureBottomRight);
			cubeVertices[9] =
				new VertexPositionNormalTexture(
				bottomLeftBack, backNormal, textureBottomRight);
			cubeVertices[10] =
				new VertexPositionNormalTexture(
				topRightBack, backNormal, textureTopLeft);
			cubeVertices[11] =
				new VertexPositionNormalTexture(
				bottomRightBack, backNormal, textureBottomLeft);

			// Top face.
			cubeVertices[12] =
				new VertexPositionNormalTexture(
				topLeftFront, topNormal, textureBottomLeft);
			cubeVertices[13] =
				new VertexPositionNormalTexture(
				topRightBack, topNormal, textureTopRight);
			cubeVertices[14] =
				new VertexPositionNormalTexture(
				topLeftBack, topNormal, textureTopLeft);
			cubeVertices[15] =
				new VertexPositionNormalTexture(
				topLeftFront, topNormal, textureBottomLeft);
			cubeVertices[16] =
				new VertexPositionNormalTexture(
				topRightFront, topNormal, textureBottomRight);
			cubeVertices[17] =
				new VertexPositionNormalTexture(
				topRightBack, topNormal, textureTopRight);

			// Bottom face. 
			cubeVertices[18] =
				new VertexPositionNormalTexture(
				bottomLeftFront, bottomNormal, textureTopLeft);
			cubeVertices[19] =
				new VertexPositionNormalTexture(
				bottomLeftBack, bottomNormal, textureBottomLeft);
			cubeVertices[20] =
				new VertexPositionNormalTexture(
				bottomRightBack, bottomNormal, textureBottomRight);
			cubeVertices[21] =
				new VertexPositionNormalTexture(
				bottomLeftFront, bottomNormal, textureTopLeft);
			cubeVertices[22] =
				new VertexPositionNormalTexture(
				bottomRightBack, bottomNormal, textureBottomRight);
			cubeVertices[23] =
				new VertexPositionNormalTexture(
				bottomRightFront, bottomNormal, textureTopRight);

			// Left face.
			cubeVertices[24] =
				new VertexPositionNormalTexture(
				topLeftFront, leftNormal, textureTopRight);
			cubeVertices[25] =
				new VertexPositionNormalTexture(
				bottomLeftBack, leftNormal, textureBottomLeft);
			cubeVertices[26] =
				new VertexPositionNormalTexture(
				bottomLeftFront, leftNormal, textureBottomRight);
			cubeVertices[27] =
				new VertexPositionNormalTexture(
				topLeftBack, leftNormal, textureTopLeft);
			cubeVertices[28] =
				new VertexPositionNormalTexture(
				bottomLeftBack, leftNormal, textureBottomLeft);
			cubeVertices[29] =
				new VertexPositionNormalTexture(
				topLeftFront, leftNormal, textureTopRight);

			// Right face. 
			cubeVertices[30] =
				new VertexPositionNormalTexture(
				topRightFront, rightNormal, textureTopLeft);
			cubeVertices[31] =
				new VertexPositionNormalTexture(
				bottomRightFront, rightNormal, textureBottomLeft);
			cubeVertices[32] =
				new VertexPositionNormalTexture(
				bottomRightBack, rightNormal, textureBottomRight);
			cubeVertices[33] =
				new VertexPositionNormalTexture(
				topRightBack, rightNormal, textureTopRight);
			cubeVertices[34] =
				new VertexPositionNormalTexture(
				topRightFront, rightNormal, textureTopLeft);
			cubeVertices[35] =
				new VertexPositionNormalTexture(
				bottomRightBack, rightNormal, textureBottomRight);
			*/

			cubeVertices = new VertexPositionColor[8];

			for ( int i = 0; i < 8; i++ ) {
				cubeVertices[i] = new VertexPositionColor(testCube.points[i].Position, Color.White);
			}

			vertexBuffer = new VertexBuffer(
				graphics.GraphicsDevice,
				VertexPositionColor.SizeInBytes * cubeVertices.Length, ResourceUsage.None
			);

			vertexBuffer.SetData<VertexPositionColor>(cubeVertices);

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
			// Allows the default game to exit on Xbox 360 and Windows
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				this.Exit();
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				testCube = new DemoCube(new Vector3(2, 5, -2), 1);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.P))
			{
				paused = true;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.R))
			{
				paused = false;
			}

			if (!paused)
			{
				doFakePhysics((float)gameTime.ElapsedGameTime.TotalSeconds);
			}


			// update position vert
			cubeVertices = new VertexPositionColor[8];

			for (int i = 0; i < 8; i++)
			{
				cubeVertices[i] = new VertexPositionColor(testCube.points[i].Position, Color.White);
			}

			vertexBuffer = new VertexBuffer(
				graphics.GraphicsDevice,
				VertexPositionColor.SizeInBytes * cubeVertices.Length, ResourceUsage.None
			);

			vertexBuffer.SetData<VertexPositionColor>(cubeVertices);



			base.Update(gameTime);
		}


		private void doFakePhysics(float TotalElapsedSeconds)
		{
			//Console.WriteLine("doing physics stuff: " + testCube.points[0].Position + " for " + TotalElapsedSeconds);
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
				p.Velocity = inhibit(p.Velocity, friction);

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

						//Console.WriteLine("Point at " + p.Position + "Collided! Pushed to " + newPos);

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

			graphics.GraphicsDevice.RenderState.CullMode =
			CullMode.CullClockwiseFace;
			graphics.GraphicsDevice.VertexDeclaration =
				basicEffectVertexDeclaration;
			graphics.GraphicsDevice.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionColor.SizeInBytes);



			// TODO: Add your drawing code here
			basicEffect.Begin();
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Begin();

				//graphics.GraphicsDevice.DrawPrimitives(
				//	PrimitiveType.TriangleList,
				//	0,
				//	12
				//);
				graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, 8);

				pass.End();
			}
			basicEffect.End();



			base.Draw(gameTime);
		}
	}
}
