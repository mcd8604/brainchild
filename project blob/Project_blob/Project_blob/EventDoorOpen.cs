using System;
using System.Collections.Generic;
using System.Text;

namespace Project_blob
{
    class EventDoorOpen
    {
        List<DoorModel> m_DoorList = new List<DoorModel>();

        public EventDoorOpen(List<DoorModel> p_DoorList)
        {
            m_DoorList = p_DoorList;
        }

        public void PerformEvent(Physics2.PhysicsPoint p)
        {
            foreach (DoorModel d in m_DoorList)
            {
                d.DoorOpen();
            }
        }
    }
}
