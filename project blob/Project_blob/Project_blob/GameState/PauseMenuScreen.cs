using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;

namespace Project_blob.GameState
{
	class PauseMenuScreen : MenuScreen
	{
		public PauseMenuScreen()
			: base("Paused")
		{
			// Flag that there is no need for the game to transition
			// off when the pause menu is on top of it.
			IsPopup = true;

			// Create our menu entries.
			MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
			MenuEntry optionsEntry = new MenuEntry("Options");
			MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

			// Hook up menu event handlers.
			resumeGameMenuEntry.Selected += OnCancel;
			optionsEntry.Selected += OptionsMenuEntrySelected;
			quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

			// Add entries to the menu.
			MenuEntries.Add(resumeGameMenuEntry);
			MenuEntries.Add(optionsEntry);
			MenuEntries.Add(quitGameMenuEntry);
		}


		void OptionsMenuEntrySelected(object sender, EventArgs e)
		{
			ScreenManager.AddScreen(new OptionsMenuScreen(true));
		}


		void QuitGameMenuEntrySelected(object sender, EventArgs e)
		{
			LoadingScreen.Load(ScreenManager, false, new MainMenuScreen());

		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

			base.Draw(gameTime);
		}
	}
}
