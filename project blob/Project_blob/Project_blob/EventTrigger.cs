using System;
using System.Collections.Generic;
using System.Text;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
    public interface EventTrigger
    {
		/// <summary>
		/// This method must not do anything world altering or performance intensive.
		/// Any events queued up after this will still fire, and will be expected the world not to be destroyed.
		/// </summary>
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

        float CoolDown
        {
            get;
            set;
        }
    }
}
