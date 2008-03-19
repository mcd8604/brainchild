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

namespace WorldMakerDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentManager content;
        Display m_Display;

        DrawableModel model;
        //VertexPositionNormalTexture[] cubeVertices;
        //VertexPositionNormalTexture[] cube2Vertices;

        Effect effect;
        //Effect celshader;
        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        SpriteFont font;

        Texture2D text;
        Texture2D text2;

        VertexDeclaration VertexDeclarationColor;
        VertexDeclaration VertexDeclarationTexture;

        bool follow = true;
        Vector3 focusPoint = new Vector3(0, 0, 0);
        Vector3 Up = Vector3.Up;
        Vector3 Horizontal = new Vector3();

        static Vector3 defaultCameraPosition = new Vector3(0, 15, 10);
        Vector3 cameraPosition = defaultCameraPosition;
        Vector2 cameraAngle = new Vector2(1f, 0.4f);
        float cameraLength = 20f;
        float playerCamMulti = 0.1f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //text = Content.Load<Texture2D>(@"test");
            //font = Content.Load<SpriteFont>(@"Courier New");

            GraphicsDevice.RenderState.PointSize = 5;

            // graphics stuff?
            InitializeEffect();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            text = Content.Load<Texture2D>("test");
            //text2 = Content.Load<Texture2D>("test2");
            effect = Content.Load<Effect>("effects");
            model = new DrawableModel();
            model.ModelObject = Content.Load<Model>("ball");

            TextureInfo ti = new TextureInfo(text, 0);
            //TextureInfo ti2 = new TextureInfo(text2, 1);

            //cubeVertices = new VertexPositionNormalTexture[36];
            //cube2Vertices = new VertexPositionNormalTexture[36];
            //DemoCube testCube = new DemoCube(Vector3.Zero, 1);
            //DemoCube testCube2 = new DemoCube(new Vector3(0, 3, 0), 1);

            VertexDeclaration basicEffectVertexDeclaration = new VertexDeclaration(
                graphics.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            float tilt = MathHelper.ToRadians(22.5f);  // 22.5 degree angle
            // Use the world matrix to tilt the cube along x and y axes.
            worldMatrix = Matrix.Identity;

            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero,
                Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),  // 45 degree angle
                (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height,
                1.0f, 100.0f);

            m_Display = new Display(worldMatrix, viewMatrix, projectionMatrix, new EffectPool(), basicEffectVertexDeclaration);

            model.setGraphicsDevice(graphics.GraphicsDevice);
            model.TranslationPriority = 1;
            //model.ScalePriority = 0;
            model.RotationPriority = 0;
            model.Rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(90));
            model.Position = Matrix.CreateTranslation(new Vector3(0, -2, 0));
            //model.Scale = Matrix.CreateScale(5);

            //vertexBuffer2.SetData<VertexPositionNormalTexture>(cube2Vertices);
            //testCube.setGraphicsDevice(graphics.GraphicsDevice);
            //testCube2.setGraphicsDevice(graphics.GraphicsDevice);
            List<Drawable> list = new List<Drawable>();
            //list.Add(testCube);
            list.Add(model);

            //List<Drawable> list2 = new List<Drawable>();
            //list2.Add(testCube2);

            m_Display.DrawnList.Add(ti, list);
            //m_Display.DrawnList.Add(ti2, list2);
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
            effect.Parameters["xLightDirection"].SetValue(Vector3.Down);
            effect.Parameters["xLightPos"].SetValue(new Vector4(5, 5, 5, 0));
            effect.Parameters["xAmbient"].SetValue(0.25f);

            effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void  UnloadContent()
        {
                content.Unload();
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

            if (InputHandler.IsKeyPressed(Keys.A))
            {
                //strif to the left
                this.focusPoint += Vector3.Normalize(Vector3.Cross(focusPoint - cameraPosition, Up));
            }
            if (InputHandler.IsKeyPressed(Keys.D))
            {
                //strif to the right
                this.focusPoint -= Vector3.Normalize(Vector3.Cross(focusPoint - cameraPosition, Up));
            }
            if (InputHandler.IsKeyPressed(Keys.W))
            {
                //move foward
                this.focusPoint += Vector3.Normalize(Vector3.Cross(Horizontal, Up));
            }
            if (InputHandler.IsKeyPressed(Keys.S))
            {
                //move backwards
                this.focusPoint -= Vector3.Normalize(Vector3.Cross(Horizontal, Up));
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
                Vector3 Offset = new Vector3((float)Math.Cos(cameraAngle.X) * cameraLength, (float)Math.Sin(cameraAngle.Y) * cameraLength, (float)Math.Sin(cameraAngle.X) * cameraLength);
                cameraPosition = focusPoint + Offset;

                viewMatrix = Matrix.CreateLookAt(cameraPosition, focusPoint, Vector3.Up);
                
                effect.Parameters["xView"].SetValue(viewMatrix);

                //effect.Parameters["xLightPos"].SetValue(new Vector4(cameraPosition.X * 0.5f, cameraPosition.Y * 0.5f, cameraPosition.Z * 0.5f, 0));
                effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
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
            m_Display.Draw();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
