using Microsoft.Xna.Framework;

namespace Physics
{
	public interface Collidable
	{
		bool couldIntersect();

		float didIntersect(Vector3 start, Vector3 end);

		float DotNormal(Vector3 pos);

		Vector3 Normal();

		Plane getPlane();
	}
}
