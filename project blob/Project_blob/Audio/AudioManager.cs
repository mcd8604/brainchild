using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;

namespace Audio
{
    public class AudioManager
    {
        // Objects to ensure singleton design
        private static volatile AudioManager _instance;
        private static object _syncRoot = new Object();

        // Our audio engine to import and run sounds
        private AudioEngine _audioEngine;

        // Sound stuff 
        private WaveBank _waveBank;
        private SoundBank _soundBank;
        private Dictionary<String, Cue> _music;

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
					_instance.initialize();
				}
			}
		}

        /// <summary>
        /// Constructor
        /// </summary>
        public AudioManager()
        {
            _music = new Dictionary<string, Cue>();
        }

        //! Instance
        public static AudioManager getSingleton
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new AudioManager();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Initial Audio Manager data
        /// </summary>
        public void initialize()
        {
			// Crash: System Invalid Operation Exception
			try
			{
				_audioEngine = new AudioEngine("Content/Audio/sound.xgs");
				_waveBank = new WaveBank(_audioEngine, "Content/Audio/Wave Bank.xwb");
				_soundBank = new SoundBank(_audioEngine, "Content/Audio/Sound Bank.xsb");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				enabled = false;
			}
        }

        /// <summary>
        /// Adds a cue into the music dictionary
        /// </summary>
        /// <param name="name">The lookup name for the cue as well as the name of the cue itself</param>
        /// <returns>The specified music cue</returns>
        public Cue addMusic(string name)
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
        public Cue getSoundFX(string name)
        {
            return _soundBank.GetCue(name);
        }

        /// <summary>
        /// Plays the specified music
        /// </summary>
        /// <param name="name">The name of the soundFX lookup id</param>
        public void playMusic(string name)
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
        public void playSoundFXs(ref Cue soundFX, string soundName, float volumeLevel, AudioListener listener, AudioEmitter emitter)
        {
            if (volumeLevel >= 0)
            {
                soundFX.Dispose();
                soundFX = _soundBank.GetCue(soundName);
                soundFX.Apply3D(listener, emitter);
                soundFX.SetVariable("Distance",  soundFX.GetVariable("Distance") / volumeLevel);
                soundFX.Play();
            }
        }

        /// <summary>
        /// Stops a cue in the music dictionary
        /// </summary>
        /// <param name="name">The lookup name for the cue as well as the name of the cue itself</param>
        public void stopMusic(String name)
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
        public void stopSoundFX(Cue soundFX)
        {
            soundFX.Stop(AudioStopOptions.Immediate);
        }

        /// <summary>
        /// Pauses the specified music
        /// </summary>
        /// <param name="name">The name of the music lookup id</param>
        public void pauseMusic(String name)
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
        public void pauseSoundFX(Cue soundFX)
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
        public void resumeMusic(String name)
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
        public void resumeSoundFX(Cue soundFX)
        {
            if (soundFX.IsPaused)
            {
                soundFX.Resume();
            }
        }

        /// <summary>
        /// Updates the audio manager's engine
        /// </summary>
        public void update()
        {
            // Update the audio engine so that it can process audio data
			if (enabled)
			{
				_audioEngine.Update();
			}
        }

    }
}
