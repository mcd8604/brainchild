using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class OptionsMenu : Menu
	{
		public OptionsMenu()
			: base()
		{
			m_BackgroundName = "menu_background";

			m_HighlightName = "menu_highlight";

			m_MenuItemNames = new ArrayList();

			usingTextSprite = true;
			m_MenuItemNames.Add("Toggle Fullscreen");
			m_MenuItemNames.Add("Toggle Music");
			m_MenuItemNames.Add("Toggle SFX");
			m_MenuItemNames.Add("Main Menu");
		}

		public override void accept()
		{
			if (m_selectedIndex == 0)
			{
				Game.graphics.ToggleFullScreen();
			}

			if (m_selectedIndex == 1)
			{
				Boolean playMusic = Music.getPlaySound();
				if (playMusic)
				{
					Music.setPlaySound(false);

				} else {
					Music.setPlaySound(true);
				}
			}

			if (m_selectedIndex == 2)
			{
				Sound.setPlaySound();
			}

			if (m_selectedIndex == 3)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Main);
			}
		}
	}
}