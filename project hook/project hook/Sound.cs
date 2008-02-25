using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace project_hook
{
	internal static class Sound
	{
		private static AudioEngine engine;
		private static WaveBank wavebank;
		private static SoundBank soundbank;
		private static Boolean playSound = true;

		internal static void Play(string name)
		{
			if (playSound)
			{
				soundbank.PlayCue(name);
			}
		}

		internal static void togglePlaySound()
		{
			playSound = !playSound;
		}

		internal static void setPlaySound(Boolean p)
		{
			playSound = p;
		}

		internal static Boolean getPlaySound()
		{
			return playSound;
		}
		/// <summary>
		/// Starts up the sound code
		/// </summary> 

		internal static void Initialize()
		{
			engine = new AudioEngine(Environment.CurrentDirectory + "/Content/Audio/bgmusic.xgs");
			wavebank = new WaveBank(engine, Environment.CurrentDirectory + "/Content/Audio/Wave Bank.xwb");
			soundbank = new SoundBank(engine, Environment.CurrentDirectory + "/Content/Audio/Sound Bank.xsb");
		}

		internal static void Update()  //  Added
		{
			engine.Update();
		}

		/// <summary>
		/// Shuts down the sound code tidily
		/// </summary>
		internal static void Shutdown()
		{
			soundbank.Dispose();
			wavebank.Dispose();
			engine.Dispose();
		}
	}
}