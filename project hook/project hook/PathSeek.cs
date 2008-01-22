using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{

	/// <summary>
	/// A path that seeks to it's target.
	/// Specifically, This path will move the base towards the goal, even if this or the goal sprite are in motion.
	/// Specify either an End point, or a Target Sprite.
	/// Path will be 'Done' when it reaches the goal, or if the optional duration expires.
	/// 
	/// Parameters:
	/// Base - Required - The Sprite this Path should act on.
	/// Speed - Required - The Scalar Speed the sprite should travel, float Pixels per Seconds.
	/// Target - Optional - The Sprite this Path should seek out, Specify either a Target or an End.
	/// End - Optional - The Vector2 point this Path should seek out, Specify either a Target or an End.
	/// Duration - Optional - How Long this Path should try to seek for, float Seconds.
	/// Rotation - Optional - Should the sprite be rotated to direction of travel, defaults to true
	/// 
	/// </summary>
	class PathSeek : PathStrategy
	{

		Sprite Object = null;
		Sprite Target = null;
		Vector2 End;
		float Speed = 0f;
		bool timed = false;
		float Duration = 0f;
		bool Rotation = true;

		public PathSeek(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			Object = (Sprite)m_Values[ValueKeys.Base];
			Speed = (float)m_Values[ValueKeys.Speed];

			timed = m_Values.ContainsKey(ValueKeys.Duration);
			if (m_Values.ContainsKey(ValueKeys.Rotation))
			{
				Rotation = (bool)m_Values[ValueKeys.Rotation];
			}

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

			if (float.IsNaN(Object.Center.X) || float.IsNaN(Object.Center.Y))
			{
				throw new ArgumentException("Object location is invalid.");
			}
			if (float.IsNaN(goal.X) || float.IsNaN(goal.Y))
			{
				throw new ArgumentException("Target location is invalid.");
			}

			Vector2 temp = goal - Object.Center;

			if (temp.Equals(Vector2.Zero))
			{
				m_Done = true;
				return; // If we are at the goal, we're done, the end.
			}
			if (Rotation)
			{
				Object.Rotation = (float)Math.Atan2(temp.Y, temp.X);
			}

			Vector2 temp2 = Vector2.Multiply(Vector2.Normalize(temp), (float)(Speed * (p_gameTime.ElapsedGameTime.TotalSeconds)));

			if (Math.Abs(temp2.X) > Math.Abs(temp.X))
			{
				temp2.X = temp.X;
			}

			if (Math.Abs(temp2.Y) > Math.Abs(temp.Y))
			{
				temp2.Y = temp.Y;
			}

			if (float.IsNaN(temp2.X) || float.IsNaN(temp2.Y))
			{
				// Last chance catch, this literally should not happen under any circumstances.
				throw new ArithmeticException("This shouldn't happen");
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
