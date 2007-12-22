using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class PathLine:PathStrategy
    {
        Sprite m_Base;
        Vector2 m_Start;
        Vector2 m_End;
        Vector2 m_Delta;
        float m_Duration;
        Vector2 elapsed;
        Vector2 tick;

        float m_TotalDuration;

        public PathLine(Dictionary<ValueKeys, Object> p_Values)
			:base(p_Values){

            m_Base = (Sprite)m_Values[ValueKeys.Base];
            m_Start = (Vector2)m_Values[ValueKeys.Start];
            m_End = (Vector2)m_Values[ValueKeys.End];
            m_Duration = (float)m_Values[ValueKeys.Duration];
            m_Delta = new Vector2((m_End.X - m_Start.X) / m_Duration,
                                    (m_End.Y - m_Start.Y) / m_Duration);
            if (float.IsInfinity(m_Delta.X))
            {
                m_Delta.X = 0.0f;
            }

            if (float.IsInfinity(m_Delta.Y))
            {
                m_Delta.Y = 0.0f;
            }
            tick = new Vector2(m_Duration * m_Delta.X, m_Duration * m_Delta.Y);
            m_TotalDuration = 0;
            elapsed = new Vector2(0f, 0f);

		}

        public override void CalculateMovement(GameTime p_gameTime)
        {
            Vector2 t_Cur = m_Base.Center;
            bool test = t_Cur.X.Equals(m_End.X);
            if (m_TotalDuration <= m_Duration)
            {
                if (!(t_Cur.X.Equals(m_End.X)) || !(t_Cur.Y.Equals(m_End.Y)))
                {
                    m_TotalDuration += (float)(p_gameTime.ElapsedGameTime.TotalMilliseconds);

                    t_Cur.X += (float)(m_Delta.X * p_gameTime.ElapsedGameTime.TotalMilliseconds);
                    t_Cur.Y += (float)(m_Delta.Y * p_gameTime.ElapsedGameTime.TotalMilliseconds);

                    m_Base.Center = t_Cur;
                }
            }           
            else
            {
                m_Done = true;
            }


        }
    }
}
