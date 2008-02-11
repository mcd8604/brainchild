using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class OptionsGameMenu : Menu
	{
		public OptionsGameMenu()
			: base()
		{
			m_BackgroundName = "menu_background";

			m_HighlightName = "menu_highlight";

			m_MenuItemNames = new ArrayList();

			usingTextSprite = true;
			m_MenuItemNames.Add("Toggle Fullscreen");
			m_MenuItemNames.Add("Toggle Music");
			m_MenuItemNames.Add("Toggle SFX");
			m_MenuItemNames.Add("Primary on rightclick");
			m_MenuItemNames.Add("Back");
		}

		public override void accept()
		{
			if (m_selectedIndex == 0)
			{
				Game.graphics.ToggleFullScreen();
			}

			if (m_selectedIndex == 1)
			{
				Music.setPlaySound(true);
				if (Music.IsPlaying("bg1"))
				{
					Music.Stop("bg1");
				}
				else
				{
					Music.Play("bg1");
				}
			}

			if (m_selectedIndex == 2)
			{
				Sound.setPlaySound();
			}

			if (m_selectedIndex == 3)
			{
				World.setPrimaryRight();
			}

			if (m_selectedIndex ==4)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Pause);
			}
		}
	}
}