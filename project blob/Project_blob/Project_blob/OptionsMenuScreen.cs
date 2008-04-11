using System;
using System.Collections.Generic;
using System.Text;

namespace Project_blob
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