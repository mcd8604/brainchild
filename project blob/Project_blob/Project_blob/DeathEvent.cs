using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
	[Serializable]
	public class DeathEvent : EventTrigger
	{

		private bool m_Solid = false;
		public bool Solid
		{
			get
			{
				return m_Solid;
			}
			set
			{
				m_Solid = value;
			}
		}

		public DeathEvent() { }

        public bool PerformEvent( PhysicsPoint point )
        {
            return GameplayScreen.CauseDeath(point.ParentBody);
		}
	}
}
