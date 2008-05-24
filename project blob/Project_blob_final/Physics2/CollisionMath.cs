using System;
using Microsoft.Xna.Framework;

namespace Physics2 {
	internal static class CollisionMath {
		private const float Small_num = float.Epsilon;

		internal static float LineStaticTriangleIntersect(Vector3 p0, Vector3 p1, Vector3 v0, Vector3 v1, Vector3 v2, out Vector3 i) {
			Vector3 u = v1 - v0;
			Vector3 v = v2 - v0;
			Vector3 n = Vector3.Cross(u, v);

			// check if line is parallel to plane
			Vector3 dir = p1 - p0;
			Vector3 w0 = p0 - v0;
			float a = -Vector3.Dot(n, w0);
			float b = Vector3.Dot(n, dir);
			if (Math.Abs(b) <= Small_num) {
				i = Util.Zero;
				return -1;
			}

			// r value between start and end
			float r = a / b;
			if (r < 0f || r > 1f) {
				i = Util.Zero;
				return -1;
			}

			// i is the impact point
			i = p0 + (dir * r);

			// more checks
			float uu = Vector3.Dot(u, u);
			float uv = Vector3.Dot(u, v);
			float vv = Vector3.Dot(v, v);
			Vector3 w = i - v0;
			float wu = Vector3.Dot(w, u);
			float wv = Vector3.Dot(w, v);

			float d = (uv * uv) - (uu * vv);

			float s = ((uv * wv) - (vv * wu)) / d;
			if (s < 0f || s > 1f) {
				return -1;
			}

			float t = ((uv * wu) - (uu * wv)) / d;
			if (t < 0f || t > 1f) {
				return -1;
			}

			// more checks
			Vector3 planeNormal = new Plane(v0, v1, v2).Normal;
			Vector3 lineNormal = Vector3.Cross(planeNormal, v2 - v1);

			float check1 = Vector3.Dot(lineNormal, v0 - v1);
			float check2 = Vector3.Dot(lineNormal, i - v1);

			if (check1 * check2 < 0) {
				return -1;
			}

			float checkn = Vector3.Dot(planeNormal, dir);

			if (checkn > 0) {
				return -1;
			}

			return r;
		}

		internal static float LineTriangleIntersect(Vector3 p0, Vector3 p1, Vector3 sv0, Vector3 sv1, Vector3 sv2, Vector3 ev0, Vector3 ev1, Vector3 ev2, out Vector3 i) {

			if (sv0 == ev0 && sv1 == ev1 && sv2 == ev2) {
				return LineStaticTriangleIntersect(p0, p1, sv0, sv1, sv2, out i);
			}

			float test = LineStaticTriangleIntersect(p0, p1, sv0, sv1, sv2, out i);

			if (test == -1) {
				return LineStaticTriangleIntersect(p0, p1, ev0, ev1, ev2, out i);
			}

			float test2 = LineStaticTriangleIntersect(p0, p1, ev0, ev1, ev2, out i);

			if (test < test2) {
				return LineStaticTriangleIntersect(p0, p1, sv0, sv1, sv2, out i);
			} else {
				return test2;
			}

			/*

			// check for degenerate triangle - initial
			Vector3 su = sv1 - sv0;
			Vector3 sv = sv2 - sv0;
			Vector3 sn = Vector3.Cross(su, sv);
			if (sn.LengthSquared() == 0) {
				i = Util.Zero;
				return -1;
			}

			// check for degenerate triangle - final
			Vector3 eu = ev1 - ev0;
			Vector3 ev = ev2 - ev0;
			Vector3 en = Vector3.Cross(eu, ev);
			if (en.LengthSquared() == 0) {
				i = Util.Zero;
				return -1;
			}

			Vector3 dir = p1 - p0;

			// check if line is parallel to plane - initial
			Vector3 w0 = p0 - sv0;
			float sa = -Vector3.Dot(sn, w0);
			float sb = Vector3.Dot(sn, dir);
			if (Math.Abs(sb) <= Small_num) {
				i = Util.Zero;
				return -1;
			}

			// check if line is parallel to plane - final
			w0 = p0 - ev0;
			float ea = -Vector3.Dot(en, w0);
			float eb = Vector3.Dot(en, dir);
			if (Math.Abs(eb) <= Small_num) {
				i = Util.Zero;
				return -1;
			}

			// r value between start and end - initial
			float sr = sa / sb;
			float er = ea / eb;
			if ((sr < 0f && er < 0f) || (sr > 1f && er > 1f)) {
				i = Util.Zero;
				return -1;
			}

			// i is the impact point - final position
			Vector3 si = p0 + (dir * sr);
			i = p0 + (dir * er);

			// more checks
			float suu = Vector3.Dot(su, su);
			float suv = Vector3.Dot(su, sv);
			float svv = Vector3.Dot(sv, sv);
			Vector3 sw = si - sv0;
			float swu = Vector3.Dot(sw, su);
			float swv = Vector3.Dot(sw, sv);

			float sd = (suv * suv) - (suu * svv);

			float euu = Vector3.Dot(eu, eu);
			float euv = Vector3.Dot(eu, ev);
			float evv = Vector3.Dot(ev, ev);
			Vector3 ew = i - ev0;
			float ewu = Vector3.Dot(ew, eu);
			float ewv = Vector3.Dot(ew, ev);

			float ed = (euv * euv) - (euu * evv);

			float ss = ((suv * swv) - (svv * swu)) / sd;
			float es = ((euv * ewv) - (evv * ewu)) / ed;
			if (ss < 0f || ss > 1f || es < 0f || es > 1f) {
				return -1;
			}

			float st = ((suv * swu) - (suu * swv)) / sd;
			float et = ((euv * ewu) - (euu * ewv)) / ed;
			if (st < 0f || st > 1f || et < 0f || et > 1f) {
				return -1;
			}

			// more checks
			Vector3 splaneNormal = new Plane(sv0, sv1, sv2).Normal;
			Vector3 slineNormal = Vector3.Cross(splaneNormal, sv2 - sv1);

			float scheck1 = Vector3.Dot(slineNormal, sv0 - sv1);
			float scheck2 = Vector3.Dot(slineNormal, i - sv1);

			Vector3 eplaneNormal = new Plane(ev0, ev1, ev2).Normal;
			Vector3 elineNormal = Vector3.Cross(eplaneNormal, ev2 - ev1);

			float echeck1 = Vector3.Dot(elineNormal, ev0 - ev1);
			float echeck2 = Vector3.Dot(elineNormal, i - ev1);

			if (scheck1 * scheck2 < 0 || echeck1 * echeck2 < 0) {
				return -1;
			}

			return Math.Min(sr, er);
			 
			*/
		}
	}
}
