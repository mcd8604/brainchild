using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project_blob.GameState
{
	class DeathScreen : MenuScreen
	{
		public DeathScreen()
			: base("You Died!")
		{
			IsPopup = true;

			// Create our menu entries.

            MenuEntry CheckPoint;
            if (GameplayScreen.GetCheckpoint() != GameplayScreen.currentArea.StartPosition)
            {
                
                CheckPoint = new MenuEntry("Start From Last Checkpoint");

                MenuEntry RestartLevel = new MenuEntry("Restart Level");
                RestartLevel.Selected += RestartLevelSelected;
                MenuEntries.Add(RestartLevel);
            }
            else
            {
                CheckPoint = new MenuEntry("Try Again");
            }

			MenuEntry ExitToHub = new MenuEntry("Exit To Hub");

            // Hook up menu event handlers.
            CheckPoint.Selected += CheckPointSelected;
            ExitToHub.Selected += ExitToHubSelected;

            // Add entries to the menu.
            MenuEntries.Add(CheckPoint);
            MenuEntries.Add(ExitToHub);
        }

        void CheckPointSelected(object sender, EventArgs e)
        {
            GameplayScreen.SetLoadCheckpoint();
            base.OnCancel();
        }

		void RestartLevelSelected(object sender, EventArgs e)
		{
            GameplayScreen.SetResetArea();
			base.OnCancel();
		}

		void ExitToHubSelected(object sender, EventArgs e)
		{
			OnCancel();
		}

		protected override void OnCancel()
		{
			GameplayScreen.SetChangeArea("HubWorld");
			base.OnCancel();
		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

			base.Draw(gameTime);
		}
	}
}
