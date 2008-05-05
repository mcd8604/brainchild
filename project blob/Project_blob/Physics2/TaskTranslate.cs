using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics2
{
    class TaskTranslate : Task
    {
        List<Vector3> m_PatrolPoints = new List<Vector3>();
        public List<Vector3> PatrolPoints
        {
            get { return m_PatrolPoints; }

            set 
            {
                m_CurDestination = m_PatrolPoints[0];
                m_PatrolPoints = value; 
            }
        }

        float m_Speed;
        public float Speed
        {
            get { return m_Speed; }
            set { m_Speed = value; }
        }

        private Vector3 m_CurDestination = new Vector3();

        public override void update(Body b)
        {
            if (Vector3.Distance(m_CurDestination, b.getCenter()) < .001)
                m_CurDestination = m_PatrolPoints[m_PatrolPoints.IndexOf(m_CurDestination) + 1];

            Vector3 accel;
            accel = Vector3.Normalize(m_CurDestination - b.getCenter()) * Speed;

            for (int i = 0; i < b.points.Count; i++ )
            {
                b.points[i].CurrentPosition = Vector3.Add(b.points[i].CurrentPosition, accel);
            }
        }
    }
}
