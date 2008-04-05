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

            Position = position;
            Scale = size;

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
            BoundingBox = new BoundingBox(new Vector3(-1.0f, -1.0f, -1.0f) * Scale, new Vector3(1.0f, 1.0f, 1.0f) * Scale);

            BoundingBoxDrawData = new VertexPositionColor[8]
            {
                new VertexPositionColor(Position + new Vector3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Min.Z), Color.Red),
                new VertexPositionColor(Position + new Vector3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Min.Z), Color.Red),
                new VertexPositionColor(Position + new Vector3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Max.Z), Color.Red),
                new VertexPositionColor(Position + new Vector3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Max.Z), Color.Red),
                new VertexPositionColor(Position + new Vector3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Min.Z), Color.Red),
                new VertexPositionColor(Position + new Vector3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Min.Z), Color.Red),
                new VertexPositionColor(Position + new Vector3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z), Color.Red),
                new VertexPositionColor(Position + new Vector3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Max.Z), Color.Red)
            };

            BoundingBoxIndex = new int[24];
            BoundingBoxIndex[0] = 0;
            BoundingBoxIndex[1] = 1;
            BoundingBoxIndex[2] = 1;
            BoundingBoxIndex[3] = 2;
            BoundingBoxIndex[4] = 2;
            BoundingBoxIndex[5] = 3;
            BoundingBoxIndex[6] = 3;
            BoundingBoxIndex[7] = 0;

            BoundingBoxIndex[8] = 4;
            BoundingBoxIndex[9] = 5;
            BoundingBoxIndex[10] = 5;
            BoundingBoxIndex[11] = 6;
            BoundingBoxIndex[12] = 6;
            BoundingBoxIndex[13] = 7;
            BoundingBoxIndex[14] = 7;
            BoundingBoxIndex[15] = 4;

            BoundingBoxIndex[16] = 0;
            BoundingBoxIndex[17] = 4;
            BoundingBoxIndex[18] = 1;
            BoundingBoxIndex[19] = 5;
            BoundingBoxIndex[20] = 2;
            BoundingBoxIndex[21] = 6;
            BoundingBoxIndex[22] = 3;
            BoundingBoxIndex[23] = 7;
        }

        public override BoundingBox GetBoundingBoxTransformed()
        {
            Vector3 min, max;
            min = BoundingBox.Min;
            max = BoundingBox.Max;

            min = Vector3.Transform(BoundingBox.Min, Matrix.CreateTranslation(Position));
            max = Vector3.Transform(BoundingBox.Max, Matrix.CreateTranslation(Position));

            return new BoundingBox(min, max);
        }

        public override void Draw(GameTime gameTime)
        {
            OctreeManager.getSingleton.Drawn += 1;

            Effect.World = Matrix.Identity;
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

                Graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList, BoundingBoxDrawData, 0, 8, BoundingBoxIndex, 0, 12);

                pass.End();
            }
            Effect.End();
        }
    }
}
