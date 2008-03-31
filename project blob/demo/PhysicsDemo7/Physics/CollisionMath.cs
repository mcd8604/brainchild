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
			Vector3 n = Vector3.Cross( u, v );
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

			float r = a / b;
			if (r < 0f || r > 1f)
			{
				return -1;
			}

			i = p0 + (dir * r );

			float uu = Vector3.Dot(u, u);
			float uv = Vector3.Dot(u, v);
			float vv = Vector3.Dot(v, v);
			Vector3 w = i - v0;
			float wu = Vector3.Dot(w, u);
			float wv = Vector3.Dot(w, v);

			float d = uv * uv - uu * vv;

			float s = (uv * wv - vv * wu) / d;

			if (s < 0f || s > 1f)
			{
				return -1;
			}
			float t = (uv * wu - uu * wv) / d;
			if (t < 0f || t > 1f)
			{
				return -1;
			}

			return r;

		}

		public static float PointTriangleIntersect(Point p, Point v0, Point v1, Point v2, out Vector3 i)
		{

			i = Vector3.Zero;

			Vector3 c_u = v1.CurrentPosition - v0.CurrentPosition;
			Vector3 c_v = v2.CurrentPosition - v0.CurrentPosition;
			Vector3 c_n = Vector3.Cross(c_u, c_v);
			Vector3 n_u = v1.PotientialPosition - v0.PotientialPosition;
			Vector3 n_v = v2.PotientialPosition - v0.PotientialPosition;
			Vector3 n_n = Vector3.Cross(n_u, n_v);
			if (c_n.LengthSquared() == 0 && n_n.LengthSquared() == 0) // degenerate triangle
			{
				return -1;
			}

			Vector3 dir = p.PotientialPosition - p.CurrentPosition;
			Vector3 c_w0 = p.CurrentPosition - v0.CurrentPosition;
			float c_a = -Vector3.Dot(c_n, c_w0);
			float c_b = Vector3.Dot(c_n, dir);
			Vector3 n_w0 = p.CurrentPosition - v0.PotientialPosition;
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
				throw new Exception();
			}

			return 0;
		}

	}
}
