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

namespace Project_blob
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        VertexPositionNormalTexture[] cubeVertices;
        VertexPositionNormalTexture[] cube2Vertices;
        Display m_Display;
        Matrix worldMatrix, viewMatrix, projectionMatrix;
        Texture2D text;
        Effect m_Effect;

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            text = Content.Load<Texture2D>("test");
            m_Effect = Content.Load<Effect>("effects");

            TextureInfo ti = new TextureInfo(text, 0);
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            cubeVertices = new VertexPositionNormalTexture[36];
            cube2Vertices = new VertexPositionNormalTexture[36];

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

            topLeftFront = new Vector3(-1.0f, 3.5f, 1.0f);
            bottomLeftFront = new Vector3(-1.0f, 1.5f, 1.0f);
            topRightFront = new Vector3(1.0f, 3.5f, 1.0f);
            bottomRightFront = new Vector3(1.0f, 1.5f, 1.0f);
            topLeftBack = new Vector3(-1.0f, 3.5f, -1.0f);
            topRightBack = new Vector3(1.0f, 3.5f, -1.0f);
            bottomLeftBack = new Vector3(-1.0f, 1.5f, -1.0f);
            bottomRightBack = new Vector3(1.0f, 1.5f, -1.0f);

            textureTopLeft = new Vector2(0.0f, 0.0f);
            textureTopRight = new Vector2(1.0f, 0.0f);
            textureBottomLeft = new Vector2(0.0f, 1.0f);
            textureBottomRight = new Vector2(1.0f, 1.0f);

            frontNormal = new Vector3(0.0f, 0.0f, 1.0f);
            backNormal = new Vector3(0.0f, 0.0f, -1.0f);
            topNormal = new Vector3(0.0f, 1.0f, 0.0f);
            bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
            leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
            rightNormal = new Vector3(1.0f, 0.0f, 0.0f);

            // Front face.
            cube2Vertices[0] =
                new VertexPositionNormalTexture(
                topLeftFront, frontNormal, textureTopLeft);
            cube2Vertices[1] =
                new VertexPositionNormalTexture(
                bottomLeftFront, frontNormal, textureBottomLeft);
            cube2Vertices[2] =
                new VertexPositionNormalTexture(
                topRightFront, frontNormal, textureTopRight);
            cube2Vertices[3] =
                new VertexPositionNormalTexture(
                bottomLeftFront, frontNormal, textureBottomLeft);
            cube2Vertices[4] =
                new VertexPositionNormalTexture(
                bottomRightFront, frontNormal, textureBottomRight);
            cube2Vertices[5] =
                new VertexPositionNormalTexture(
                topRightFront, frontNormal, textureTopRight);

            // Back face.
            cube2Vertices[6] =
                new VertexPositionNormalTexture(
                topLeftBack, backNormal, textureTopRight);
            cube2Vertices[7] =
                new VertexPositionNormalTexture(
                topRightBack, backNormal, textureTopLeft);
            cube2Vertices[8] =
                new VertexPositionNormalTexture(
                bottomLeftBack, backNormal, textureBottomRight);
            cube2Vertices[9] =
                new VertexPositionNormalTexture(
                bottomLeftBack, backNormal, textureBottomRight);
            cube2Vertices[10] =
                new VertexPositionNormalTexture(
                topRightBack, backNormal, textureTopLeft);
            cube2Vertices[11] =
                new VertexPositionNormalTexture(
                bottomRightBack, backNormal, textureBottomLeft);

            // Top face.
            cube2Vertices[12] =
                new VertexPositionNormalTexture(
                topLeftFront, topNormal, textureBottomLeft);
            cube2Vertices[13] =
                new VertexPositionNormalTexture(
                topRightBack, topNormal, textureTopRight);
            cube2Vertices[14] =
                new VertexPositionNormalTexture(
                topLeftBack, topNormal, textureTopLeft);
            cube2Vertices[15] =
                new VertexPositionNormalTexture(
                topLeftFront, topNormal, textureBottomLeft);
            cube2Vertices[16] =
                new VertexPositionNormalTexture(
                topRightFront, topNormal, textureBottomRight);
            cube2Vertices[17] =
                new VertexPositionNormalTexture(
                topRightBack, topNormal, textureTopRight);

            // Bottom face. 
            cube2Vertices[18] =
                new VertexPositionNormalTexture(
                bottomLeftFront, bottomNormal, textureTopLeft);
            cube2Vertices[19] =
                new VertexPositionNormalTexture(
                bottomLeftBack, bottomNormal, textureBottomLeft);
            cube2Vertices[20] =
                new VertexPositionNormalTexture(
                bottomRightBack, bottomNormal, textureBottomRight);
            cube2Vertices[21] =
                new VertexPositionNormalTexture(
                bottomLeftFront, bottomNormal, textureTopLeft);
            cube2Vertices[22] =
                new VertexPositionNormalTexture(
                bottomRightBack, bottomNormal, textureBottomRight);
            cube2Vertices[23] =
                new VertexPositionNormalTexture(
                bottomRightFront, bottomNormal, textureTopRight);

            // Left face.
            cube2Vertices[24] =
                new VertexPositionNormalTexture(
                topLeftFront, leftNormal, textureTopRight);
            cube2Vertices[25] =
                new VertexPositionNormalTexture(
                bottomLeftBack, leftNormal, textureBottomLeft);
            cube2Vertices[26] =
                new VertexPositionNormalTexture(
                bottomLeftFront, leftNormal, textureBottomRight);
            cube2Vertices[27] =
                new VertexPositionNormalTexture(
                topLeftBack, leftNormal, textureTopLeft);
            cube2Vertices[28] =
                new VertexPositionNormalTexture(
                bottomLeftBack, leftNormal, textureBottomLeft);
            cube2Vertices[29] =
                new VertexPositionNormalTexture(
                topLeftFront, leftNormal, textureTopRight);

            // Right face. 
            cube2Vertices[30] =
                new VertexPositionNormalTexture(
                topRightFront, rightNormal, textureTopLeft);
            cube2Vertices[31] =
                new VertexPositionNormalTexture(
                bottomRightFront, rightNormal, textureBottomLeft);
            cube2Vertices[32] =
                new VertexPositionNormalTexture(
                bottomRightBack, rightNormal, textureBottomRight);
            cube2Vertices[33] =
                new VertexPositionNormalTexture(
                topRightBack, rightNormal, textureTopRight);
            cube2Vertices[34] =
                new VertexPositionNormalTexture(
                topRightFront, rightNormal, textureTopLeft);
            cube2Vertices[35] =
                new VertexPositionNormalTexture(
                bottomRightBack, rightNormal, textureBottomRight);

            VertexDeclaration basicEffectVertexDeclaration = new VertexDeclaration(
                graphics.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            float tilt = MathHelper.ToRadians(22.5f);  // 22.5 degree angle
            // Use the world matrix to tilt the cube along x and y axes.
            worldMatrix = Matrix.CreateRotationX(tilt) *
                Matrix.CreateRotationY(tilt);

            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero,
                Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),  // 45 degree angle
                (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height,
                1.0f, 100.0f);

            m_Display = new Display(worldMatrix, viewMatrix, projectionMatrix, new EffectPool(), basicEffectVertexDeclaration);

            VertexBuffer vertexBuffer = new VertexBuffer(
                graphics.GraphicsDevice,
                VertexPositionNormalTexture.SizeInBytes * cubeVertices.Length,
                BufferUsage.None
            );

            vertexBuffer.SetData<VertexPositionNormalTexture>(cubeVertices);

            VertexBuffer vertexBuffer2 = new VertexBuffer(
                graphics.GraphicsDevice,
                VertexPositionNormalTexture.SizeInBytes * cubeVertices.Length,
                BufferUsage.None
            );

            vertexBuffer2.SetData<VertexPositionNormalTexture>(cube2Vertices);

            List<VertexBuffer> list = new List<VertexBuffer>();
            list.Add(vertexBuffer);
            list.Add(vertexBuffer2);

            m_Display.DrawnList.Add(ti, list);


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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            m_Display.Draw(PrimitiveType.TriangleList, 12, VertexPositionNormalTexture.SizeInBytes);

            base.Draw(gameTime);
        }
    }
}
