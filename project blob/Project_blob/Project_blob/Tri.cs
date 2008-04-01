using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob
{
	public class Tri : T
	{

		public static bool DEBUG_DrawNormal = false;

		internal Physics.Point[] points;

		VertexPositionColor[] vertices = new VertexPositionColor[5]; // max 5 for drawnormal

		Color color;

		private GraphicsDevice theDevice;
		private VertexBuffer myVertexBuffer;

		public Tri(Physics.Point point1, Physics.Point point2, Physics.Point point3, Color p_color)
		{
			points = new Physics.Point[3];

			points[0] = point1;
			points[1] = point2;
			points[2] = point3;

			color = p_color;

			//

			vertices[0] = new VertexPositionColor(points[0].Position, color);
			vertices[1] = new VertexPositionColor(points[1].Position, color);
			vertices[2] = new VertexPositionColor(points[2].Position, color);
			//if (DEBUG_DrawNormal)
			//{
				vertices[3] = new VertexPositionColor(Vector3.Negate(getOrigin()), Color.Pink);
				vertices[4] = new VertexPositionColor(Vector3.Negate(getOrigin()) + Normal(), Color.Red);
			//}
		}

		public bool couldIntersect(Physics.Point p)
		{
			return p != points[0] && p != points[1] && p != points[2];
		}

		public float DotNormal(Vector3 pos)
		{
			return new Plane(points[0].Position, points[1].Position, points[2].Position).DotNormal(pos + getOrigin());
		}

		private Vector3 getOrigin()
		{
			return Vector3.Negate((points[0].Position + points[1].Position + points[2].Position) / 3f);
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

        public bool shouldPhysicsBlock(Physics.Point p)
        {
            return true;
        }

		public Plane getPlane()
		{
			return new Plane(points[0].Position, points[1].Position, points[2].Position);
		}

		public VertexPositionColor[] getTriangleVertexes()
		{
			vertices[0].Position = points[0].Position;
			vertices[1].Position = points[1].Position;
			vertices[2].Position = points[2].Position;

			if (DEBUG_DrawNormal)
			{
				vertices[3].Position = Vector3.Negate(getOrigin());
				vertices[4].Position = Vector3.Negate(getOrigin()) + Normal();
			}

			return vertices;
		}

		public VertexBuffer getVertexBuffer()
		{
			myVertexBuffer.SetData<VertexPositionColor>(getTriangleVertexes());
			return myVertexBuffer;
		}

		public void setGraphicsDevice(GraphicsDevice device)
		{
			theDevice = device;
			myVertexBuffer = new VertexBuffer(device, VertexPositionColor.SizeInBytes * 5, BufferUsage.None);
		}

		public int getVertexStride()
		{
			return VertexPositionColor.SizeInBytes;
		}

		public void DrawMe()
		{
			theDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
			if (DEBUG_DrawNormal)
			{
				theDevice.DrawPrimitives(PrimitiveType.LineList, 3, 1);
			}
		}

		public void ApplyForce(Vector3 at, Vector3 f)
		{

			float dist0 = Vector3.Distance(points[0].Position, at);
			float dist1 = Vector3.Distance(points[1].Position, at);
			float dist2 = Vector3.Distance(points[2].Position, at);
			float total = dist0 + dist1 + dist2;

			points[0].CurrentForce += f * (dist0 / total);
			points[1].CurrentForce += f * (dist1 / total);
			points[2].CurrentForce += f * (dist2 / total);

		}

		public void ImpartVelocity(Vector3 at, Vector3 v)
		{


			float dist0 = Vector3.Distance(points[0].Position, at);
			float dist1 = Vector3.Distance(points[1].Position, at);
			float dist2 = Vector3.Distance(points[2].Position, at);
			float total = dist0 + dist1 + dist2;

			points[0].NextVelocity += v * (dist0 / total);
			points[1].NextVelocity += v * (dist1 / total);
			points[2].NextVelocity += v * (dist2 / total);

		}

        public Physics.Material getMaterial()
        {
            return new Physics.NormalMaterial();
        }

        public Vector3[] getCollisionVerticies()
        {
            throw new Exception("Not used");
        }

        public Vector3[] getNextCollisionVerticies()
        {
            throw new Exception("Not used");
        }

        public void test(Physics.Point p)
        {

            throw new Exception("do nothing!");
        }

        public bool inBoundingBox(Vector3 v)
        {
            throw new Exception("do nothing");
        }
	}
}