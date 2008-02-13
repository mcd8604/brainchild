using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class MainMenu : Menu
	{
		public MainMenu()
			: base()
		{
			m_BackgroundName = "menu_background";

			m_HighlightName = "menu_highlight";

			m_MenuItemNames = new ArrayList();
			//m_MenuItemNames.Add("menu_newgame");
			//m_MenuItemNames.Add("menu_quit");

			usingTextSprite = true;
			m_MenuItemNames.Add("Start");
			m_MenuItemNames.Add("Instructions");
			m_MenuItemNames.Add("Options");
			m_MenuItemNames.Add("Quit");
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
				Menus.setCurrentMenu(Menus.MenuScreens.Instructions1); 
			}

			if (m_selectedIndex == 2)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Options);
			}

			if (m_selectedIndex == 3)
			{
				Menus.Exit = true;
			}
		}
	}
}
