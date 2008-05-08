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
        private List<Vector3> _cameraUps;
		public List<Vector3> CameraUps
		{
			get
			{
				return _cameraUps;
			}
			set
			{
				_cameraUps = value;
			}
		}
        private List<Vector3> _cameraLooks;
		public List<Vector3> CameraLooks
		{
			get
			{
				return _cameraLooks;
			}
			set
			{
				_cameraLooks = value;
			}
		}
        private List<Vector3> _cameraPos;
		public List<Vector3> CameraPos
		{
			get
			{
				return _cameraPos;
			}
			set
			{
				_cameraPos = value;
			}
		}

        public CameraEvent() { }

        public CameraEvent(List<Vector3> cameraPos, List<Vector3> cameraLooks, List<Vector3> cameraUps)
        {
            _cameraPos = cameraPos;
            _cameraLooks = cameraLooks;
            _cameraUps = cameraUps;
        }

        public void PerformEvent(PhysicsPoint p)
        {
			GameplayScreen.game.SetUpCinematicCamera(_cameraPos, _cameraLooks, _cameraUps);
        }
    }
}
