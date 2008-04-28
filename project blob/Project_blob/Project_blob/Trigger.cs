using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project_blob.GameState;

namespace Project_blob
{
	public class Trigger : Physics.CollidableTri
	{
		private EventTrigger _triggeredEvent;
		public EventTrigger TriggeredEvent { get { return _triggeredEvent; } }

		public static List<EventTrigger> Events = new List<EventTrigger>();

		public Trigger(VertexPositionNormalTexture point1, VertexPositionNormalTexture point2, VertexPositionNormalTexture point3, EventTrigger triggeredEvent)
			: base(point1, point2, point3)
		{
			_triggeredEvent = triggeredEvent;
		}

		public override bool shouldPhysicsBlock(Physics.Point p)
		{
			if (!Events.Contains(TriggeredEvent))
			{
				Events.Add(TriggeredEvent);
			}
			return false;
		}

		public override void TriggerEvents()
		{
			foreach (EventTrigger triggered in Events)
			{
				triggered.PerformEvent(GameplayScreen.game);
			}
			Events.Clear();
		}
	}
}
