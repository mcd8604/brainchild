using System;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class CollidableStaticTri : CollidableStatic
	{

		Vector3 Point1;
		Vector3 Point2;
		Vector3 Point3;

		Plane Plane;

		public CollidableStaticTri(Vector3 point1, Vector3 point2, Vector3 point3)
		{
			Point1 = point1;
			Point2 = point2;
			Point3 = point3;

			Plane = new Plane(Point1, Point2, Point3);

			boundingbox.expandToInclude(Point1);
			boundingbox.expandToInclude(Point2);
			boundingbox.expandToInclude(Point3);
		}

		public override float didIntersect(Vector3 start, Vector3 end)
		{
			return CollisionMath.LineStaticTriangleIntersect(start, end, Point1, Point2, Point3);
		}

		public override Vector3 Normal()
		{
			return Plane.Normal;
		}

		public override void update() { }

	}
}
