using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class Level
	{
		public Level()
		{

		}

		public void Load(String p_LevelName)
		{
			LevelReader lr = new LevelReader(p_LevelName);

		}

		public void Update(GameTime p_GameTime)
		{

		}
	}
}
