using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;

namespace Project_blob
{
    [Serializable]
    public class TransitionEvent : EventTrigger {
        
        private String _area;
        private Vector3 _position;
    
        public TransitionEvent(String area, float xPos, float yPos, float zPos)
        {
            _area = area;
            _position = new Vector3(xPos, yPos, zPos);
        }

        public void PerformEvent(GameplayScreen gameRef)
        {
            gameRef.ChangeArea(_area, _position);
        }
    }
}
