using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{

    /// <summary>
    /// A path that seeks to it's target.
    /// Specify either an End point, or a Target Sprite.
    /// Path will be 'Done' when it reaches the end, or if the optional duration expires.
    /// </summary>
    class PathSeek : PathStrategy
    {

        Sprite Object = null;
        Sprite Target = null;
        Vector2 End;
        float Speed = 0f;
        bool timed = false;
        float Duration = 0f;

        public PathSeek(Dictionary<ValueKeys, Object> p_Values)
            : base(p_Values)
        {
            Object = (Sprite)m_Values[ValueKeys.Base];
            Speed = (float)m_Values[ValueKeys.Speed];

            timed = m_Values.ContainsKey(ValueKeys.Duration);

            if (m_Values.ContainsKey(ValueKeys.Target))
            {
                Target = (Sprite)m_Values[ValueKeys.Target];
            }
            if (m_Values.ContainsKey(ValueKeys.End))
            {
                if (Target != null)
                {
                    throw new ArgumentException("Seek may not have both a Target and an End.");
                }
                End = (Vector2)m_Values[ValueKeys.End];
            }
            else
            {
                if (Target == null)
                {
                    throw new ArgumentException("Seek must have either a Target or an End.");
                }
            }

        }

        public override void CalculateMovement(GameTime p_gameTime)
        {
            m_Done = false;
            Vector2 goal;
            if (Target != null)
            {
                goal = Target.Center;
            }
            else
            {
                goal = End;
            }

            Vector2 temp = goal - Object.Center;

            if (temp.Equals(Vector2.Zero))
            {
                m_Done = true;
            }

            double angle = (double)Math.Atan2(temp.Y, temp.X);
            Object.Rotation = (float)angle;

            double delta = Speed * (p_gameTime.ElapsedGameTime.TotalSeconds);

            Vector2 temp2 = new Vector2();
            temp2.X = (float)(delta * Math.Cos(angle));
            temp2.Y = (float)(delta * Math.Sin(angle));

            if (Math.Abs(temp2.X) > Math.Abs(temp.X))
            {
                temp2.X = temp.X;
            }

            if (Math.Abs(temp2.Y) > Math.Abs(temp.Y))
            {
                temp2.Y = temp.Y;
            }

            Object.Center = Vector2.Add(Object.Center, temp2);

            if (temp2.Equals(temp))
            {
                m_Done = true;
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

		public override void Set()
		{
			if (m_Values.ContainsKey(ValueKeys.Start))
			{
				Object.Center = (Vector2)m_Values[ValueKeys.Start];
			}
			if (timed)
			{
				Duration = (float)m_Values[ValueKeys.Duration];
			}
		}

    }
}
