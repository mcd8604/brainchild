using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class Menu : Sprite
	{
		protected int m_selectedIndex;

		protected String m_BackgroundName;
		protected Sprite m_BackgroundSprite;

		protected String m_HighlightName;
		protected Sprite m_HightlightSprite;

		protected ArrayList m_MenuItemNames;
		protected ArrayList m_MenuItemSprites;

		protected String m_MenuCursorName;
		protected Sprite m_MenuCursorSprite;

		protected Boolean usingTextSprite;

		public Menu()
		{
			//default textures
			m_BackgroundName = "menu_background";
			m_HighlightName = "menu_highlight";

			m_MenuItemNames = new ArrayList();

			m_MenuCursorName = "menuCursor";
		}

		public virtual void Load()
		{
			//background sprite
			GameTexture bgTexture = TextureLibrary.getGameTexture(m_BackgroundName, "");
			float xPos = Game.graphics.GraphicsDevice.Viewport.Width * 0.5f;
			float yPos = Game.graphics.GraphicsDevice.Viewport.Height * 0.5f;
			m_BackgroundSprite = new Sprite(
#if !FINAL
				m_BackgroundName,
#endif
				Vector2.Zero, bgTexture.Height, bgTexture.Width, bgTexture, 200f, true, 0, Depth.MenuLayer.Background);
			m_BackgroundSprite.Center = new Vector2(xPos, yPos);
			attachSpritePart(m_BackgroundSprite);

			//cursor sprite
            GameTexture cursorTexture = TextureLibrary.getGameTexture(m_MenuCursorName, "");
			m_MenuCursorSprite = new Sprite(
#if !FINAL
				m_MenuCursorName,
#endif
				Vector2.Zero, cursorTexture.Height, cursorTexture.Width, cursorTexture, 1f, true, 0f, Depth.MenuLayer.Cursor);
			m_MenuCursorSprite.Task = new TaskAttachTo(InputHandler.getMousePosition, new Vector2(cursorTexture.Width * 0.5f, cursorTexture.Height * 0.5f));
			attachSpritePart(m_MenuCursorSprite);

			m_MenuItemSprites = new ArrayList();
			if (m_MenuItemNames.Count != 0)
			{
				if (usingTextSprite)
				{
					createTextSprites();
				}
				else
				{
					createImageSprites();
				}
			}
		}

		private void createTextSprites()
		{
			int textHeight = 32;
			int topBuffer = 32;

			//create each menu item from TextSprites
			for (int i = 0; i < m_MenuItemNames.Count; i++)
			{
				float xPos = m_BackgroundSprite.Center.X;
				float yPos = topBuffer + m_BackgroundSprite.Position.Y + (textHeight * i);

				TextSprite mis = new TextSprite((String)m_MenuItemNames[i], new Vector2(xPos, yPos));

				m_MenuItemSprites.Add(mis);
				attachSpritePart(mis);
			}

			//set highlighted
			setHighlightSprite();
			TextSprite selSprite = (TextSprite)m_MenuItemSprites[m_selectedIndex];
			selSprite.Color = Color.Yellow;
		}

		private void createImageSprites()
		{

			//create each menu sprite - image textures
			for (int i = 0; i < m_MenuItemNames.Count; i++)
			{
				GameTexture curTexture = TextureLibrary.getGameTexture((String)m_MenuItemNames[i], "");
				float xPos = m_BackgroundSprite.Center.X;
				float yPos = m_BackgroundSprite.Center.Y;
				//set position based on other menu sprites
				for (int k = 0; k < m_MenuItemSprites.Count; k++)
				{
					Sprite s = (Sprite)m_MenuItemSprites[k];
					yPos += s.Height;
				}
				Sprite mis = new Sprite(
#if !FINAL
					(String)m_MenuItemNames[i],
#endif
					Vector2.Zero, curTexture.Height, curTexture.Width, curTexture, 255f, true, 0, Depth.MenuLayer.Text);
				mis.Center = new Vector2(xPos, yPos);
				m_MenuItemSprites.Add(mis);
				attachSpritePart(mis);
			}

			//create highlight sprite
			Sprite selSprite = (Sprite)m_MenuItemSprites[m_selectedIndex];
			GameTexture highlightTexture = TextureLibrary.getGameTexture(m_HighlightName, "");
			m_HightlightSprite = new Sprite(
#if !FINAL
				m_HighlightName,
#endif
				new Vector2(selSprite.Position.X, selSprite.Position.Y), selSprite.Texture.Height, selSprite.Texture.Width, highlightTexture, 255f, true, 0, Depth.MenuLayer.Highlight);
			attachSpritePart(m_HightlightSprite);
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
			if (InputHandler.IsActionPressed(Actions.Pause))
			{
				cancel();               
			}

			if (InputHandler.IsActionPressed(Actions.Up))
			{
				up();
			}

			if (InputHandler.IsActionPressed(Actions.Down))
			{
				down();
			}

			if (InputHandler.IsActionPressed(Actions.MenuAccept))
			{
				accept();
			}

			if (InputHandler.HasMouseMoved())
			{
				selectSpriteByCoord(InputHandler.MousePosition);
			}
		}

		protected Boolean selectSpriteByCoord(Vector2 mousePos)
		{
			if (m_MenuItemSprites != null)
			{
				foreach (Sprite s in m_MenuItemSprites)
				{
					if (mousePos.X >= s.Position.X && mousePos.Y >= s.Position.Y &&
						mousePos.X <= s.Position.X + s.Width && mousePos.Y <= s.Position.Y + s.Height)
					{
						setSelectedIndex(m_MenuItemSprites.IndexOf(s));
						return true;
					}
				}
			}
			return false;
		}

		public override void Draw(SpriteBatch p_SpriteBatch)
		{
			p_SpriteBatch.Begin();
			base.Draw(p_SpriteBatch);
			p_SpriteBatch.End();
		}

		protected void down()
		{
			if (m_selectedIndex < m_MenuItemSprites.Count - 1)
			{
				m_selectedIndex++;
			}
			else
			{
				m_selectedIndex = 0;
			}

			setHighlightSprite();
		}

		protected void up()
		{
			if (m_selectedIndex > 0)
			{
				m_selectedIndex--;
			}
			else
			{
				m_selectedIndex = m_MenuItemSprites.Count - 1;
			}

			setHighlightSprite();
		}

		public void setSelectedIndex(int index)
		{
			if (index < 0)
			{
				index = 0;
			}
			else if (index > m_MenuItemSprites.Count - 1)
			{
				index = m_MenuItemSprites.Count - 1;
			}

			m_selectedIndex = index;
			setHighlightSprite();
		}

		protected void setHighlightSprite()
		{
			Sprite selSprite = (Sprite)m_MenuItemSprites[m_selectedIndex];
			if (selSprite is TextSprite)
			{
				foreach (Sprite s in m_MenuItemSprites)
				{
					s.Color = Color.White;
				}
				selSprite.Color = Color.Yellow;
			}
			else
			{
				if (m_HightlightSprite != null)
				{
					m_HightlightSprite.Width = selSprite.Width;
					m_HightlightSprite.Center = selSprite.Center;
				}
			}
		}

		public abstract void accept();

		public abstract void cancel();
	}
}
