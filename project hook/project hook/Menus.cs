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

        public enum MenuScreens{
            Main,
            Pause,
            Options,
            Loading,
            Start,
            None,
			Title,
			DevLogo,
			RITLogo
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
			TextureLibrary.LoadTexture("menu_background");
			TextureLibrary.LoadTexture("menu_highlight");
			TextureLibrary.LoadTexture("menu_newgame");
			TextureLibrary.LoadTexture("menu_exitgame");
			TextureLibrary.LoadTexture("menu_restart");
			TextureLibrary.LoadTexture("menu_quit");
			TextureLibrary.LoadTexture("RITLogo");
			TextureLibrary.LoadTexture("menu_Title");
			TextureLibrary.LoadTexture("virus");
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

        public static Menu getCurrentMenu(){
            if(m_SelectedMenu == MenuScreens.Main){
                m_HasChanged = false;
                return new MainMenu();
			}
			else if (m_SelectedMenu == MenuScreens.Pause)
			{
				m_HasChanged = false;
				return new PauseMenu();
			}
			else if (m_SelectedMenu == MenuScreens.Title)
			{
				m_HasChanged = false;
				return new TitleScreen();
			}
			else if (m_SelectedMenu == MenuScreens.DevLogo)
			{
				m_HasChanged = false;
				return new BrainChildLogo();
			}
			else if (m_SelectedMenu == MenuScreens.RITLogo)
			{
				m_HasChanged = false;
				return new RITLogo();
			}

            return null;
        }
    }
}


