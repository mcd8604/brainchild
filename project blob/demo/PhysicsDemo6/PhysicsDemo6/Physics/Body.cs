using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
	public abstract class Body
	{

		public abstract IEnumerable<Point> getPoints();

		public abstract IEnumerable<Collidable> getCollidables();

		public abstract IEnumerable<Spring> getSprings();

		public abstract float getVolume();

	}
}