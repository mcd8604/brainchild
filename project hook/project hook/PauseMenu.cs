using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class PauseMenu : Menu
	{

		public PauseMenu()
			: base()
		{
			m_BackgroundName = "menu_background";

			m_HighlightName = "menu_highlight";

			m_MenuItemNames = new ArrayList();
			m_MenuItemNames.Add("menu_restart");
			m_MenuItemNames.Add("menu_exitgame");
			m_MenuItemNames.Add("menu_quit");
		}

		public override void accept()
		{
			if (m_selectedIndex == 0)
			{
				World.CreateWorld = true;
				Menus.setCurrentMenu(Menus.MenuScreens.None);
			}

			if (m_selectedIndex == 1)
			{
				World.DestroyWorld = true;
				Menus.setCurrentMenu(Menus.MenuScreens.Main);
			}

			if (m_selectedIndex == 2)
			{
				Menus.Exit = true; 
			}
		}
	}
}
