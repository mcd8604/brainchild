using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	/*
	 * Description: Keeps track of which menus are displayed 
	 * 
	 * TODO: add support for multiple menus
	 */
	class MenuManager
	{
		public static Menu titleMenu;


		public static void initTitleMenu()
		{
			ArrayList menuNames = new ArrayList();
			menuNames[0] = "menu_newgame";
			menuNames[1] = "menu_exitgame";
			titleMenu = new Menu(menuNames, "menu_background");
		}

		public static void loadMenuTextures()
		{
			TextureLibrary.LoadTexture("menu_background");
			TextureLibrary.LoadTexture("menu_newgame");
			TextureLibrary.LoadTexture("menu_exitgame");
		}

	}
}
