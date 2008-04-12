using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project_blob
{
    public class TransitionEvent : EventTrigger {
        
        private Area _area;
        private Vector3 _position;
    
        public TransitionEvent(Area area, float xPos, float yPos, float zPos)
        {
            _area = area;
            _position = new Vector3(xPos, yPos, zPos);
        }

        public void PerformEvent()
        {
        }
    }
}
