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

        //private Vector3 shapeSize;
        //private Vector3 shapePosition;
        //private VertexPositionColor[] shapeVertices;
        //private VertexPositionNormalTexture[] shapeVertices;
        //private int shapeTriangles;
        //private VertexBuffer shapeBuffer;

        //BasicEffect effect;
        Matrix worldMatrix;

        //CameraManager _cameraManager;
        BasicCamera camera;

        // Add transforms for the triangle
        private Matrix triangleTransform;
        private VertexPositionColor[] triangleData;

        // The basic effect to use when drawing the geometry
        private BasicEffect basicEffect;
        private VertexDeclaration vertexDeclaration;

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

            worldMatrix = Matrix.Identity;

            camera = new BasicCamera();

            BuildTriangle(Vector3.One, Vector3.One);
            
            //BuildCube(Vector3.One, Vector3.One);

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

            // TODO: use this.Content to load your game content here
            basicEffect = new BasicEffect(graphics.GraphicsDevice, null);
            basicEffect.Projection = camera.Projection;
            vertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice,
                                     VertexPositionColor.VertexElements);
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

            //Move forward
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                camera.MoveForward();

            //Move Back
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                camera.MoveBack();

            //Strafe Left
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                camera.StrafeLeft();

            //Strafe Right
            if (Keyboard.GetState().IsKeyDown(Keys.E))
                camera.StrafeRight();

            //Turn left
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                camera._yaw += camera.rotationSpeed;
                //camera.RotateCameraY();
                camera.RotateCamera();
            }

            //Turn right
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                camera._yaw -= camera.rotationSpeed;
                //camera.RotateCameraY();
                camera.RotateCamera();
            }

            //Move up
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camera.MoveUp();
            }

            //Move down
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camera.MoveDown();
            }

            //Rotate down
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                camera._pitch += camera.rotationSpeed;
                //camera.RotateCameraX();
                camera.RotateCamera();
            }

            //Rotate up
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                camera._pitch -= camera.rotationSpeed;
                //camera.RotateCameraX();
                camera.RotateCamera();
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

            // Set the transform for the triangle, then draw it, using the created effect
            Matrix tempTransform = Matrix.CreateRotationY(0) * triangleTransform;
            basicEffect.World = tempTransform;
            basicEffect.VertexColorEnabled = true;
            basicEffect.View = camera.GetViewMatrix();
            basicEffect.Projection = camera.Projection;

            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                // Begin the current pass
                pass.Begin();

                graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList, triangleData, 0, 4);

                // End the current pass
                pass.End();
            }

            basicEffect.End();

            //effect.Begin();
            //graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, shapeTriangles);
            //effect.End();

            base.Draw(gameTime);
        }

        public void BuildCube(Vector3 size, Vector3 position)
        {
            //shapeSize = size;
            //shapePosition = position;
            ////shapeTriangles = 12;
            ////shapeVertices = new VertexPositionNormalTexture[36];
            //shapeVertices = new VertexPositionColor[8];

            //Vector3 topLeftFront = shapePosition + new Vector3(-1.0f, 1.0f, -1.0f) * shapeSize;
            //Vector3 bottomLeftFront = shapePosition + new Vector3(-1.0f, -1.0f, -1.0f) * shapeSize;
            //Vector3 topRightFront = shapePosition + new Vector3(1.0f, 1.0f, -1.0f) * shapeSize;
            //Vector3 bottomRightFront = shapePosition + new Vector3(1.0f, -1.0f, -1.0f) * shapeSize;
            //Vector3 topLeftBack = shapePosition + new Vector3(-1.0f, 1.0f, 1.0f) * shapeSize;
            //Vector3 topRightBack = shapePosition + new Vector3(1.0f, 1.0f, 1.0f) * shapeSize;
            //Vector3 bottomLeftBack = shapePosition + new Vector3(-1.0f, -1.0f, 1.0f) * shapeSize;
            //Vector3 bottomRightBack = shapePosition + new Vector3(1.0f, -1.0f, 1.0f) * shapeSize;

            //shapeVertices[0] = new VertexPositionColor(topLeftFront, Color.Red);
            //shapeVertices[1] = new VertexPositionColor(bottomLeftFront, Color.Blue);
            //shapeVertices[2] = new VertexPositionColor(topRightFront, Color.Yellow);
            //shapeVertices[3] = new VertexPositionColor(bottomRightFront, Color.Purple);
            //shapeVertices[4] = new VertexPositionColor(topLeftBack, Color.Green);
            //shapeVertices[5] = new VertexPositionColor(topRightBack, Color.Red);
            //shapeVertices[6] = new VertexPositionColor(bottomLeftBack, Color.Blue);
            //shapeVertices[7] = new VertexPositionColor(bottomRightBack, Color.Yellow);
        }

        public void BuildTriangle(Vector3 size, Vector3 position)
        {
            triangleTransform = Matrix.CreateTranslation(new Vector3(-1.5f, 0.0f, 6.0f));

            // Initialize the triangle's data (with Vertex Colors)
            triangleData = new VertexPositionColor[12]
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
            };
        }
    }
}
