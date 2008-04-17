using System;
using System.Collections.Generic;
using System.Text;
using Project_blob.GameState;

namespace Project_blob
{
    public interface EventTrigger
    {
        void PerformEvent(GameplayScreen gameRef);
    }
}
