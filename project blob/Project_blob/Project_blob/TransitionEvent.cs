using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;
using Physics2;
using System.ComponentModel;

namespace Project_blob
{
    [Serializable]
    public class TransitionEvent : EventTrigger {

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

        private String _area;
        [TypeConverter(typeof(TypeConverterArea))]
		public String Area
		{
			get
			{
				return _area;
			}
			set
			{
				_area = value;
                _position = Level.GetArea(value).StartPosition;
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
    

        public TransitionEvent() { }

        public TransitionEvent(String area) 
        {
            _area = area;
        }
    
        public TransitionEvent(String area, float xPos, float yPos, float zPos)
        {
            _area = area;
            _position = new Vector3(xPos, yPos, zPos);
        }

        public bool PerformEvent( PhysicsPoint p )
        {
            GameplayScreen.game.SetChangeArea(_area, _position);
            return true;
        }
    }
}
