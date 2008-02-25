using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	class MenuTitleScreen : Menu
	{

		internal MenuTitleScreen()
		{
			//change to so texture that is is made for our title screen
			m_BackgroundName = "Title";
		}

		//internal override void Load()
		//{
		//    base.Load();

		//    //add the text
		//    //m_Text = new TextSprite("Press Enter", new Microsoft.Xna.Framework.Vector2(400, 500), Color.White, Depth.MenuLayer.Text);
		//    //m_MenuItemSprites.Add(m_Text);
		//    //attachSpritePart(m_Text);
		//}

		//internal override void Update(GameTime p_Time)
		//{
		//    //add text here

		//    //toggle it onand off here
		//    base.Update(p_Time);
		//}

		internal override void accept()
		{
			//load up main menu
			Menus.setCurrentMenu(Menus.MenuScreens.Main);
		}
		internal override void cancel()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.Main);
		}

	}
}
