using System;
using System.Collections.Generic;
using System.Text;
using Physics;

namespace Project_blob.GameState
{
    class OptionsMenuScreen : MenuScreen
    {
        MenuEntry fullscreenMenuEntry;
        MenuEntry aliasingMenuEntry;
        MenuEntry audioMenuEntry;
        MenuEntry threadedMenuEntry;

        static bool fullscreen = false;
        static bool aliasing = true;
        static bool audio = true;
        enum threaded { Always, Automatic, Never };
        static threaded currentThreaded = threaded.Automatic;

        public OptionsMenuScreen() : base("Options")
        {
            fullscreenMenuEntry = new MenuEntry(string.Empty);
            aliasingMenuEntry = new MenuEntry(string.Empty);
            audioMenuEntry = new MenuEntry(string.Empty);
            threadedMenuEntry = new MenuEntry(string.Empty);
            MenuEntry backMenuEntry = new MenuEntry("Back");

            setMenuText();

            fullscreenMenuEntry.Selected += fullscreenSelected;
            aliasingMenuEntry.Selected += aliasingSelected;
            audioMenuEntry.Selected += audioSelected;
            threadedMenuEntry.Selected += threadedSelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(fullscreenMenuEntry);
            MenuEntries.Add(aliasingMenuEntry);
            MenuEntries.Add(audioMenuEntry);
            MenuEntries.Add(threadedMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void setMenuText()
        {
            fullscreenMenuEntry.Text = "Fullscreen: " + (fullscreen ? "On" : "Off");
            aliasingMenuEntry.Text = "Anti-Aliasing: " + (aliasing ? "On" : "Off");
            audioMenuEntry.Text = "Audio: " + (audio ? "On" : "Off");
            threadedMenuEntry.Text = "Multithreading: " + currentThreaded;
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

        void audioSelected(object sender, EventArgs e)
        {
            audio = !audio;
            //toggle audio on/off
            setMenuText();
        }

        void threadedSelected(object sender, EventArgs e)
        {
            currentThreaded++;

            if (currentThreaded > threaded.Never)
                currentThreaded = 0;
            if (currentThreaded == threaded.Always)
                PhysicsManager.enableParallel = PhysicsManager.ParallelSetting.Always;
            if (currentThreaded == threaded.Automatic)
                PhysicsManager.enableParallel = PhysicsManager.ParallelSetting.Automatic;
            if (currentThreaded == threaded.Never)
                PhysicsManager.enableParallel = PhysicsManager.ParallelSetting.Never;
            setMenuText();
        }
    }
}