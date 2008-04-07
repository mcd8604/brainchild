using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
	public abstract class PhysicsManager
	{

		public static PhysicsManager getInstance()
		{
			return new PhysicsSeq();
		}

	}
}
