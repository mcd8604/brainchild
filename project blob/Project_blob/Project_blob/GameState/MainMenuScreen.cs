using System;
using System.Collections.Generic;
using System.Text;

namespace Project_blob.GameState
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen() : base("Main Menu")
        {
            // Create our menu entries.
            MenuEntry startMenuEntry = new MenuEntry("Start");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            startMenuEntry.Selected += startMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(startMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void startMenuEntrySelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen());
        }

        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(false));
        }

        protected override void OnCancel()
        {
            ScreenManager.Exit();
        }
    }
}
