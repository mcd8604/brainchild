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

		private World m_Game;
		public World Game
		{
			get
			{
				return m_Game;
			}
			set
			{
				m_Game = value;
			}
		}

		private int m_EventDistance = 0;

		public LevelHandler()
		{
		}
		public LevelHandler(Dictionary<int, List<Event>> p_Events, World p_Game)
		{
			Events = p_Events;
			Game = p_Game;
		}

		public void CheckEvents(float p_UpToDistance)
		{
			int CurrentPosition = Convert.ToInt32( Math.Ceiling(p_UpToDistance) );

			for (; m_EventDistance < CurrentPosition; m_EventDistance++)
			{
				if (m_Events.ContainsKey(m_EventDistance))
				{
					//read events
					Event[] t_List = new Event[m_Events[m_EventDistance].Count];
					m_Events[m_EventDistance].CopyTo(t_List);
					for (int i = 0; i < m_Events[m_EventDistance].Count; ++i)
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
					m_Events[m_EventDistance].Clear();
				}
			}
		}

		public void CreateCollidable(Collidable p_Collidable)
		{
			//add the sprtie to the sprtiebatch in the game class
			//Sprite.addSprite(p_Collidable);
			m_Game.AddSprite(p_Collidable);
		}

		public void ChangeSpeed(int p_Speed)
		{
			//change the speed in the game file
			World.Position.setSpeed(p_Speed);
		}

		public void ChangeFile(String p_FileName)
		{
			//change the fileName in the levelReader class
			m_Game.ChangeFile(p_FileName);
			//tell it to read the file
		}
	}
}
