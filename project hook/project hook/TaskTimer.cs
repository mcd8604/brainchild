using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskTimer : Task
	{
		private float m_Duration = 0f;
		internal float Duration
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
		internal float DurationRemaining
		{
			get
			{
				return m_DurationRemaining;
			}
		}
		internal TaskTimer() { }
		internal TaskTimer(float p_Duration)
		{
			Duration = p_Duration;
		}
		internal override bool IsComplete(Sprite on)
		{
			return DurationRemaining <= 0;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			m_DurationRemaining -= (float)at.ElapsedGameTime.TotalSeconds;
		}
		internal override Task copy()
		{
			return new TaskTimer(m_Duration);
		}
		internal override void reset()
		{
			m_DurationRemaining = m_Duration;
		}
	}
}
