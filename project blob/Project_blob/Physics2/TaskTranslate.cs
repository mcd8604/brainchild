using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
    public class TaskTranslate : Task
    {
        public IList<Vector3> PatrolPoints;
        public float Speed;
        protected int m_index = 0;

        public TaskTranslate(IList<Vector3> patrolPoints, float speed)
        {
            PatrolPoints = patrolPoints;
            Speed = speed;
        }

        public override void update(Body b, float time)
        {
            Vector3 CurrentDestination = PatrolPoints[m_index];
            Vector3 BodyCenter = b.getCenter();

            float travel = Speed * time;
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
