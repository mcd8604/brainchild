using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace OctreeCulling
{
    class Cube : SceneObject
    {
        private Matrix _rectangleTransform;
        public Matrix RectangleTransform
        {
            get { return _rectangleTransform; }
            set { _rectangleTransform = value; }
        }

        private VertexPositionColor[] _rectangleData;
        public VertexPositionColor[] RectangleData
        {
            get { return _rectangleData; }
        }

        public Cube(Vector3 size, Vector3 position, BasicEffect effect, GraphicsDeviceManager graphics)
        {
            Effect = effect;
            Graphics = graphics;

            _rectangleTransform = Matrix.CreateTranslation(position);

            // Initialize the Rectangle's data (Do not need vertex colors)
            _rectangleData = new VertexPositionColor[36]
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

            CreateBoundingSphere();
            CreateBoundingBox();
        }

        protected override void CreateBoundingSphere()
        {
            
        }

        protected override void CreateBoundingBox()
        {

        }

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);

            Effect.World = _rectangleTransform;
            Effect.View = CameraManager.getSingleton.ActiveCamera.View;
            Effect.Projection = CameraManager.getSingleton.ActiveCamera.Projection;
            Effect.VertexColorEnabled = true;

            Effect.Begin();
            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                // Begin the current pass
                pass.Begin();

                // Draw the six different surfaces of the cube
                Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList, _rectangleData, 0, 12);

                pass.End();
            }
            Effect.End();
        }
    }
}
