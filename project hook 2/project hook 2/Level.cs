using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class Level
	{
		private int m_Distance;
		public int Distance
		{
			get
			{
				return m_Distance;
			}
			set
			{
				m_Distance = value;
			}
		}


		public Level()
		{
			m_Distance = 0;
		}

		public Level(int p_Distance)
		{
			m_Distance = p_Distance;
		}

		public void Load(String p_LevelName)
		{
			LevelReader lr = new LevelReader(p_LevelName);

		}

		public void Update(GameTime p_GameTime)
		{
			//increment distance

			//check levelreader for new events
		}
	}
}
