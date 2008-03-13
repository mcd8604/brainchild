#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace Camera
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ContentManager content;

        Camera camera;

        // Add transforms for the triangle
        private Matrix triangleTransform;
        private VertexPositionColor[] triangleData;

        // The basic effect to use when drawing the geometry
        private BasicEffect basicEffect;
        private VertexDeclaration vertexDeclaration;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            //Instantiate a new Camera class
            camera = new Camera(graphics);

            triangleTransform = Matrix.CreateTranslation(new Vector3(-1.5f, 0.0f, -6.0f));

            // Initialize the triangle's data (with Vertex Colors)
            triangleData = new VertexPositionColor[12]
            {
                new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.Blue),
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Green),
                new VertexPositionColor(new Vector3(0.0f, 1.0f, 0.0f), Color.Red),
                new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.Green),
                new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.Blue),
                new VertexPositionColor(new Vector3(0.0f, 1.0f, 0.0f), Color.Red),
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Blue),
                new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.Green),
                new VertexPositionColor(new Vector3(0.0f, 1.0f, 0.0f), Color.Red),
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Green),
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Blue),
                new VertexPositionColor(new Vector3(0.0f, 1.0f, 0.0f), Color.Red),
            };

            base.Initialize();
        }


        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                // Create the effect which will be used to set matrices, and colors for rendering
                basicEffect = new BasicEffect(graphics.GraphicsDevice, null);
                basicEffect.Projection = camera.GetProjectionMatrix(); 
            }

            vertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice,
                                     VertexPositionColor.VertexElements);
        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
            {
                // TODO: Unload any ResourceManagementMode.Automatic content
                content.Unload();
            }

            // TODO: Unload any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
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
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                camera.StrafeLeft();

            //Strafe Right
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                camera.StrafeRight();

            //Turn left
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camera.turnAmt += camera.rotationSpeed;
                camera.RotateCamera();
            }

            //Turn right
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camera.turnAmt -= camera.rotationSpeed;
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

            // Set the vertex declaration
            graphics.GraphicsDevice.VertexDeclaration = vertexDeclaration;

            // Set the transform for the triangle, then draw it, using the created effect
            Matrix tempTransform = Matrix.CreateRotationY(0) * triangleTransform;
            basicEffect.World = tempTransform;
            basicEffect.VertexColorEnabled = true;
            basicEffect.View = camera.GetViewMatrix();
            basicEffect.Projection = camera.GetProjectionMatrix();

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

            base.Draw(gameTime);
        }
    }
}
