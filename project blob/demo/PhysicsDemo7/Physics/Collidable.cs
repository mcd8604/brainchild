using Microsoft.Xna.Framework;

namespace Physics
{
    public interface Collidable
    {
        /// <summary>
        /// Could this collidable possibly be hit by Point p.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        bool couldIntersect(Point p);

        /// <summary>
        /// Does the line segment from start to end intersect with this collidable.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        float didIntersect(Vector3 start, Vector3 end);

        /// <summary>
        /// Should this point p, which hit this collidable, be blocked.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        bool shouldPhysicsBlock(Point p);

        /// <summary>
        /// The Dot Product to the normal of this collidable surface, if applicable.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        float DotNormal(Vector3 pos);

        /// <summary>
        /// The Normal to this collidable surface, if applicable.
        /// </summary>
        /// <returns></returns>
        Vector3 Normal();

        /// <summary>
        /// This collidable was hit by a point, and this force was transferred into the collidable.
        /// </summary>
        /// <param name="at"></param>
        /// <param name="f"></param>
        void ApplyForce(Vector3 at, Vector3 f);

        /// <summary>
        /// This collidable was hit by a point, and this velocity was transferred into the collidable.
        /// </summary>
        /// <param name="at"></param>
        /// <param name="v"></param>
        void ImpartVelocity(Vector3 at, Vector3 v);

        Material getMaterial();

		Vector3[] getCollisionVerticies();
		Vector3[] getNextCollisionVerticies();

		void test( Point p );

		bool inBoundingBox(Vector3 v);
    }
}
