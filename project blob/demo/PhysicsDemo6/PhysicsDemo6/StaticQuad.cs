using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo6
{
	class StaticQuad : T
	{
		internal Plane myPlane;
		internal Vector3 max;
		internal Vector3 min;
		internal VertexPositionColor[] vertices;

		internal Vector3 Origin;

		private GraphicsDevice theDevice;
		private VertexBuffer myVertexBuffer;

		public StaticQuad(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, Color color)
		{
			vertices = new VertexPositionColor[4];

			myPlane = new Plane(point1, point2, point3);

			max = Vector3.Max(point1, point2);
			max = Vector3.Max(max, point3);
			max = Vector3.Max(max, point4);

			min = Vector3.Min(point1, point2);
			min = Vector3.Min(min, point3);
			min = Vector3.Min(min, point4);

			vertices[0] = new VertexPositionColor(point1, color);
			vertices[1] = new VertexPositionColor(point2, color);
			vertices[2] = new VertexPositionColor(point3, color);
			vertices[3] = new VertexPositionColor(point4, color);

			Origin = Vector3.Negate((point1 + point2 + point3 + point4) / 4);
		}

		public bool couldIntersect(Physics.Point p)
		{
			return true;
		}

		public float DotNormal(Vector3 pos)
		{
			return myPlane.DotNormal(pos + Origin);
		}

		public Vector3 Normal()
		{
			return myPlane.Normal;
		}

		public float didIntersect(Vector3 start, Vector3 end)
		{

			float lastVal = DotNormal(start);
			float thisVal = DotNormal(end);

			if (lastVal > 0 && thisVal < 0) // we were 'above' now 'behind'
			{

				float u = lastVal / (lastVal - thisVal);
				// check limits
				Vector3 newPos = (start * (1 - u)) + (end * u);

				if (newPos.X >= min.X - 0.1f && newPos.X <= max.X + 0.1f &&
					newPos.Y >= min.Y - 0.1f && newPos.Y <= max.Y + 0.1f &&
					newPos.Z >= min.Z - 0.1f && newPos.Z <= max.Z + 0.1f)
				{
					return u;
				}

			}

			return float.MaxValue;

		}

        public bool shouldPhysicsBlock(Physics.Point p)
        {
            return true;
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
			myVertexBuffer = new VertexBuffer(device, VertexPositionColor.SizeInBytes * 4, BufferUsage.None);
			myVertexBuffer.SetData<VertexPositionColor>(vertices);
		}

		public int getVertexStride()
		{
			return VertexPositionColor.SizeInBytes;
		}

		public void DrawMe()
		{
			theDevice.DrawPrimitives(PrimitiveType.TriangleFan, 0, 2);
		}

		public void ApplyForce(Vector3 at, Vector3 f) { }

		public void ImpartVelocity(Vector3 at, Vector3 v) { }

        public Physics.Material getMaterial()
        {
            return new Physics.NormalMaterial();
        }

	}
}
