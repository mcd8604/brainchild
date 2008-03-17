using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo5
{
	class Tri : T
	{

		//internal Plane myPlane;

		//internal VertexPositionColor[] vertices;

		//internal Vector3 Origin;

        internal Physics.Point[] points;

        Color color;

        public Tri(Physics.Point point1, Physics.Point point2, Physics.Point point3, Color p_color)
		{
			//vertices = new VertexPositionColor[3];

			//myPlane = new Plane(point1, point2, point3);

			//vertices[0] = new VertexPositionColor(point1, color);
			//vertices[1] = new VertexPositionColor(point2, color);
			//vertices[2] = new VertexPositionColor(point3, color);

			//Origin = Vector3.Negate((point1 + point2 + point3) / 3);

            points = new Physics.Point[3];

            points[0] = point1;
            points[1] = point2;
            points[2] = point3;

            color = p_color;
		}

		public bool couldIntersect()
		{
			return true;
		}

		public float DotNormal(Vector3 pos)
		{
			return new Plane(points[0].Position, points[1].Position, points[2].Position).DotNormal(pos + getOrigin());
		}

        private Vector3 getOrigin()
        {
            return Vector3.Negate((points[0].Position + points[1].Position + points[2].Position) / 3);
        }

		public Vector3 Normal()
		{
            return new Plane(points[0].Position, points[1].Position, points[2].Position).Normal;
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

                Vector3 AB = points[1].Position - points[0].Position;
                Vector3 BC = points[2].Position - points[1].Position;
                Vector3 CA = points[0].Position - points[2].Position;

                Vector3 AP = points[0].Position - newPos;
                Vector3 BP = points[1].Position - newPos;
                Vector3 CP = points[2].Position - newPos;

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
            return new Plane(points[0].Position, points[1].Position, points[2].Position);
		}

		public VertexPositionColor[] getTriangleVertexes()
		{
            VertexPositionColor[] vertices = new VertexPositionColor[3];

            vertices[0] = new VertexPositionColor(points[0].Position, color);
            vertices[1] = new VertexPositionColor(points[1].Position, color);
            vertices[2] = new VertexPositionColor(points[2].Position, color);
			return vertices;
		}

		public void DrawMe(GraphicsDevice device)
		{
			device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
		}


	}
}