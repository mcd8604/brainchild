using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskSeekTarget : Task
	{
		private Sprite m_Target = null;
		public Sprite Target
		{
			get
			{
				return m_Target;
			}
			set
			{
				m_Target = value;
			}
		}

		private float m_Speed = 0f;
		public float Speed
		{
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
			}
		}

		private bool m_ReachedGoal = false;

		public TaskSeekTarget() { }
		public TaskSeekTarget(Sprite p_Target, float p_Speed)
		{
			Target = p_Target;
			Speed = p_Speed;
		}

		public override bool Complete
		{
			get { return m_ReachedGoal; }
		}

		public override void Update(Sprite on, GameTime at)
		{
			Vector2 temp = m_Target.Center - on.Center;
			if (temp == Vector2.Zero)
			{
				m_ReachedGoal = true;
				return;
			}
			m_ReachedGoal = false;
			Vector2 temp2 = Vector2.Multiply(Vector2.Normalize(temp), (float)(Speed * (at.ElapsedGameTime.TotalSeconds)));
			if (Math.Abs(temp2.X) > Math.Abs(temp.X))
			{
				temp2.X = temp.X;
			}
			if (Math.Abs(temp2.Y) > Math.Abs(temp.Y))
			{
				temp2.Y = temp.Y;
			}
			on.Center = Vector2.Add(on.Center, temp2);
		}
	}
}
