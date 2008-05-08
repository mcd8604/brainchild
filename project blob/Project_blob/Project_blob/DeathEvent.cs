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
        public DeathEvent() { }

        public void PerformEvent(PhysicsPoint point)
        {

        }
    }
}
