using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob
{
	class StaticTri : T
	{

		internal Plane myPlane;

		internal VertexPositionNormalTexture[] vertices;

		internal Vector3 Origin;

		private GraphicsDevice theDevice;
		private VertexBuffer myVertexBuffer;

		public StaticTri(Vector3 point1, Vector3 point2, Vector3 point3, Color color)
		{
			vertices = new VertexPositionNormalTexture[3];

            myPlane = new Plane(point1, point3, point2);

            vertices[0] = new VertexPositionNormalTexture(point1, Vector3.Up, Vector2.Zero);
            vertices[1] = new VertexPositionNormalTexture(point2, Vector3.Up, new Vector2(0,1));
            vertices[2] = new VertexPositionNormalTexture(point3, Vector3.Up, Vector2.One);

			Origin = Vector3.Negate((point1 + point2 + point3) / 3);
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

				// temp - this is overly verbose and not terribly efficient, but it works

				Vector3 AB = vertices[1].Position - vertices[0].Position;
				Vector3 BC = vertices[2].Position - vertices[1].Position;
				Vector3 CA = vertices[0].Position - vertices[2].Position;

				Vector3 AP = vertices[0].Position - newPos;
				Vector3 BP = vertices[1].Position - newPos;
				Vector3 CP = vertices[2].Position - newPos;

				Vector3 A = Vector3.Cross(AP, AB);
				Vector3 B = Vector3.Cross(BP, BC);
				Vector3 C = Vector3.Cross(CP, CA);

				Vector3 t = (A + B + C);
				float sl = t.Length();

				float tl = A.Length() + B.Length() + C.Length();

				if (Math.Abs(sl - tl) < 0.1)
				{
					return u;
				}

			}

			return float.MaxValue;

		}

		public Plane getPlane()
		{
			return myPlane;
		}

		public VertexPositionNormalTexture[] getTriangleVertexes()
		{
			return vertices;
		}

		public VertexBuffer getVertexBuffer() {
			return myVertexBuffer;
		}

		public void setGraphicsDevice(GraphicsDevice device)
		{
			theDevice = device;
			myVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.SizeInBytes * 3, BufferUsage.None);
			myVertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
		}

		public int getVertexStride()
		{
            return VertexPositionNormalTexture.SizeInBytes;
		}

		public void DrawMe()
		{
			theDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
		}

		public void ApplyForce(Vector3 at, Vector3 f) { }

		public void ImpartVelocity(Vector3 at, Vector3 v) { }
	}
}
