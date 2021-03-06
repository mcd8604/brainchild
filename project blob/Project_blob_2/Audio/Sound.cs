using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Audio {
	public class Sound {

		private AudioEmitter audioEmitter = new AudioEmitter();
		private Cue collisionSound;
		private bool playingSound = false;

		internal Sound(string soundName) {
			collisionSound = AudioManager.getSoundFX(soundName);
			audioEmitter.DopplerScale = 1f;
		}

		internal Sound(string soundName, Vector3 position) {
			collisionSound = AudioManager.getSoundFX(soundName);
			audioEmitter.DopplerScale = 1f;
			audioEmitter.Position = position;
		}

		public void ensureExistance()
		{
			if (collisionSound.IsDisposed)
			{
				collisionSound = AudioManager.getSoundFX(collisionSound.Name);
			}
		}

		public void applyVolume(float volumeLevel)
		{
			collisionSound.SetVariable("Volume", volumeLevel);
		}

		public void updateAmbient3D(AudioListener Listener) {
			if (collisionSound.IsPlaying)
			{
#if DEBUG
				if (float.IsNaN(Listener.Position.X) || float.IsNaN(Listener.Position.Y) || float.IsNaN(Listener.Position.Z)) {
					throw new Exception("Invalid listener position");
				}
#endif
				collisionSound.Apply3D(Listener, audioEmitter);
			}
		}

		/// <summary>
		/// Stops the sound file
		/// </summary>
		public void stop() {
			if (!collisionSound.IsDisposed)
			{
				collisionSound.Stop(AudioStopOptions.Immediate);
				collisionSound.Dispose();
			}
		}

		public void startSound()
		{
			if (!collisionSound.IsPlaying)
			{
				AudioManager.playSoundFXs(ref collisionSound, 5.0f, audioEmitter);
			}
		}


		public void play(Vector3 SoundLocation, float Magnitude) {
			if (Magnitude == 0f) {
				return;
			}
			float volumeLevel = (float)Math.Log(Magnitude * 0.002f);

			if (volumeLevel > 0) {
				try
				{
					if (collisionSound.IsDisposed)
					{
						collisionSound = AudioManager.getSoundFX(collisionSound.Name);
					}
					if (playingSound)
					{
						if (!collisionSound.IsPlaying)
						{
							playingSound = false;
						}
					}
					else
					{
						if (collisionSound != null)
						{
							playingSound = true;
							audioEmitter.Position = SoundLocation;
							AudioManager.playSoundFXs(ref collisionSound, volumeLevel, audioEmitter);
						}
					}
				}
				catch (Exception e)
				{
#if DEBUG
					Log.Out.WriteLine("Sound Exception:");
#endif
					Log.Out.WriteLine(e);
#if DEBUG
					Log.Out.WriteLine("The above Exception was handled.");
#endif
				}
			}
		}
	}
}