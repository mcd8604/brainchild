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


namespace UseBasicEffect
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        VertexPositionNormalTexture[] cubeVertices;
        VertexDeclaration basicEffectVertexDeclaration;
        VertexBuffer vertexBuffer;
		IndexBuffer indexBuffer;
        BasicEffect basicEffect;

		Texture2D text;

		Effect celshader;

        GraphicsDeviceManager graphics;
		ContentManager shaders;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
			shaders = new ContentManager(Services);
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            InitializeTransform();
            InitializeEffect();
            InitializeCube();
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
        /// Initializes the transforms used for the 3D model.
        /// </summary>
        private void InitializeTransform()
        {
            float tilt = MathHelper.ToRadians(22.5f);  // 22.5 degree angle
            // Use the world matrix to tilt the cube along x and y axes.
            worldMatrix = Matrix.CreateRotationX(tilt) *
                Matrix.CreateRotationY(tilt);

            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero,
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
            basicEffect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
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
			basicEffect.TextureEnabled = true;

            basicEffect.World = worldMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;

			text = Content.Load<Texture2D>(@"textures\\test");
			basicEffect.Texture = text;

			celshader = Content.Load<Effect>(@"shaders\Cel");

			//Matrix World = Matrix.CreateScale(modelscale) * Matrix.CreateFromQuaternion(modelrotation) * Matrix.CreateTranslation(blob1Position);
			if (celshader.Parameters["World"] != null)
				celshader.Parameters["World"].SetValue(worldMatrix);

			if (celshader.Parameters["Projection"] != null)
				celshader.Parameters["Projection"].SetValue(projectionMatrix);

			if (celshader.Parameters["DiffuseLightColor"] != null)
				celshader.Parameters["DiffuseLightColor"].SetValue(new Vector4(0.6f, 0.6f, 0.6f, 1.0f));

			if (celshader.Parameters["LightPosition"] != null)
				celshader.Parameters["LightPosition"].SetValue(new Vector3(5.0f, 600.0f, 600.0f));

			if (celshader.Parameters["LayerOneSharp"] != null)
				celshader.Parameters["LayerOneSharp"].SetValue(0.6f);

			if (celshader.Parameters["LayerOneRough"] != null)
				celshader.Parameters["LayerOneRough"].SetValue(0.0f);

			if (celshader.Parameters["LayerOneContrib"] != null)
				celshader.Parameters["LayerOneContrib"].SetValue(0.05f);

			if (celshader.Parameters["LayerTwoSharp"] != null)
				celshader.Parameters["LayerTwoSharp"].SetValue(0.85f);

			if (celshader.Parameters["LayerTwoRough"] != null)
				celshader.Parameters["LayerTwoRough"].SetValue(10.0f);

			if (celshader.Parameters["LayerTwoContrib"] != null)
				celshader.Parameters["LayerTwoContrib"].SetValue(0.3f);

			if (celshader.Parameters["EdgeOffset"] != null)
				celshader.Parameters["EdgeOffset"].SetValue(0.016f);


        }

        /// <summary>
        /// Initializes the vertices and indices of the 3D model.
        /// </summary>
        private void InitializeCube()
        {

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

			short[] ind = new short[36];
			for (short i = 0; i < 36; i++)
				ind[i] = i;

			indexBuffer = new IndexBuffer(graphics.GraphicsDevice, typeof(short), 36, BufferUsage.None);
			indexBuffer.SetData<short>(ind);

            vertexBuffer = new VertexBuffer(
                graphics.GraphicsDevice,
                VertexPositionNormalTexture.SizeInBytes * cubeVertices.Length,
                BufferUsage.None
            );

            vertexBuffer.SetData<VertexPositionNormalTexture>(cubeVertices);

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
            {
                this.Exit();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            graphics.GraphicsDevice.Clear(Color.SteelBlue);
            graphics.GraphicsDevice.RenderState.CullMode =
            CullMode.CullClockwiseFace;

            graphics.GraphicsDevice.VertexDeclaration =
                basicEffectVertexDeclaration;
            graphics.GraphicsDevice.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);

            // This code would go between a device 
            // BeginScene-EndScene block.
			
			//basicEffect.Begin();
			//foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			//{
			//    pass.Begin();

			//    graphics.GraphicsDevice.DrawPrimitives(
			//        PrimitiveType.TriangleList,
			//        0,
			//        12
			//    );

			//    pass.End();
			//}
			//basicEffect.End();


			graphics.GraphicsDevice.Indices = indexBuffer;
			graphics.GraphicsDevice.VertexDeclaration = basicEffectVertexDeclaration;
			graphics.GraphicsDevice.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);

			celshader.Begin();
			foreach(EffectPass pass in celshader.CurrentTechnique.Passes)
			{
				pass.Begin();

				//graphics.GraphicsDevice.DrawPrimitives(
				//	PrimitiveType.TriangleList,
				//	0,
				//	12
				//);

				//// Change the device settings for each part to be rendered
				graphics.GraphicsDevice.Textures[0] = text;
				//// Make sure we use the texture for the current part also
				//// Finally draw the actual triangles on the screen
				graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 36, 0, 12);

				pass.End();
			}
			celshader.End();

            base.Draw(gameTime);
        }
    }
}
