using Microsoft.Xna.Framework;

namespace Physics2
{
	public class CollidableTri : Collidable
	{

		public CollidableTri(Point point1, Point point2, Point point3, Body parentBody)
			: base(parentBody)
		{ }

		public override float didIntersect(Vector3 start, Vector3 end)
		{
			// pointtriangleintersect
			throw new System.Exception("The method or operation is not implemented.");
		}

		public override Material getMaterial()
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		public override Vector3 Normal()
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		public override void onCollision(Point p)
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		public override void update(float TotalElapsedSeconds)
		{
			throw new System.Exception("The method or operation is not implemented.");
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
