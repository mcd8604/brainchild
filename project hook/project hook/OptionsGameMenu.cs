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
			if (Game.graphics.IsFullScreen)
			{
				m_MenuItemNames.Add("Fullscreen On");
			} else {
				m_MenuItemNames.Add("Fullscreen Off");
			}
			if (Music.IsPlaying("bg2"))
			{
				m_MenuItemNames.Add("Music On");
			} else {
				m_MenuItemNames.Add("Music Off");
			}
			if (Sound.getPlaySound())
			{
				m_MenuItemNames.Add("Sound Effects On");
			} else {
				m_MenuItemNames.Add("Sound Effects Off");
			}
			if (World.getPrimaryRight())
			{
				m_MenuItemNames.Add("Rightclick Primary On");
			} else {
				m_MenuItemNames.Add("Rightclick Primary Off");
			}
			m_MenuItemNames.Add("Back");
		}

		public override void accept()
		{
			if (m_selectedIndex == 0)
			{
				Game.graphics.ToggleFullScreen();
				if (((TextSprite)m_MenuItemSprites[0]).Text == "Fullscreen On")
				{
					((TextSprite)m_MenuItemSprites[0]).Text = "Fullscreen Off";
				} else {
					((TextSprite)m_MenuItemSprites[0]).Text = "Fullscreen On";
				}
			}

			if (m_selectedIndex == 1)
			{
				Music.setPlaySound(true);
				if (Music.IsPlaying("bg1"))
				{
					Music.Stop("bg2");
					((TextSprite)m_MenuItemSprites[1]).Text = "Music Off";
				} else {
					Music.Play("bg2");
					((TextSprite)m_MenuItemSprites[1]).Text = "Music On";
				}
			}

			if (m_selectedIndex == 2)
			{
				Sound.setPlaySound();
				if (((TextSprite)m_MenuItemSprites[2]).Text == "Sound Effects On")
				{
					((TextSprite)m_MenuItemSprites[2]).Text = "Sound Effects Off";
				} else {
					((TextSprite)m_MenuItemSprites[2]).Text = "Sound Effects On";
				}
			}

			if (m_selectedIndex == 3)
			{
				World.setPrimaryRight();
				if (((TextSprite)m_MenuItemSprites[3]).Text == "Rightclick Primary On")
				{
					((TextSprite)m_MenuItemSprites[3]).Text = "Rightclick Primary Off";
				} else {
					((TextSprite)m_MenuItemSprites[3]).Text = "Rightclick Primary On";
				}
			}

			if (m_selectedIndex ==4)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Pause);
			}
		}
	}
}