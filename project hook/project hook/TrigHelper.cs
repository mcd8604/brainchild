using System;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public static class TrigHelper
	{

		/// <summary>
		/// Calculates the angle that an object should face, given its position, its
		/// target's position, its current angle, and its maximum turning speed.
		/// </summary>
		public static float TurnToFace(Vector2 position, Vector2 faceTo, float currentAngle, float turnSpeed)
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
			float x = faceTo.X - position.X;
			float y = faceTo.Y - position.Y;

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
		public static float WrapAngle(float radians)
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