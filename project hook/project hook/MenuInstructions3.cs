using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class MenuInstructions3 : Menu
	{
		double m_Delay = 20;

		public MenuInstructions3()
			: base()
		{
			m_BackgroundName = "Instructions3";
		}

		internal override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			m_Delay -= p_Time.ElapsedGameTime.TotalSeconds;

			if (m_Delay < 0)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Main);
			}
		}
		public override void accept()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.Main);
		}
		public override void cancel()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.Main);
		}
	}
}