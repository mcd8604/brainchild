using System;
using System.Collections.Generic;
using System.Text;

namespace Project_blob.GameState
{
    class OptionsMenuScreen : MenuScreen
    {
        MenuEntry fullscreenMenuEntry;
        MenuEntry aliasingMenuEntry;

        static bool fullscreen = false;
        static bool aliasing = true;

        public OptionsMenuScreen() : base("Options")
        {
            fullscreenMenuEntry = new MenuEntry(string.Empty);
            aliasingMenuEntry = new MenuEntry(string.Empty);
            MenuEntry backMenuEntry = new MenuEntry("Back");

            setMenuText();

            fullscreenMenuEntry.Selected += fullscreenSelected;
            aliasingMenuEntry.Selected += aliasingSelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(fullscreenMenuEntry);
            MenuEntries.Add(aliasingMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void setMenuText()
        {
            fullscreenMenuEntry.Text = "Fullscreen: " + (fullscreen ? "on" : "off");
            aliasingMenuEntry.Text = "Anti-Aliasing: " + (aliasing ? "on" : "off");
        }

        void fullscreenSelected(object sender, EventArgs e)
        {
            fullscreen = !fullscreen;
            ScreenManager.toggleScreen();
            setMenuText();
        }

        void aliasingSelected(object sender, EventArgs e)
        {
            aliasing = !aliasing;
            ScreenManager.toggleAliasing();
            setMenuText();
        }
    }
}