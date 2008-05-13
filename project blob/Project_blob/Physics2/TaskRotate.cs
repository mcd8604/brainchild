using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics2
{
    class TaskRotate : Task
    {
        private Vector3 m_Axis;
        public Vector3 Axis
        {
            get
            {
                return m_Axis;
            }
            set
            {
                m_Axis = value;
            }
        }

        private float m_Speed;
        public float Speed
        {
            get
            {
                return m_Speed;
            }
            set
            {
                m_Speed = value;
            }
        }

        private float m_Speed;

        public TaskRotate() { }

        public TaskRotate( Vector3 axis, float speed )
        {
            m_Axis = axis;
            m_Speed = speed;
        }

        public override void update( Body b, float time )
        {
            Vector3 BodyCenter = b.getCenter();
            foreach ( PhysicsPoint p in b.getPoints() )
            {
                float dist = Vector3.Distance( BodyCenter, p.CurrentPosition );

            }
        }
    }
}
