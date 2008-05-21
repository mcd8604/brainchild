using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;

namespace Audio {

	public static class AudioManager {
		// Our audio engine to import and run sounds
		private static AudioEngine _audioEngine;

		private static Cue curMusic;

		private static bool _musicVolChanged = false;

		// Sound stuff 
		private static WaveBank _waveBank;
		private static SoundBank _soundBank;
		private static List<Sound> _ambientSounds = new List<Sound>();

		// Use for adjusting music volume from menu
		private static float musicVolume = 100.0f;
		public static float MusicVolume
		{
			get { return musicVolume; }
			set
			{
				musicVolume = value;
				_musicVolChanged = true;
			}
		}

		// Use for adjusting soundFX volume from menu
		private static float soundFXVolume = 100.0f;
		public static float SoundFXVolume {
			get { return soundFXVolume; }
			set { soundFXVolume = value; }
		}

		// Hard-coded sound cue for blob
		private static Cue blobSound;

		private static System.Threading.Thread _ambientSoundThread;
		private static bool _runAmbience = true;

		private static AudioListener _audioListener;
		public static AudioListener Listener {
			set { _audioListener = value; }
		}

		// Bool for if we should not apply 3D effects
		private static bool mono = false;

		private static bool enabled = true;
		public static bool Enabled {
			get {
				return enabled;
			}
			set {
				enabled = value;
				if (enabled) {
					initialize();
				}
			}
		}

		/// <summary>
		/// Initial Audio Manager data
		/// </summary>
		public static void initialize() {
			try {
				_audioEngine = new AudioEngine("Content/Audio/sound.xgs");
				_waveBank = new WaveBank(_audioEngine, "Content/Audio/Wave Bank.xwb");
				_soundBank = new SoundBank(_audioEngine, "Content/Audio/Sound Bank.xsb");
				initializeAmbience();
				blobSound = getSoundFX("blobCollision");
			} catch (Exception e) {
#if DEBUG
				Log.Out.WriteLine("Audio Manager Exception:");
#endif
				Log.Out.WriteLine(e);
#if DEBUG
				Log.Out.WriteLine("The above Exception was handled.");
#endif
				enabled = false;
			}
		}

		private static void initializeAmbience() {
			try {
				_runAmbience = true;
				_ambientSoundThread = new System.Threading.Thread(AudioBackgroundThread);
				_ambientSoundThread.IsBackground = true;
				_ambientSoundThread.Name = "Ambient Sound Thread";
				_ambientSoundThread.Priority = System.Threading.ThreadPriority.Normal;
				_ambientSoundThread.Start();
			} catch (Exception e) {
#if DEBUG
				Log.Out.WriteLine("Audio Manager Exception:");
#endif
				Log.Out.WriteLine(e);
#if DEBUG
				Log.Out.WriteLine("The above Exception was handled.");
#endif
			}
		}

		private static void AudioBackgroundThread() {
			do {
				if (!mono && _audioListener != null) {
					for (int i = 0; i < _ambientSounds.Count; i++) {
						try {
							_ambientSounds[i].ensureExistance();
							_ambientSounds[i].updateAmbient3D(_audioListener);
							_ambientSounds[i].startSound();
							_ambientSounds[i].applyVolume(soundFXVolume);
						} catch (Exception e) {
#if DEBUG
							Log.Out.WriteLine("Audio Manager Exception:");
#endif
							Log.Out.WriteLine(e);
#if DEBUG
							Log.Out.WriteLine("The above Exception was handled.");
#endif
						}
						
					}
				}
				update();
				System.Threading.Thread.Sleep(50);
			} while (_runAmbience);
			ClearAmbientSounds();
			
		}

		/// <summary>
		/// Loads in a new set of ambient sounds for a level given a list of sound names and their respective positions
		/// </summary>
		/// <param name="soundNames">The list of names of all the ambient sounds</param>
		/// <param name="positions">The list of positions of all the ambient sounds</param>
		public static void LoadAmbientSounds(List<AmbientSoundInfo> ambientSounds) {
			if (_ambientSoundThread != null)
			{
				_runAmbience = false;
				System.Threading.Thread.Sleep(100);
			}
			if (ambientSounds != null) {
				foreach (AmbientSoundInfo info in ambientSounds) {
					_ambientSounds.Add(new Sound(info.Name, info.Position));
				}
			}
			initializeAmbience();
		}

		/// <summary>
		/// Clears all ambient sounds that are currently stored
		/// </summary>
		public static void ClearAmbientSounds() {
			for (int i = 0; i < _ambientSounds.Count; i++) {
				_ambientSounds[i].stop();
			}
			_ambientSounds.Clear();
		}

		/// <summary>
		/// Constructs a sound object that can utilize 3D sound from the given sound name
		/// </summary>
		/// <param name="soundName">The name of the sound file</param>
		/// <returns>If the name of the sound name is a sound file, the sound object of the given sound file</returns>
		public static Sound getSound(string soundName) {
			if (!string.IsNullOrEmpty(soundName) && soundName != "none") {
				return new Sound(soundName);
			}
			return null;
		}

		/// <summary>
		/// Sets a cue into the current music holder
		/// </summary>
		/// <param name="name">The name of the cue</param>
		public static void setMusic(string name) {
			if (curMusic != null)
			{
				curMusic.Stop(AudioStopOptions.Immediate);
				curMusic.Dispose();
			}
			curMusic = _soundBank.GetCue(name);
			curMusic.SetVariable("Volume", musicVolume);
			curMusic.Play();
		}

