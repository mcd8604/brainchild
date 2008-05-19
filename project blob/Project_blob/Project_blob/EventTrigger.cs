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
		/// What is 'anything world altering'?
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
    }
}
