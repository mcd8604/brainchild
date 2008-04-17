using Microsoft.Xna.Framework;

namespace Physics2
{
	public abstract class Collidable : Actor
	{

        private Body parent;

        public Collidable(Body parentBody)
        {
            parent = parentBody;
        }

        public abstract bool couldIntersect(Vector3 start, Vector3 end);

        public abstract float didIntersect(Vector3 start, Vector3 end);

        public abstract Material getMaterial();

        public abstract Vector3 Normal();

        public abstract void onCollision(Point p);

        public abstract void update();

	}
}
