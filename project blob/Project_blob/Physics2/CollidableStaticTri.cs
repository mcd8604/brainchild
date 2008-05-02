using System;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class CollidableStaticTri : CollidableStatic
	{

		public CollidableStaticTri(Vector3 point1, Vector3 point2, Vector3 point3, BodyStatic parentBody)
			: base(parentBody)
		{ }

		public override float didIntersect(Vector3 start, Vector3 end)
		{
			//linestatictriangleintersect
			throw new Exception("The method or operation is not implemented.");
		}

		public override Material getMaterial()
		{
			throw new Exception("The method or operation is not implemented.");
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

		public override void ApplyForce(Vector3 at, Vector3 f)
		{
			throw new Exception("The method or operation is not implemented.");
		}

        public override void ImpartVelocity(Vector3 at, Vector3 vel)
        {
            throw new Exception("The method or operation is not implemented.");
        }

	}
}
