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

        DrawableModel model;
        DrawableModel model2;
        //VertexPositionNormalTexture[] cubeVertices;
        //VertexPositionNormalTexture[] cube2Vertices;

        const String effectName = "Cel";

        Effect effect;
        //Effect celshader;
        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        //SpriteFont font;

        Texture2D text;
        Texture2D text2;
        Texture2D pointText;

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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //text = Content.Load<Texture2D>(@"test");
            //font = Content.Load<SpriteFont>(@"Courier New");

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
            if(effectName != "basic")
                effect = Content.Load<Effect>(@"Shaders\\" + effectName);

            text = Content.Load<Texture2D>(@"Models\\free-grass-texture");
            text2 = Content.Load<Texture2D>(@"Textures\\test");
            pointText = Content.Load<Texture2D>(@"Textures\\point_text");

            //effect = Content.Load<Effect>("effects");
            model = new DrawableModel("cube");
            model.ModelObject = Content.Load<Model>(@"Models\\cube");

            model2 = new DrawableModel("ball");
            model2.ModelObject = Content.Load<Model>(@"Models\\ball");

            TextureInfo ti = new TextureInfo(text, 0);
            TextureInfo ti2 = new TextureInfo(text2, 1);

            _activeDrawable = model;

            //cubeVertices = new VertexPositionNormalTexture[36];
            //cube2Vertices = new VertexPositionNormalTexture[36];
            //DemoCube testCube = new DemoCube(Vector3.Zero, 1);
            //DemoCube testCube2 = new DemoCube(new Vector3(0, 3, 0), 1);

            VertexDeclaration basicEffectVertexDeclaration = new VertexDeclaration(
                graphics.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            worldMatrix = Matrix.Identity;

            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero,
                Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),  // 45 degree angle
                (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height,
                1.0f, 100.0f);

            if(effectName == "basic")
            {
                Level.Level.AddArea("testArea", new Area(worldMatrix, viewMatrix, projectionMatrix, basicEffectVertexDeclaration));
            }
            else if (effectName == "effects")
            {
                effect.Parameters["xView"].SetValue(viewMatrix);
                effect.Parameters["xProjection"].SetValue(projectionMatrix);
                effect.Parameters["xWorld"].SetValue(worldMatrix);

                effect.Parameters["xEnableLighting"].SetValue(true);
                //effect.Parameters["xShowNormals"].SetValue(true);
                //effect.Parameters["xLightDirection"].SetValue(Vector3.Down);
                effect.Parameters["xLightPos"].SetValue(new Vector4(5, 5, 5, 0));
                effect.Parameters["xAmbient"].SetValue(0.5f);

                Level.Level.AddArea("testArea", new Area(worldMatrix, basicEffectVertexDeclaration, effect, "xWorld", "xTexture", "Textured"));
            }
            else if (effectName == "Cel")
            {
                if (effect.Parameters["World"] != null)
                    effect.Parameters["World"].SetValue(worldMatrix);

                if (effect.Parameters["Projection"] != null)
                    effect.Parameters["Projection"].SetValue(projectionMatrix);

                if (effect.Parameters["DiffuseLightColor"] != null)
                    effect.Parameters["DiffuseLightColor"].SetValue(new Vector4(0.75f, 0.75f, 0.75f, 1.0f));

                if (effect.Parameters["LightPosition"] != null)
                    effect.Parameters["LightPosition"].SetValue(lightPos);

                if (effect.Parameters["LayerOneSharp"] != null)
                    effect.Parameters["LayerOneSharp"].SetValue(.9f);

                if (effect.Parameters["LayerOneRough"] != null)
                    effect.Parameters["LayerOneRough"].SetValue(0.15f);

                if (effect.Parameters["LayerOneContrib"] != null)
                    effect.Parameters["LayerOneContrib"].SetValue(0.08f);

                if (effect.Parameters["LayerTwoSharp"] != null)
                    effect.Parameters["LayerTwoSharp"].SetValue(0.05f);

                if (effect.Parameters["LayerTwoRough"] != null)
                    effect.Parameters["LayerTwoRough"].SetValue(2.0f);

                if (effect.Parameters["LayerTwoContrib"] != null)
                    effect.Parameters["LayerTwoContrib"].SetValue(0.4f);

                if (effect.Parameters["EdgeOffset"] != null)
                    effect.Parameters["EdgeOffset"].SetValue(0.03f);

                Level.Level.AddArea("testArea", new Area(worldMatrix, basicEffectVertexDeclaration, effect, "World", "NONE", null));
            }
            _activeArea = Level.Level.Areas["testArea"];
            //effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
            VertexDeclarationTexture = new VertexDeclaration(GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            //m_Display = new Display(worldMatrix, VertexDeclarationTexture, effect, "xWorld", "xTexture");

            model.setGraphicsDevice(graphics.GraphicsDevice);
            model2.setGraphicsDevice(graphics.GraphicsDevice);
            model.TranslationPriority = 2;
            model.ScalePriority = 1;
            model.RotationPriority = 0;
            //model.RotationPriority = 0;
            //model.Rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(45));
            //model.Position = Matrix.CreateTranslation(new Vector3(0, -2, 0));
            //model.Scale = Matrix.CreateScale(5);

            //vertexBuffer2.SetData<VertexPositionNormalTexture>(cube2Vertices);
            //testCube.setGraphicsDevice(graphics.GraphicsDevice);
            //testCube2.setGraphicsDevice(graphics.GraphicsDevice);
            //List<Drawable> list = new List<Drawable>();
            ////list.Add(testCube);
            //list.Add(model);

            //List<Drawable> list2 = new List<Drawable>();
            //list2.Add(model2);

            _activeArea.Display.BlackTexture = pointText;
            _activeArea.Display.ShowAxis = true;
            _activeArea.AddDrawable("cube", ti, model);
            _activeArea.AddDrawable("sphere", ti2, model2);
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

                if (_activeArea.Display.CurrentEffect is BasicEffect)
                {
                    ((BasicEffect)_activeArea.Display.CurrentEffect).View = viewMatrix;
                }
                else
                {
                    if (effectName == "effects")
                       _activeArea.Display.CurrentEffect.Parameters["xView"].SetValue(viewMatrix);
                    else if (effectName == "Cel")
                       _activeArea.Display.CurrentEffect.Parameters["View"].SetValue(viewMatrix);
                }

                //m_Display.TestEffect.Parameters["xView"].SetValue(viewMatrix);

                //effect.Parameters["xLightPos"].SetValue(new Vector4(cameraPosition.X * 0.5f, cameraPosition.Y * 0.5f, cameraPosition.Z * 0.5f, 0));

                if (effectName == "effects")
                    effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
                else if (effectName == "Cel")
                    effect.Parameters["EyePosition"].SetValue(new Vector3(cameraPosition.X, cameraPosition.Y, cameraPosition.Z));
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
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            _activeArea.Display.Draw();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
