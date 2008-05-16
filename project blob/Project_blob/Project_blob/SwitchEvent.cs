using System;
using System.Collections.Generic;
using System.Text;
using Project_blob.GameState;
using Physics2;
using System.ComponentModel;
using System.Drawing.Design;

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
		[Editor(typeof(MultiTypeCollectionEditor), typeof(UITypeEditor))]
		public List<EventTrigger> Events
		{
			get
			{
				return m_Events;
			}
			set
			{
				m_Events = value;
			}
		}

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
