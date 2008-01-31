using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskTimer : Task
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
				m_DurationRemaining = Duration;
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
		public TaskTimer() { }
		public TaskTimer(float p_Duration)
		{
			Duration = p_Duration;
		}
		public override bool IsComplete(Sprite on)
		{
			return DurationRemaining <= 0;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			m_DurationRemaining -= (float)at.ElapsedGameTime.TotalSeconds;
		}
		public override Task copy()
		{
			return new TaskTimer(m_Duration);
		}
		public override void reset()
		{
			m_DurationRemaining = m_Duration;
		}
	}
}
