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
			// Draw the selected entry in yellow, otherwise white.
			Color color = isSelected ? Color.Yellow : Color.White;

			// Pulsate the size of the selected menu entry.
			double time = gameTime.TotalGameTime.TotalSeconds;

			float pulsate = (float)Math.Sin(time * 6) + 1;

			float scale = 1 + pulsate * 0.05f * selectionFade;

			// Modify the alpha to fade text out during transitions.
			color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

			// Draw text, centered on the middle of each line.
			ScreenManager screenManager = GameScreen.ScreenManager;
			SpriteBatch spriteBatch = screenManager.SpriteBatch;
			SpriteFont font = screenManager.Font;

			Vector2 origin = new Vector2(0, font.LineSpacing / 2);

			spriteBatch.DrawString(font, text, position, color, 0,
								   origin, scale, SpriteEffects.None, 0);
		}

		public virtual int GetHeight(MenuScreen screen)
		{
			return GameScreen.ScreenManager.Font.LineSpacing;
		}
	}
}
