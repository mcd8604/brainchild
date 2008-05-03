using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
    [Serializable]
    public class TransitionEvent : EventTrigger {
        
        private String _area;
		public String Area
		{
			get
			{
				return _area;
			}
			set
			{
				_area = value;
			}
		}
        private Vector3 _position;
		public Vector3 Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}
    
        public TransitionEvent(String area, float xPos, float yPos, float zPos)
        {
            _area = area;
            _position = new Vector3(xPos, yPos, zPos);
        }

        public void PerformEvent(PhysicsPoint p)
        {
            GameplayScreen.game.ChangeArea(_area, _position);
        }
    }
}
