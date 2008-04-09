using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    public class MaterialCustom : Material
    {
        private float m_Friction = 1f;

        private float m_Cling = 0f;

        public MaterialCustom() { }

        public MaterialCustom(float p_Friction) {
            m_Friction = p_Friction;
        }

        public MaterialCustom(float p_Friction, float p_Cling)
        {
            m_Friction = p_Friction;
            m_Cling = p_Cling;
        }

        public override float getFriction()
        {
            return m_Friction;
        }

        public void setFriction(float p_Friction)
        {
            m_Friction = p_Friction;
        }

        public override float getCling()
        {
            return m_Cling;
        }

        public void setCling(float p_Cling)
        {
            m_Cling = p_Cling;
        }
    }
}
