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
		bool timed;
		float Duration;

		public PathStraight(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			Object = (Sprite)m_Values[ValueKeys.Base];
			Velocity = (Vector2)m_Values[ValueKeys.Speed];
			timed = m_Values.ContainsKey(ValueKeys.Duration);
			if (timed)
			{
				Duration = (float)m_Values[ValueKeys.Duration];
			}

		}

		public override void CalculateMovement(GameTime p_gameTime)
		{

			Vector2 temp = Vector2.Multiply(Velocity, (float)p_gameTime.ElapsedGameTime.TotalSeconds);
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

	}
}
