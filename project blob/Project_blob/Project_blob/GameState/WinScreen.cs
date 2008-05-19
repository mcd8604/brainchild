using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project_blob.GameState
{
	class WinScreen : MenuScreen
	{
		public WinScreen()
			: base("You Win!")
		{
			IsPopup = true;

			// Create our menu entries.
			MenuEntry ReplayLevel = new MenuEntry("Replay Level");
			MenuEntry Continue = new MenuEntry("Continue");

			// Hook up menu event handlers.
			ReplayLevel.Selected += ReplayLevelSelected;
			Continue.Selected += OnCancel;

			// Add entries to the menu.
			MenuEntries.Add(ReplayLevel);
			MenuEntries.Add(Continue);
		}

		void ReplayLevelSelected(object sender, EventArgs e)
		{
			GameplayScreen.game.SetChangeArea(Level.GetAreaName(GameplayScreen.currentArea));
			OnCancel();
		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

			base.Draw(gameTime);
		}
	}
}
