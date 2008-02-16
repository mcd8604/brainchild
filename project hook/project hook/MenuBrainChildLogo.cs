using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class MenuBrainChildLogo : Menu
	{
		double m_Delay = 5;

		public MenuBrainChildLogo()
		{
			//change to so texture that is is made for our title screen
			m_BackgroundName = "bcg";
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			m_Delay -= p_Time.ElapsedGameTime.TotalSeconds;

			if (m_Delay < 0)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.RITLogo);
			}
		}
		public override void accept()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.RITLogo);
		}
		public override void cancel()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.RITLogo);
		}
	}
}