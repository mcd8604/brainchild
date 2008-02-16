using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class Menus
	{
		private static Boolean m_HasChanged;
		public static Boolean HasChanged
		{
			get
			{
				return m_HasChanged;
			}
		}

		private static Boolean m_Exit = false;
		public static Boolean Exit
		{
			get
			{
				return m_Exit;
			}
			set
			{
				m_Exit = value;
			}
		}

		private static List<MenuScreens> m_PreviousMenus;

		private static MenuScreens m_SelectedMenu;
		public static MenuScreens SelectedMenu
		{
			get
			{
				return m_SelectedMenu;
			}
		}

		public enum MenuScreens
		{
			Main,
			Pause,
			Options,
			OptionsGame,
			Instructions1,
			Instructions2,
			Instructions3,
			Loading,
			Start,
			None,
			TitleScreen,
			BrainChildLogo,
			RITLogo,
			GameOver,
			Credits,
			HighScores
		}

		public static void ini()
		{
			iniTextures();
			m_PreviousMenus = new List<MenuScreens>();
			m_SelectedMenu = MenuScreens.None;
			m_HasChanged = false;
		}

		public static void iniTextures()
		{
			//load all menu textures here instead of during instansiation
			TextureLibrary.LoadTexture("bcg");
			TextureLibrary.LoadTexture("menu_background");
			TextureLibrary.LoadTexture("menu_highlight");
			TextureLibrary.LoadTexture("menu_newgame");
			TextureLibrary.LoadTexture("menu_exitgame");
			TextureLibrary.LoadTexture("menu_restart");
			TextureLibrary.LoadTexture("menu_quit");
			TextureLibrary.LoadTexture("RITLogo");
			TextureLibrary.LoadTexture("Title");
			TextureLibrary.LoadTexture("virus");
			TextureLibrary.LoadTexture("Instructions1");
			TextureLibrary.LoadTexture("Instructions2");
			TextureLibrary.LoadTexture("Instructions3");
			TextureLibrary.LoadTexture("Credits");
			TextureLibrary.LoadTexture("menuCursor");
		}

		public static void setCurrentMenu(MenuScreens p_NewMenu)
		{
			m_PreviousMenus.Add(m_SelectedMenu);
			m_SelectedMenu = p_NewMenu;
			m_HasChanged = true;
		}

		public static void returnToPreviousMenu()
		{
			m_SelectedMenu = m_PreviousMenus[m_PreviousMenus.Count - 1];
			m_PreviousMenus.RemoveAt(m_PreviousMenus.Count - 1);
			m_HasChanged = true;
		}

		public static Menu getCurrentMenu()
		{
			if (m_SelectedMenu == MenuScreens.Main)
			{
				m_HasChanged = false;
				return new MenuMain();
			}
			else if (m_SelectedMenu == MenuScreens.Pause)
			{
				m_HasChanged = false;
				return new MenuPause();
			}
			else if (m_SelectedMenu == MenuScreens.Options)
			{
				m_HasChanged = false;
				return new MenuOptions();
			}
			else if (m_SelectedMenu == MenuScreens.OptionsGame)
			{
				m_HasChanged = false;
				return new MenuOptionsGame();
			}
			else if (m_SelectedMenu == MenuScreens.Instructions1)
			{
				m_HasChanged = false;
				return new MenuInstructions1();
			}
			else if (m_SelectedMenu == MenuScreens.Instructions2)
			{
				m_HasChanged = false;
				return new MenuInstructions2();
			}
			else if (m_SelectedMenu == MenuScreens.Instructions3)
			{
				m_HasChanged = false;
				return new MenuInstructions3();
			}
			else if (m_SelectedMenu == MenuScreens.TitleScreen)
			{
				m_HasChanged = false;
				return new MenuTitleScreen();
			}
			else if (m_SelectedMenu == MenuScreens.BrainChildLogo)
			{
				m_HasChanged = false;
				return new MenuBrainChildLogo();
			}
			else if (m_SelectedMenu == MenuScreens.RITLogo)
			{
				m_HasChanged = false;
				return new MenuRITLogo();
			}
			else if (m_SelectedMenu == MenuScreens.GameOver)
			{
				m_HasChanged = false;
				return new MenuGameOver();
			}
			else if (m_SelectedMenu == MenuScreens.Credits)
			{
				m_HasChanged = false;
				return new MenuCredits();
			}
			else if (m_SelectedMenu == MenuScreens.HighScores)
			{
				m_HasChanged = false;
				return new MenuHighScore();
			}

			return null;
		}
	}
}


