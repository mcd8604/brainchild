using System;
using System.Collections.Generic;
using System.Text;
using Project_blob.GameState;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
    [Serializable]
    public class CameraEvent : EventTrigger
	{

		private bool m_Solid = false;
		public bool Solid
		{
			get
			{
				return m_Solid;
			}
			set
			{
				m_Solid = value;
			}
		}

        private List<Engine.CameraFrame> cameraFrames = new List<Engine.CameraFrame>();
		public List<Engine.CameraFrame> CameraFrames
		{
			get
			{
				return cameraFrames;
			}
			set
			{
				cameraFrames = value;
			}
		}

		public CameraEvent() { }

		public CameraEvent(List<Engine.CameraFrame> CameraFrames)
        {
			cameraFrames = CameraFrames;
        }

        public bool PerformEvent( PhysicsPoint p )
        {
			GameplayScreen.game.SetUpCinematicCamera(cameraFrames);
            return true;
		}
	}
}
