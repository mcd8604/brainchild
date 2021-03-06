using Microsoft.Xna.Framework;

namespace Physics2
{
	public abstract class Collidable
	{
		public Body parent;

		protected AxisAlignedBoundingBox boundingbox = new AxisAlignedBoundingBox();

		protected Material material = null;

		public Collidable() { }

		public virtual bool couldIntersect(Vector3 start, Vector3 end)
		{
			return boundingbox.lineIntersects(start, end);
		}

		public abstract float didIntersect(Vector3 start, Vector3 end, out Vector3 hit);

		public virtual AxisAlignedBoundingBox getBoundingBox()
		{
			return boundingbox;
		}

		public virtual Material getMaterial()
		{
			if (material == null)
			{
				return parent.getMaterial();
			}
			else
			{
				return material;
			}
		}

		public virtual void setMaterial(Material m)
		{
			material = m;
		}

		public virtual Vector3 getRelativeVelocity(CollisionEvent e)
		{
			return parent.getRelativeVelocity(e);
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

		public abstract Plane Plane
		{
			get;
		}

		// temporary??
		public abstract Plane PotentialPlane {
			get;
		}

		public abstract Vector3 Normal
		{
			get;
		}

		// temporary??
		public abstract Vector3 PotentialNormal {
			get;
		}

		public virtual void onCollision(CollisionEvent e)
		{
			parent.onCollision(e);
		}

		public abstract void update();

		public abstract void updatePosition();

	}
}
