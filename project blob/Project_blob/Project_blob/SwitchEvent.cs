using System;
using System.Collections.Generic;
using System.Text;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
    public class SwitchEvent : EventTrigger
    {
        List<EventTrigger> m_Events = new List<EventTrigger>();

        public SwitchEvent(List<EventTrigger> p_Events)
        {
            m_Events = p_Events;
        }

        public void PerformEvent(PhysicsPoint point)
        {
            foreach (EventTrigger e in m_Events)
            {
                e.PerformEvent(point);
            }
        }
    }
}
