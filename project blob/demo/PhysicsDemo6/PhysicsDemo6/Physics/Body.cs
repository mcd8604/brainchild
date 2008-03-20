using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
	public interface Body
	{

		IEnumerable<Point> getPoints();

		IEnumerable<Collidable> getCollidables();

		IEnumerable<Spring> getSprings();

		float getVolume();

	}
}