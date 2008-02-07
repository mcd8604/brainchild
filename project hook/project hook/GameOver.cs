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
			m_MenuItemNames.Add("Restart Level");
			m_MenuItemNames.Add("New Game");
			m_MenuItemNames.Add("Exit to Main Menu");
			m_MenuItemNames.Add("Quit to Windows");
		}

		public override void accept()
		{
			if (m_selectedIndex == 0)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.None);
				World.RestartLevel = true;
			}

			if (m_selectedIndex == 1)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.None);
				World.CreateWorld = true;
			}

			if (m_selectedIndex == 2)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Main);
				World.DestroyWorld = true;
			}

			if (m_selectedIndex == 3)
			{
				Menus.Exit = true;
			}
		}
	}
}
