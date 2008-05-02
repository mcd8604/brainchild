using Microsoft.Xna.Framework;

namespace Physics2
{
	public abstract class Collidable : Actor
	{

		private Body parent;

		private AxisAlignedBoundingBox boundingbox;

		public Collidable(Body parentBody)
		{
			parent = parentBody;
		}

		public virtual bool couldIntersect(Vector3 start, Vector3 end)
		{
			return boundingbox.lineIntersects(start, end);
		}

		public abstract float didIntersect(Vector3 start, Vector3 end);

		public virtual AxisAlignedBoundingBox getBoundingBox()
		{
			return boundingbox;
		}

		public abstract Material getMaterial();

		public virtual Vector3 getRelativeVelocity(Point p)
		{
			return parent.getRelativeVelocity(p);
		}

		public virtual Vector3 getVelocity()
		{
			return parent.getVelocity();
		}

		public abstract void ApplyForce(Vector3 at, Vector3 f);

		public abstract void ImpartVelocity(Vector3 at, Vector3 vel);

		public virtual bool isSolid()
		{
			return parent.isSolid();
		}

		public virtual bool isStatic()
		{
			return parent.isStatic();
		}

		public abstract Vector3 Normal();

		public abstract void onCollision(Point p);

		public abstract void update(float TotalElapsedSeconds);

	}
}
