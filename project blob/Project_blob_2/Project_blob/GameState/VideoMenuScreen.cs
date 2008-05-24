using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;

namespace Project_blob.GameState
{
	class VideoMenuScreen : MenuScreen
	{
		MenuEntry resolutionMenuEntry;
		MenuEntry fullscreenMenuEntry;
		MenuEntry aliasingMenuEntry;
		MenuEntry vsyncMenuEntry;
		MenuEntry fpsMenuEntry;

		Resolution resolution = ScreenManager.CurrentResolution;
		bool Fullscreen = ScreenManager.IsFullScreen;
		bool AntiAliasing = ScreenManager.IsAntiAliasing;
		bool vsync = ScreenManager.VSync;
		bool showFPS = GameplayScreen.FPS;

		public VideoMenuScreen()
			: base("Video Options")
		{
			IsPopup = true;

			resolutionMenuEntry = new MenuEntry();
			fullscreenMenuEntry = new MenuEntry();
			aliasingMenuEntry = new MenuEntry();
			vsyncMenuEntry = new MenuEntry();
			fpsMenuEntry = new MenuEntry();
			MenuEntry applyMenuEntry = new MenuEntry("Apply");
			MenuEntry backMenuEntry = new MenuEntry("Back");

			setMenuText();

			resolutionMenuEntry.Selected += resolutionSelected;
			fullscreenMenuEntry.Selected += fullscreenSelected;
			aliasingMenuEntry.Selected += aliasingSelected;
			vsyncMenuEntry.Selected += vsyncSelected;
			fpsMenuEntry.Selected += fpsSelected;
			applyMenuEntry.Selected += apply;
			backMenuEntry.Selected += OnCancel;

			MenuEntries.Add(resolutionMenuEntry);
			MenuEntries.Add(fullscreenMenuEntry);
			MenuEntries.Add(aliasingMenuEntry);
			MenuEntries.Add(vsyncMenuEntry);
			MenuEntries.Add(fpsMenuEntry);
			MenuEntries.Add(applyMenuEntry);
			MenuEntries.Add(backMenuEntry);
		}

		void setMenuText()
		{
			resolutionMenuEntry.Text = "Resolution: " + resolution;
			fullscreenMenuEntry.Text = "Fullscreen: " + (Fullscreen ? "On" : "Off");
			aliasingMenuEntry.Text = "Anti-Aliasing: " + (AntiAliasing ? "On" : "Off");
			vsyncMenuEntry.Text = "VSync: " + (vsync ? "On" : "Off");
			fpsMenuEntry.Text = "Show FPS: " + (showFPS ? "On" : "Off");
		}

		void resolutionSelected(object sender, EventArgs e)
		{
			resolution = ScreenManager.Resolutions[(ScreenManager.Resolutions.IndexOf(resolution) + 1) % ScreenManager.Resolutions.Count];
			setMenuText();
		}

		void fullscreenSelected(object sender, EventArgs e)
		{
			Fullscreen = !Fullscreen;
			setMenuText();
		}

		void aliasingSelected(object sender, EventArgs e)
		{
			AntiAliasing = !AntiAliasing;
			setMenuText();
		}

		void vsyncSelected(object sender, EventArgs e)
		{
			vsync = !vsync;
			setMenuText();
		}

		void fpsSelected(object sender, EventArgs e)
		{
			showFPS = !showFPS;
			setMenuText();
		}

		void apply(object sender, EventArgs e)
		{
			ScreenManager.setResolution(resolution);

			if (Fullscreen != ScreenManager.IsFullScreen)
			{
				ScreenManager.ToggleFullScreen();
			}

			if (AntiAliasing != ScreenManager.IsAntiAliasing)
			{
				ScreenManager.ToggleAntiAliasing();
			}

			ScreenManager.VSync = vsync;

			GameplayScreen.FPS = showFPS;

			setMenuText();
		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.FadeBackBufferToBlack(TransitionAlpha);

			base.Draw(gameTime);
		}
	}
}
