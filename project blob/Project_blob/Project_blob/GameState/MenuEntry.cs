using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Project_blob.GameState
{
	class MenuEntry
	{
		string text;

		float selectionFade;

		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		public Texture2D m_SpriteEntry;
		public Texture2D SpriteEntry
		{
			get { return m_SpriteEntry; }
			set { m_SpriteEntry = value; }
		}

		public event EventHandler<EventArgs> Selected;

		protected internal virtual void OnSelectEntry()
		{
			if (Selected != null)
				Selected(this, EventArgs.Empty);
		}

		public MenuEntry() { }
		public MenuEntry(string text)
		{
			this.text = text;
		}

		public MenuEntry(Texture2D p_EntrySprite)
		{
			m_SpriteEntry = p_EntrySprite;
		}

		public virtual void Update(MenuScreen screen, bool isSelected,
													  GameTime gameTime)
		{
			// When the menu selection changes, entries gradually fade between
			// their selected and deselected appearance, rather than instantly
			// popping to the new state.
			float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

			if (isSelected)
				selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
			else
				selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
		}

		public virtual void Draw(MenuScreen screen, Vector2 position,
								 bool isSelected, GameTime gameTime)
		{
			ScreenManager screenManager = GameScreen.ScreenManager;
			SpriteBatch spriteBatch = screenManager.SpriteBatch;

			

			
			
			if (m_SpriteEntry == null)
			{
				// Draw text, centered on the middle of each line.
				SpriteFont font = screenManager.Font;
				Vector2 origin = new Vector2(0, font.LineSpacing * 0.5f);
				// Draw the selected entry in yellow, otherwise white.
				Color color = isSelected ? Color.Yellow : Color.White;

				// Modify the alpha to fade text out during transitions.
				color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

				spriteBatch.DrawString(font, text, position, color, 0,
									   origin, 1, SpriteEffects.None, 0);
			}
			else
			{
				Color color = isSelected ? Color.White : new Color(255,255,255, 100);
				spriteBatch.Draw(m_SpriteEntry, position, color); 
			}
		}

		public virtual int GetHeight(MenuScreen screen)
		{
			return GameScreen.ScreenManager.Font.LineSpacing;
		}
	}
}
