using System;
using System.Collections.Generic;
using System.Text;

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
			}
		}

		public TaskTimer() { }

		public TaskTimer(float p_Duration)
		{
			Duration = p_Duration;
		}

		public override bool Complete
		{
			get
			{
				return Duration <= 0;
			}
		}

		public override void Update(Sprite on, Microsoft.Xna.Framework.GameTime at)
		{
			Duration -= (float)at.ElapsedGameTime.TotalSeconds;
		}
	}
}
