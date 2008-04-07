using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics
{
    public abstract class Body
    {

        public abstract Vector3 getCenter();

		public abstract Vector3 getNextCenter();

        public abstract IEnumerable<Point> getPoints();

		public abstract IEnumerable<Collidable> getCollidables();

		public abstract IEnumerable<Spring> getSprings();

		public virtual bool shouldPhysicsBlock(Point p) {
			return true;
		}

    }
}
