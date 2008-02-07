using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class Boss : Ship
	{
		List<Event> m_EventList;
		bool m_EventTrigger;
		bool EventTrigger
		{
			get
			{
				return m_EventTrigger;
			}
			set
			{
				m_EventTrigger = value;
			}
		}

		public Boss()
		{
			m_EventList = new List<Event>();
			//temp
			m_EventList.Add(new Event(80));
		}

		public Boss(List<Event> p_EventList)
		{
			m_EventList = p_EventList;
		}
			

		public override void RegisterCollision(Collidable p_Other)
		{
#if DEBUG
			Console.WriteLine("The Trigger has been hit by " + p_Other + "!");
#endif
			base.RegisterCollision(p_Other);
		}

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);
			
			//temp
			if (World.Position.Speed == 0)
			{
				if (this.ToBeRemoved == true || this.Faction != Factions.Enemy)
					EventTrigger = true;
			}
			//end temp

			if (m_EventTrigger && m_EventList.Count > 0)
			{
				if (m_EventList[0].Type == Event.Types.ChangeSpeed)
					World.Position.setSpeed(m_EventList[0].Speed);

				m_EventList.RemoveAt(0);
			}
		}

		
	}
}
