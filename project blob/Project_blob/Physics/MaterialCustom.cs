using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    public class MaterialCustom : Material
    {
        private float m_Friction = 1f;

        public MaterialCustom() { }

        public MaterialCustom(float p_Friction) {
            m_Friction = p_Friction;
        }

        public override float getFriction()
        {
            return m_Friction;
        }

        public void setFriction(float f)
        {
            m_Friction = f;
        }
    }
}
