using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;

namespace Audio
{
	// The sound card and speakers are static, so why not have the AudioManager be static too
	public static class AudioManager
	{
		// Our audio engine to import and run sounds
		private static AudioEngine _audioEngine;

		// Sound stuff 
		private static WaveBank _waveBank;
		private static SoundBank _soundBank;
		private static Dictionary<String, Cue> _music = new Dictionary<string, Cue>();
		private static List<Sound> _ambientSounds = new List<Sound>();

		private static System.Threading.Thread _ambientSoundThread;
		private static bool _runAmbience = true;

		private static AudioListener _audioListener;
		public static AudioListener Listener
		{
			set { _audioListener = value; }
		}

		// Bool for if we should not apply 3D effects
		private static bool mono = false;

		private static bool enabled = true;
		public static bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
				if (enabled)
				{
					initialize();
				}
			}
		}

		/// <summary>
		/// Initial Audio Manager data
		/// </summary>
		public static void initialize()
		{
			try
			{
				_audioEngine = new AudioEngine("Content/Audio/sound.xgs");
				_waveBank = new WaveBank(_audioEngine, "Content/Audio/Wave Bank.xwb");
				_soundBank = new SoundBank(_audioEngine, "Content/Audio/Sound Bank.xsb");
				initializeAmbience();
			}
			catch (Exception e)
			{
				Console.WriteLine("Audio Manager Exception:");
				Console.WriteLine(e);
				Console.WriteLine("The above Exception was handled.");
				enabled = false;
			}
		}

		private static void initializeAmbience()
		{
			if (_ambientSoundThread != null)
			{
				_runAmbience = false;
				System.Threading.Thread.Sleep(50);
			}
			try
			{
				foreach (Sound sound in _ambientSounds)
				{
					sound.startSound();
				}
				_ambientSoundThread = new System.Threading.Thread(AudioBackgroundThread);
				_ambientSoundThread.IsBackground = true;
				_ambientSoundThread.Name = "Ambient Sound Thread";
				_ambientSoundThread.Priority = System.Threading.ThreadPriority.Normal;
				_runAmbience = true;
				_ambientSoundThread.Start();
			}
			catch (Exception e)
			{
#if DEBUG
				Console.WriteLine("Audio Manager Exception:");
#endif
				Console.WriteLine(e);
#if DEBUG
				Console.WriteLine("The above Exception was handled.");
#endif
				enabled = false;
			}
		}

		private static void AudioBackgroundThread()
		{
			do
			{
				if (!mono && _audioListener != null)
				{
					foreach (Sound sound in _ambientSounds)
					{
						sound.updateAmbient3D(_audioListener);
						update();
					}
				}
				System.Threading.Thread.Sleep(50);
			} while (_runAmbience);
		}

		/// <summary>
		/// Loads in a new set of ambient sounds for a level given a list of sound names and their respective positions
		/// </summary>
		/// <param name="soundNames">The list of names of all the ambient sounds</param>
		/// <param name="positions">The list of positions of all the ambient sounds</param>
		public static void LoadAmbientSounds(List<AmbientSoundInfo> ambientSounds)
		{
			if (ambientSounds != null)
			{
				_ambientSounds.Clear();
				foreach (AmbientSoundInfo info in ambientSounds)
				{
					_ambientSounds.Add(new Sound(info.Name, info.Position));
				}
				initializeAmbience();
			}
		}

		/// <summary>
		/// Constructs a sound object that can utilize 3D sound from the given sound name
		/// </summary>
		/// <param name="soundName">The name of the sound file</param>
		/// <returns>If the name of the sound name is a sound file, the sound object of the given sound file</returns>
		public static Sound getSound(string soundName)
		{
			if (!soundName.Equals("") && !soundName.Equals("none"))
			{
				return new Sound(soundName);
			}
			return null;
		}

		/// <summary>
		/// Adds a cue into the music dictionary
		/// </summary>
		/// <param name="name">The lookup name for the cue as well as the name of the cue itself</param>
		/// <returns>The specified music cue</returns>
		public static Cue addMusic(string name)
		{
			Cue retVal;

			if (!_music.ContainsKey(name))
			{
				retVal = _soundBank.GetCue(name);
				_music.Add(name, retVal);
			}
			else
			{
				retVal = _music[name];
			}

			return retVal;
		}

		/// <summary>
		/// Gets the cue of the given sound
		/// </summary>
		/// <param name="name">The name of the soundFX cue</param>
		/// <returns>The specified soundFX cue</returns>
		public static Cue getSoundFX(string name)
		{
			if (enabled)
			{
				return _soundBank.GetCue(name);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Plays the specified music
		/// </summary>
		/// <param name="name">The name of the soundFX lookup id</param>
		public static void playMusic(string name)
		{
			if (_music.ContainsKey(name) && !_music[name].IsPlaying)
			{
				_music[name].Dispose();
				_music[name] = _soundBank.GetCue(name);
				_music[name].Play();
			}
		}

		/// <summary>
		/// Plays the specified soundFX
		/// </summary>
		/// <param name="name">The cue of the soundFX</param>
		/// <param name="soundName">The name of the soundFX lookup id</param>
		/// <param name="listener">The listener of the sound to be played</param>
		/// <param name="emitter">The emitter of the sound to be played</param>
		public static void playSoundFXs(ref Cue soundFX, float volumeLevel, AudioEmitter emitter)
		{
			if (volumeLevel >= 0)
			{
				soundFX.Dispose();
				soundFX = _soundBank.GetCue(soundFX.Name);
				if (_audioListener != null && !mono)
				{
					try
					{
						soundFX.Apply3D(_audioListener, emitter);
						soundFX.SetVariable("Distance", soundFX.GetVariable("Distance") / volumeLevel);
					}
					catch (Exception e)
					{
						Console.WriteLine("Audio Manager Exception:");
						Console.WriteLine(e);
						Console.WriteLine("The above Exception was handled.");
						mono = true;
					}
				}
				soundFX.Play();
			}
		}

		/// <summary>
		/// Stops a cue in the music dictionary
		/// </summary>
		/// <param name="name">The lookup name for the cue as well as the name of the cue itself</param>
		public static void stopMusic(String name)
		{
			if (_music.ContainsKey(name))
			{
				_music[name].Stop(AudioStopOptions.Immediate);
			}
		}

		/// <summary>
		/// Stops the cue of the given soundFX
		/// </summary>
		/// <param name="soundFX">The cue of the given soundFX<</param>
		public static void stopSoundFX(Cue soundFX)
		{
			soundFX.Stop(AudioStopOptions.Immediate);
		}

		/// <summary>
		/// Pauses the specified music
		/// </summary>
		/// <param name="name">The name of the music lookup id</param>
		public static void pauseMusic(String name)
		{
			if (_music.ContainsKey(name))
			{
				if (_music[name].IsPlaying)
				{
					_music[name].Pause();
				}
			}
		}

		/// <summary>
		/// Pauses the specified soundFX
		/// </summary>
		/// <param name="soundFX">The cue of the given soundFX</param>
		public static void pauseSoundFX(Cue soundFX)
		{
			if (soundFX.IsPlaying)
			{
				soundFX.Pause();
			}
		}


		/// <summary>
		/// Resumes the specified music
		/// </summary>
		/// <param name="name">The name of the music lookup id</param>
		public static void resumeMusic(String name)
		{
			if (_music.ContainsKey(name))
			{
				if (_music[name].IsPaused)
				{
					_music[name].Resume();
				}
			}
		}

		/// <summary>
		/// Resumes the specified soundFX
		/// </summary>
		/// <param name="name">The The cue of the given soundFX</param>
		public static void resumeSoundFX(Cue soundFX)
		{
			if (soundFX.IsPaused)
			{
				soundFX.Resume();
			}
		}

		/// <summary>
		/// Updates the audio manager's engine
		/// </summary>
		public static void update()
		{
			// Update the audio engine so that it can process audio data
			if (enabled)
			{
				_audioEngine.Update();
			}
		}

	}
}
