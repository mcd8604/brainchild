using System;
using Microsoft.Xna.Framework;

namespace Physics
{
	public static class CollisionMath
	{

		const float Small_num = float.Epsilon;

		public static float LineStaticTriangleIntersect(Vector3 p0, Vector3 p1, Vector3 v0, Vector3 v1, Vector3 v2, out Vector3 i)
		{

			i = Vector3.Zero;

			Vector3 u = v1 - v0;
			Vector3 v = v2 - v0;
			Vector3 n = Vector3.Cross(u, v);
			if (n.LengthSquared() == 0) // degenerate triangle
			{
				return -1;
			}

			Vector3 dir = p1 - p0;
			Vector3 w0 = p0 - v0;
			float a = -Vector3.Dot(n, w0);
			float b = Vector3.Dot(n, dir);
			if (Math.Abs(b) <= Small_num) // parallel to plane
			{
				return -1;
			}

			// addition
			//if (a < 0 && b < 0)
			//{
			//    return -1;
			//}

			float r = a / b;
			if (r < 0f || r > 1f)
			{
				return -1;
			}

			i = p0 + (dir * r);

			float uu = Vector3.Dot(u, u);
			float uv = Vector3.Dot(u, v);
			float vv = Vector3.Dot(v, v);
			Vector3 w = i - v0;
			float wu = Vector3.Dot(w, u);
			float wv = Vector3.Dot(w, v);

			float d = (uv * uv) - (uu * vv);

			float s = ((uv * wv) - (vv * wu)) / d;

			if (s < 0f || s > 1f)
			{
				return -1;
			}
			float t = ((uv * wu) - (uu * wv)) / d;
			if (t < 0f || t > 1f)
			{
				return -1;
			}



			Vector3 planeNormal = new Plane(v0, v1, v2).Normal;
			Vector3 lineNormal = Vector3.Cross(planeNormal, v2 - v1);

			float check1 = Vector3.Dot(lineNormal, v0 - v1);
			float check2 = Vector3.Dot(lineNormal, i - v1);

			if (check1 * check2 < 0)
			{
				return -1;
			}


			//Vector3 check1 = Vector3.Cross(v0 - v1, v2 - v1);
			//Vector3 check2 = Vector3.Cross(i - v1, v2 - v1);

			//if (check1.Length() * check2.Length() < 0)
			//{
			//    return -1;
			//}


			//float check1 = Vector3.Dot(v0 - v1, v2 - v1);
			//float check1i = Vector3.Dot(v0 - v1, i - v1);
			//if (check1i < check1)
			//{
			//    return -1;
			//}

			//float check2 = Vector3.Dot(v0 - v2, v1 - v2);
			//float check2i = Vector3.Dot(v0 - v2, i - v2);
			//if (check2i < check2)
			//{
			//    return -1;
			//}


			/*
			Vector3 q = i - v2;
			float qu = Vector3.Dot(q, u);
			float qv = Vector3.Dot(q, v);

			//float d = (uv * uv) - (uu * vv);

			float sq = ((uv * qv) - (vv * qu)) / d;

			if (sq < 0f || sq > 1f)
			{
				return -1;
			}
			float tq = ((uv * qu) - (uu * qv)) / d;
			if (tq < 0f || tq > 1f)
			{
				return -1;
			}
			 */


			//if (t > 0.5 || s > 0.5)
			//{
			//    return -1;
			//}

			return r;

		}

		public static float PointTriangleIntersect(Point p, Point v0, Point v1, Point v2, out Vector3 i)
		{

			i = Vector3.Zero;

			Vector3 c_u = v1.PhysicsCurrentPosition - v0.PhysicsCurrentPosition;
			Vector3 c_v = v2.PhysicsCurrentPosition - v0.PhysicsCurrentPosition;
			Vector3 c_n = Vector3.Cross(c_u, c_v);
			Vector3 n_u = v1.potentialPosition - v0.potentialPosition;
			Vector3 n_v = v2.potentialPosition - v0.potentialPosition;
			Vector3 n_n = Vector3.Cross(n_u, n_v);
			if (c_n.LengthSquared() == 0 && n_n.LengthSquared() == 0) // degenerate triangle
			{
				return -1;
			}

			Vector3 dir = p.potentialPosition - p.PhysicsCurrentPosition;
			Vector3 c_w0 = p.PhysicsCurrentPosition - v0.PhysicsCurrentPosition;
			float c_a = -Vector3.Dot(c_n, c_w0);
			float c_b = Vector3.Dot(c_n, dir);
			Vector3 n_w0 = p.PhysicsCurrentPosition - v0.potentialPosition;
			float n_a = -Vector3.Dot(n_n, n_w0);
			float n_b = Vector3.Dot(n_n, dir);
			if (Math.Abs(c_b) <= Small_num && Math.Abs(c_b) <= Small_num) // parallel to plane
			{
				return -1;
			}

			float c_r = c_a / c_b;
			float n_r = n_a / n_b;

			if (c_r > 0 && n_r < 0)
			{
				// collidable passed through point
				//throw new Exception();

				p.PhysicsCurrentPosition += (v0.potentialPosition - v0.PhysicsCurrentPosition);
			}

			return 0;
		}

	}
}
