using System;
using System.Collections.Generic;
using System.Text;
using Project_blob.GameState;
using Microsoft.Xna.Framework;

namespace Project_blob
{
    [Serializable]
    public class CameraEvent : EventTrigger
    {
        private List<Vector3> _cameraUps;
        private List<Vector3> _cameraLooks;
        private List<Vector3> _cameraPos;

        public CameraEvent(List<Vector3> cameraPos, List<Vector3> cameraLooks, List<Vector3> cameraUps)
        {
            _cameraPos = cameraPos;
            _cameraLooks = cameraLooks;
            _cameraUps = cameraUps;
        }

        public void PerformEvent(GameplayScreen gameRef)
        {
            gameRef.SetUpCinematicCamera(_cameraPos, _cameraLooks, _cameraUps);
        }
    }
}
