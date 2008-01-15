using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class BrainChildLogo : Menu
	{
		int m_Delay;
		double m_Time;

		public BrainChildLogo()
			: base()
		{
			//change to so texture that is is made for our title screen
			m_BackgroundName = "virus";

			m_Time = 0;
			m_Delay = 5;
		}

		/*protected override void Init()
		{
			base.Init();
			m_Created = 0;
			m_Time = 0;

			//60fps*5sec=300mil sec
			m_Delay = 300;
		}*/

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			m_Time += p_Time.ElapsedGameTime.TotalSeconds;

			if (Game.m_KeyHandler.IsActionPressed(KeyHandler.Actions.Pause) || m_Time >= m_Delay)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.RITLogo);
			}
		}
	}
}