using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	internal class Boss : Ship
	{
		private List<Event> m_EventList;
		private int m_EventTrigger;
		internal int EventTrigger
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

		internal Boss()
		{
			m_EventList = new List<Event>();
			//temp
			m_EventList.Add(new Event(80));
		}

		internal Boss(List<Event> p_EventList)
		{
			m_EventList = p_EventList;
		}

		internal override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);

			//temp
			if (World.Position.Speed == 0)
			{
				if (ToBeRemoved || Faction != Factions.Enemy)
				{
					m_EventTrigger = 1;
				}
			}
			//end temp

			while (m_EventTrigger > 0 && m_EventList.Count > 0)
			{
				if (m_EventList[0].Type == Event.Types.ChangeSpeed)
				{
					World.Position.setSpeed(m_EventList[0].Speed);
				}

				m_EventList.RemoveAt(0);
				m_EventTrigger--;
			}
		}


	}
}
