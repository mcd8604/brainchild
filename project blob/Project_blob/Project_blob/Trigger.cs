using System.Collections.Generic;
using Physics2;

namespace Project_blob
{
	public class Trigger : Body
	{
		private EventTrigger _triggeredEvent;
		public EventTrigger TriggeredEvent { get { return _triggeredEvent; } }

        public Trigger(Body ParentBody, List<PhysicsPoint> p_points, List<Collidable> p_collidables, List<Spring> p_springs, List<Task> p_tasks, string p_collisionSound, EventTrigger triggeredEvent)
            : base(ParentBody, p_points, p_collidables, p_springs, p_tasks, p_collisionSound)
		{
			_triggeredEvent = triggeredEvent;
		}

		public override bool isSolid()
		{
			return false;
		}

		public override void onCollision(PhysicsPoint p)
		{
            base.onCollision(p);
			_triggeredEvent.PerformEvent(p);
		}
	}
}
