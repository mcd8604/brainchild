using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Audio
{
	public class Sound
	{

		private AudioEmitter audioEmitter = new AudioEmitter();
		private Cue collisionSound;
		private bool playingSound = false;

		internal Sound(string soundName)
		{
			collisionSound = AudioManager.getSoundFX(soundName);
			audioEmitter.DopplerScale = 0f;
			audioEmitter.Forward = Vector3.Forward;
			audioEmitter.Up = Vector3.Up;
			audioEmitter.Position = Vector3.Zero;
			audioEmitter.Velocity = Vector3.Zero;
		}

        internal Sound(string soundName, Vector3 position) {
            collisionSound = AudioManager.getSoundFX(soundName);
            audioEmitter.DopplerScale = 0f;
            audioEmitter.Forward = Vector3.Forward;
            audioEmitter.Up = Vector3.Up;
            audioEmitter.Position = position;
            audioEmitter.Velocity = Vector3.Zero;
        }

        public void updateAmbient3D(AudioListener Listener) {
            collisionSound.Apply3D(Listener, audioEmitter);
        }

        public void startSound() {
            if( !collisionSound.IsPlaying ) {
                AudioManager.playSoundFXs( ref collisionSound, 1.0f, audioEmitter );
            }
        }


		public void play(Vector3 SoundLocation, float Magnitude)
		{
            if (Magnitude == 0f)
            {
                return;
            }
			float volumeLevel = (float)Math.Log(Magnitude / 500);

            if (volumeLevel > 0)
            {
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
		}
	}
}
