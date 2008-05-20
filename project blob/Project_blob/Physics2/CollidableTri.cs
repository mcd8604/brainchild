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

		public override float didIntersect(Vector3 start, Vector3 end, out Vector3 hit)
		{
			return CollisionMath.LineTriangleIntersect(start, end, Point1.CurrentPosition, Point2.CurrentPosition, Point3.CurrentPosition, Point1.PotentialPosition, Point2.PotentialPosition, Point3.PotentialPosition, out hit);
		}

		public override Plane Plane
		{
			get
			{
				return m_Plane;
			}
		}

		// temporary??
		public override Plane PotentialPlane {
			get {
				return new Plane(Point1.PotentialPosition, Point2.PotentialPosition, Point3.PotentialPosition);
			}
		}

		// temporary?
		public override Plane NextPlane
		{
			get
			{
				return new Plane(Point1.NextPosition, Point2.NextPosition, Point3.NextPosition);
			}
		}

		public override Vector3 Normal
		{
			get
			{
				return m_Plane.Normal;
			}
		}

		// temporary??
		public override Vector3 PotentialNormal {
			get {
				return new Plane(Point1.PotentialPosition, Point2.PotentialPosition, Point3.PotentialPosition).Normal;
			}
		}

		// temporary?
		public override Vector3 NextNormal
		{
			get
			{
				return new Plane(Point1.NextPosition, Point2.NextPosition, Point3.NextPosition).Normal;
			}
		}

		public override void update() {
			boundingbox.expandToInclude(Point1.PotentialPosition);
			boundingbox.expandToInclude(Point2.PotentialPosition);
			boundingbox.expandToInclude(Point3.PotentialPosition);
		}

		public override void updatePosition()
		{
			boundingbox.clear();
			boundingbox.expandToInclude(Point1.CurrentPosition);
			boundingbox.expandToInclude(Point2.CurrentPosition);
			boundingbox.expandToInclude(Point3.CurrentPosition);


			m_Plane = new Plane(Point1.CurrentPosition, Point2.CurrentPosition, Point3.CurrentPosition);
		}

		public override void ApplyForce(Vector3 at, Vector3 f)
		{
			if (parent.affectedByCollisions()) {
				// TODO
				Point1.ForceNextFrame += f / 3f;
				Point2.ForceNextFrame += f / 3f;
				Point3.ForceNextFrame += f / 3f;
			}
		}

		public override void ImpartVelocity(Vector3 at, Vector3 vel)
		{
			if (parent.affectedByCollisions()) {
				// TODO
				Point1.NextVelocity += vel / 3f;
				Point2.NextVelocity += vel / 3f;
				Point3.NextVelocity += vel / 3f;
			}
		}

	}
}
