using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskRotateToAngle : Task
	{
		float m_Angle = 0f;
		public float Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}
		public TaskRotateToAngle() { }
		public TaskRotateToAngle(float p_Angle) {
			Angle = p_Angle;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			on.Rotation = Angle;
		}
	}
}
