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
	[Obsolete]
	class PathStraight : PathStrategy
	{


		Sprite Object;
		Vector2 Velocity;
		Vector2 End;
		float speed;
		bool timed;
		float Duration;
		bool Rotation = true;
		bool DerivedVelocity = false;

		public PathStraight(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			Object = (Sprite)m_Values[ValueKeys.Base];

			if (m_Values.ContainsKey(ValueKeys.Velocity))
			{
				Velocity = (Vector2)m_Values[ValueKeys.Velocity];
			}
			else if (m_Values.ContainsKey(ValueKeys.Angle) ){
				float angle = (float)m_Values[ValueKeys.Angle];
				speed = (float)m_Values[ValueKeys.Speed];
				Velocity.X = speed * (float)Math.Cos(angle);
				Velocity.Y = speed * (float)Math.Sin(angle);
			}
			else if (m_Values.ContainsKey(ValueKeys.End) )
			{
				DerivedVelocity = true;
				End = (Vector2)m_Values[ValueKeys.End];
				speed = (float)m_Values[ValueKeys.Speed];
			} else {
				throw new ArgumentException("Path Straight dictionary did not contain required parameters");
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
			}

			Vector2 temp = Vector2.Multiply(Velocity, (float)p_gameTime.ElapsedGameTime.TotalSeconds);

			if (DerivedVelocity)
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

			if (DerivedVelocity)
			{

				if (float.IsNaN(Object.Center.X) || float.IsNaN(Object.Center.Y))
				{
					throw new ArgumentException("Object location is invalid.");
				}
				if (float.IsNaN(End.X) || float.IsNaN(End.Y))
				{
					throw new ArgumentException("Target location is invalid.");
				}

				Vector2 temp = End - Object.Center;

				Velocity = Vector2.Multiply(Vector2.Normalize(temp), (float)speed);

			}

			if (Rotation)
			{
				Object.Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
			}

			if (timed)
			{
				Duration = (float)m_Values[ValueKeys.Duration];
			}
		}
	}
}
