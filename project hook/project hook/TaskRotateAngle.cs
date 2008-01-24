using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class TaskRotateAngle : Task
	{

		float m_Angle = 0f;
		public float Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		public TaskRotateAngle() { }
		public TaskRotateAngle(float p_Angle) {
			Angle = p_Angle;
		}

		public override bool Complete
		{
			get { return false; }
		}
		public override void Update(Sprite on, Microsoft.Xna.Framework.GameTime at)
		{
			on.Rotation = Angle;
		}
	}
}
