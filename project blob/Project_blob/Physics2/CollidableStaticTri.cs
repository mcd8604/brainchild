using System;
using Microsoft.Xna.Framework;

namespace Physics2
{
	class CollidableStaticTri : CollidableStatic
	{

		public CollidableStaticTri(Vector3 point1, Vector3 point2, Vector3 point3, BodyStatic parentBody)
			: base(parentBody)
		{ }

		public override bool couldIntersect(Vector3 start, Vector3 end)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override float didIntersect(Vector3 start, Vector3 end)
		{
			//linestatictriangleintersect
			throw new Exception("The method or operation is not implemented.");
		}

		public override AxisAlignedBoundingBox getBoundingBox()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override Material getMaterial()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool isSolid()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool isStatic()
		{
			return base.isStatic();
		}

		public override Vector3 Normal()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void onCollision(Point p)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void update(float TotalElapsedSeconds)
		{
			throw new Exception("The method or operation is not implemented.");
		}

        public override void ImpartVelocity(Vector3 vel)
        {
            throw new Exception("The method or operation is not implemented.");
        }

	}
}
