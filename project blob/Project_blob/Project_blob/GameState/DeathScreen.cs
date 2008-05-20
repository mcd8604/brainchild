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
            MenuEntry CheckPoint = new MenuEntry("Start From Checkpoint");
            MenuEntry RestartLevel = new MenuEntry("Restart Level");
			MenuEntry ExitToHub = new MenuEntry("Exit To Hub");

            // Hook up menu event handlers.
            CheckPoint.Selected += CheckPointSelected;
            RestartLevel.Selected += RestartLevelSelected;
			ExitToHub.Selected += ExitToHubSelected;

			// Add entries to the menu.
            MenuEntries.Add(CheckPoint);
            MenuEntries.Add(RestartLevel);
			MenuEntries.Add(ExitToHub);
        }

        void CheckPointSelected(object sender, EventArgs e)
        {
            GameplayScreen.game.SetLoadCheckpoint();
            base.OnCancel();
        }

		void RestartLevelSelected(object sender, EventArgs e)
		{
            GameplayScreen.game.SetResetArea();
			base.OnCancel();
		}

		void ExitToHubSelected(object sender, EventArgs e)
		{
			OnCancel();
		}

		protected override void OnCancel()
		{
			GameplayScreen.game.SetChangeArea("HubWorld");
			base.OnCancel();
		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

			base.Draw(gameTime);
		}
	}
}
