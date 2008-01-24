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
		Vector2 minaccel = new Vector2(-1000, -1000);
		Vector2 maxaccel = new Vector2(1000, 1000);
		Vector2 minspeed = new Vector2(-500, -500);
		Vector2 maxspeed = new Vector2(500, 500);

		public TaskTether() { }
		public TaskTether(Sprite p_AttachedTo) {
			m_AttachedTo = p_AttachedTo;
		}

		public override bool Complete
		{
			get { return false; }
		}

		public override void Update(Sprite on, Microsoft.Xna.Framework.GameTime at)
		{

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

			speed = Vector2.Multiply(Vector2.Clamp(Vector2.Add(speed, Vector2.Multiply(Vector2.Clamp(new Vector2(deltaX * Math.Abs(deltaX), deltaY * Math.Abs(deltaY)), minaccel, maxaccel), (float)at.ElapsedGameTime.TotalSeconds)), minspeed, maxspeed), friction);

			Vector2 temp = Vector2.Multiply(speed, (float)at.ElapsedGameTime.TotalSeconds);

			on.Center = Vector2.Add(on.Center, temp);

		}

	}
}
