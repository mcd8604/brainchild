using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	/// <summary>
	/// A Path that travels in a straight line.
	/// Path will be 'Done' if the optional duration expires, or if a velocity is not specified, when it reaches the end point.
	/// 
	/// Parameters:
	/// Base - Required - The Sprite this Path should act on.
	/// Velocity - Optional - The Vector2 Velocity this sprite should move, Pixels per Seconds.
	/// Speed - Optional - If Velocity is not specified, The Scalar Speed the sprite should travel, float Pixels per Seconds.
	/// End - Optional - If Velocity is not specified, The Vector2 point this Path should travel to.
	/// Duration - Optional - How Long this Path should try to seek for, float Seconds.
	/// 
	/// </summary>
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

		}

		public override void CalculateMovement(GameTime p_gameTime)
		{
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
			m_Done = false;

			if (!flag)
			{

				Vector2 temp = End - Object.Center;
				double angle = (double)Math.Atan2(temp.Y, temp.X);
				Object.Rotation = (float)angle;

				Velocity = new Vector2();
				Velocity.X = (float)(speed * Math.Cos(angle));
				Velocity.Y = (float)(speed * Math.Sin(angle));

			}

			if (timed)
			{
				Duration = (float)m_Values[ValueKeys.Duration];
			}
		}
	}
}
