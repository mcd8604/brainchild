using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class MenuOptions : Menu
	{
		internal MenuOptions()
		{
			m_BackgroundName = "menu_background";

			m_HighlightName = "menu_highlight";

			m_MenuItemNames = new ArrayList();

			usingTextSprite = true;
			if (Game.graphics.IsFullScreen)
			{
				m_MenuItemNames.Add("Fullscreen On");
			}
			else
			{
				m_MenuItemNames.Add("Fullscreen Off");
			}
			if (Music.getPlaySound())
			{
				m_MenuItemNames.Add("Music On");
			}
			else
			{
				m_MenuItemNames.Add("Music Off");
			}
			if (Sound.getPlaySound())
			{
				m_MenuItemNames.Add("Sound Effects On");
			}
			else
			{
				m_MenuItemNames.Add("Sound Effects Off");
			}
			if (World.getPrimaryRight())
			{
				m_MenuItemNames.Add("Rightclick Primary On");
			}
			else
			{
				m_MenuItemNames.Add("Rightclick Primary Off");
			}
			m_MenuItemNames.Add("Main Menu");
		}

		internal override void accept()
		{
			if (m_selectedIndex == 0)
			{
				Game.graphics.ToggleFullScreen();
				if (Game.graphics.IsFullScreen)
				{
					((TextSprite)m_MenuItemSprites[0]).Text = "Fullscreen On";
				}
				else
				{
					((TextSprite)m_MenuItemSprites[0]).Text = "Fullscreen Off";
				}
			}

			if (m_selectedIndex == 1)
			{
				if (Music.getPlaySound())
				{
					Music.setPlaySound(false);
					((TextSprite)m_MenuItemSprites[1]).Text = "Music Off";
				}
				else
				{
					Music.setPlaySound(true);
					((TextSprite)m_MenuItemSprites[1]).Text = "Music On";
				}
			}

			if (m_selectedIndex == 2)
			{
				Sound.togglePlaySound();
				if (Sound.getPlaySound())
				{
					((TextSprite)m_MenuItemSprites[2]).Text = "Sound Effects On";
				}
				else
				{
					((TextSprite)m_MenuItemSprites[2]).Text = "Sound Effects Off";
				}
			}

			if (m_selectedIndex == 3)
			{
				World.togglePrimaryRight();
				if (World.getPrimaryRight())
				{
					((TextSprite)m_MenuItemSprites[3]).Text = "Rightclick Primary On";
				}
				else
				{
					((TextSprite)m_MenuItemSprites[3]).Text = "Rightclick Primary Off";
				}
			}

			if (m_selectedIndex == 4)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Main);
			}
		}
		internal override void cancel()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.Main);
		}
	}
}