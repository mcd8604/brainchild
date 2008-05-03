using Physics2;

namespace Project_blob
{
	public class Trigger : Body
	{
		private EventTrigger _triggeredEvent;
		public EventTrigger TriggeredEvent { get { return _triggeredEvent; } }

		public Trigger(EventTrigger triggeredEvent)
			: base()
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
