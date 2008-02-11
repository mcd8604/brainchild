using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class Instructions3 : Menu
	{
		int m_Delay;
		double m_Time;

		public Instructions3()
			: base()
		{
			m_BackgroundName = "Instructions3";

			m_Time = 0;
			m_Delay = 10;
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