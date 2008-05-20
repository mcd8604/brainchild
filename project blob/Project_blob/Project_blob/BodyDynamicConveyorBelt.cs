using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
	class BodyDynamicConveyorBelt : DrawableBody
	{

		private Vector3 m_Direction;
		public Vector3 Direction
		{
			get
			{
				return m_Direction;
			}
			set
			{
				m_Direction = value;
				m_Velocity = m_Direction * m_Speed;
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
                m_Velocity = m_Direction * m_Speed;
			}
		}

		private Vector3 m_Velocity;

        private ConveyerBeltDynamic m_ConveyorBelt;

		//dynamic constructor
        public BodyDynamicConveyorBelt(Body ParentBody, List<PhysicsPoint> p_points, List<Collidable> p_collidables, List<Spring> p_springs, List<Task> p_tasks, ConveyerBeltDynamic p_ConveyorBelt, Dictionary<int, PhysicsPoint> p_pointMap)
			: base(ParentBody, p_points, p_collidables, p_springs, p_tasks, p_ConveyorBelt, p_pointMap)
		{
			m_Direction = p_ConveyorBelt.Direction;
			m_Speed = p_ConveyorBelt.Speed;
			m_Velocity = m_Direction * m_Speed;
			m_ConveyorBelt = p_ConveyorBelt;
		}

		public override Vector3 getRelativeVelocity(CollisionEvent e)
		{
			return m_Velocity;
		}

        public override void update(float TotalElapsedSeconds)
        {
			m_ConveyorBelt.Dist += TotalElapsedSeconds;
			base.update(TotalElapsedSeconds);
        }

	}
}
