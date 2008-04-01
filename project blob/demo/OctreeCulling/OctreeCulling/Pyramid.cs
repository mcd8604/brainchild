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
    class Pyramid : SceneObject
    {
        // Add transforms for the triangle
        private Matrix _triangleTransform;
        public Matrix TriangleTransform
        {
            get { return _triangleTransform; }
            set { _triangleTransform = value; }
        }

        private VertexPositionColor[] _triangleData;
        public VertexPositionColor[] TriangleData
        {
            get { return _triangleData; }
        }

        public Pyramid(Vector3 size, Vector3 position, BasicEffect effect, GraphicsDeviceManager graphics)
        {
            Effect = effect;
            Graphics = graphics;

            _triangleTransform = Matrix.CreateTranslation(position);

            // Initialize the triangle's data (with Vertex Colors)
            _triangleData = new VertexPositionColor[18]
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

            Effect.World = _triangleTransform;
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
                    PrimitiveType.TriangleList, _triangleData, 0, 6);

                pass.End();
            }
            Effect.End();
        }
    }
}
