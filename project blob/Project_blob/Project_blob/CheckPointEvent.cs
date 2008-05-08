using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
    [Serializable]
    public class CheckPointEvent : EventTrigger
    {
        public CheckPointEvent() { }

        public void PerformEvent(PhysicsPoint point)
        {

        }
    }
}
