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
		/// <summary>
		/// Starts up the sound code
		/// </summary> 

		public static void Initialize()
		{
			engine = new AudioEngine("../../../Content/Audio/Win/bgmusic.xgs");
			wavebank = new WaveBank(engine, "../../../Content/Audio/Win/Wave Bank.xwb");
			soundbank = new SoundBank(engine, "../../../Content/Audio/Win/Sound Bank.xsb");
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