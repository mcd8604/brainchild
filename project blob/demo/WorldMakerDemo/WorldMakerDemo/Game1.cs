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
using WorldMakerDemo.Level;


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

        Vector3 lightPos = new Vector3(0,20,0);

        Drawable _activeDrawable;
        public Drawable ActiveDrawable
        {
            get { return _activeDrawable; }
            set { _activeDrawable = value; }
        }

        Area _activeArea;
        public Area ActiveArea
        {
            get { return _activeArea; }
            set { _activeArea = value; }
        }

        public const String EFFECT_TYPE = "Cel";

        private String _effectName;
        public String EffectName
        {
            get { return _effectName; }
            set { _effectName = value; }
        }

        //Effect celshader;
        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        //SpriteFont font;

        const String POINT_TEXT = "point_text";

        //VertexDeclaration VertexDeclarationColor;
        VertexDeclaration VertexDeclarationTexture;

        bool follow = true;
        public Vector3 focusPoint = new Vector3(0, 0, 0);
        Vector3 Up = Vector3.Up;
        Vector3 Horizontal = new Vector3();
        Vector3 Run = new Vector3();

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
            _effectName = EFFECT_TYPE;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsDevice.RenderState.PointSize = 5;

            InputHandler.LoadDefaultBindings();

            //gui
            System.Windows.Forms.Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            new System.Threading.Thread(delegate()
            {
                System.Windows.Forms.Application.Run(new ModelEditor(this));
            }).Start();

            new System.Threading.Thread(delegate()
            {
                System.Windows.Forms.Application.Run(new LevelEditor(this));
            }).Start();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            if(EFFECT_TYPE != "basic")
                EffectManager.getSingleton.AddEffect(EFFECT_TYPE, Content.Load<Effect>(@"Shaders\\" + EFFECT_TYPE));


            TextureManager.getSingleton.AddTexture("grass", Content.Load<Texture2D>(@"Models\\free-grass-texture"));
            TextureManager.getSingleton.AddTexture("test", Content.Load<Texture2D>(@"Textures\\test"));
            TextureManager.getSingleton.AddTexture("point_text", Content.Load<Texture2D>(@"Textures\\point_text"));

            ModelManager.getSingleton.AddModel("cube", content.Load<Model>(System.Environment.CurrentDirectory + "/Content/Models/cube"));
            ModelManager.getSingleton.AddModel("ball", content.Load<Model>(System.Environment.CurrentDirectory + "/Content/Models/ball"));
            ModelManager.getSingleton.AddModel("ground", content.Load<Model>(System.Environment.CurrentDirectory + "/Content/Models/ground"));

            graphics.GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                graphics.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            worldMatrix = Matrix.Identity;

            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero,
                Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),  // 45 degree angle
                (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height,
                1.0f, 100.0f);

            if (EFFECT_TYPE == "basic")
            {
                Level.Level.AddArea("testArea", new Area(worldMatrix, viewMatrix, projectionMatrix));
            }
            else if (EFFECT_TYPE == "effects")
            {
                EffectManager.getSingleton.GetEffect(_effectName).Parameters["xView"].SetValue(viewMatrix);
                EffectManager.getSingleton.GetEffect(_effectName).Parameters["xProjection"].SetValue(projectionMatrix);
                EffectManager.getSingleton.GetEffect(_effectName).Parameters["xWorld"].SetValue(worldMatrix);

                EffectManager.getSingleton.GetEffect(_effectName).Parameters["xEnableLighting"].SetValue(true);
                //EffectManager.getSingleton.GetEffect(_effectName).Parameters["xShowNormals"].SetValue(true);
                //EffectManager.getSingleton.GetEffect(_effectName).Parameters["xLightDirection"].SetValue(Vector3.Down);
                EffectManager.getSingleton.GetEffect(_effectName).Parameters["xLightPos"].SetValue(new Vector4(5, 5, 5, 0));
                EffectManager.getSingleton.GetEffect(_effectName).Parameters["xAmbient"].SetValue(0.5f);

                Level.Level.AddArea("testArea", new Area(worldMatrix, _effectName, "xWorld", "xTexture", "Textured"));
            }
            else if (EFFECT_TYPE == "Cel")
            {
                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["World"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["World"].SetValue(worldMatrix);

                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["Projection"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["Projection"].SetValue(projectionMatrix);

                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["DiffuseLightColor"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["DiffuseLightColor"].SetValue(new Vector4(0.75f, 0.75f, 0.75f, 1.0f));

                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LightPosition"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["LightPosition"].SetValue(lightPos);

                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneSharp"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneSharp"].SetValue(.9f);

                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneRough"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneRough"].SetValue(0.15f);

                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneContrib"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerOneContrib"].SetValue(0.08f);

                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoSharp"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoSharp"].SetValue(0.05f);

                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoRough"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoRough"].SetValue(2.0f);

                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoContrib"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["LayerTwoContrib"].SetValue(0.4f);

                if (EffectManager.getSingleton.GetEffect(_effectName).Parameters["EdgeOffset"] != null)
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["EdgeOffset"].SetValue(0.03f);

                Level.Level.AddArea("testArea", new Area(worldMatrix, _effectName, "World", "NONE", null));
            }
            _activeArea = Level.Level.Areas["testArea"];
            //effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
            VertexDeclarationTexture = new VertexDeclaration(GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            _activeArea.Display.TextureName = POINT_TEXT;
            _activeArea.Display.ShowAxis = true;
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
            Horizontal = Vector3.Normalize(Vector3.Cross(focusPoint - cameraPosition, Up));
            Run = Vector3.Normalize(Vector3.Cross(Horizontal, Up));

            if (InputHandler.IsKeyPressed(Keys.A))
            {
                //strif to the left
                this.focusPoint += Horizontal;
                //Console.WriteLine(focusPoint);
            }
            if (InputHandler.IsKeyPressed(Keys.D))
            {
                //strif to the right
                this.focusPoint -= Horizontal;
                //Console.WriteLine(focusPoint);
            }
            if (InputHandler.IsKeyPressed(Keys.W))
            {
                //move foward
                this.focusPoint += Run;
                //Console.WriteLine(focusPoint);
            }
            if (InputHandler.IsKeyPressed(Keys.S))
            {
                //move backwards
                this.focusPoint -= Run;
                //Console.WriteLine(focusPoint);
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

                if (EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName) is BasicEffect)
                {
                    ((BasicEffect)EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName)).View = viewMatrix;
                }
                else
                {
                    if (EFFECT_TYPE == "effects")
                        EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName).Parameters["xView"].SetValue(viewMatrix);
                    else if (EFFECT_TYPE == "Cel")
                        EffectManager.getSingleton.GetEffect(_activeArea.Display.EffectName).Parameters["View"].SetValue(viewMatrix);
                }

                //m_Display.TestEffect.Parameters["xView"].SetValue(viewMatrix);

                //effect.Parameters["xLightPos"].SetValue(new Vector4(cameraPosition.X * 0.5f, cameraPosition.Y * 0.5f, cameraPosition.Z * 0.5f, 0));

                if (EFFECT_TYPE == "effects")
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
                else if (EFFECT_TYPE == "Cel")
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters["EyePosition"].SetValue(new Vector3(cameraPosition.X, cameraPosition.Y, cameraPosition.Z));
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            foreach (String str in LevelEditor.DrawablesToDelete)
            {
                _activeArea.RemoveDrawable(str);
            }
            LevelEditor.DrawablesToDelete.Clear();
            foreach (DrawableInfo drawableInfo in LevelEditor.DrawablesToAdd)
            {
                _activeArea.AddDrawable(drawableInfo.name, drawableInfo.textureInfo, drawableInfo.drawable);
            }
            LevelEditor.DrawablesToAdd.Clear();
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            _activeArea.Display.Draw(graphics.GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
