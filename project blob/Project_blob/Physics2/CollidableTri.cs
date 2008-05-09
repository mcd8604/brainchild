using Microsoft.Xna.Framework;

namespace Physics2
{
	public class CollidableTri : Collidable
	{

		PhysicsPoint Point1;
		PhysicsPoint Point2;
		PhysicsPoint Point3;

		Plane m_Plane;

		public CollidableTri(PhysicsPoint point1, PhysicsPoint point2, PhysicsPoint point3)
		{
			Point1 = point1;
			Point2 = point2;
			Point3 = point3;

			m_Plane = new Plane(Point1.CurrentPosition, Point2.CurrentPosition, Point3.CurrentPosition);

			boundingbox.expandToInclude(Point1.CurrentPosition);
			boundingbox.expandToInclude(Point2.CurrentPosition);
			boundingbox.expandToInclude(Point3.CurrentPosition);
		}

		public override float didIntersect(Vector3 start, Vector3 end)
		{
			return CollisionMath.LineTriangleIntersect(start, end, Point1, Point2, Point3);
		}

		public override Plane Plane
		{
			get
			{
				return m_Plane;
			}
		}

		public override Vector3 Normal
		{
			get
			{
				return m_Plane.Normal;
			}
		}

		public override void update()
		{
			boundingbox.clear();
			boundingbox.expandToInclude(Point1.CurrentPosition);
			boundingbox.expandToInclude(Point2.CurrentPosition);
			boundingbox.expandToInclude(Point3.CurrentPosition);

			m_Plane = new Plane(Point1.CurrentPosition, Point2.CurrentPosition, Point3.CurrentPosition);
		}

		public override void ApplyForce(Vector3 at, Vector3 f)
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		public override void ImpartVelocity(Vector3 at, Vector3 vel)
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

	}
}
