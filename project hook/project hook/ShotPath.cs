using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class ShotPath:PathStrategy
    {
        Collidable m_Base;
        Vector2 m_Start;
        Vector2 m_End;
		double m_Slope;
		float m_Speed;		
		double m_Delta;

        public ShotPath(Dictionary<ValueKeys, Object> p_Values)
			:base(p_Values)
		{
            m_Base = (Collidable)m_Values[ValueKeys.Base];
            m_Start = (Vector2)m_Values[ValueKeys.Start];
            m_End = (Vector2)m_Values[ValueKeys.End];
			m_Speed = (float)m_Values[ValueKeys.Speed];

			float x = m_End.X - m_Start.X;
			float y = m_End.Y - m_Start.Y;
			m_Slope = (double)Math.Atan2(y, x);
			m_Base.Rotation = (float)(m_Slope + MathHelper.PiOver2);
		}

        public override void CalculateMovement(GameTime p_GameTime)
        {
			Vector2 t_Cur = m_Base.Center;
			if (t_Cur.X > 0 || t_Cur.X <= 800 || t_Cur.Y > 0 || t_Cur.Y >= 600)
			{
				//d=V*T
				m_Delta = m_Speed * p_GameTime.ElapsedGameTime.TotalSeconds;
				
				double t_ChangeX = m_Delta * Math.Cos(m_Slope);
				double t_ChangeY = m_Delta * Math.Sin(m_Slope);
				
				t_Cur.X += (float)t_ChangeX;
				t_Cur.Y += (float)t_ChangeY;
				
				m_Base.Center = t_Cur;
			}
			else
			{
				m_Done = true;
			}
        }
    }
}