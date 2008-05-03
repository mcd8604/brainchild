using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Physics2;

namespace Project_blob
{
	public class StaticTri : CollidableStaticTri, Drawable
	{

		internal Plane myPlane;

		internal VertexPositionColor[] vertices;

		internal Vector3 Origin;

		private GraphicsDevice theDevice;
		private VertexBuffer myVertexBuffer;

		public StaticTri(Vector3 point1, Vector3 point2, Vector3 point3, Color color)
			:base(point1, point2, point3)
		{
			vertices = new VertexPositionColor[3];

			myPlane = new Plane(point1, point2, point3);

			vertices[0] = new VertexPositionColor(point1, color);
			vertices[1] = new VertexPositionColor(point2, color);
			vertices[2] = new VertexPositionColor(point3, color);

			Origin = Vector3.Negate((point1 + point2 + point3) / 3);
		}

        public Plane getPlane()
		{
			return myPlane;
		}

        public VertexPositionColor[] getTriangleVertexes()
		{
			return vertices;
		}

        public VertexBuffer getVertexBuffer()
        {
			return myVertexBuffer;
		}

		public void setGraphicsDevice(GraphicsDevice device)
		{
			theDevice = device;
			myVertexBuffer = new VertexBuffer(device, VertexPositionColor.SizeInBytes * 3, BufferUsage.None);
			myVertexBuffer.SetData<VertexPositionColor>(vertices);
		}

        public int getVertexStride()
		{
			return VertexPositionColor.SizeInBytes;
		}

        public void DrawMe()
		{
			theDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
		}

        public override void ApplyForce(Vector3 at, Vector3 f) { }

        public override void ImpartVelocity(Vector3 at, Vector3 v) { }

        #region Drawable Members


        public TextureInfo GetTextureKey()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public BoundingBox GetBoundingBox()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public BoundingSphere GetBoundingSphere()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
