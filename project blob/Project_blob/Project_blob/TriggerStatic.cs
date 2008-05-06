using System;
using System.Collections.Generic;
using System.Text;
using Physics2;

namespace Project_blob
{
    class TriggerStatic : BodyStatic
    {
        private EventTrigger _triggeredEvent;
		public EventTrigger TriggeredEvent { get { return _triggeredEvent; } }

		public TriggerStatic(IList<CollidableStatic> Collidables, Body ParentBody, EventTrigger triggeredEvent)
			: base(Collidables, ParentBody)
		{
			_triggeredEvent = triggeredEvent;
		}

		public override bool isSolid()
		{
			return false;
		}

		public override void onCollision(PhysicsPoint p)
		{
			_triggeredEvent.PerformEvent(p);
		}
    }
}
