using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo5
{
	class StaticTri : T
	{

		internal Plane myPlane;

		internal VertexPositionColor[] vertices;

		internal Vector3 Origin;

		public StaticTri(Vector3 point1, Vector3 point2, Vector3 point3, Color color)
		{
			vertices = new VertexPositionColor[3];

			myPlane = new Plane(point1, point2, point3);

			vertices[0] = new VertexPositionColor(point1, color);
			vertices[1] = new VertexPositionColor(point2, color);
			vertices[2] = new VertexPositionColor(point3, color);

			Origin = Vector3.Negate((point1 + point2 + point3) / 3);
		}

		public bool couldIntersect()
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

		public VertexPositionColor[] getTriangleVertexes()
		{
			return vertices;
		}

		public void DrawMe(GraphicsDevice device)
		{
			device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
		}


	}
}
