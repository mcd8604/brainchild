using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class ShotPath:PathStrategy
    {
        Shot m_Base;
        Vector2 m_Start;
        Vector2 m_End;

        Vector2 m_Delta;

		int m_Delay = 100;
		double m_LastFrame;

        public ShotPath(Dictionary<ValueKeys, Object> p_Values)
			:base(p_Values)
		{
            m_Base = (Shot)m_Values[ValueKeys.Base];
            m_Start = (Vector2)m_Values[ValueKeys.Start];
            m_End = (Vector2)m_Values[ValueKeys.End];

			m_Delta = new Vector2((m_End.X - m_Start.X) / m_Base.Speed,
								   (m_End.Y - m_Start.Y) / m_Base.Speed);
			m_LastFrame = 0;
		}

        public override void CalculateMovement(GameTime p_GameTime)
        {
			Vector2 t_Cur = m_Base.Center;
			if (t_Cur.X > 0 && t_Cur.X <= m_End.X &&
				t_Cur.Y > 0 && t_Cur.Y >= m_End.Y)
			{
				////This makes the shot mopve in the Y direction based on the speed of the shot relative to the elapsed time
				//t_Cur.X = t_Cur.X - m_Base.Speed * p_GameTime.ElapsedGameTime.Milliseconds/1000.0f;
				//t_Cur.Y = t_Cur.Y - m_Base.Speed * p_GameTime.ElapsedGameTime.Milliseconds / 1000.0f;
				//m_Base.Center = t_Cur;

				if (p_GameTime.TotalGameTime.TotalMilliseconds >= m_LastFrame + m_Delay)
				{
					t_Cur.X += (float)(m_Delta.X * p_GameTime.ElapsedGameTime.TotalMilliseconds);
					t_Cur.Y += (float)(m_Delta.Y * p_GameTime.ElapsedGameTime.TotalMilliseconds);

					m_Base.Center = t_Cur;
					m_LastFrame = p_GameTime.TotalGameTime.TotalMilliseconds;
				}                 
			}
			else
			{
				m_Done = true;
			}
        }
    }
}