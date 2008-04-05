using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob
{
    class CollidableTri : Physics.Collidable
    {
		internal Plane myPlane;

		internal Vector3[] vertices;

		internal Vector3 Origin;

        public CollidableTri(VertexPositionNormalTexture point1, VertexPositionNormalTexture point2, VertexPositionNormalTexture point3)
		{
			vertices = new Vector3[3];

			myPlane = new Plane(point1.Position, point2.Position, point3.Position);

            vertices[0] = point1.Position;
            vertices[1] = point2.Position;
            vertices[2] = point3.Position;

            myPlane.Normal = Vector3.Normalize(Vector3.Add(point3.Normal, Vector3.Add(point1.Normal, point2.Normal)));

			Origin = Vector3.Negate((point1.Position + point2.Position + point3.Position) / 3);
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

				Vector3 AB = vertices[1] - vertices[0];
				Vector3 BC = vertices[2] - vertices[1];
				Vector3 CA = vertices[0] - vertices[2];

				Vector3 AP = vertices[0] - newPos;
				Vector3 BP = vertices[1] - newPos;
				Vector3 CP = vertices[2] - newPos;

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

        public virtual bool shouldPhysicsBlock(Physics.Point p)
        {
            return true;
        }

		public Plane getPlane()
		{
			return myPlane;
		}

		public Vector3[] getTriangleVertexes()
		{
			return vertices;
		}

		public void ApplyForce(Vector3 at, Vector3 f) {
            //throw new Exception("Cannot Apply Force on CollidableTri");
        }

        public void ImpartVelocity(Vector3 at, Vector3 v)
        {
            //throw new Exception("Cannot Impart Velocity on CollidableTri");
        }

        public Physics.Material getMaterial()
        {
            return new Physics.MaterialBasic();
        }

        public Vector3[] getCollisionVerticies()
        {
            //throw new Exception("Not used");
            return vertices;
        }

        public Vector3[] getNextCollisionVerticies()
        {
            //throw new Exception("Not used");
            return vertices;
        }

        public bool inBoundingBox(Vector3 v)
        {
            throw new Exception("do nothing");
        }

        public void test(Physics.Point p)
        {
            //throw new Exception("do nothing");
        }
    }
}
