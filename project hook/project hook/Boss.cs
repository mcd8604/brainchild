using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class Boss : Ship
	{
		List<Event> m_EventList;
		int m_EventTrigger;
		int EventTrigger
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

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);

			//temp
			if (World.Position.Speed == 0)
			{
				if (ToBeRemoved || Faction != Factions.Enemy)
					EventTrigger = 1;
			}
			//end temp

			while (m_EventTrigger > 0 && m_EventList.Count > 0)
			{
				if (m_EventList[0].Type == Event.Types.ChangeSpeed)
					World.Position.setSpeed(m_EventList[0].Speed);

				m_EventList.RemoveAt(0);
				m_EventTrigger--;
			}
		}


	}
}
