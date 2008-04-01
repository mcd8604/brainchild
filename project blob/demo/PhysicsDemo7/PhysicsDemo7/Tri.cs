using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo7
{
    class Tri : T
    {

		public void test(Physics.Point p)
		{
			Vector3 i;
			Physics.CollisionMath.PointTriangleIntersect(p, points[0], points[1], points[2], out i);
		}

        public static bool DEBUG_DrawNormal = false;
        const int Num_Vertex = 5; // max 5 for drawnormal

        internal Physics.Point[] points;

        VertexPositionColor[] vertices = new VertexPositionColor[Num_Vertex]; 

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


            vertices[0] = new VertexPositionColor(points[0].CurrentPosition, color);
            vertices[1] = new VertexPositionColor(points[1].CurrentPosition, color);
            vertices[2] = new VertexPositionColor(points[2].CurrentPosition, color);

            vertices[3] = new VertexPositionColor(Vector3.Negate(getOrigin()), Color.Pink);
            vertices[4] = new VertexPositionColor(Vector3.Negate(getOrigin()) + Normal(), Color.Red);

        }

        public bool couldIntersect(Physics.Point p)
        {
            return p != points[0] && p != points[1] && p != points[2];
        }

        private Vector3 getOrigin()
        {
            return Vector3.Negate((points[0].NextPosition + points[1].NextPosition + points[2].NextPosition) / 3f);
        }

        public Vector3 Normal()
        {
            return new Plane(points[0].NextPosition, points[1].NextPosition, points[2].NextPosition).Normal;
        }

        public float DotNormal(Vector3 pos)
        {
            return new Plane(points[0].NextPosition, points[1].NextPosition, points[2].NextPosition).DotNormal(pos + getOrigin());
        }

		public Vector3[] getCollisionVerticies()
		{
			Vector3[] ret = new Vector3[3];
			ret[0] = points[0].CurrentPosition;
			ret[1] = points[1].CurrentPosition;
			ret[2] = points[2].CurrentPosition;
			return ret;
		}
		public Vector3[] getNextCollisionVerticies()
		{
			Vector3[] ret = new Vector3[3];
			ret[0] = points[0].NextPosition;
			ret[1] = points[1].NextPosition;
			ret[2] = points[2].NextPosition;
			return ret;
		}

        public float didIntersect(Vector3 last, Vector3 next)
        {
            float before = new Plane(points[0].CurrentPosition, points[1].CurrentPosition, points[2].CurrentPosition).DotNormal(last + getOrigin());
            float later = DotNormal(next);

            if (before >= 0 && later <= 0)
            {

                float u = before / (before - later);
                // check limits
                Vector3 newPos = (last * (1 - u)) + (next * u);

				while (DotNormal(newPos) <= 0)
				{
				    newPos += (Normal() * 0.001f);
				    //++DEBUG_BumpLoops;
				}

                // temp - this is overly verbose and not terribly efficient, but it works - not

                Vector3 AB = points[1].NextPosition - points[0].NextPosition;
                Vector3 BC = points[2].NextPosition - points[1].NextPosition;
                Vector3 CA = points[0].NextPosition - points[2].NextPosition;

                Vector3 AP = points[0].NextPosition - newPos;
                Vector3 BP = points[1].NextPosition - newPos;
                Vector3 CP = points[2].NextPosition - newPos;

                Vector3 A = Vector3.Cross(AP, AB);
                Vector3 B = Vector3.Cross(BP, BC);
                Vector3 C = Vector3.Cross(CP, CA);

				if (((A.X >= -0.001 && B.X >= -0.001 && C.X >= -0.001) || (A.X <= 0.001 && B.X <= 0.001 && C.X <= 0.001)) &&
					((A.Y >= -0.001 && B.Y >= -0.001 && C.Y >= -0.001) || (A.Y <= 0.001 && B.Y <= 0.001 && C.Y <= 0.001)) &&
					((A.Z >= -0.001 && B.Z >= -0.001 && C.Z >= -0.001) || (A.Z <= 0.001 && B.Z <= 0.001 && C.Z <= 0.001)))
                {
                    return u;
                }
                else
                {
                    //Console.WriteLine("Check: " + A + ", " + B);
                }
            }

            return float.MaxValue;
        }


		public bool inBoundingBox(Vector3 i)
		{
			return true;
		}


        public bool shouldPhysicsBlock(Physics.Point p)
        {
            return true;
        }

        public void ApplyForce(Vector3 at, Vector3 f)
        {

            float dist0 = Vector3.Distance(points[0].NextPosition, at);
            float dist1 = Vector3.Distance(points[1].NextPosition, at);
            float dist2 = Vector3.Distance(points[2].NextPosition, at);
            float total = dist0 + dist1 + dist2;

            points[0].ForceNextFrame += f * (dist0 / total);
            points[1].ForceNextFrame += f * (dist1 / total);
            points[2].ForceNextFrame += f * (dist2 / total);

        }

        public void ImpartVelocity(Vector3 at, Vector3 v)
        {


            float dist0 = Vector3.Distance(points[0].NextPosition, at);
            float dist1 = Vector3.Distance(points[1].NextPosition, at);
            float dist2 = Vector3.Distance(points[2].NextPosition, at);
            float total = dist0 + dist1 + dist2;

            // next next?

            //points[0].NextVelocity += v * (dist0 / total);
            //points[1].NextVelocity += v * (dist1 / total);
            //points[2].NextVelocity += v * (dist2 / total);

        }

        public Physics.Material getMaterial()
        {
            return new Physics.MaterialBasic();
        }

        // Drawable:

        public VertexPositionColor[] getTriangleVertexes()
        {
            vertices[0].Position = points[0].NextPosition;
            vertices[1].Position = points[1].NextPosition;
            vertices[2].Position = points[2].NextPosition;

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
            myVertexBuffer = new VertexBuffer(device, VertexPositionColor.SizeInBytes * Num_Vertex, BufferUsage.None);
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

    }
}
