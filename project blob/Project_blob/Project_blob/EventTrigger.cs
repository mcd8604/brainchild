using System;
using System.Collections.Generic;
using System.Text;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
    public interface EventTrigger
    {
		bool PerformEvent(PhysicsPoint p);

		int NumTriggers
		{
			get;
			set;
		}

		bool Solid
		{
			get;
			set;
		}
    }
}
