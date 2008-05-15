using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
	class BodyStaticConveyerBelt : Body
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

        private ConveyerBeltStatic m_StaticModel;

        public BodyStaticConveyerBelt(List<CollidableStatic> Collidables, Body ParentBody, ConveyerBeltStatic staticModel)
            : base(Collidables, ParentBody)
        {
            m_Direction = staticModel.Direction;
            m_Speed = staticModel.Speed;
            m_Velocity = m_Direction * m_Speed;
            m_StaticModel = staticModel;
		}

		public override Vector3 getRelativeVelocity(CollisionEvent e)
		{
			return m_Velocity;
		}

        public override void update(float TotalElapsedSeconds)
        {
            m_StaticModel.TextureOffsetX -= this.Speed * m_StaticModel.TextureScaleX * (m_Direction.X + m_Direction.Z) * 0.005f;
            base.update(TotalElapsedSeconds);
        }

	}
}
