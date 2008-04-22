using Microsoft.Xna.Framework;

namespace Physics2
{
	internal class CollisionEvent
	{

		internal Point point;
		internal Collidable collidable;
		internal float when;
		internal bool isStatic;

		internal CollisionEvent(Point p, Collidable c, float u)
		{
			point = p;
			collidable = c;
			when = u;
			isStatic = c.isStatic();
		}

		internal static int CompareEvents(CollisionEvent a, CollisionEvent b)
		{

			if (a.isStatic && !b.isStatic)
			{
				return -1;
			}
			if (b.isStatic && !a.isStatic)
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
