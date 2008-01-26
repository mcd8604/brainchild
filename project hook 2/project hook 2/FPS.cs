using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	class FPS
	{

		protected float m_FPS;
		public float Value
		{
			get
			{
				return m_FPS;
			}
		}

		protected float m_UpdateInterval = 1.0f;
		public float UpdateInterval
		{
			get
			{
				return m_UpdateInterval;
			}
			set
			{
				m_UpdateInterval = value;
			}

		}

		protected float m_TimeSinceLastUpdate = 0.0f;
		protected int m_Framecount = 0;

		public void Update(GameTime p_Time)
		{
			m_TimeSinceLastUpdate += (float)p_Time.ElapsedRealTime.TotalSeconds;
			if (m_TimeSinceLastUpdate > m_UpdateInterval)
			{
				m_FPS = m_Framecount / m_TimeSinceLastUpdate;
				m_TimeSinceLastUpdate = 0.0f;
				m_Framecount = 0;
			}
		}

		public void Draw(SpriteBatch p_SpriteBatch)
		{
			++m_Framecount;
		}

		public override String ToString()
		{
			return m_FPS.ToString();
		}

	}
}
