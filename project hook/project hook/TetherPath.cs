using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class TetherPath : PathStrategy
    {

        Sprite Object;
        Sprite AttachedTo;

        Vector2 speed = Vector2.Zero;
        float friction = 0.95f;
        int deathzone = 50;
        Vector2 minaccel = new Vector2(-1000, -1000);
        Vector2 maxaccel = new Vector2(1000, 1000);
        Vector2 minspeed = new Vector2(-500, -500);
        Vector2 maxspeed = new Vector2(500, 500);

        public TetherPath(Dictionary<ValueKeys, Object> p_Values)
            : base(p_Values)
        {
            Object = (Sprite)m_Values[ValueKeys.Base];
            AttachedTo = (Sprite)m_Values[ValueKeys.Target];
        }

        public override void CalculateMovement(GameTime p_gameTime)
        {

            float deltaX = AttachedTo.Center.X - Object.Center.X;
            float deltaY = AttachedTo.Center.Y - Object.Center.Y;

            if (Math.Abs(deltaX) < deathzone)
            {
                deltaX = 0;
            }
            else
            {
                deltaX += (-deathzone * Math.Sign(deltaX));
            }
            if (Math.Abs(deltaY) < deathzone)
            {
                deltaY = 0;
            }
            else
            {
                deltaY += (-deathzone * Math.Sign(deltaY));
            }

            speed = Vector2.Multiply(Vector2.Clamp(Vector2.Add(speed, Vector2.Multiply(Vector2.Clamp(new Vector2(deltaX * Math.Abs(deltaX), deltaY * Math.Abs(deltaY)), minaccel, maxaccel), (float)p_gameTime.ElapsedGameTime.TotalSeconds)), minspeed, maxspeed), friction);

            Vector2 temp = Vector2.Multiply(speed, (float)p_gameTime.ElapsedGameTime.TotalSeconds);

			Vector2 previousPos = Object.Center;
            Object.Center = Vector2.Add(Object.Center, temp);

			if (MathHelper.Distance(Object.Center.X, AttachedTo.Center.X) > 5 && MathHelper.Distance(Object.Center.Y, AttachedTo.Center.Y) > 5)
				Object.Degree = TurnToFace(Object.Center, previousPos, Object.Degree, .5f);			

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
