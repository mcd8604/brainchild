using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PathStraight : PathStrategy
	{

		Sprite Object;
		Vector2 Velocity;
        Vector2 End;
        float speed;
        double angle;
        Vector2 temp;
        bool flag;
		bool timed;
		float Duration;

		public PathStraight(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			Object = (Sprite)m_Values[ValueKeys.Base];

            if (m_Values.ContainsKey(ValueKeys.Velocity))
            {
                flag = true;
                Velocity = (Vector2)m_Values[ValueKeys.Velocity];
            }
            else
            {
                flag = false;
                End = (Vector2)m_Values[ValueKeys.End];
                speed = (float)m_Values[ValueKeys.Speed];

                temp = End - Object.Center;
                angle = (double)Math.Atan2(temp.Y, temp.X);
                Object.Rotation = (float)angle;
            }

			timed = m_Values.ContainsKey(ValueKeys.Duration);
			if (timed)
			{
				Duration = (float)m_Values[ValueKeys.Duration];
			}

		}

		public override void CalculateMovement(GameTime p_gameTime)
		{
            if (flag)
            {
                Vector2 temp = Vector2.Multiply(Velocity, (float)p_gameTime.ElapsedGameTime.TotalSeconds);
                Object.Center = Vector2.Add(Object.Center, temp);
            }
            else
            {
                double delta = speed * (p_gameTime.ElapsedGameTime.TotalSeconds);

                Vector2 temp2 = new Vector2();
                temp2.X = (float)(delta * Math.Cos(angle));
                temp2.Y = (float)(delta * Math.Sin(angle));

                if ( (Math.Abs(temp2.X) > Math.Abs(temp.X)) || (Math.Abs(temp2.Y) > Math.Abs(temp.Y) ) )
                {
                    m_Done = true;
                }

                Object.Center = Vector2.Add(Object.Center, temp2);
            }

			if (timed)
			{
				Duration -= (float)p_gameTime.ElapsedGameTime.TotalSeconds;
				if (Duration <= 0)
				{
					m_Done = true;
				}
			}

		}

	}
}
