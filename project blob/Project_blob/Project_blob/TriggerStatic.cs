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

		public float CoolDown = 1f;
		private float Time = 0f;

		public TriggerStatic(IList<CollidableStatic> Collidables, Body ParentBody, string p_collisionAudio, EventTrigger triggeredEvent)
			: base(Collidables, ParentBody, p_collisionAudio)
		{
			_triggeredEvent = triggeredEvent;
		}

		public override bool isSolid()
		{
			return false;
		}

		public override void update(float TotalElapsedSeconds)
		{
			Time -= TotalElapsedSeconds;
			base.update(TotalElapsedSeconds);
		}

        public override void onCollision(CollisionEvent e)
		{
			if (Time < 0)
			{
                if ( _triggeredEvent.PerformEvent( e.point ) )
                {
                    Time = CoolDown;
                }
			}
		}
    }
}
