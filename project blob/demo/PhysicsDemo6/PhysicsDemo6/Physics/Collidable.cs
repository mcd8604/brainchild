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



		float DotNormal(Vector3 pos);

		Vector3 Normal();

		Plane getPlane();

		void ApplyForce(Vector3 at, Vector3 f);

		void ImpartVelocity(Vector3 at, Vector3 v);
	}
}
