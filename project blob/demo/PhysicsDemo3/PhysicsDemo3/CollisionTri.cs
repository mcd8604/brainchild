using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo3
{
	public class CollisionTri
	{
		Plane myPlane;
		Vector3 max;
		Vector3 min;
		VertexPositionColor[] vertices = new VertexPositionColor[3];
		Vector3 Origin;

		public CollisionTri(Vector3 point1, Vector3 point2, Vector3 point3, Color color)
		{
			myPlane = new Plane(point1, point2, point3);

			max = Vector3.Max(point1, point2);
			max = Vector3.Max(max, point3);

			min = Vector3.Min(point1, point2);
			min = Vector3.Min(min, point3);

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
				Vector3 newPos = (start * (1 - u)) + (end * u);
				// check limits
				if (newPos.X >= min.X - 0.1f && newPos.X <= max.X + 0.1f &&
					newPos.Y >= min.Y - 0.1f && newPos.Y <= max.Y + 0.1f &&
					newPos.Z >= min.Z - 0.1f && newPos.Z <= max.Z + 0.1f)
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