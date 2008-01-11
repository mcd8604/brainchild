using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class TailAttackPath:PathStrategy
    {
        Path m_CurrentPath;
		Vector2 m_Start;
        Vector2 m_End;
		Tail m_Base;
		PlayerShip m_PlayerShip;
		float m_Speed;
		bool changed = false;

        public TailAttackPath(Dictionary<ValueKeys, Object> p_Values)
			:base(p_Values)
		{
			m_Start = (Vector2)m_Values[ValueKeys.Start];
			m_End = (Vector2)m_Values[ValueKeys.End];
			m_Base = (Tail)m_Values[ValueKeys.Base];
			m_Speed = (float)m_Values[ValueKeys.Speed];
			m_PlayerShip = (PlayerShip)m_Values[ValueKeys.Target];
			Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.End, (Vector2)m_Values[ValueKeys.End]);
			dic.Add(PathStrategy.ValueKeys.Start, (Vector2)m_Values[ValueKeys.Start]);
			dic.Add(PathStrategy.ValueKeys.Base, (Collidable)m_Values[ValueKeys.Base]);
			dic.Add(PathStrategy.ValueKeys.Speed, (float)m_Values[ValueKeys.Speed]);
			m_CurrentPath = new Path(Path.Paths.Shot, dic);
		}

        public override void CalculateMovement(GameTime p_GameTime)
        {
			m_CurrentPath.CalculateMovement(p_GameTime);
			m_Base.Degree  = TurnToFace(m_Base.Center, m_End, m_Base.Degree, 10) + 90;
			if(MathHelper.Distance(m_Base.Center.X,m_End.X) < 10 && MathHelper.Distance(m_Base.Center.Y,m_End.Y) < 10 && !changed)
			{
				Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
				dic.Add(PathStrategy.ValueKeys.End, m_PlayerShip.Center);
				dic.Add(PathStrategy.ValueKeys.Start, m_End);
				dic.Add(PathStrategy.ValueKeys.Base, m_Base);
				dic.Add(PathStrategy.ValueKeys.Speed, (m_Speed*2.5f));
				m_CurrentPath = new Path(Path.Paths.Shot,dic);
				changed = true;
			}
			if (MathHelper.Distance(m_Base.Center.X, m_PlayerShip.Center.X) < 50 && MathHelper.Distance(m_Base.Center.Y, m_PlayerShip.Center.Y) < 50 && changed)
			{
				m_Base.TailReturned();
			}
        }

		/// <summary>
		/// Calculates the angle that an object should face, given its position, its
		/// target's position, its current angle, and its maximum turning speed.
		/// </summary>
		private static float TurnToFace(Vector2 position, Vector2 faceThis,
			float currentAngle, float turnSpeed)
		{
			// consider this diagram:
			//         B 
			//        /|
			//      /  |
			//    /    | y
			//  / o    |
			// A--------
			//     x
			// 
			// where A is the position of the object, B is the position of the target,
			// and "o" is the angle that the object should be facing in order to 
			// point at the target. we need to know what o is. using trig, we know that
			//      tan(theta)       = opposite / adjacent
			//      tan(o)           = y / x
			// if we take the arctan of both sides of this equation...
			//      arctan( tan(o) ) = arctan( y / x )
			//      o                = arctan( y / x )
			// so, we can use x and y to find o, our "desiredAngle."
			// x and y are just the differences in position between the two objects.
			float x = faceThis.X - position.X;
			float y = faceThis.Y - position.Y;

			// we'll use the Atan2 function. Atan will calculates the arc tangent of 
			// y / x for us, and has the added benefit that it will use the signs of x
			// and y to determine what cartesian quadrant to put the result in.
			// http://msdn2.microsoft.com/en-us/library/system.math.atan2.aspx
			float desiredAngle = (float)Math.Atan2(y, x);

			// so now we know where we WANT to be facing, and where we ARE facing...
			// if we weren't constrained by turnSpeed, this would be easy: we'd just 
			// return desiredAngle.
			// instead, we have to calculate how much we WANT to turn, and then make
			// sure that's not more than turnSpeed.

			// first, figure out how much we want to turn, using WrapAngle to get our
			// result from -Pi to Pi ( -180 degrees to 180 degrees )
			float difference = WrapAngle(desiredAngle - currentAngle);

			// clamp that between -turnSpeed and turnSpeed.
			difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

			// so, the closest we can get to our target is currentAngle + difference.
			// return that, using WrapAngle again.
			return WrapAngle(currentAngle + difference);
		}

		/// <summary>
		/// Returns the angle expressed in radians between -Pi and Pi.
		/// <param name="radians">the angle to wrap, in radians.</param>
		/// <returns>the input value expressed in radians from -Pi to Pi.</returns>
		/// </summary>
		private static float WrapAngle(float radians)
		{
			while (radians < -MathHelper.Pi)
			{
				radians += MathHelper.TwoPi;
			}
			while (radians > MathHelper.Pi)
			{
				radians -= MathHelper.TwoPi;
			}
			return radians;
		}
    }
}