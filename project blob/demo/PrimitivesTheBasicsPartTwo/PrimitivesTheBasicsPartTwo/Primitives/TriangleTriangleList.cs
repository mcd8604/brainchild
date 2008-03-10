using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    public class TriangleTriangleList : DrawableGameComponent
    {
        // a basic effect, which contains the shaders that we will use to draw our
        // primitives.
        BasicEffect basicEffect;

        // an array of the vertices that have to be drawn
        VertexPositionColor[] vertices = new VertexPositionColor[3];

        // the vertex declaration that will be set on the device for drawing. this is 
        // created automatically using VertexPositionColor's vertex elements.
        VertexDeclaration vertexDeclaration;

        public TriangleTriangleList(Game game)
            : base(game) { }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            base.LoadGraphicsContent(loadAllContent);
            // set up a new basic effect, and enable vertex colors.
            basicEffect = new BasicEffect(GraphicsDevice, null);
            basicEffect.VertexColorEnabled = true;

            // projection uses CreateOrthographicOffCenter to create 2d projection
            // matrix with 0,0 in the upper left.
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, 0,
                0, 1);

            // create a vertex declaration, which tells the graphics card what kind of
            // data to expect during a draw call. We're drawing using
            // VertexPositionColors, so we'll use those vertex elements.
            vertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

            // create our triangle by setting up the vertices.
            // because we use a trianglelist as primitivetype, we only need 3 vertices
            vertices[0].Position = new Vector3(250, 100, 0);
            vertices[0].Color = Color.Red;

            vertices[1].Position = new Vector3(350, 200, 0);
            vertices[1].Color = Color.Red;

            vertices[2].Position = new Vector3(250, 200, 0);
            vertices[2].Color = Color.Red;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            // prepare the graphics device for drawing by setting the vertex declaration
            GraphicsDevice.VertexDeclaration = vertexDeclaration;
            // tell our basic effect to begin.
            basicEffect.Begin();
            basicEffect.CurrentTechnique.Passes[0].Begin();

            // draw the vertices as a trianglelist.
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 1);

            // tell basic effect that we're done.
            basicEffect.CurrentTechnique.Passes[0].End();
            basicEffect.End();
        }
    }
}
