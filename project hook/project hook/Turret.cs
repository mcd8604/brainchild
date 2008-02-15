using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
    class Turret : ShipPart
    {
        protected float m_StartAngle=400;
        public float StartAngle
        {
            get
            {
                return m_StartAngle;
            }
            set
            {
                if (m_StartAngle < 0)
                {
                    m_StartAngle = 360 - value;
                }
                else
                {
                    m_StartAngle = value;
                }                
            }
        }

        protected float m_BendAmount;
        public float BendAmount
        {
            get
            {
                return m_BendAmount;
            }
            set
            {
                m_BendAmount = value;
            }
        }

        public Turret(float p_Bend)
        {
            BendAmount = p_Bend;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
        {
            if (m_StartAngle == 400)
            {
                m_StartAngle = this.ParentShip.RotationDegrees;
            }
            base.Update(p_Time);
            //Sprite t_Test=this.Parts.IndexOf(null,this.Parts.Count-1);
            //Console.WriteLine(this.Parts..RotationDegrees);
            if (RotationDegrees > m_StartAngle + m_BendAmount)
            {
                RotationDegrees = m_StartAngle + m_BendAmount;
            }
            else if (RotationDegrees < m_StartAngle - m_BendAmount)
            {
                RotationDegrees = m_StartAngle - m_BendAmount;
            }
            this.shoot();
        }
    }
}
