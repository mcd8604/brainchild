using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace Physics2
{
	[Serializable]
    public class TaskTranslate : Task
    {
        protected List<Vector3> m_PatrolPoints;
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public List<Vector3> PatrolPoints 
        {
            get
            {
                return m_PatrolPoints;
            }
            set
            {
                m_PatrolPoints = value;
            }
        }
        protected float m_Speed;
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
		protected bool m_IsRepeating;
		public bool IsRepeating
		{
			get
			{
				return m_IsRepeating;
			}
			set
			{
				m_IsRepeating = value;
			}
		}
        protected int m_index = 0;
        
        public TaskTranslate() { }

        public TaskTranslate(List<Vector3> patrolPoints, float speed)
        {
            m_PatrolPoints = patrolPoints;
            m_Speed = speed;
        }

        public override void update(Body b, float time)
        {
            Vector3 CurrentDestination = m_PatrolPoints[m_index];
            Vector3 BodyCenter = b.getCenter();

            float travel = m_Speed * time;
            float dist = Vector3.Distance(CurrentDestination, BodyCenter);

            if (dist < travel)
            {
                travel = dist;
                ++m_index;
            }

            Vector3 delta = Vector3.Normalize(CurrentDestination - BodyCenter) * travel;

            foreach (PhysicsPoint p in b.points)
            {
                p.PotentialPosition = p.CurrentPosition + delta;
            }
        }
    }
}
