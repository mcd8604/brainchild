using Microsoft.Xna.Framework;

namespace Physics
{
	public interface Collidable
	{
		bool couldIntersect(Point p);

		float didIntersect(Vector3 start, Vector3 end);

		float DotNormal(Vector3 pos);

		Vector3 Normal();

		Plane getPlane();

		void ApplyForce(Vector3 at, Vector3 f);

		void ImpartVelocity(Vector3 at, Vector3 v);
	}
}
