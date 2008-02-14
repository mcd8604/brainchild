using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace project_hook
{
	public static class Music
	{
		private static AudioEngine engine;
		private static WaveBank wavebank;
		private static SoundBank soundbank;
		private static Hashtable cueTable = new Hashtable();
		private static Boolean playSound = true;

		public static Cue Play(string name)
		{
			try
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
			catch (ArgumentException e)
			{
				Game.Out.WriteLine("Music Exception: " + e);
				return null;
			}
		}

		public static void setPlaySound(Boolean p)
		{
			if (p)
			{
				playSound = true;
			}
			else
			{
				playSound = false;
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
			engine = new AudioEngine("../../../Content/Audio/Win/bgmusic.xgs");
			wavebank = new WaveBank(engine, "../../../Content/Audio/Win/Wave Bank.xwb");
			soundbank = new SoundBank(engine, "../../../Content/Audio/Win/Sound Bank.xsb");
		}

		public static void Update()  //  Added
		{
			engine.Update();
		}

		public static void Stop(string name)
		{
			((Cue)cueTable[name]).Stop(AudioStopOptions.Immediate);
			cueTable.Remove(name);
		}

		public static bool IsPlaying(string name)
		{
			return cueTable.Contains(name);
		}

		/// <summary>
		/// Shuts down the sound code tidily
		/// </summary>
		public static void Shutdown()
		{
			soundbank.Dispose();
			wavebank.Dispose();
			engine.Dispose();
			foreach (Cue it in cueTable)
				Stop(it.Name);
		}
	}
}


