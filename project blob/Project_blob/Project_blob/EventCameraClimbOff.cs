using System;
using System.Collections.Generic;
using System.Text;

namespace Project_blob
{
    [Serializable]
    class OffCameraClimbEvent : EventTrigger
    {
        #region EventTrigger Members

        public bool PerformEvent(Physics2.PhysicsPoint p)
        {
            //GameState.GameplayScreen.game.blob_Climbing = false;
            return true;
        }

        private int m_NumTriggers = -1;
        public int NumTriggers
        {
            get
            {
                return m_NumTriggers;
            }
            set
            {
                m_NumTriggers = value; ;
            }
        }

        private bool m_Solid = true;
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

        #endregion
    }
}
