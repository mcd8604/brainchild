using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class MenuMain : Menu
	{

		private const int DELAY_SECONDS = 20;
		private double idleTime = 0;

		public MenuMain()
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
			m_MenuItemNames.Add("High Scores");
			m_MenuItemNames.Add("Credits");
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
				Menus.setCurrentMenu(Menus.MenuScreens.HighScores);
			}

			if (m_selectedIndex == 4)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Credits);
			}

			if (m_selectedIndex == 5)
			{
				Menus.Exit = true;
			}
		}
		public override void cancel()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.BrainChildLogo);
		}

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);
			if (InputHandler.HasMouseMoved())
			{
				idleTime = 0;
			}
			else
			{
				idleTime += p_Time.ElapsedRealTime.TotalSeconds;
			}
			if (idleTime > DELAY_SECONDS)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Instructions1);
			}
		}
	}
}
