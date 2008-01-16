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
            }

			timed = m_Values.ContainsKey(ValueKeys.Duration);
			if (timed)
			{
				Duration = (float)m_Values[ValueKeys.Duration];
			}

		}

		public override void CalculateMovement(GameTime p_gameTime)
		{
			m_Done = false;

			Vector2 temp = Vector2.Multiply(Velocity, (float)p_gameTime.ElapsedGameTime.TotalSeconds);

			if (!flag)
			{
				if ((Math.Abs(temp.X) > Math.Abs((End - Object.Center).X)) && (Math.Abs(temp.Y) > Math.Abs((End - Object.Center).Y)))
				{
					m_Done = true;
				}
			}

			Object.Center = Vector2.Add(Object.Center, temp);

			if (timed)
			{
				Duration -= (float)p_gameTime.ElapsedGameTime.TotalSeconds;
				if (Duration <= 0)
				{
					m_Done = true;
				}
			}

		}

		public override void Set()
		{
			Vector2 temp = End - Object.Center;
			double angle = (double)Math.Atan2(temp.Y, temp.X);
			Object.Rotation = (float)angle;

			Velocity = new Vector2();
			Velocity.X = (float)(speed * Math.Cos(angle));
			Velocity.Y = (float)(speed * Math.Sin(angle));

			if (timed)
			{
				Duration = (float)m_Values[ValueKeys.Duration];
			}
		}
	}
}
