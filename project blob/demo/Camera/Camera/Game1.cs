/*  Author: Josh Wilson
 * 
 *  Credits: Jeromy Walsh for the Pyramid and Cube. The tutorial that includes these can be found here:
 *      http://www.gamedev.net/community/forums/topic.asp?topic_id=464662
 * 
 * */

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

        Camera camera1;
        Camera camera2;

        Camera activeCamera;

        // Transform and Data for the triangle
        private Matrix triangleTransform;
        private VertexPositionColor[] triangleData;

        // Transform and Data for the Cube
        private Matrix rectangleTransform;
        private VertexPositionColor[] rectangleData;

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
            camera1 = new Camera(graphics);
            camera2 = new Camera(graphics);
            activeCamera = camera1;

            triangleTransform = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 6.0f));
            rectangleTransform = Matrix.CreateTranslation(new Vector3(1.5f, 0.0f, 7.0f));

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

            // Initialize the Rectangle's data (Do not need vertex colors)
            rectangleData = new VertexPositionColor[36]
            {
                // Front Surface
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f),Color.Red),
                new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f),Color.Red), 
                new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f),Color.Red), 
                new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f),Color.Red), 
                new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f),Color.Red), 
                new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f),Color.Red),  

                // Front Surface
                new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f),Color.Yellow),
                new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f),Color.Yellow), 
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f),Color.Yellow),
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f),Color.Yellow),
                new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f),Color.Yellow),  
                new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f),Color.Yellow), 

                // Left Surface
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f),Color.Blue),
                new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f),Color.Blue),
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f),Color.Blue),
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f),Color.Blue),
                new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f),Color.Blue),
                new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f),Color.Blue),

                // Right Surface
                new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f),Color.Violet),
                new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f),Color.Violet),
                new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f),Color.Violet),
                new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f),Color.Violet),
                new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f),Color.Violet),
                new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f),Color.Violet),

                // Top Surface
                new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f),Color.Green),
                new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f),Color.Green),
                new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f),Color.Green),
                new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f),Color.Green),
                new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f),Color.Green),
                new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f),Color.Green),

                // Bottom Surface
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f),Color.Orange),
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f),Color.Orange),
                new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f),Color.Orange),
                new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f),Color.Orange),
                new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f),Color.Orange),
                new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f),Color.Orange),
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
                basicEffect.Projection = camera1.GetProjectionMatrix(); 
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
                activeCamera.MoveForward();

            //Move Back
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                activeCamera.MoveBack();

            //Strafe Left
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                activeCamera.StrafeLeft();

            //Strafe Right
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                activeCamera.StrafeRight();

            //Turn left
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                activeCamera.turnAmt += camera1.rotationSpeed;
                activeCamera.RotateCamera();
            }

            //Turn right
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                activeCamera.turnAmt -= camera1.rotationSpeed;
                activeCamera.RotateCamera();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                activeCamera = camera1;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                activeCamera = camera2;
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
            Matrix tempTransform = triangleTransform; //Matrix.CreateRotationY(0) * 
            basicEffect.World = tempTransform;
            basicEffect.VertexColorEnabled = true;
            basicEffect.View = activeCamera.GetViewMatrix();
            basicEffect.Projection = activeCamera.GetProjectionMatrix();

            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                // Begin the current pass
                pass.Begin();

                // Draw the Pyramid
                graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList, triangleData, 0, 4);

                tempTransform = rectangleTransform;

                basicEffect.World = tempTransform;
                basicEffect.CommitChanges();

                // Draw the Cube
                graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList, rectangleData, 0, 12);

                // End the current pass
                pass.End();
            }

            basicEffect.End(); 

            base.Draw(gameTime);
        }
    }
}
