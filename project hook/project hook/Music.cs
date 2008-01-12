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

		public static Cue Play(string name)
		{
			Cue returnVal = soundbank.GetCue(name);
			cueTable.Add(name, returnVal);
			return returnVal;
		}

		/// <summary>
		/// Starts up the sound code
		/// </summary>
		public static void Initialize()
		{
			engine = new AudioEngine("../../../Content/Audio/Win/HookSound.xgs");
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


