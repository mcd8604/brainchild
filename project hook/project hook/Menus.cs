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
            None
        }

        public static void ini()
		{
            m_PreviousMenus = new List<MenuScreens>();
            m_SelectedMenu = MenuScreens.None;
            m_HasChanged = false;
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

            return null;
        }
    }
}


