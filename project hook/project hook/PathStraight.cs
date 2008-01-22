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
	/// Rotation - Optional - Should the path rotate the sprite, defaults to true;
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
		bool Rotation = true;

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

			if (m_Values.ContainsKey(ValueKeys.Rotation))
			{
				Rotation = (bool)m_Values[ValueKeys.Rotation];
			}

		}

		public override void CalculateMovement(GameTime p_gameTime)
		{

			if (float.IsNaN(Object.Center.X) || float.IsNaN(Object.Center.Y))
			{
				throw new ArgumentException("Object location is invalid.");
				return;
			}

			Vector2 temp = Vector2.Multiply(Velocity, (float)p_gameTime.ElapsedGameTime.TotalSeconds);

			if (!flag)
			{
				if ((Math.Abs(temp.X) > Math.Abs((End - Object.Center).X)) && (Math.Abs(temp.Y) > Math.Abs((End - Object.Center).Y)))
				{
					m_Done = true;
				}
			}

			if (float.IsNaN(temp.X) || float.IsNaN(temp.Y))
			{
				// Last chance catch, this literally should not happen under any circumstances.
				throw new ArithmeticException("This shouldn't happen");
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

				if (float.IsNaN(Object.Center.X) || float.IsNaN(Object.Center.Y))
				{
					throw new ArgumentException("Object location is invalid.");
					return;
				}
				if (float.IsNaN(End.X) || float.IsNaN(End.Y))
				{
					throw new ArgumentException("Target location is invalid.");
					return;
				}

				Vector2 temp = End - Object.Center;
				if (Rotation)
				{
					Object.Rotation = (float)Math.Atan2(temp.Y, temp.X);
				}

				Velocity = Vector2.Multiply(Vector2.Normalize(temp), (float)speed);

			}

			if (timed)
			{
				Duration = (float)m_Values[ValueKeys.Duration];
			}
		}
	}
}
