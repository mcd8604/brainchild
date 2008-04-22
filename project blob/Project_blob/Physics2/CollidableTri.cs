using Microsoft.Xna.Framework;

namespace Physics2
{
	class CollidableTri : Collidable
	{

		public CollidableTri(Point point1, Point point2, Point point3, Body parentBody)
			: base(parentBody)
		{ }

		public override bool couldIntersect(Vector3 start, Vector3 end)
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		public override float didIntersect(Vector3 start, Vector3 end)
		{
			// pointtriangleintersect
			throw new System.Exception("The method or operation is not implemented.");
		}

		public override AxisAlignedBoundingBox getBoundingBox()
		{
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

	}
}
