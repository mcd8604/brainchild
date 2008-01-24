using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	[Obsolete]
	class PathTether : PathStrategy
	{

		Sprite Object;
		Sprite AttachedTo;

		Vector2 speed = Vector2.Zero;
		// TODO: Better implementation of friction..
		float friction = 0.95f;
		int deathzone = 50;
		Vector2 minaccel = new Vector2(-1000, -1000);
		Vector2 maxaccel = new Vector2(1000, 1000);
		Vector2 minspeed = new Vector2(-500, -500);
		Vector2 maxspeed = new Vector2(500, 500);

		public PathTether(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			Object = (Sprite)m_Values[ValueKeys.Base];
			AttachedTo = (Sprite)m_Values[ValueKeys.Target];
		}

		public override void CalculateMovement(GameTime p_gameTime)
		{

			if (float.IsNaN(Object.Center.X) || float.IsNaN(Object.Center.Y))
			{
				throw new ArgumentException("Object location is invalid.");
			}
			if (float.IsNaN(AttachedTo.Center.X) || float.IsNaN(AttachedTo.Center.Y))
			{
				throw new ArgumentException("Target location is invalid.");
			}

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

			if (float.IsNaN(temp.X) || float.IsNaN(temp.Y))
			{
				// Last chance catch, this literally should not happen under any circumstances.
				throw new ArithmeticException("This shouldn't happen");
			}

			Vector2 previousPos = Object.Center;
			Object.Center = Vector2.Add(Object.Center, temp);


			if (MathHelper.Distance(Object.Center.X, AttachedTo.Center.X) > 5 && MathHelper.Distance(Object.Center.Y, AttachedTo.Center.Y) > 5)
				Object.Rotation = TrigHelper.TurnToFace(Object.Center, previousPos, Object.Rotation, .5f);

		}


	}
}
