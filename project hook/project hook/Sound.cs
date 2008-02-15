using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace project_hook
{
	public static class Sound
	{
		private static AudioEngine engine;
		private static WaveBank wavebank;
		private static SoundBank soundbank;
		private static Boolean playSound = true;

		public static void Play(string name)
		{
			if (playSound)
			{
				soundbank.PlayCue(name);
			}
		}

		public static void setPlaySound()
		{
			if (playSound)
			{
				playSound = false;
			} else {
				playSound = true;
			}
		}

		public static Boolean getPlaySound()
		{
			return playSound;
		}
		/// <summary>
		/// Starts up the sound code
		/// </summary> 

		public static void Initialize()
		{
            engine = new AudioEngine(Environment.CurrentDirectory + "/Content/Audio/bgmusic.xgs");
            wavebank = new WaveBank(engine, Environment.CurrentDirectory + "/Content/Audio/Wave Bank.xwb");
            soundbank = new SoundBank(engine, Environment.CurrentDirectory + "/Content/Audio/Sound Bank.xsb");
		}

		public static void Update()  //  Added
		{
			engine.Update();
		}

		/// <summary>
		/// Shuts down the sound code tidily
		/// </summary>
		public static void Shutdown()
		{
			soundbank.Dispose();
			wavebank.Dispose();
			engine.Dispose();
		}
	}
}