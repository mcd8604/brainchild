using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
    [Serializable]
    public class CheckPointEvent : EventTrigger
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

        private Vector3 m_CheckPoint = Vector3.Zero;
        public Vector3 CheckPoint
        {
            get
            {
                return m_CheckPoint;
            }
            set
            {
                m_CheckPoint = value;
            }
        }

        public CheckPointEvent() { }

        public CheckPointEvent(Vector3 p_CheckPoint)
        {
            m_CheckPoint = p_CheckPoint;
        }

        public bool PerformEvent(PhysicsPoint point)
        {
            GameplayScreen.SetCheckPoint( m_CheckPoint );
            return true;
        }
    }
}
