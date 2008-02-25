using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskStraightVelocity : Task
	{
		private Vector2 m_Velocity = Vector2.Zero;
		internal Vector2 Velocity
		{
			get
			{
				return m_Velocity;
			}
			set
			{
				m_Velocity = value;
			}
		}
		internal TaskStraightVelocity() { }
		internal TaskStraightVelocity(Vector2 p_Velocity)
		{
			Velocity = p_Velocity;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			on.Center += Vector2.Multiply(Velocity, (float)at.ElapsedGameTime.TotalSeconds);
		}
		internal override Task copy()
		{
			return new TaskStraightVelocity(m_Velocity);
		}
	}
}
