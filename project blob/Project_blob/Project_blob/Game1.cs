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
using System.Collections;

using Engine;

namespace Project_blob
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model blobModel;

        Blob theBlob;
        Vector3 blobStartPosition = new Vector3(-30, 10, -40);
        Texture blobTexture;

        Effect effect;
        Effect celEffect;

        Matrix worldMatrix;
        //Matrix viewMatrix;
        //Matrix projectionMatrix;

        VertexDeclaration VertexDeclarationColor;
        VertexDeclaration VertexDeclarationTexture;

        List<Drawable> drawables = new List<Drawable>();

        Vector4 lightPosition;

        Physics.PhysicsManager physics;

        bool paused = false;
        bool controllermode = false;
        bool follow = true;

        bool drawMode = true;
        bool points = false;

        SpriteFont font;
        //fps
        float time = 0f;
        float update = 1f;
        int frames = 0;
        string fps = "";

        static Vector3 defaultCameraPosition = new Vector3(0, 15, 10);
        //Vector3 cameraPosition = defaultCameraPosition;
        Vector2 cameraAngle = new Vector2(1f, 0.4f);
        float cameraLengthMulti = 1f;
        float cameraLength = 20f;
        float playerCamMulti = 0.1f;

        bool OrientCamera = false;

        float playerMoveMulti = 50f;

        //Display theDisplay;

        Area currentArea;

        System.Diagnostics.Stopwatch physicsTime = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch drawTime = new System.Diagnostics.Stopwatch();

        

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
            resetPhysics();
            InputHandler.LoadDefaultBindings();

            lightPosition = new Vector4(5, 5, 5, 0);

            GraphicsDevice.RenderState.PointSize = 5;

            base.Initialize();
        }

        private void resetPhysics()
        {
            physics = Physics.PhysicsManager.getInstance();

            physics.AirFriction = 2f;

            physics.Player.Traction.Minimum = 0f;
            physics.Player.Traction.Origin = 100f;
            physics.Player.Traction.Maximum = 200f;

            physics.Player.Cling.Minimum = 0f;
            physics.Player.Cling.Origin = 25f;
            physics.Player.Cling.Maximum = 50f;

            physics.Player.Resilience.Minimum = 10f;
            physics.Player.Resilience.Origin = 40f;
            physics.Player.Resilience.Maximum = 80f;
            physics.Player.Resilience.Delta = 10;

            physics.Player.Volume.Minimum = 0f;
            physics.Player.Volume.Origin = 10f;
            physics.Player.Volume.Maximum = 500f;
            physics.Player.Volume.Delta = 1;

            physics.AddGravity(new Physics.GravityVector(9.8f, new Vector3(0f, -1.0f, 0f)));
        }

        private void resetBlob()
        {
            theBlob = new Blob(blobModel, blobStartPosition);
            theBlob.setGraphicsDevice(GraphicsDevice);

            physics.Player.PlayerBody = theBlob;
            physics.AddPoints(theBlob.points);
            physics.AddSprings(theBlob.springs);
            physics.AddBody(theBlob);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load fonts
            font = Content.Load<SpriteFont>(@"Fonts\\Courier New");

            //load shaders
            celEffect = Content.Load<Effect>(@"Shaders\\Cel");

            blobModel = this.Content.Load<Model>(@"Models\\soccerball");

            blobTexture = this.Content.Load<Texture2D>(@"Textures\\point_text");

            resetBlob();

            //load default level
            Level.LoadLevel("playground", "celEffect");

            //List of Static Drawables to add to Scene
            List<Drawable> staticDrawables = new List<Drawable>();

            //load first area
            if (Level.Areas.Count > 0)
            {
                IEnumerator e = Level.Areas.Values.GetEnumerator();
                e.MoveNext();
                e.MoveNext();
                //e.MoveNext();
                currentArea = (Area)e.Current;
                currentArea.Display.ShowAxis = false;
                currentArea.Display.GameMode = true;

                //Give the SceneManager a reference to the display
                SceneManager.getSingleton.Display = currentArea.Display;

                //load level models and textures
                IEnumerator drawablesEnum = currentArea.Drawables.GetEnumerator();
                //int i = 0;
                List<TextureInfo> textureInfos = new List<TextureInfo>();
                while (drawablesEnum.MoveNext())
                {
                    KeyValuePair<String, Drawable> kvp = (KeyValuePair<String, Drawable>)drawablesEnum.Current;
                    Drawable d = (Drawable)kvp.Value;
                    if (d is StaticModel)
                    {
                        StaticModel dm = (StaticModel)d;
                        Model model = Content.Load<Model>(@"Models\\" + dm.ModelName);
                        ModelManager.getSingleton.AddModel(dm.ModelName, model);
                        //TextureManager.getSingleton.AddTexture(dm.TextureName, Content.Load<Texture2D>(@"Textures\\" + dm.TextureName));
                        //textureInfos.Add(new TextureInfo(dm.TextureName, i++));
                        //Collidables
                        physics.AddCollidables(dm.createCollidables(model));

                        //physics.AddCollidableBox(dm.GetBoundingBox(), dm.createCollidables(model));
                        foreach (TextureInfo info in currentArea.Display.DrawnList.Keys)
                        {
                            if (currentArea.Display.DrawnList[info].Contains(dm))
                            {
                                dm.TextureKey = info;
                            }
                        }
                        staticDrawables.Add(dm);
                    }
                }

                //change to level list, rather than drawn
                foreach (TextureInfo ti in currentArea.Display.DrawnList.Keys)
                {
                    TextureManager.getSingleton.AddTexture(ti.TextureName, Content.Load<Texture2D>(@"Textures\\" + ti.TextureName));
                }
            }
            else
            {
                //empty level
            }

            //Add the Static Drawables to the Octree
            List<Drawable> temp = new List<Drawable>(staticDrawables);
            SceneManager.getSingleton.BuildOctree(ref temp);


            //Initialize the camera
            BasicCamera camera = new BasicCamera();
            camera.FieldOfView = MathHelper.ToRadians(45.0f);
            camera.AspectRatio = (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height;
            camera.NearPlane = 1.0f;
            camera.FarPlane = 1000.0f;

            camera.Position = new Vector3(0, 0, -10);
            camera.Target = Vector3.Zero;
            camera.Up = Vector3.Up;

            CameraManager.getSingleton.AddCamera("default", camera);
            CameraManager.getSingleton.SetActiveCamera("default");

            InitializeEffect();

            //theDisplay = new Display(worldMatrix, viewMatrix, projectionMatrix);
            //theDisplay.DrawnList.Add(

            //camera.Postiion = defaultCameraPosition;
        }

        /*private void addToPhysicsAndDraw(T t)
        {
            physics.AddCollidable(t);
            drawables.Add(t);
        }*/

        /// <summary>
        /// Initializes the basic effect (parameter setting and technique selection)
        /// used for the 3D model.
        /// </summary>
        private void InitializeEffect()
        {
            worldMatrix = Matrix.Identity;

            //viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -10), new Vector3(0, 0, 0), Vector3.Up);
            //camera.View = Matrix.CreateLookAt(new Vector3(0, 0, -10), new Vector3(0, 0, 0), Vector3.Up);

            //projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            //    MathHelper.ToRadians(45),  // 45 degree angle
            //    (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height,
            //    1.0f, 1000.0f);
            //camera.Projection = Matrix.CreatePerspectiveFieldOfView(camera.FieldOfView,  // 45 degree angle
            //    camera.AspectRatio, camera.NearPlane, camera.FarPlane);

            VertexDeclarationTexture = new VertexDeclaration(GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            VertexDeclarationColor = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

            effect = Content.Load<Effect>("Shaders\\effects");

            //effect.Parameters["xView"].SetValue(viewMatrix);
            //effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xView"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
            effect.Parameters["xProjection"].SetValue(CameraManager.getSingleton.ActiveCamera.Projection);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            effect.Parameters["xTexture"].SetValue(blobTexture);
            effect.Parameters["xEnableLighting"].SetValue(true);
            //effect.Parameters["xShowNormals"].SetValue(true);
            //effect.Parameters["xLightDirection"].SetValue(Vector3.Down);
            effect.Parameters["xLightPos"].SetValue(new Vector4(0, 5, 0, 0));
            effect.Parameters["xAmbient"].SetValue(0.25f);

            effect.Parameters["xCameraPos"].SetValue(new Vector4(0, 0, -5, 0));

            EffectManager.getSingleton.AddEffect("effects", effect);

            celEffect.Parameters["World"].SetValue(worldMatrix);
            //celEffect.Parameters["View"].SetValue(viewMatrix);
            //celEffect.Parameters["Projection"].SetValue(projectionMatrix);
            celEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
            celEffect.Parameters["Projection"].SetValue(CameraManager.getSingleton.ActiveCamera.Projection);

            celEffect.Parameters["DiffuseLightColor"].SetValue(new Vector4(0.75f, 0.75f, 0.75f, 1.0f));
            celEffect.Parameters["LightPosition"].SetValue(new Vector3(1.0f, 600.0f, 600.0f));
            celEffect.Parameters["LayerOneSharp"].SetValue(.3f);
            celEffect.Parameters["LayerOneRough"].SetValue(10.0f);
            celEffect.Parameters["LayerOneContrib"].SetValue(0.08f);
            celEffect.Parameters["LayerTwoSharp"].SetValue(0.10f);
            celEffect.Parameters["LayerTwoRough"].SetValue(1.0f);
            celEffect.Parameters["LayerTwoContrib"].SetValue(0.2f);
            celEffect.Parameters["EdgeOffset"].SetValue(0.009f);

            //celEffect.Parameters["EyePosition"].SetValue(cameraPosition);
            celEffect.Parameters["EyePosition"].SetValue(CameraManager.getSingleton.ActiveCamera.Position);

            EffectManager.getSingleton.AddEffect("celEffect", celEffect);

            BasicEffect be = new BasicEffect(GraphicsDevice, null);

            be.Alpha = 1.0f;
            be.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            be.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            be.SpecularPower = 5.0f;
            be.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

            be.DirectionalLight0.Enabled = true;
            be.DirectionalLight0.DiffuseColor = Vector3.One;
            be.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
            be.DirectionalLight0.SpecularColor = Vector3.One;

            be.LightingEnabled = true;
            be.TextureEnabled = true;

            be.World = worldMatrix;
            //be.View = viewMatrix;
            //be.Projection = projectionMatrix;
            be.View = CameraManager.getSingleton.ActiveCamera.View;
            be.Projection = CameraManager.getSingleton.ActiveCamera.Projection;

            EffectManager.getSingleton.AddEffect("basic", be);
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
            //Update Camera
            //camera.Update(gameTime);
            CameraManager.getSingleton.Update(gameTime);

            InputHandler.Update();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputHandler.IsKeyPressed(Keys.Escape))
            {
                this.Exit();
            }
            if (InputHandler.IsActionPressed(Actions.Reset))
            {
                resetBlob();
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
                    //cameraPosition = defaultCameraPosition;
                    CameraManager.getSingleton.ActiveCamera.Position = defaultCameraPosition;
                    //viewMatrix = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 4, 0), Vector3.Up);
                    //effect.Parameters["xView"].SetValue(viewMatrix);
                    //camera.View = Matrix.CreateLookAt(camera.Postiion, new Vector3(0, 4, 0), Vector3.Up);
                    CameraManager.getSingleton.ActiveCamera.Target = new Vector3(0, 4, 0);
                    CameraManager.getSingleton.ActiveCamera.Up = Vector3.Up;

                    effect.Parameters["xView"].SetValue(CameraManager.getSingleton.ActiveCamera.View);

                    //celEffect.Parameters["EyePosition"].SetValue(cameraPosition);
                    celEffect.Parameters["EyePosition"].SetValue(CameraManager.getSingleton.ActiveCamera.Position);
                    //celEffect.Parameters["View"].SetValue(viewMatrix);
                    celEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
                    //effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
                    effect.Parameters["xCameraPos"].SetValue(new Vector4(CameraManager.getSingleton.ActiveCamera.Position, 0));
                }
            }

            if (InputHandler.IsKeyPressed(Keys.S))
            {
                //theBlob.setSpringForce(92.5f);
                physics.Player.Resilience.Target = 1f;
            }
            if (InputHandler.IsKeyPressed(Keys.D))
            {
                //theBlob.setSpringForce(62.5f);
                physics.Player.Resilience.Target = 0.5f;
            }
            if (InputHandler.IsKeyPressed(Keys.W))
            {
                //theBlob.setSpringForce(12.5f);
                physics.Player.Resilience.Target = 0f;
            }
            if (InputHandler.IsKeyPressed(Keys.Q))
            {
                //Physics.PhysicsManager.TEMP_SurfaceFriction = 2f;
                physics.Player.Traction.Target = 1f;
                physics.Player.Cling.Target = 1f;
            }
            if (InputHandler.IsKeyPressed(Keys.E))
            {
                //Physics.PhysicsManager.TEMP_SurfaceFriction = 0.5f;
                physics.Player.Traction.Target = 0f;
                physics.Player.Cling.Target = 0f;
            }
            if (InputHandler.IsKeyPressed(Keys.A))
            {
                //Physics.PhysicsManager.TEMP_SurfaceFriction = 1f;
                physics.Player.Traction.Target = 0.5f;
                physics.Player.Cling.Target = 0.5f;
            }
            if (InputHandler.IsKeyPressed(Keys.Z))
            {
                drawMode = !drawMode;
            }
            if (InputHandler.IsKeyPressed(Keys.O))
            {
                OrientCamera = !OrientCamera;
            }
            if (InputHandler.IsKeyPressed(Keys.OemPeriod))
            {
                points = !points;
            }
            if (InputHandler.IsKeyPressed(Keys.N))
            {
                Tri.DEBUG_DrawNormal = !Tri.DEBUG_DrawNormal;
            }

            // Xbox
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                controllermode = true;
            }
            if (controllermode)
            {
                //theBlob.setSpringForce(12.5f + (GamePad.GetState(PlayerIndex.One).Triggers.Right * 80f));
                physics.Player.Resilience.Target = GamePad.GetState(PlayerIndex.One).Triggers.Right;
                //Physics.PhysicsManager.TEMP_SurfaceFriction = GamePad.GetState(PlayerIndex.One).Triggers.Left * 24f;
                physics.Player.Traction.Target = GamePad.GetState(PlayerIndex.One).Triggers.Left;
                /*float vb = MathHelper.Clamp(physics.ImpactThisFrame - 0.1f, 0f, 1f);
                InputHandler.SetVibration(vb, 0f);*/
            }

            // Quick Torque
            Vector2 move = InputHandler.GetAnalogAction(AnalogActions.Movement);
            if (move != Vector2.Zero)
            {
                Vector3 Up;
                /*if (OrientCamera)
                {
                    Up = physics.getUp(theBlob.getCenter());
                }
                else
                {*/
                Up = Vector3.Up;
                //}
                //Vector3 Horizontal = Vector3.Normalize(Vector3.Cross(theBlob.getCenter() - cameraPosition, Up));
                Vector3 Horizontal = Vector3.Normalize(Vector3.Cross(theBlob.getCenter() - CameraManager.getSingleton.ActiveCamera.Position, Up));
                Vector3 Run = Vector3.Normalize(Vector3.Cross(Horizontal, Up));

                physics.Player.applyTorque(move.Y * playerMoveMulti, Horizontal);
                physics.Player.applyTorque(move.X * playerMoveMulti, Run);

                //foreach (Physics.Point p in theBlob.points)
                //{
                //    p.CurrentForce += Vector3.Normalize(Vector3.Cross(p.Position - theBlob.getCenter(), Horizontal)) * (move.Y * playerMoveMulti);
                //    p.CurrentForce += Vector3.Normalize(Vector3.Cross(p.Position - theBlob.getCenter(), Run)) * (move.X * playerMoveMulti);
                //}
            }

            if (InputHandler.IsKeyPressed(Keys.PageUp))
            {
                theBlob.setSpringLength(0.1f);
                cameraLengthMulti *= 1.015f;
            }
            else if (InputHandler.IsKeyPressed(Keys.PageDown))
            {
                theBlob.setSpringLength(-0.1f);
                cameraLengthMulti *= 0.985f;
            }


            if (InputHandler.IsKeyDown(Keys.X))
            {
                //theBlob.idealVolume = theBlob.baseVolume + 50f;
                physics.Player.Volume.Target = 1f;
            }
            else if (InputHandler.IsKeyDown(Keys.C))
            {
                //theBlob.idealVolume = theBlob.baseVolume + 50f;
                physics.Player.Volume.Target = 0f;
            }
            else
            {
                //theBlob.idealVolume = theBlob.baseVolume;
                physics.Player.Volume.Target = 0.5f;
            }
            //Console.WriteLine(theBlob.getVolume());
            //theBlob.update();

            if (!paused)
            {
                physicsTime.Reset();
                physicsTime.Start();
                physics.update((float)gameTime.ElapsedGameTime.TotalSeconds);
                physicsTime.Stop();
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
                Vector3 Offset = new Vector3((float)Math.Cos(cameraAngle.X) * cameraLength * cameraLengthMulti, (float)Math.Sin(cameraAngle.Y) * cameraLength * cameraLengthMulti, (float)Math.Sin(cameraAngle.X) * cameraLength * cameraLengthMulti);
                //cameraPosition = theBlob.getCenter() + Offset;
                CameraManager.getSingleton.ActiveCamera.Position = theBlob.getCenter() + Offset;

                // new Vector3(10, 10, 20)
                /*if (OrientCamera)
                {
                    viewMatrix = Matrix.CreateLookAt(cameraPosition, theBlob.getCenter(), physics.getUp(theBlob.getCenter()));
                }
                else
                {*/
                //viewMatrix = Matrix.CreateLookAt(cameraPosition, theBlob.getCenter(), Vector3.Up);
                //camera.View = Matrix.CreateLookAt(camera.Postiion, theBlob.getCenter(), Vector3.Up);
                CameraManager.getSingleton.ActiveCamera.Target = theBlob.getCenter();
                //}
                //effect.Parameters["xView"].SetValue(viewMatrix);
                //celEffect.Parameters["View"].SetValue(viewMatrix);
                effect.Parameters["xView"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
                celEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);

                //effect.Parameters["xLightPos"].SetValue(new Vector4(cameraPosition.X * 0.5f, cameraPosition.Y * 0.5f, cameraPosition.Z * 0.5f, 0));
                //effect.Parameters["xCameraPos"].SetValue(new Vector4(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0));
                effect.Parameters["xCameraPos"].SetValue(new Vector4(CameraManager.getSingleton.ActiveCamera.Position, 0));
            }

            //cubeVertexBuffer.SetData<VertexPositionNormalTexture>(theBlob.getTriangleVertexes());



            // light
            effect.Parameters["xLightPos"].SetValue(lightPosition);



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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            drawTime.Reset();
            drawTime.Start();
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

            // Box
            effect.CurrentTechnique = effect.Techniques["Textured"];
            GraphicsDevice.Textures[0] = blobTexture;
            celEffect.Begin();
            foreach (EffectPass pass in celEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                theBlob.DrawMe();
                pass.End();
            }
            celEffect.End();

            // Collision Tris
            /*effect.CurrentTechnique = effect.Techniques["Colored"];
            GraphicsDevice.VertexDeclaration = VertexDeclarationColor;
			GraphicsDevice.Indices = null;
			foreach (Drawable d in drawables)
			{
				//VertexPositionColor[] temp = d.getTriangleVertexes();
				//VertexBuffer tempVertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * temp.Length, BufferUsage.None);
				//tempVertexBuffer.SetData<VertexPositionColor>(temp);
				//GraphicsDevice.Vertices[0].SetSource(tempVertexBuffer, 0, VertexPositionColor.SizeInBytes);
				GraphicsDevice.Vertices[0].SetSource(d.getVertexBuffer(), 0, d.getVertexStride());
				effect.Begin();
				foreach (EffectPass pass in effect.CurrentTechnique.Passes)
				{
					pass.Begin();
					d.DrawMe();
					pass.End();
				}
				effect.End();
			}*/

            //Octree Cull the Static Drawables
            foreach (TextureInfo info in currentArea.Display.DrawnList.Keys)
            {
                currentArea.Display.DrawnList[info].Clear();
            }
            SceneManager.getSingleton.UpdateVisibleDrawables(gameTime);

            //Level Models
            currentArea.Display.WorldParameterName = "World";
            currentArea.Display.Draw(GraphicsDevice);

            if (points)
            {
                // Corner Dots -
                effect.CurrentTechnique = effect.Techniques["Colored"];
                GraphicsDevice.VertexDeclaration = VertexDeclarationColor;
                GraphicsDevice.RenderState.DepthBufferEnable = false;
                VertexPositionColor[] dotVertices = new VertexPositionColor[theBlob.points.Count];
                for (int i = 0; i < theBlob.points.Count; ++i)
                {
                    dotVertices[i] = new VertexPositionColor(theBlob.points[i].CurrentPosition, Color.Black);
                }
                VertexBuffer dotvertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * theBlob.points.Count, BufferUsage.None);
                dotvertexBuffer.SetData<VertexPositionColor>(dotVertices);
                GraphicsDevice.Vertices[0].SetSource(dotvertexBuffer, 0, VertexPositionColor.SizeInBytes);
                effect.Begin();
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, theBlob.points.Count);
                    pass.End();
                }
                effect.End();
            }
            drawTime.Stop();
            // GUI
            GraphicsDevice.RenderState.FillMode = FillMode.Solid;
            spriteBatch.Begin();
            spriteBatch.DrawString(font, fps, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "Phys: " + physicsTime.Elapsed.TotalMilliseconds, new Vector2(0, 30), Color.White);
            spriteBatch.DrawString(font, "Draw: " + drawTime.Elapsed.TotalMilliseconds, new Vector2(0, 60), Color.White);
            if (physics.Player.Resilience.Target < 0.5)
            {
                spriteBatch.DrawString(font, "Soft", new Vector2(150, 0), Color.White);
            }
            else if (physics.Player.Resilience.Target > 0.5)
            {
                spriteBatch.DrawString(font, "Firm", new Vector2(150, 0), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, "Normal", new Vector2(150, 0), Color.White);
            }
            if (physics.Player.Traction.Target < 0.5)
            {
                spriteBatch.DrawString(font, "Slick", new Vector2(350, 0), Color.White);
            }
            else if (physics.Player.Traction.Target > 0.5)
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
            //spriteBatch.DrawString(font, physics.DEBUG_BumpLoops.ToString(), new Vector2(550, 0), Color.White);
            spriteBatch.DrawString(font, "Vol: " + theBlob.getVolume().ToString(), new Vector2(545, 30), Color.White);
            spriteBatch.DrawString(font, "Next Vol: " + theBlob.getNextVolume().ToString(), new Vector2(450, 60), Color.White);
            //spriteBatch.DrawString(font, theBlob.getNewVolume().ToString(), new Vector2(675, 0), Color.White);
            spriteBatch.DrawString(font, "Collidables: " + physics.DEBUG_GetNumCollidables(), new Vector2(500, 0), Color.White);

            spriteBatch.DrawString(font, "Drawn: " + SceneManager.getSingleton.Drawn, new Vector2(0, 90), Color.White);
            
            spriteBatch.End();

            //fps
            ++frames;

            //base.Draw(gameTime);
        }
    }
}
