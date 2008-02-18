using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	class MenuHighScore : Menu
	{

		public MenuHighScore()
		{ }

		public override void Load()
		{
			//base.Load();

			GameTexture bgTexture = TextureLibrary.getGameTexture(m_BackgroundName);
			float xCen = Game.graphics.GraphicsDevice.Viewport.Width * 0.5f;
			float yCen = Game.graphics.GraphicsDevice.Viewport.Height * 0.5f;
			m_BackgroundSprite = new Sprite(
#if !FINAL
				m_BackgroundName,
#endif
				Vector2.Zero, 330, 184, bgTexture, 200f, true, 0, Depth.MenuLayer.Background);
			m_BackgroundSprite.Center = new Vector2(xCen, yCen);
			attachSpritePart(m_BackgroundSprite);

			attachSpritePart(new TextSprite("HighScores:", new Vector2(xCen, yCen - 200), Color.Yellow));

			int[] list = Game.HighScores.Scores;

			for (int i = 0; i < HighScore.size; i++)
			{
				if (list[i] > 0)
				{
					attachSpritePart(new TextSprite(list[i].ToString(), new Vector2(xCen, (yCen - 142) + (32 * i)), Color.WhiteSmoke));
				}
			}
		}

		public override void accept()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.Main);
		}
		public override void cancel()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.Main);
		}

	}
}
