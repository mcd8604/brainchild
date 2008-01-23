using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class LevelHandler
	{
		private Dictionary<int, List<Event>> m_Events;
		public Dictionary<int, List<Event>> Events
		{
			get
			{
				return m_Events;
			}
			set
			{
				m_Events = value;
			}
		}

		public LevelHandler()
		{
		}
		public LevelHandler(Dictionary<int, List<Event>> p_Events)
		{
			Events = p_Events;
		}

		public void CheckEvents(int p_Distance)
		{
			if (m_Events.ContainsKey(p_Distance))
			{
				//read events
				Event[] t_List = new Event[m_Events[p_Distance].Count];
				m_Events[p_Distance].CopyTo(t_List);
				for (int i = 0; i < m_Events[p_Distance].Count; ++i)
				{
					if (t_List[i].Type.Equals("Collidable"))
					{
						CreateCollidable(t_List[i].Collidable);
					}
					else if (t_List[i].Type.Equals("FileChange"))
					{
						ChangeFile(t_List[i].FileName);
					}
					else if (t_List[i].Type.Equals("ChangeSpeed"))
					{
						ChangeSpeed(t_List[i].Speed);
					}
				}
				m_Events[p_Distance].Clear();
			}
		}

		public void CreateCollidable(Collidable p_Collidable)
		{
			//add the sprtie to the sprtiebatch in the game class
			Game1.AddCollidable(p_Collidable);
		}

		public void ChangeSpeed(int p_Speed)
		{
			//change the speed in the game file
			Game1.m_Speed = p_Speed;
		}

		public void ChangeFile(String p_FileName)
		{
			//change the fileName in the levelReader class
			Game1.ChangeFile(p_FileName);
			//tell it to read the file
		}
	}
}
