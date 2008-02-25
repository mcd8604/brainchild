using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace project_hook
{
	internal static class Music
	{
		private static AudioEngine engine;
		private static WaveBank wavebank;
		private static SoundBank soundbank;
		private static Hashtable cueTable = new Hashtable();
		private static Boolean playSound = true;

		internal static Cue Play(string name)
		{
			Cue returnVal = null;
			if (playSound)
			{
				returnVal = soundbank.GetCue(name);
				cueTable.Add(name, returnVal);
				returnVal.Play();
				//soundbank.PlayCue(name);
			}
			return returnVal;
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

		internal static void Stop(string name)
		{
			((Cue)cueTable[name]).Stop(AudioStopOptions.Immediate);
			cueTable.Remove(name);
		}

		internal static bool IsPlaying(string name)
		{
			return cueTable.Contains(name);
		}

		/// <summary>
		/// Shuts down the sound code tidily
		/// </summary>
		internal static void Shutdown()
		{
			soundbank.Dispose();
			wavebank.Dispose();
			engine.Dispose();
			foreach (Cue it in cueTable)
				Stop(it.Name);
		}
	}
}


