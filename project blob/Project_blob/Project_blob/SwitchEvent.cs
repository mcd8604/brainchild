using System;
using System.Collections.Generic;
using System.Text;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
    public class SwitchEvent : EventTrigger
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

        List<EventTrigger> m_Events = new List<EventTrigger>();

        public SwitchEvent() { }

        public SwitchEvent(List<EventTrigger> p_Events)
        {
            m_Events = p_Events;
        }

        public bool PerformEvent( PhysicsPoint point )
        {
            bool partialSuccess = false;
            foreach (EventTrigger e in m_Events)
            {
                if ( e.PerformEvent( point ) )
                {
                    partialSuccess = true;
                }
            }
            return partialSuccess;
        }
    }
}
