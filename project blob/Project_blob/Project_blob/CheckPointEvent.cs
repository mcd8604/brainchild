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

		private int m_NumTriggers = -1;
		public int NumTriggers
		{
			get
			{
				return m_NumTriggers;
			}
			set
			{
				m_NumTriggers = value;
			}
		}

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

        private float m_CoolDown = 1f;
        public float CoolDown
        {
            get
            {
                return m_CoolDown;
            }
            set
            {
                m_CoolDown = value;
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
			GameplayScreen.TextEvent = "CheckPoint";
			GameplayScreen.TextEventHit = true;
            return true;
        }
    }
}