		/// <summary>
		/// Gets the cue of the given sound
		/// </summary>
		/// <param name="name">The name of the soundFX cue</param>
		/// <returns>The specified soundFX cue</returns>
		public static Cue getSoundFX(string name) {
			if (enabled) {
				return _soundBank.GetCue(name);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Playing of sounds primarily meant for menu soundFXs
		/// </summary>
		/// <param name="soundFX">Cue of the sound effect to play</param>
		public static void playSoundFX(ref Cue soundFX) {
			soundFX.Dispose();
			soundFX = _soundBank.GetCue(soundFX.Name);
			soundFX.SetVariable("Volume", soundFXVolume);
			soundFX.Play();
		}

		/// <summary>
		/// Plays the specified soundFX
		/// </summary>
		/// <param name="name">The cue of the soundFX</param>
		/// <param name="soundName">The name of the soundFX lookup id</param>
		/// <param name="listener">The listener of the sound to be played</param>
		/// <param name="emitter">The emitter of the sound to be played</param>
		public static void playSoundFXs(ref Cue soundFX, float volumeLevel, AudioEmitter emitter) {
			if (volumeLevel >= 0) {
				soundFX.Dispose();
				soundFX = _soundBank.GetCue(soundFX.Name);
				if (!blobSound.IsPlaying)
				{
					blobSound.Dispose();
					blobSound = _soundBank.GetCue(blobSound.Name);
				}
				if (_audioListener != null && !mono) {

#if DEBUG
					if (float.IsNaN(_audioListener.Position.X) || float.IsNaN(_audioListener.Position.Y) || float.IsNaN(_audioListener.Position.Z)) {
						throw new Exception("Invalid listener position");
					}
#endif

					try {
						if (float.IsNaN(_audioListener.Position.Length()))
						{
							_audioListener.Position = Vector3.Zero;
						}
						if (float.IsNaN(_audioListener.Velocity.Length()))
						{
							_audioListener.Velocity = Vector3.Zero;
						}
						soundFX.Apply3D(_audioListener, emitter);
						soundFX.SetVariable("Distance", soundFX.GetVariable("Distance") / volumeLevel);
						if (!blobSound.IsPlaying)
						{
							blobSound.Apply3D(_audioListener, emitter);
							blobSound.SetVariable("Distance", blobSound.GetVariable("Distance") / volumeLevel);
						}
					}
					catch (InvalidOperationException ioe)
					{
#if DEBUG
						Log.Out.WriteLine("Audio Manager InvalidOperationException:");
#endif
						Log.Out.WriteLine(ioe);
#if DEBUG
						Log.Out.WriteLine("The above Exception was handled.");
#endif
					}
					catch (Exception e) {
#if DEBUG
						Log.Out.WriteLine("Audio Manager Exception:");
#endif
						Log.Out.WriteLine(e);
#if DEBUG
						Log.Out.WriteLine("The above Exception was handled.");
#endif
						mono = true;
					}
				}
				if (mono) {
					soundFX.SetVariable("Distance", (float)Math.Pow(300, 1 / (volumeLevel * volumeLevel)));
				}
				soundFX.SetVariable("Volume", soundFXVolume);
				soundFX.Play();
				if (!blobSound.IsPlaying)
				{
					blobSound.SetVariable("Volume", soundFXVolume);
					blobSound.Play();
				}
			}
		}

		/// <summary>
		/// Stops a cue in the music dictionary
		/// </summary>
		/// <param name="name">The lookup name for the cue as well as the name of the cue itself</param>
		public static void stopMusic() {
			if (curMusic != null) {
				curMusic.Stop(AudioStopOptions.Immediate);
			}
		}

		/// <summary>
		/// Stops the cue of the given soundFX
		/// </summary>
		/// <param name="soundFX">The cue of the given soundFX<</param>
		public static void stopSoundFX(Cue soundFX) {
			soundFX.Stop(AudioStopOptions.Immediate);
		}

		/// <summary>
		/// Pauses the specified music
		/// </summary>
		/// <param name="name">The name of the music lookup id</param>
		public static void pauseMusic(string name) {
			if (curMusic != null)
			{
				if (curMusic.IsPlaying)
				{
					curMusic.Pause();
				}
			}
		}

		/// <summary>
		/// Pauses the specified soundFX
		/// </summary>
		/// <param name="soundFX">The cue of the given soundFX</param>
		public static void pauseSoundFX(Cue soundFX) {
			if (soundFX.IsPlaying) {
				soundFX.Pause();
			}
		}


		/// <summary>
		/// Resumes the specified music
		/// </summary>
		/// <param name="name">The name of the music lookup id</param>
		public static void resumeMusic(string name) {
			if (curMusic != null)
			{
				if (curMusic.IsPaused)
				{
					curMusic.Resume();
				}
			}
		}

		/// <summary>
		/// Resumes the specified soundFX
		/// </summary>
		/// <param name="name">The The cue of the given soundFX</param>
		public static void resumeSoundFX(Cue soundFX) {
			if (soundFX.IsPaused) {
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
				if (curMusic != null && _musicVolChanged)
				{
					_musicVolChanged = false;
					setMusic(curMusic.Name);
				}
				_audioEngine.Update();
			}
		}
		

		public static List<string> getAudioFilenames() {
			string[] audio = System.IO.Directory.GetFiles(@"Content\\Audio");
			List<string> result = new List<string>();
			foreach (string file in audio) {
				if (file.EndsWith(".wav")) {
					int start = file.LastIndexOf("\\") + 1;
					int length = file.LastIndexOf(".") - start;
					result.Add(file.Substring(start, length));
				}
			}
			return result;
		}

	}
}
