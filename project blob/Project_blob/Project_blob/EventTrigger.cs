using System;
using System.Collections.Generic;
using System.Text;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
    public interface EventTrigger
    {
        void PerformEvent(PhysicsPoint p);
    }
}
