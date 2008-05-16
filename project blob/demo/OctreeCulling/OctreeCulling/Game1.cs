/*  Author: Josh Wilson
 *  
 *  Credits: Nathan Levesque for Octree: 
 *      http://rhysyngsun.spaces.live.com/Blog/cns!FBBD62480D87119D!129.entry
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // The basic effect to use when drawing the geometry
        private BasicEffect basicEffect;
        private VertexDeclaration vertexDeclaration;

        bool drawMode = true;

        string culling = "Off";

        Pyramid pyramid;
        Cube cube;

        SpriteFont font;

        //FPS counter variables
        //float time = 0f;
        TimeSpan time;
        //float fpsUpdate = 1.0f;
        int frames = 0;
        int fps = 0;

        //Test variables
        List<SceneObject> _objects;
        List<SceneObject> _listGraphObjects;
        List<Portal> _portalObjects;

        bool _cull = true;//false;
        int _culled = 0;
        int _drawn = 0;
        int _total = 0;

        int graphType = 1;

        //Matrix objScaleMatrix;
        //float objScale = 0.005f;

        Stopwatch timer;

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
            _listGraphObjects = new List<SceneObject>();
            _portalObjects = new List<Portal>();

            TestCamera camera = new TestCamera(graphics.GraphicsDevice.Viewport);
            camera.Position = new Vector3(0.0f, 10.0f, -40.0f);
            //camera.Position = new Vector3(250.0f, 250.0f, 2000.0f);
            CameraManager.getSingleton.AddCamera("default", camera);

            camera = new TestCamera(graphics.GraphicsDevice.Viewport);
            camera.FarPlane = 300.0f;
            camera.Position = new Vector3(0.0f, 0.0f, 0.0f);
            CameraManager.getSingleton.AddCamera("test", camera);

            CameraManager.getSingleton.SetActiveCamera("default");

            //Used to track the amount of time it takes to cull and draw all the objects
            timer = new Stopwatch();

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

            // TODO: use this.Content to load your game content here
            basicEffect = new BasicEffect(graphics.GraphicsDevice, null);
            basicEffect.Projection = CameraManager.getSingleton.ActiveCamera.Projection;
            vertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice,
                                     VertexPositionColor.VertexElements);

            graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;

            List<int> tempList1 = new List<int>();
            List<int> tempList2 = new List<int>();
            List<int> tempList3 = new List<int>();
            List<int> tempList4 = new List<int>();
            List<int> tempList5 = new List<int>();
            List<int> tempList6 = new List<int>();
            List<int> tempList7 = new List<int>();
            tempList1.Add(1);
            tempList2.Add(2);
            tempList3.Add(1);
            tempList3.Add(2);
            tempList4.Add(3);
            tempList5.Add(2);
            tempList5.Add(3);
            tempList6.Add(4);
            tempList7.Add(2);
            tempList7.Add(4);

            //Load all objects
            for (int i = 0; i < 2; ++i)//30
            {
                for (int j = 0; j < 2; ++j)//30
                {
                    for (int k = 0; k < 2; ++k)//30
                    {
                        //cube = new Cube(Vector3.One, new Vector3(5 * i, 5* j, 5 * k), basicEffect, graphics);
                        ////pyramid = new Pyramid(Vector3.One, new Vector3(5 * i, 5 * j, 5 * k), basicEffect, graphics);

                        //_objects.Add(cube);
                        ////_objects.Add(pyramid);

                        cube = new Cube(Vector3.One, new Vector3(5 * i - 1, 5 * j - 1, 5 * k), basicEffect, graphics, tempList1);
                        _objects.Add(cube);

                        cube = new Cube(Vector3.One, new Vector3(5 * i - 1, 5 * j - 1, 5 * k + 20), basicEffect, graphics, tempList2);
                        _objects.Add(cube);

                        cube = new Cube(Vector3.One, new Vector3(5 * i - 1, 5 * j - 1, 5 * k + 40), basicEffect, graphics, tempList4);
                        _objects.Add(cube);

                        cube = new Cube(Vector3.One, new Vector3(5 * i + 13, 5 * j - 1, 5 * k + 25), basicEffect, graphics, tempList6);
                        _objects.Add(cube);
                    }
                    
                }

            }
            #region room1
            //Near wall
            cube = new Cube(new Vector3(10f, 10f, 0.3f), new Vector3(0, 0, -3), basicEffect, graphics, tempList1);
            _objects.Add(cube);

            //Left wall
            cube = new Cube(new Vector3(0.3f, 10f, 10f), new Vector3(10, 0, 7), basicEffect, graphics, tempList1);
            _objects.Add(cube);

            //Right wall
            cube = new Cube(new Vector3(0.3f, 10f, 10f), new Vector3(-10, 0, 7), basicEffect, graphics, tempList1);
            _objects.Add(cube);
                        
            //Ceiling
            cube = new Cube(new Vector3(10f, 0.3f, 10f), new Vector3(0, -10, 7), basicEffect, graphics, tempList1);
            _objects.Add(cube);

            //Floor
            cube = new Cube(new Vector3(10f, 0.3f, 10f), new Vector3(0, 10, 7), basicEffect, graphics, tempList1);
            _objects.Add(cube);

            //Far wall pieces (in rooms 1 and 2)
            cube = new Cube(new Vector3(4f, 10f, 0.3f), new Vector3(-6, 0, 17), basicEffect, graphics, tempList3);
            _objects.Add(cube);

            cube = new Cube(new Vector3(4f, 10f, 0.3f), new Vector3(6, 0, 17), basicEffect, graphics, tempList3);
            _objects.Add(cube);

            //cube = new Cube(new Vector3(2f, 4f, 0.3f), new Vector3(0, 6, 17), basicEffect, graphics, tempList3);
            //_objects.Add(cube);

            //cube = new Cube(new Vector3(2f, 4f, 0.3f), new Vector3(0, -6, 17), basicEffect, graphics, tempList3);
            //_objects.Add(cube);
            #endregion
            #region room2
            //Near wall
            //cube = new Cube(new Vector3(10f, 10f, 0.3f), new Vector3(0, 0, 17), basicEffect, graphics, tempList2);
            //_objects.Add(cube);

            //Left wall
            //cube = new Cube(new Vector3(0.3f, 10f, 10f), new Vector3(10, 0, 27), basicEffect, graphics, tempList2);
            //_objects.Add(cube);

            //Right wall
            cube = new Cube(new Vector3(0.3f, 10f, 10f), new Vector3(-10, 0, 27), basicEffect, graphics, tempList2);
            _objects.Add(cube);

            //Far wall
            //cube = new Cube(new Vector3(10f, 10f, 0.3f), new Vector3(0, 0, 37), basicEffect, graphics, tempList2);
            //_objects.Add(cube);

            //Ceiling
            cube = new Cube(new Vector3(10f, 0.3f, 10f), new Vector3(0, -10, 27), basicEffect, graphics, tempList2);
            _objects.Add(cube);

            //Floor
            cube = new Cube(new Vector3(10f, 0.3f, 10f), new Vector3(0, 10, 27), basicEffect, graphics, tempList2);
            _objects.Add(cube);
            #endregion
            #region room3
            //Far wall
            cube = new Cube(new Vector3(10f, 10f, 0.3f), new Vector3(0, 0, 57), basicEffect, graphics, tempList4);
            _objects.Add(cube);

            //Left wall
            cube = new Cube(new Vector3(0.3f, 10f, 10f), new Vector3(10, 0, 47), basicEffect, graphics, tempList4);
            _objects.Add(cube);

            //Right wall
            cube = new Cube(new Vector3(0.3f, 10f, 10f), new Vector3(-10, 0, 47), basicEffect, graphics, tempList4);
            _objects.Add(cube);

            //Ceiling
            cube = new Cube(new Vector3(10f, 0.3f, 10f), new Vector3(0, -10, 47), basicEffect, graphics, tempList4);
            _objects.Add(cube);

            //Floor
            cube = new Cube(new Vector3(10f, 0.3f, 10f), new Vector3(0, 10, 47), basicEffect, graphics, tempList4);
            _objects.Add(cube);

            //Near wall pieces (in rooms 2 and 3)
            cube = new Cube(new Vector3(4f, 10f, 0.3f), new Vector3(-6, 0, 37), basicEffect, graphics, tempList5);
            _objects.Add(cube);

            cube = new Cube(new Vector3(4f, 10f, 0.3f), new Vector3(6, 0, 37), basicEffect, graphics, tempList5);
            _objects.Add(cube);

            cube = new Cube(new Vector3(2f, 4f, 0.3f), new Vector3(0, 6, 37), basicEffect, graphics, tempList5);
            _objects.Add(cube);

            cube = new Cube(new Vector3(2f, 4f, 0.3f), new Vector3(0, -6, 37), basicEffect, graphics, tempList5);
            _objects.Add(cube);
            #endregion
            #region room4
            //Far wall
            cube = new Cube(new Vector3(10f, 10f, 0.3f), new Vector3(20, 0, 37), basicEffect, graphics, tempList6);
            _objects.Add(cube);

            //Left wall
            cube = new Cube(new Vector3(0.3f, 10f, 10f), new Vector3(30, 0, 27), basicEffect, graphics, tempList6);
            _objects.Add(cube);

            //Right wall
            //cube = new Cube(new Vector3(0.3f, 10f, 10f), new Vector3(10, 0, 27), basicEffect, graphics, tempList6);
            //_objects.Add(cube);

            //Ceiling
            cube = new Cube(new Vector3(10f, 0.3f, 10f), new Vector3(20, -10, 27), basicEffect, graphics, tempList6);
            _objects.Add(cube);

            //Floor
            cube = new Cube(new Vector3(10f, 0.3f, 10f), new Vector3(20, 10, 27), basicEffect, graphics, tempList6);
            _objects.Add(cube);

            //Right wall pieces (in rooms 2 and 4)
            cube = new Cube(new Vector3(0.3f, 10f, 4f), new Vector3(10, 0, 21), basicEffect, graphics, tempList7);
            _objects.Add(cube);

            cube = new Cube(new Vector3(0.3f, 10f, 4f), new Vector3(10, 0, 33), basicEffect, graphics, tempList7);
            _objects.Add(cube);

            cube = new Cube(new Vector3(0.3f, 4f, 2f), new Vector3(10, 6, 27), basicEffect, graphics, tempList7);
            _objects.Add(cube);

            cube = new Cube(new Vector3(0.3f, 4f, 2f), new Vector3(10, -6, 27), basicEffect, graphics, tempList7);
            _objects.Add(cube);
            #endregion

            //Doorway cube object (will become a portal)
			//cube = new Cube(new Vector3(2f, 10f, 0.3f), new Vector3(0, 0, 17), basicEffect, graphics, tempList1);
			//_objects.Add(cube);

            //cube = new Cube(new Vector3(2f, 2f, 0.3f), new Vector3(0, 0, 37), basicEffect, graphics, tempList4);
            //_objects.Add(cube);

            //cube = new Cube(new Vector3(0.3f, 2f, 2f), new Vector3(0, 0, 27), basicEffect, graphics, tempList6);
            //_objects.Add(cube);

            Portal portal = new Portal(new Vector3(2f, 10f, 0f), new Vector3(0, 0, 17));
            //portal.Position = new Vector3(0, 0, 17);
            //portal.Scale = new Vector3(2f, 2f, 0.3f);
            portal.ConnectedSectors.Add(1);//SceneManager.getSingleton.PortalScene.Sectors[1]);
            portal.ConnectedSectors.Add(2);//SceneManager.getSingleton.PortalScene.Sectors[2]);
            _portalObjects.Add(portal);

            portal = new Portal(new Vector3(2f, 2f, 0f), new Vector3(0, 0, 37));
            portal.ConnectedSectors.Add(2);
            portal.ConnectedSectors.Add(3);
            _portalObjects.Add(portal);

            portal = new Portal(new Vector3(0f, 2f, 2f), new Vector3(10, 0, 27));
            portal.ConnectedSectors.Add(2);
            portal.ConnectedSectors.Add(4);
            _portalObjects.Add(portal);

            _listGraphObjects = new List<SceneObject>(_objects);

            //Set scenegraph to Portal structure
            SceneManager.getSingleton.GraphType = SceneManager.SceneGraphType.Portal;

            SceneManager.getSingleton.Distribute(_objects);
            SceneManager.getSingleton.DistributePortals(_portalObjects);
            _total = _listGraphObjects.Count;
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
            //CameraManager.getSingleton.ActiveCamera.Update(gameTime);
            CameraManager.getSingleton.Update(gameTime);
            CameraManager.getSingleton.GetCamera("test").Update(gameTime);
            
            InputHandler.Update();

            #region fps counter
            time += gameTime.ElapsedGameTime;

            if (time > TimeSpan.FromSeconds(1))
            {
                time -= TimeSpan.FromSeconds(1);
                fps = frames;
                frames = 0;
            }
            #endregion

            Vector3 translation = Vector3.Zero;
            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //Move forward
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                CameraManager.getSingleton.ActiveCamera.Translate(new Vector3(0.0f, 0.0f, 0.2f));
            }

            //Move Back
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                CameraManager.getSingleton.ActiveCamera.Translate(new Vector3(0.0f, 0.0f, -0.2f));
            }

            //Strafe Left
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                CameraManager.getSingleton.ActiveCamera.Translate(new Vector3(0.2f, 0.0f, 0.0f));
            }

            //Strafe Right
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                CameraManager.getSingleton.ActiveCamera.Translate(new Vector3(-0.2f, 0.0f, 0.0f));
            }

            //Move up
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                CameraManager.getSingleton.ActiveCamera.Translate(new Vector3(0.0f, 0.2f, 0.0f));
            }

            //Move down
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                CameraManager.getSingleton.ActiveCamera.Translate(new Vector3(0.0f, -0.2f, 0.0f));
            }

            //Turn left
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                CameraManager.getSingleton.ActiveCamera.RotateY(1.0f);
            }

            //Turn right
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                CameraManager.getSingleton.ActiveCamera.RotateY(-1.0f);
            }

            //Rotate down
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                CameraManager.getSingleton.ActiveCamera.RotateX(1.0f);
            }

            //Rotate up
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                CameraManager.getSingleton.ActiveCamera.RotateX(-1.0f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //CameraManager.getSingleton.ActiveCamera.Reset();
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
                //OctreeManager.getSingleton.Cull = !OctreeManager.getSingleton.Cull;
                SceneManager.getSingleton.Cull = !SceneManager.getSingleton.Cull;
                
                _cull = !_cull;

                //if (OctreeManager.getSingleton.Cull)
                if (SceneManager.getSingleton.Cull)
                {
                    culling = "On";
                }
                else
                {
                    culling = "Off";
                }
                //_cull = !_cull;
                //    if (_cull)
                //    {
                //        culling = "On";
                //    }
                //    else
                //    {
                //        culling = "Off";
                //    }
            }

            if (InputHandler.IsKeyPressed(Keys.M))
            {
                if (graphType == 0)
                    graphType = 1;
                else if (graphType == 1)
                    graphType = 0;
            }

            //Turn left
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                CameraManager.getSingleton.GetCamera("test").RotateY(1.0f);
            }

            //Turn right
            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                CameraManager.getSingleton.GetCamera("test").RotateY(-1.0f);
            }

            //Move forward
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                CameraManager.getSingleton.GetCamera("test").Translate(new Vector3(0.0f, 0.0f, 0.1f));
            }

            //Move Back
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                CameraManager.getSingleton.GetCamera("test").Translate(new Vector3(0.0f, 0.0f, -0.1f));
            }

            //Move up
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
            {
                CameraManager.getSingleton.GetCamera("test").Translate(new Vector3(0.0f, 0.1f, 0.0f));
            }

            //Move down
            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                CameraManager.getSingleton.GetCamera("test").Translate(new Vector3(0.0f, -0.1f, 0.0f));
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

            //timer.Start();
            //SceneManager.getSingleton.Draw(gameTime);
            //timer.Stop();

            if (graphType == 1)
            {
                timer.Start();
                //OctreeManager.getSingleton.Draw(gameTime);
                SceneManager.getSingleton.Draw(gameTime);
                timer.Stop();
            }
            else if (graphType == 0)
            {
                timer.Start();
                //Test
                _culled = 0;
                _drawn = 0;
                foreach (SceneObject obj in _listGraphObjects)
                {
                    if (_cull)
                    {
                        //if (CameraManager.getSingleton.ActiveCamera.Frustum.Contains(obj.GetBoundingBoxTransformed()) == ContainmentType.Disjoint)
                        ContainmentType type = CameraManager.getSingleton.GetCamera("test").Frustum.Contains(obj.GetBoundingBoxTransformed());
                        if (type == ContainmentType.Disjoint)
                        //if (CameraManager.getSingleton.GetCamera("test").Frustum.Contains(obj.GetBoundingBoxTransformed()) == ContainmentType.Disjoint)
                        {
                            ++_culled;
                        }
                        else
                        {
                            obj.Draw(gameTime);
                            ++_drawn;
                        }
                    }
                    else
                    {
                        obj.Draw(gameTime);
                        ++_drawn;
                    }
                }
                //End Test
                timer.Stop();
            }

            //*
            //Draw camera frustum
            basicEffect.World = Matrix.Identity;
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

				//if (SceneManager.getSingleton.PortalScene.Sectors[SceneManager.getSingleton.PortalScene.CurrSector].DrawPortal)
				//{
				//    graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
				//        PrimitiveType.LineList, SceneManager.getSingleton.PortalScene.Sectors[1].BoundingFrustumDrawData,
				//        0, 8, SceneManager.getSingleton.PortalScene.Sectors[1].BoundingFrustumIndex, 0, 12);
				//}

                // End the current pass
                pass.End();
            }
            basicEffect.End(); 
            // * */

            graphics.GraphicsDevice.RenderState.FillMode = FillMode.Solid;

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "FPS: " + fps, new Vector2(10.0f, 10.0f), Color.White);
            spriteBatch.DrawString(font, "Culling: " + culling, new Vector2(10.0f, 30.0f), Color.White);
            if (graphType == 0)
            {
                spriteBatch.DrawString(font, "Object Count: " + _total, new Vector2(10.0f, 50.0f), Color.White);
                spriteBatch.DrawString(font, "Objects Drawn: " + _drawn, new Vector2(10.0f, 70.0f), Color.White);
                spriteBatch.DrawString(font, "Objects Culled: " + _culled, new Vector2(10.0f, 90.0f), Color.White);
                spriteBatch.DrawString(font, "Graph Mode: List", new Vector2(10.0f, 150.0f), Color.White);
            }
            else if (graphType == 1)
            {
                //int culled = OctreeManager.getSingleton.SceneObjectCount - OctreeManager.getSingleton.Drawn;
                int culled = SceneManager.getSingleton.SceneObjectCount - SceneManager.getSingleton.Drawn;
                //spriteBatch.DrawString(font, "Object Count: " + OctreeManager.getSingleton.SceneObjectCount, new Vector2(10.0f, 50.0f), Color.White);
                //spriteBatch.DrawString(font, "Objects Drawn: " + OctreeManager.getSingleton.Drawn, new Vector2(10.0f, 70.0f), Color.White);
                //spriteBatch.DrawString(font, "Objects Culled: " + culled.ToString(), new Vector2(10.0f, 90.0f), Color.White);
                spriteBatch.DrawString(font, "Object Count: " + SceneManager.getSingleton.SceneObjectCount, new Vector2(10.0f, 50.0f), Color.White);
                spriteBatch.DrawString(font, "Objects Drawn: " + SceneManager.getSingleton.Drawn, new Vector2(10.0f, 70.0f), Color.White);
                spriteBatch.DrawString(font, "Objects Culled: " + culled.ToString(), new Vector2(10.0f, 90.0f), Color.White);
                spriteBatch.DrawString(font, "Graph Mode: " + SceneManager.getSingleton.GraphType.ToString(), new Vector2(10.0f, 150.0f), Color.White);
            }
            spriteBatch.DrawString(font, "Camera Position: {" +
                CameraManager.getSingleton.GetCamera("test").Position.X + ", " +
                CameraManager.getSingleton.GetCamera("test").Position.Y + ", " +
                CameraManager.getSingleton.GetCamera("test").Position.Z + "}", new Vector2(10.0f, 110.0f), Color.White);

            //spriteBatch.DrawString(font, "Object Count: " + SceneManager.getSingleton.SceneObjectCount, new Vector2(10.0f, 50.0f), Color.White);
            //spriteBatch.DrawString(font, "Objects Drawn: " + SceneManager.getSingleton.Drawn, new Vector2(10.0f, 70.0f), Color.White);
            //spriteBatch.DrawString(font, "Objects Culled: " + SceneManager.getSingleton.Culled, new Vector2(10.0f, 90.0f), Color.White);

            spriteBatch.DrawString(font, "Time to Cull+Draw: " + timer.Elapsed.TotalMilliseconds + " milliseconds", new Vector2(10.0f, 130.0f), Color.White);
            
            spriteBatch.End();

            if (drawMode)
            {
                graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
                graphics.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
            }

            //Update frames counter for FPS
            ++frames;

            timer.Reset();

            base.Draw(gameTime);
        }
    }
}
