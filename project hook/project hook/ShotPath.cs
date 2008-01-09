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

        double m_Delta;

		int m_Delay = 100;
		double m_LastFrame;

		float m_Speed;
		double m_Slope;

        public ShotPath(Dictionary<ValueKeys, Object> p_Values)
			:base(p_Values)
		{
            m_Base = (Shot)m_Values[ValueKeys.Base];
            m_Start = (Vector2)m_Values[ValueKeys.Start];
            m_End = (Vector2)m_Values[ValueKeys.End];

			//m_Delta = new Vector2((m_End.X - m_Start.X),
			//                       (m_End.Y - m_Start.Y));
			//m_LastFrame = 0;
			m_Speed = m_Base.Speed;
			float t_ChangeX = (m_End.X - m_Start.X);
			float t_ChangeY = -(m_End.Y - m_Start.Y);
			if (t_ChangeX != 0)
			{
				if (t_ChangeY == 0)
				{
					if (t_ChangeX > 0)
					{
						m_Slope = 0;
					}
					else if (t_ChangeX < 0)
					{
						//m_Slope = 180;
						m_Slope = MathHelper.Pi;
					}
				}
				else
				{
					m_Slope = (t_ChangeY / t_ChangeX); //MathHelper.ToRadians(t_ChangeY / t_ChangeX);
				}
			}
			else if (t_ChangeX == 0)
			{
				if (t_ChangeY > 0)
				{
					m_Slope = 90;
					m_Slope = MathHelper.PiOver2;
				}
				else if (t_ChangeY < 0)
				{
					m_Slope = MathHelper.PiOver2*3;
				}
			}
			//Console.WriteLine(m_Slope);
		}

        public override void CalculateMovement(GameTime p_GameTime)
        {
			Vector2 t_Cur = m_Base.Center;
			//if(t_Cur.X>=0 && t_Cur.Y>=0)
			if (t_Cur.X > 0 && t_Cur.X <= m_End.X &&
				t_Cur.Y > 0 && t_Cur.Y >= m_End.Y)
			{
				////This makes the shot mopve in the Y direction based on the speed of the shot relative to the elapsed time
				//t_Cur.X = t_Cur.X - m_Base.Speed * p_GameTime.ElapsedGameTime.Milliseconds/1000.0f;
				//t_Cur.Y = t_Cur.Y - m_Base.Speed * p_GameTime.ElapsedGameTime.Milliseconds / 1000.0f;
				//m_Base.Center = t_Cur;
				//Console.WriteLine("hey");

				//d=V*T
				m_Delta = m_Speed * p_GameTime.ElapsedGameTime.TotalSeconds;
				//Console.WriteLine("distance="+m_Delta);

				double t_ChangeX = m_Delta * Math.Cos(m_Slope);
				double t_ChangeY = m_Delta * Math.Sin(m_Slope);
				//Console.WriteLine("slope=" + m_Slope);
				//Console.WriteLine("change in x=" + t_ChangeX);
				//Console.WriteLine("change in y=" + t_ChangeY);


				if (m_Slope >= 0 && m_Slope < MathHelper.PiOver2)
				{
					t_Cur.X += (float)t_ChangeX;
					t_Cur.Y -= (float)t_ChangeY;
				}
				else if (m_Slope >= MathHelper.PiOver2 && m_Slope < MathHelper.Pi)
				{
					t_Cur.X -= (float)t_ChangeX;
					t_Cur.Y -= (float)t_ChangeY;
				}
				else if (m_Slope >= MathHelper.Pi && m_Slope < MathHelper.PiOver2 * 3)
				{
					t_Cur.X -= (float)t_ChangeX;
					t_Cur.Y += (float)t_ChangeY;
				}
				else if (m_Slope >= MathHelper.PiOver2 * 3 && m_Slope < MathHelper.TwoPi)
				{
					t_Cur.X += (float)t_ChangeX;
					t_Cur.Y += (float)t_ChangeY;
				}

				m_Base.Center = t_Cur;
				//Console.WriteLine("pos=" + m_Base.Center);
			}
			else
			{
				m_Done = true;
			}
        }
    }
}