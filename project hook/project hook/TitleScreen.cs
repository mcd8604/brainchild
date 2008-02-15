using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	class TitleScreen : Menu
	{
		TextSprite m_Text;

		public TitleScreen()
			: base()
		{
			//change to so texture that is is made for our title screen
			m_BackgroundName = "Title";
		}

		public override void Load(GraphicsDeviceManager gdm)
		{
			base.Load(gdm);

			//add the text
			//m_Text = new TextSprite("Press Enter", new Microsoft.Xna.Framework.Vector2(400, 500), Color.White, Depth.MenuLayer.Text);
			//m_MenuItemSprites.Add(m_Text);
			//attachSpritePart(m_Text);
		}

		public override void accept()
		{
			//load up main menu
			Menus.setCurrentMenu(Menus.MenuScreens.Main);
		}

		public override void Update(GameTime p_Time)
		{
			//add text here

			//toggle it onand off here
			base.Update(p_Time);
		}

	}
}
