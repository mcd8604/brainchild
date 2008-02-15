using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	class HighScoreDisplay : Menu
	{

		public HighScoreDisplay()
		{


		}

		public override void Load()
		{
			//base.Load();

			GameTexture bgTexture = TextureLibrary.getGameTexture(m_BackgroundName, "");
			float xCen = Game.graphics.GraphicsDevice.Viewport.Width * 0.5f;
			float yCen = Game.graphics.GraphicsDevice.Viewport.Height * 0.5f;
			m_BackgroundSprite = new Sprite(
#if !FINAL
				m_BackgroundName,
#endif
Vector2.Zero, Convert.ToInt32( bgTexture.Height * 1.25f), Convert.ToInt32( bgTexture.Width * 0.5f ), bgTexture, 200f, true, 0, Depth.MenuLayer.Background);
			m_BackgroundSprite.Center = new Vector2(xCen, yCen);
			attachSpritePart(m_BackgroundSprite);

			attachSpritePart( new TextSprite("HighScores:", new Vector2( xCen, yCen - 240 ), Color.Yellow ) );

			int[] list = Game.HighScores.Scores;

			for (int i = 0; i < HighScore.size; i++)
			{
				if ( list[i] > 0 ) {
					attachSpritePart(new TextSprite(list[i].ToString(), new Vector2(xCen, (yCen - 160 ) + (32 * i)), Color.WhiteSmoke));
				}
			}
		}

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			if (m_Parts != null)
			{
				m_Parts.RemoveAll(isToBeRemoved);
				foreach (Sprite part in m_Parts)
				{
					part.Update(p_Time);
				}
			}
			if (InputHandler.IsActionPressed(Actions.MenuAccept) || InputHandler.IsActionPressed(Actions.MenuBack))
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Main);
			}
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
		{
			base.Draw(p_SpriteBatch);
		}

	}
}
