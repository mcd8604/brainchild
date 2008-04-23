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

		public OptionsMenuScreen()
			: base("Options")
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
			fullscreenMenuEntry.Text = "Fullscreen: " + (ScreenManager.IsFullScreen ? "On" : "Off");
			aliasingMenuEntry.Text = "Anti-Aliasing: " + (ScreenManager.IsAntiAliasing ? "On" : "Off");
			audioMenuEntry.Text = "Audio: " + /*(audio ? "On" : "Off")*/ "N/A";
			threadedMenuEntry.Text = "Multithreading: " + PhysicsManager.enableParallel;
		}

		void fullscreenSelected(object sender, EventArgs e)
		{
			ScreenManager.ToggleFullScreen();
			setMenuText();
		}

		void aliasingSelected(object sender, EventArgs e)
		{
			ScreenManager.ToggleAntiAliasing();
			setMenuText();
		}

		void audioSelected(object sender, EventArgs e)
		{
			//toggle audio on/off
			setMenuText();
		}

		void threadedSelected(object sender, EventArgs e)
		{

			if (PhysicsManager.enableParallel == PhysicsManager.ParallelSetting.Automatic)
			{
				PhysicsManager.enableParallel = PhysicsManager.ParallelSetting.Always;
			}
			else if (PhysicsManager.enableParallel == PhysicsManager.ParallelSetting.Always)
			{
				PhysicsManager.enableParallel = PhysicsManager.ParallelSetting.Never;
			}
			else
			{
				PhysicsManager.enableParallel = PhysicsManager.ParallelSetting.Automatic;
			}

			setMenuText();
		}
	}
}