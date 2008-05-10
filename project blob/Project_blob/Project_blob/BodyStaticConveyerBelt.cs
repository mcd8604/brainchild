using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
	class BodyStaticConveyerBelt : BodyStatic
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
				m_Velocity = Vector3.Multiply(Direction, Speed);
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
				m_Velocity = Vector3.Multiply(Direction, Speed);
			}
		}

		private Vector3 m_Velocity;

        public BodyStaticConveyerBelt(IList<CollidableStatic> Collidables, Body ParentBody, string p_collisionAudio)
            : base(Collidables, ParentBody, p_collisionAudio)
		{
		}

		public override Vector3 getRelativeVelocity(CollisionEvent e)
		{
			return m_Velocity;
		}

	}
}
