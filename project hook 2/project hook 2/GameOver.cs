using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace project_hook
{
	class GameOver : Menu
	{
		public GameOver()
		{
			m_BackgroundName = "menu_background";

			m_HighlightName = "menu_highlight";

			m_MenuItemNames = new ArrayList();

			usingTextSprite = true;
			m_MenuItemNames.Add("Restart Game");
			m_MenuItemNames.Add("Exit to Main Menu");
			m_MenuItemNames.Add("Quit to Windows");
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
