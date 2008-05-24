using System;
using System.Collections.Generic;
using System.Text;
using Physics2;

namespace Project_blob
{
	internal class TriggerBody : Body
	{
		private EventTrigger _triggeredEvent;
		public EventTrigger TriggeredEvent { get { return _triggeredEvent; } }

		private float Time = 0f;

		private int Count = 0;

		//static constructor
		public TriggerBody(List<CollidableStatic> Collidables, Body ParentBody, EventTrigger triggeredEvent)
			: base(Collidables, ParentBody)
		{
			_triggeredEvent = triggeredEvent;
		}

		//"dynamic" constructor
		public TriggerBody(Body ParentBody, List<PhysicsPoint> p_points, List<Collidable> p_collidables, List<Spring> p_springs, List<Task> p_tasks, EventTrigger triggeredEvent)
			: base(ParentBody, p_points, p_collidables, p_springs, p_tasks, false)
		{
			_triggeredEvent = triggeredEvent;
		}

		public override bool isSolid()
		{
			return _triggeredEvent.Solid;
		}

		public override void update(float TotalElapsedSeconds)
		{
			Time -= TotalElapsedSeconds;
			base.update(TotalElapsedSeconds);
		}

		public override void onCollision(CollisionEvent e)
		{
			if ((Time < 0 && (_triggeredEvent.NumTriggers < 0 || Count < _triggeredEvent.NumTriggers)) && e.point.ParentBody is Blob)
			{
				if (_triggeredEvent.PerformEvent(e.point))
				{
					Time = _triggeredEvent.CoolDown;
					Count++;
					base.onCollision(e);
				}
			}
			
		}
	}
}
