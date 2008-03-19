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
        Texture2D text2;
        DrawableModel model;
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
            //text = Content.Load<Texture2D>("test");
            //text2 = Content.Load<Texture2D>("test2");
            m_Effect = Content.Load<Effect>("effects");
            model = new DrawableModel();
            model.ModelObject = Content.Load<Model>("ball");

            //TextureInfo ti = new TextureInfo(text, 0);
            //TextureInfo ti2 = new TextureInfo(text2, 1);
            
            cubeVertices = new VertexPositionNormalTexture[36];
            cube2Vertices = new VertexPositionNormalTexture[36];
            DemoCube testCube = new DemoCube(Vector3.Zero, 1);
            DemoCube testCube2 = new DemoCube(new Vector3(0, 3, 0), 1);
            
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

            model.setGraphicsDevice(graphics.GraphicsDevice);
            model.Position = Matrix.CreateTranslation(new Vector3(0,-5,0));

            vertexBuffer2.SetData<VertexPositionNormalTexture>(cube2Vertices);
            testCube.setGraphicsDevice(graphics.GraphicsDevice);
            testCube2.setGraphicsDevice(graphics.GraphicsDevice);
            List<Drawable> list = new List<Drawable>();
            list.Add(testCube);
            list.Add(model);

            List<Drawable> list2 = new List<Drawable>();
            list2.Add(testCube2);

            m_Display.DrawnList.Add(ti, list);
            m_Display.DrawnList.Add(ti2, list2);
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

            m_Display.Draw();

            base.Draw(gameTime);
        }
    }
}
