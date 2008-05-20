using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;

namespace Project_blob.GameState
{
	class AudioMenuScreen : MenuScreen
	{
		MenuEntry soundMenuEntry;
		MenuEntry musicUpEntry;
		MenuEntry sfxUpEntry;

		bool audio = Audio.AudioManager.Enabled;
		float music = Audio.AudioManager.MusicVolume;
		float sfx = Audio.AudioManager.SoundFXVolume;

		public AudioMenuScreen()
			: base("Audio Options")
		{
			IsPopup = true;

			soundMenuEntry = new MenuEntry();
			musicUpEntry = new MenuEntry();
			MenuEntry musicDownEntry = new MenuEntry("Music -");
			sfxUpEntry = new MenuEntry();
			MenuEntry sfxDownEntry = new MenuEntry("SFX -");
			MenuEntry applyMenuEntry = new MenuEntry("Apply");
			MenuEntry backMenuEntry = new MenuEntry("Back");

			setMenuText();

			soundMenuEntry.Selected += soundSelected;
			musicUpEntry.Selected += musicUpSelected;
			musicDownEntry.Selected += musicDownSelected;
			sfxUpEntry.Selected += sfxUpSelected;
			sfxDownEntry.Selected += sfxDownSelected;
			applyMenuEntry.Selected += apply;
			backMenuEntry.Selected += OnCancel;

			MenuEntries.Add(soundMenuEntry);
			MenuEntries.Add(musicUpEntry);
			MenuEntries.Add(musicDownEntry);
			MenuEntries.Add(sfxUpEntry);
			MenuEntries.Add(sfxDownEntry);
			MenuEntries.Add(applyMenuEntry);
			MenuEntries.Add(backMenuEntry);
		}

		void setMenuText()
		{
			soundMenuEntry.Text = "Sound: " + (audio ? "On" : "Off");
			musicUpEntry.Text = "Music + " + music;
			sfxUpEntry.Text = "SFX + " + sfx;
		}
			
		void soundSelected(object sender, EventArgs e)
		{
			audio = !audio;
			setMenuText();
		}

		void musicUpSelected(object sender, EventArgs e)
		{
			if (music < 100)
			{
				music+= 10;
			}
			setMenuText();
		}

		void musicDownSelected(object sender, EventArgs e)
		{
			if (music > 0)
			{
				music-= 10;
			}
			setMenuText();
		}

		void sfxUpSelected(object sender, EventArgs e)
		{
			if (sfx < 100)
			{
				sfx+= 10;
			}
			setMenuText();
		}

		void sfxDownSelected(object sender, EventArgs e)
		{
			if (sfx > 0)
			{
				sfx-= 10;
			}
			setMenuText();
		}

		void apply(object sender, EventArgs e)
		{
			Audio.AudioManager.Enabled = audio;

			setMenuText();
		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.FadeBackBufferToBlack(TransitionAlpha);
			Audio.AudioManager.MusicVolume = music;
			Audio.AudioManager.SoundFXVolume = sfx;
			base.Draw(gameTime);
		}
	}
}
