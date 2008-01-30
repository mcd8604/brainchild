using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class TaskStraightVelocity : Task
	{
		private Vector2 m_Velocity = Vector2.Zero;
		public Vector2 Velocity
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
		public TaskStraightVelocity() { }
		public TaskStraightVelocity(Vector2 p_Velocity)
		{
			Velocity = p_Velocity;
		}
		protected override void Do(Sprite on, GameTime at)
		{
				on.Center += Vector2.Multiply(Velocity, (float)at.ElapsedGameTime.TotalSeconds);
		}
		public override Task copy()
		{
			return new TaskStraightVelocity(m_Velocity);
		}
	}
}
