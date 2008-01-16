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
			m_MenuItemNames.Add("New Game");
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
				Menus.Exit = true;   
			}
		}
	}    
}
