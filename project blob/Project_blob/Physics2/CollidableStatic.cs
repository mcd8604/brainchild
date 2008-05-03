using Microsoft.Xna.Framework;

namespace Physics2
{
	public abstract class CollidableStatic : Collidable
	{

		public CollidableStatic(){ }

		public override bool isStatic()
		{
			return true;
		}

		public override Vector3 getVelocity()
		{
			return Vector3.Zero;
		}

		public override void ApplyForce(Vector3 at, Vector3 f) { }

		public override void ImpartVelocity(Vector3 at, Vector3 vel) { }

	}
}
