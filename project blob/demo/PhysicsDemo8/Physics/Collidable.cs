using Microsoft.Xna.Framework;

namespace Physics
{
	public abstract class Collidable
	{

		private Body parent;

		public Collidable(Body parentBody)
		{
			parent = parentBody;
		}

		/// <summary>
        /// Could this collidable possibly be hit by Point p.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
		public abstract bool couldIntersect(Point p);

		/// <summary>
		/// Does the line segment from start to end intersect with this collidable.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public abstract float didIntersect(Vector3 start, Vector3 end);

		/// <summary>
		/// Should this point p, which hit this collidable, be blocked.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public virtual bool shouldPhysicsBlock(Point p) {
			return parent.shouldPhysicsBlock(p);
		}

		/// <summary>
		/// The Dot Product to the normal of this collidable surface, if applicable.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public abstract float DotNormal(Vector3 pos);

		/// <summary>
		/// The Normal to this collidable surface, if applicable.
		/// </summary>
		/// <returns></returns>
		public abstract Vector3 Normal();

		/// <summary>
		/// This collidable was hit by a point, and this force was transferred into the collidable.
		/// </summary>
		/// <param name="at"></param>
		/// <param name="f"></param>
		public abstract void ApplyForce(Vector3 at, Vector3 f);

		/// <summary>
		/// This collidable was hit by a point, and this velocity was transferred into the collidable.
		/// </summary>
		/// <param name="at"></param>
		/// <param name="v"></param>
		public abstract void ImpartVelocity(Vector3 at, Vector3 v);

		public virtual Material getMaterial()
		{
			return Material.getDefaultMaterial();
		}

		//public abstract Vector3[] getCollisionVerticies();
		public abstract Vector3[] getNextCollisionVerticies();

		public abstract void test(Point p);

		public abstract bool inBoundingBox(Vector3 v);

	}
}
