using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class WorldPosition
	{
		
		private float m_Speed = 0f;
		private float m_BackgroundSpeed = 0f ;
		private float m_Distance = 0f;

		internal WorldPosition() { }
		internal WorldPosition(float p_Speed)
		{
			setSpeed(p_Speed);
		}
		internal WorldPosition(float p_Speed, float p_StartDistance)
		{
			m_Distance = p_StartDistance;
			setSpeed(p_Speed);
		}

		internal float Distance
		{
			get 
			{
				return m_Distance;
			}
		}

		internal float Speed
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

		internal float BackgroundSpeed
		{
			get
			{
				return m_BackgroundSpeed;
			}
			set
			{
				m_BackgroundSpeed = value;
			}
		}

		internal void resetDistance()
		{
			m_Distance = 0;
		}

		internal void setSpeed(float p_Speed)
		{
			m_Speed = p_Speed;
			m_BackgroundSpeed = p_Speed * 0.5f;
		}

		internal void Update(GameTime p_GameTime)
		{
			m_Distance += m_Speed * (float)(p_GameTime.ElapsedGameTime.TotalSeconds);
		}

	}
}
