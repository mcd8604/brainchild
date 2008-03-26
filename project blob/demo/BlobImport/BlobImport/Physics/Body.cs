using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics
{
	public interface Body
	{

        Vector3 getCenter();

		IEnumerable<Point> getPoints();

		IEnumerable<Collidable> getCollidables();

		IEnumerable<Spring> getSprings();

		float getVolume();

	}
}