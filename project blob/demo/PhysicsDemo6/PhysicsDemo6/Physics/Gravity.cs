using Microsoft.Xna.Framework;

namespace Physics
{
	public interface Gravity
	{

		/// <summary>
		/// Calculate the force of gravity at the Point p.
		/// </summary>
		/// <param name="p">The reference point. This should not be modifed.</param>
		/// <returns>A Vector3 from the point p with the direction and magnitude of the force of gravity.</returns>
		Vector3 getForceOn(Point p);

	}
}
