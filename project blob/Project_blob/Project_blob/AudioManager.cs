using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;

namespace Project_blob
{
    public class AudioManager {

        // Objects to ensure singleton design
        private static volatile AudioManager _instance;
        private static object _syncRoot = new Object();

        // Our audio engine to import and run sounds
        private AudioEngine _audioEngine;

        // Sound stuff 
        private WaveBank _waveBank;
        private SoundBank _soundBank;
        private Dictionary<String, Cue> _music;
        private Dictionary<String, Cue> _soundFXs;

        /// <summary>
        /// Constructor
        /// </summary>
        public AudioManager() {
            _music = new Dictionary<string, Cue>();
            _soundFXs = new Dictionary<string, Cue>();
        }

        //! Instance
        public static AudioManager getSingleton {
            get {
                if (_instance == null) {
                    lock (_syncRoot) {
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
        public void initialize() {
            _audioEngine = new AudioEngine("Content/Audio/sound.xgs");
            _waveBank = new WaveBank(_audioEngine, "Content/Audio/Wave Bank.xwb");
            _soundBank = new SoundBank(_audioEngine, "Content/Audio/Sound Bank.xsb");
        }

        /// <summary>
        /// Adds a cue into the music dictionary
        /// </summary>
        /// <param name="name">The lookup name for the cue as well as the name of the cue itself</param>
        /// <returns>The specified music cue</returns>
        public Cue addMusic(String name) {
            Cue retVal;
            
            if (!_music.ContainsKey(name)) {
                retVal = _soundBank.GetCue(name);
                _music.Add(name, retVal);
            } else {
                retVal = _music[name];
            }

            return retVal;
        }

        /// <summary>
        /// Adds a cue into the soundFXs dictionary
        /// </summary>
        /// <param name="name">The lookup name for the cue as well as the name of the cue itself</param>
        /// <returns>The specified soundFX cue</returns>
        public Cue addSoundFX(String name) {
            Cue retVal;

            if (!_soundFXs.ContainsKey(name)) {
                retVal = _soundBank.GetCue(name);
                _soundFXs.Add(name, retVal);
            } else {
                retVal = _soundFXs[name];
            }

            return retVal;
        }

        /// <summary>
        /// Plays the specified music
        /// </summary>
        /// <param name="name">The name of the soundFX lookup id</param>
        public void playMusic(String name) {
            if (_music.ContainsKey(name)) {
                _music[name].Dispose();
                _music[name] = _soundBank.GetCue(name);
                _music[name].Play();
            }
        }

        /// <summary>
        /// Plays the specified soundFX
        /// </summary>
        /// <param name="name">The name of the soundFX lookup id</param>
        public void playSoundFXs(String name) {
            if (_soundFXs.ContainsKey(name)) {
                _soundFXs[name].Dispose();
                _soundFXs[name] = _soundBank.GetCue(name);
                _soundFXs[name].Play();
            }
        }

        /// <summary>
        /// Stops a cue in the music dictionary
        /// </summary>
        /// <param name="name">The lookup name for the cue as well as the name of the cue itself</param>
        /// <returns>The specified music cue</returns>
        public void stopMusic(String name) {
            if (_music.ContainsKey(name)) {
                _music[name].Stop(AudioStopOptions.Immediate);
            }
        }

        /// <summary>
        /// Stops a cue in the soundFXs dictionary
        /// </summary>
        /// <param name="name">The lookup name for the cue as well as the name of the cue itself</param>
        /// <returns>The specified soundFX cue</returns>
        public void stopSoundFX(String name) {
            if (_soundFXs.ContainsKey(name)) {
                _soundFXs[name].Stop(AudioStopOptions.Immediate);
            }
        }

        /// <summary>
        /// Pauses the specified music
        /// </summary>
        /// <param name="name">The name of the music lookup id</param>
        public void pauseMusic(String name) {
            if (_music.ContainsKey(name)) {
                if (_music[name].IsPlaying) {
                    _music[name].Pause();
                }
            }
        }

        /// <summary>
        /// Pauses the specified soundFX
        /// </summary>
        /// <param name="name">The name of the soundFX lookup id</param>
        public void pauseSoundFXs(String name) {
            if (_soundFXs.ContainsKey(name)) {
                if (_soundFXs[name].IsPlaying) {
                    _soundFXs[name].Pause();
                }
            }
        }

        /// <summary>
        /// Pauses all sounds currently playing
        /// </summary>
        public void pauseAll() {
            foreach (Cue cue in _music.Values) {
                if (cue.IsPlaying) {
                    cue.Pause();
                }
            }
            foreach (Cue cue in _soundFXs.Values) {
                if (cue.IsPlaying) {
                    cue.Pause();
                }
            }
        }

        /// <summary>
        /// Resumes the specified music
        /// </summary>
        /// <param name="name">The name of the music lookup id</param>
        public void resumeMusic(String name) {
            if (_music.ContainsKey(name)) {
                if (_music[name].IsPaused) {
                    _music[name].Resume();
                }
            }
        }

        /// <summary>
        /// Resumes the specified soundFX
        /// </summary>
        /// <param name="name">The name of the soundFX lookup id</param>
        public void resumeSoundFXs(String name) {
            if (_soundFXs.ContainsKey(name)) {
                if (_soundFXs[name].IsPaused) {
                    _soundFXs[name].Resume();
                }
            }
        }

        /// <summary>
        /// Resumes all sounds currently paused
        /// </summary>
        public void resumeAll() {
            foreach (Cue cue in _music.Values) {
                if (cue.IsPaused) {
                    cue.Resume();
                }
            }
            foreach (Cue cue in _soundFXs.Values) {
                if (cue.IsPaused) {
                    cue.Resume();
                }
            }
        }

        /// <summary>
        /// Updates the audio manager's engine
        /// </summary>
        public void update() {
            // Update the audio engine so that it can process audio data
            _audioEngine.Update();
        }

    }
}
