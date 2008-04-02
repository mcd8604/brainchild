/*  Author: Josh Wilson
 *  
 *  Credits: Nathan Levesque for Octree: 
 *      http://rhysyngsun.spaces.live.com/Blog/cns!FBBD62480D87119D!129.entry
 * 
 * 
 * */
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

namespace OctreeCulling
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //BasicEffect effect;
        Matrix worldMatrix;

        //CameraManager _cameraManager;
        //BasicCamera camera;

        // Add transforms for the triangle
        //private Matrix triangleTransform;
        //private VertexPositionColor[] triangleData;

        //private Matrix rectangleTransform;
        //private VertexPositionColor[] rectangleData;

        // The basic effect to use when drawing the geometry
        private BasicEffect basicEffect;
        private VertexDeclaration vertexDeclaration;

        bool drawMode = true;

        string culling = "Off";

        Pyramid pyramid;
        Cube cube;

        SpriteFont font;

        //FPS counter variables
        float time = 0f;
        float update = 1f;
        int frames = 0;
        string fps = "";

        //Test variables
        List<SceneObject> _objects;
        bool _cull = false;
        int _culled = 0;
        int _drawn = 0;
        int _total = 0;

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
            //_cameraManager = new CameraManager();

            _objects = new List<SceneObject>();

            worldMatrix = Matrix.Identity;

            BasicCamera camera = new BasicCamera();
            camera.AspectRatio = graphics.GraphicsDevice.Viewport.Width / graphics.GraphicsDevice.Viewport.Height;
            CameraManager.getSingleton.AddCamera("default", camera);

            camera = new BasicCamera();
            camera.AspectRatio = graphics.GraphicsDevice.Viewport.Width / graphics.GraphicsDevice.Viewport.Height;
            camera.FarPlane = 10.0f;
            CameraManager.getSingleton.AddCamera("test", camera);

            CameraManager.getSingleton.SetActiveCamera("default");

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

            // Load the default sprite font.
            font = Content.Load<SpriteFont>(@"Fonts/ComicSansMS");
            //font = Content.Load<SpriteFont>(@"Fonts/Courier New");

            // TODO: use this.Content to load your game content here
            basicEffect = new BasicEffect(graphics.GraphicsDevice, null);
            basicEffect.Projection = CameraManager.getSingleton.ActiveCamera.Projection;
            vertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice,
                                     VertexPositionColor.VertexElements);

            graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;

            //Load all objects
            pyramid = new Pyramid(Vector3.One, new Vector3(-1.0f, 0.0f, 6.0f), basicEffect, graphics);
            cube = new Cube(Vector3.One, new Vector3(1.5f, 0.0f, 7.0f), basicEffect, graphics);

            SceneManager.getSingleton.AddObject(pyramid);
            SceneManager.getSingleton.AddObject(cube);

            _objects.Add(pyramid);
            _objects.Add(cube);
            _total = _objects.Count;
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
            CameraManager.getSingleton.ActiveCamera.Update(gameTime);
            
            InputHandler.Update();

            // Update the FPS counter.
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > update)
            {
                fps = Convert.ToInt32(frames / time).ToString();
                time = 0;
                frames = 0;
            }
            ++frames;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //Move forward
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                CameraManager.getSingleton.ActiveCamera.MoveForward();
            }

            //Move Back
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                CameraManager.getSingleton.ActiveCamera.MoveBack();
            }

            //Strafe Left
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                CameraManager.getSingleton.ActiveCamera.StrafeLeft();
            }

            //Strafe Right
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                CameraManager.getSingleton.ActiveCamera.StrafeRight();
            }

            //Turn left
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                CameraManager.getSingleton.ActiveCamera.Yaw += CameraManager.getSingleton.ActiveCamera.RotationSpeed;
                //camera.RotateCameraY();
                CameraManager.getSingleton.ActiveCamera.RotateCamera();
            }

            //Turn right
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                CameraManager.getSingleton.ActiveCamera.Yaw -= CameraManager.getSingleton.ActiveCamera.RotationSpeed;
                //camera.RotateCameraY();
                CameraManager.getSingleton.ActiveCamera.RotateCamera();
            }

            //Move up
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                CameraManager.getSingleton.ActiveCamera.MoveUp();
            }

            //Move down
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                CameraManager.getSingleton.ActiveCamera.MoveDown();
            }

            //Rotate down
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                CameraManager.getSingleton.ActiveCamera.Pitch += CameraManager.getSingleton.ActiveCamera.RotationSpeed;
                //camera.RotateCameraX();
                CameraManager.getSingleton.ActiveCamera.RotateCamera();
            }

            //Rotate up
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                CameraManager.getSingleton.ActiveCamera.Pitch -= CameraManager.getSingleton.ActiveCamera.RotationSpeed;
                //camera.RotateCameraX();
                CameraManager.getSingleton.ActiveCamera.RotateCamera();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                CameraManager.getSingleton.ActiveCamera.Reset();
            }

            if (InputHandler.IsKeyPressed(Keys.Tab))
            {
                drawMode = !drawMode;

                if (graphics.GraphicsDevice.RenderState.FillMode == FillMode.Solid)
                {
                    graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
                    graphics.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
                }
                else
                {
                    graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                    graphics.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
                }
            }

            if (InputHandler.IsKeyPressed(Keys.C))
            {
                SceneManager.getSingleton.Cull = !SceneManager.getSingleton.Cull;

                if (SceneManager.getSingleton.Cull)
                {
                    culling = "On";
                }
                else
                {
                    culling = "Off";
                }
                //_cull = !_cull;
                //if (_cull)
                //{
                //    culling = "On";
                //}
                //else
                //{
                //    culling = "Off";
                //}
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

            // TODO: Add your drawing code here

            // Set the vertex declaration
            graphics.GraphicsDevice.VertexDeclaration = vertexDeclaration;


            SceneManager.getSingleton.Draw(gameTime);

            //Test
            //_culled = 0;
            //_drawn = 0;
            //foreach(SceneObject obj in _objects)
            //{
            //    if (_cull)
            //    {
            //        if (CameraManager.getSingleton.ActiveCamera.Frustum.Contains(obj.GetBoundingBoxTransformed()) == ContainmentType.Disjoint)
            //        {
            //            ++_culled;
            //        }
            //        else
            //        {
            //            obj.Draw(gameTime);
            //            ++_drawn;
            //        }
            //    }
            //    else
            //    {
            //        obj.Draw(gameTime);
            //        ++_drawn;
            //    }
            //}
            //End Test

            //pyramid.Draw(gameTime);
            //cube.Draw(gameTime);

            //*
            //Draw camera frustum
            basicEffect.World = Matrix.CreateTranslation(CameraManager.getSingleton.GetCamera("test").Position);
            //basicEffect.World = Matrix.CreateWorld(CameraManager.getSingleton.GetCamera("test").Position,
            //    CameraManager.getSingleton.GetCamera("test").LookAt,
            //    CameraManager.getSingleton.GetCamera("test").Up);
            basicEffect.View = CameraManager.getSingleton.ActiveCamera.View;
            basicEffect.Projection = CameraManager.getSingleton.ActiveCamera.Projection;
            basicEffect.VertexColorEnabled = true;

            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                // Begin the current pass
                pass.Begin();

                graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList, CameraManager.getSingleton.GetCamera("test").BoundingFrustumDrawData,
                    0, 8, CameraManager.getSingleton.GetCamera("test").BoundingFrustumIndex, 0, 12);

                //graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                //    PrimitiveType.LineList, CameraManager.getSingleton.ActiveCamera.BoundingFrustumDrawData,
                //    0, 8, CameraManager.getSingleton.ActiveCamera.BoundingFrustumIndex, 0, 12);

                // End the current pass
                pass.End();
            }
            basicEffect.End(); 
            // * */

            graphics.GraphicsDevice.RenderState.FillMode = FillMode.Solid;

            spriteBatch.Begin();

            //spriteBatch.DrawString(font, "FPS: " + fps, new Vector2(10.0f, 10.0f), Color.White);
            //spriteBatch.DrawString(font, "Culling: " + culling, new Vector2(10.0f, 30.0f), Color.White);
            //spriteBatch.DrawString(font, "Object Count: " + _total, new Vector2(10.0f, 50.0f), Color.White);
            //spriteBatch.DrawString(font, "Objects Drawn: " + _drawn, new Vector2(10.0f, 70.0f), Color.White);
            //spriteBatch.DrawString(font, "Objects Culled: " + _culled, new Vector2(10.0f, 90.0f), Color.White);

            spriteBatch.DrawString(font, "FPS: " + fps, new Vector2(10.0f, 10.0f), Color.White);
            spriteBatch.DrawString(font, "Culling: " + culling, new Vector2(10.0f, 30.0f), Color.White);
            spriteBatch.DrawString(font, "Object Count: " + SceneManager.getSingleton.SceneObjectCount, new Vector2(10.0f, 50.0f), Color.White);
            spriteBatch.DrawString(font, "Objects Drawn: " + SceneManager.getSingleton.Drawn, new Vector2(10.0f, 70.0f), Color.White);
            spriteBatch.DrawString(font, "Objects Culled: " + SceneManager.getSingleton.Culled, new Vector2(10.0f, 90.0f), Color.White);

            spriteBatch.End();

            if (drawMode)
            {
                graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
                graphics.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
            }

            base.Draw(gameTime);
        }

        /*Not Used
        public void BuildTriangle(Vector3 size, Vector3 position)
        {
            triangleTransform = Matrix.CreateTranslation(position);

            // Initialize the triangle's data (with Vertex Colors)
            triangleData = new VertexPositionColor[18]
            {
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, 1.0f) * size, Color.Blue),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, 1.0f) * size, Color.Green),
                new VertexPositionColor(position + new Vector3(0.0f, 1.0f, 0.0f) * size, Color.Red),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, -1.0f) * size, Color.Green),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, 1.0f) * size, Color.Blue),
                new VertexPositionColor(position + new Vector3(0.0f, 1.0f, 0.0f) * size, Color.Red),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, -1.0f) * size, Color.Blue),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, -1.0f) * size, Color.Green),
                new VertexPositionColor(position + new Vector3(0.0f, 1.0f, 0.0f) * size, Color.Red),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, 1.0f) * size, Color.Green),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, -1.0f) * size, Color.Blue),
                new VertexPositionColor(position + new Vector3(0.0f, 1.0f, 0.0f) * size, Color.Red),

                // Bottom Surface
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, -1.0f) * size,Color.Orange),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, 1.0f) * size,Color.Orange),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, -1.0f) * size,Color.Orange),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, -1.0f) * size,Color.Orange),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, 1.0f) * size,Color.Orange),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, 1.0f) * size,Color.Orange),
            };
        }

        public void BuildCube(Vector3 size, Vector3 position)
        {
            rectangleTransform = Matrix.CreateTranslation(position);

            // Initialize the Rectangle's data (Do not need vertex colors)
            rectangleData = new VertexPositionColor[36]
            {
                // Front Surface
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, 1.0f) * size,Color.Red),
                new VertexPositionColor(position + new Vector3(-1.0f, 1.0f, 1.0f) * size,Color.Red), 
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, 1.0f) * size,Color.Red), 
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, 1.0f) * size,Color.Red), 
                new VertexPositionColor(position + new Vector3(-1.0f, 1.0f, 1.0f) * size,Color.Red), 
                new VertexPositionColor(position + new Vector3(1.0f, 1.0f, 1.0f) * size,Color.Red),  

                // Front Surface
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, -1.0f) * size,Color.Yellow),
                new VertexPositionColor(position + new Vector3(1.0f, 1.0f, -1.0f) * size,Color.Yellow), 
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, -1.0f) * size,Color.Yellow),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, -1.0f) * size,Color.Yellow),
                new VertexPositionColor(position + new Vector3(1.0f, 1.0f, -1.0f) * size,Color.Yellow),  
                new VertexPositionColor(position + new Vector3(-1.0f, 1.0f, -1.0f) * size,Color.Yellow), 

                // Left Surface
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, -1.0f) * size,Color.Blue),
                new VertexPositionColor(position + new Vector3(-1.0f, 1.0f, -1.0f) * size,Color.Blue),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, 1.0f) * size,Color.Blue),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, 1.0f) * size,Color.Blue),
                new VertexPositionColor(position + new Vector3(-1.0f, 1.0f, -1.0f) * size,Color.Blue),
                new VertexPositionColor(position + new Vector3(-1.0f, 1.0f, 1.0f) * size,Color.Blue),

                // Right Surface
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, 1.0f) * size,Color.Violet),
                new VertexPositionColor(position + new Vector3(1.0f, 1.0f, 1.0f) * size,Color.Violet),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, -1.0f) * size,Color.Violet),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, -1.0f) * size,Color.Violet),
                new VertexPositionColor(position + new Vector3(1.0f, 1.0f, 1.0f) * size,Color.Violet),
                new VertexPositionColor(position + new Vector3(1.0f, 1.0f, -1.0f) * size,Color.Violet),

                // Top Surface
                new VertexPositionColor(position + new Vector3(-1.0f, 1.0f, 1.0f) * size,Color.Green),
                new VertexPositionColor(position + new Vector3(-1.0f, 1.0f, -1.0f) * size,Color.Green),
                new VertexPositionColor(position + new Vector3(1.0f, 1.0f, 1.0f) * size,Color.Green),
                new VertexPositionColor(position + new Vector3(1.0f, 1.0f, 1.0f) * size,Color.Green),
                new VertexPositionColor(position + new Vector3(-1.0f, 1.0f, -1.0f) * size,Color.Green),
                new VertexPositionColor(position + new Vector3(1.0f, 1.0f, -1.0f) * size,Color.Green),

                // Bottom Surface
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, -1.0f) * size,Color.Orange),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, 1.0f) * size,Color.Orange),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, -1.0f) * size,Color.Orange),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, -1.0f) * size,Color.Orange),
                new VertexPositionColor(position + new Vector3(-1.0f, -1.0f, 1.0f) * size,Color.Orange),
                new VertexPositionColor(position + new Vector3(1.0f, -1.0f, 1.0f) * size,Color.Orange),
            };
        }
         * */

        public void BuildTestCameraFrustum()
        {
            BasicCamera testCamera = (BasicCamera)CameraManager.getSingleton.GetCamera("test");

            Plane top = testCamera.Frustum.Top;
            Plane bottom = testCamera.Frustum.Bottom;
            Plane left = testCamera.Frustum.Left;
            Plane right = testCamera.Frustum.Right;
            Plane near = testCamera.Frustum.Near;
            Plane far = testCamera.Frustum.Far;            
        }
    }
}
