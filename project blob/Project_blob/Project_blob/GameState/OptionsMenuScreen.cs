using System;
using System.Collections.Generic;
using System.Text;

namespace Project_blob.GameState
{
    class OptionsMenuScreen : MenuScreen
    {
        public OptionsMenuScreen() : base("Options")
        {
            MenuEntry backMenuEntry = new MenuEntry("Back");

            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(backMenuEntry);
        }
    }
}