using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskTether : Task
	{

		private Sprite m_AttachedTo = null;

		private Vector2 speed = Vector2.Zero;

		// TODO: Better implementation of friction..
		private const float friction = 0.95f;
		private const int deadzone = 50;
		private readonly Vector2 minaccel = new Vector2(-2000, -2000);
		private readonly Vector2 maxaccel = new Vector2(2000, 2000);
		private readonly Vector2 minspeed = new Vector2(-500, -500);
		private readonly Vector2 maxspeed = new Vector2(500, 500);

		internal TaskTether() { }
		internal TaskTether(Sprite p_AttachedTo)
		{
			m_AttachedTo = p_AttachedTo;
		}

		protected override void Do(Sprite on, Microsoft.Xna.Framework.GameTime at)
		{

			// Not as dependant on framerate, still not good

			float deltaX = m_AttachedTo.Center.X - on.Center.X;
			float deltaY = m_AttachedTo.Center.Y - on.Center.Y;

			if (Math.Abs(deltaX) < deadzone)
			{
				deltaX = 0;
			}
			else
			{
				deltaX += (-deadzone * Math.Sign(deltaX));
			}
			if (Math.Abs(deltaY) < deadzone)
			{
				deltaY = 0;
			}
			else
			{
				deltaY += (-deadzone * Math.Sign(deltaY));
			}

			Vector2 e = new Vector2(deltaX * Math.Abs(deltaX), deltaY * Math.Abs(deltaY));
			Vector2 d = Vector2.Clamp(e, minaccel, maxaccel);
			Vector2 c = Vector2.Multiply(d, 0.005f);
			Vector2 b = Vector2.Add(speed, c);
			Vector2 a = Vector2.Clamp(b, minspeed, maxspeed);
			speed = Vector2.Multiply(a, friction);

			Vector2 temp = Vector2.Multiply(speed, (float)at.ElapsedGameTime.TotalSeconds);

			on.Center = Vector2.Add(on.Center, temp);

		}
		internal override Task copy()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
