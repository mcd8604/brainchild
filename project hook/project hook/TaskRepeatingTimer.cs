using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskRepeatingTimer : Task
	{
		private float m_Duration = 0f;
		public float Duration
		{
			get
			{
				return m_Duration;
			}
			set
			{
				m_Duration = value;
				m_DurationRemaining = m_Duration;
			}
		}
		private float m_DurationRemaining = 0f;
		public float DurationRemaining
		{
			get
			{
				return m_DurationRemaining;
			}
		}
		public TaskRepeatingTimer() { }
		public TaskRepeatingTimer(float p_Duration)
		{
			Duration = p_Duration;
			m_DurationRemaining = Duration;
		}
		public override bool IsComplete(Sprite on)
		{
			if (DurationRemaining <= 0)
			{
				m_DurationRemaining = Duration;
				return true;
			}
			return false;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			m_DurationRemaining -= (float)at.ElapsedGameTime.TotalSeconds;
		}
		public override Task copy()
		{
			return new TaskRepeatingTimer(m_Duration);
		}
	}
}
