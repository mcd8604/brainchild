using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace Project_blob.GameState
{
	abstract class MenuScreen : GameScreen
	{
		List<MenuEntry> menuEntries = new List<MenuEntry>();
		int selectedEntry = 0;
		string menuTitle;

		protected IList<MenuEntry> MenuEntries
		{
			get { return menuEntries; }
		}

		public MenuScreen(string menuTitle)
		{
			this.menuTitle = menuTitle;

			TransitionOnTime = TimeSpan.FromSeconds(0.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);
		}

		public override void HandleInput()
		{
			// Move to the previous menu entry?
			if (InputHandler.IsActionPressed(Actions.MenuUp))
			{
				selectedEntry--;

				if (selectedEntry < 0)
					selectedEntry = menuEntries.Count - 1;
			}

			// Move to the next menu entry?
			if (InputHandler.IsActionPressed(Actions.MenuDown))
			{
				++selectedEntry;

				if (selectedEntry >= menuEntries.Count)
					selectedEntry = 0;
			}

			// Accept or cancel the menu?
			if (InputHandler.IsActionPressed(Actions.MenuAccept))
			{
				OnSelectEntry(selectedEntry);
			}
			else if (InputHandler.IsActionPressed(Actions.MenuCancel))
			{
				OnCancel();
			}
		}

		protected virtual void OnSelectEntry(int entryIndex)
		{
			menuEntries[selectedEntry].OnSelectEntry();
		}

		protected virtual void OnCancel()
		{
			ExitScreen();
		}

		protected void OnCancel(object sender, EventArgs e)
		{
			OnCancel();
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus,
													   bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			// Update each nested MenuEntry object.
			for (int i = 0; i < menuEntries.Count; ++i)
			{
				bool isSelected = IsActive && (i == selectedEntry);

				menuEntries[i].Update(this, isSelected, gameTime);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			SpriteFont font = ScreenManager.Font;

			Vector2 position = new Vector2(100, 150);

			// Make the menu slide into place during transitions, using a
			// power curve to make things look more interesting (this makes
			// the movement slow down as it nears the end).
			float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

			if (ScreenState == ScreenState.TransitionOn)
				position.X -= transitionOffset * 256;
			else
				position.X += transitionOffset * 512;

			spriteBatch.Begin();

			// Draw each menu entry in turn.
			for (int i = 0; i < menuEntries.Count; ++i)
			{
				MenuEntry menuEntry = menuEntries[i];

				bool isSelected = IsActive && (i == selectedEntry);

				menuEntry.Draw(this, position, isSelected, gameTime);

				position.Y += menuEntry.GetHeight(this)+ 20;
			}

			// Draw the menu title.
			Vector2 titlePosition = new Vector2(426, 80);
			Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
			Color titleColor = new Color(192, 192, 192, TransitionAlpha);
			float titleScale = 1.25f;

			titlePosition.Y -= transitionOffset * 100;

			spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
								   titleOrigin, titleScale, SpriteEffects.None, 0);

			spriteBatch.End();
		}
	}
}
