using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskTether : Task
	{

		Sprite m_AttachedTo = null;

		Vector2 speed = Vector2.Zero;
		// TODO: Better implementation of friction..
		float friction = 0.95f;
		int deathzone = 50;
		Vector2 minaccel = new Vector2(-2000, -2000);
		Vector2 maxaccel = new Vector2(2000, 2000);
		Vector2 minspeed = new Vector2(-500, -500);
		Vector2 maxspeed = new Vector2(500, 500);

		public TaskTether() { }
		public TaskTether(Sprite p_AttachedTo) {
			m_AttachedTo = p_AttachedTo;
		}

		protected override void Do(Sprite on, Microsoft.Xna.Framework.GameTime at)
		{

			// Not as dependant on framerate, still not good

			float deltaX = m_AttachedTo.Center.X - on.Center.X;
			float deltaY = m_AttachedTo.Center.Y - on.Center.Y;

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

			Vector2 e = new Vector2(deltaX * Math.Abs(deltaX), deltaY * Math.Abs(deltaY));
			Vector2 d = Vector2.Clamp(e, minaccel, maxaccel);
			Vector2 c = Vector2.Multiply(d, 0.005f);
			Vector2 b = Vector2.Add(speed, c);
			Vector2 a = Vector2.Clamp(b, minspeed, maxspeed);
			speed = Vector2.Multiply(a, friction);

			Vector2 temp = Vector2.Multiply(speed, (float)at.ElapsedGameTime.TotalSeconds);

			on.Center = Vector2.Add(on.Center, temp);

		}

	}
}
