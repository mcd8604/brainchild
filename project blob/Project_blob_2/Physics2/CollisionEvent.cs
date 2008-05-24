using Microsoft.Xna.Framework;

namespace Physics2
{
	public class CollisionEvent
	{

		public PhysicsPoint point;
		public Collidable collidable;
		internal float when;
		internal Vector3 collisionPoint;
		internal float impact;

		internal CollisionEvent(PhysicsPoint p, Collidable c, float u, Vector3 cp)
		{
			point = p;
			collidable = c;
			when = u;
			collisionPoint = cp;
		}

		internal bool isStatic()
		{
			return collidable.isStatic();
		}

		internal bool isBlocking()
		{
			return collidable.isSolid();
		}

		internal void trigger()
		{
			collidable.onCollision(this);
		}

		internal static int CompareEvents(CollisionEvent a, CollisionEvent b)
		{

			if (a.isBlocking() && !b.isBlocking())
			{
				return -1;
			}
			if (b.isBlocking() && !a.isBlocking())
			{
				return 1;
			}

			if (a.isStatic() && !b.isStatic())
			{
				return -1;
			}
			if (b.isStatic() && !a.isStatic())
			{
				return 1;
			}
			if (a.when == b.when)
			{
				return 0;
			}
			else if (b.when - a.when > 0)
			{
				return -1;
			}
			else
			{
				return 1;
			}

		}

	}
}
