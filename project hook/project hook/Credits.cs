using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class Credits : Menu
	{
		int m_Delay;
		double m_Time;

		public Credits()
			: base()
		{
			//change to so texture that is is made for our title screen
			m_BackgroundName = "Credits";

			m_Time = 0;
			m_Delay = 20;
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			m_Time += p_Time.ElapsedGameTime.TotalSeconds;

			if (InputHandler.IsActionPressed(Actions.Pause) || InputHandler.IsActionPressed(Actions.MenuAccept) || m_Time >= m_Delay)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Main);
			}
		}
	}
}
