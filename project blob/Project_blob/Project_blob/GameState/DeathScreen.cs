using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project_blob.GameState
{
	class DeathScreen : MenuScreen
	{
		public DeathScreen()
			: base("You Lose!")
		{
			IsPopup = true;

			// Create our menu entries.
			MenuEntry ReplayLevel = new MenuEntry("Replay Level");

			// Hook up menu event handlers.
			ReplayLevel.Selected += ReplayLevelSelected;

			// Add entries to the menu.
			MenuEntries.Add(ReplayLevel);
		}

		void ReplayLevelSelected(object sender, EventArgs e)
		{
			GameplayScreen.game.SetChangeArea(Level.GetAreaName(GameplayScreen.currentArea));
			GameplayScreen.physics.Player.Dead = false;
			GameplayScreen.deadSet = false;
			OnCancel();
		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

			base.Draw(gameTime);
		}
	}
}
