using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskRotateByAngle : Task
	{
		float m_Angle = 0f;
		public float Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}
		public float AngleDegrees
		{
			get { return MathHelper.ToDegrees(Angle); }
			set { Angle = MathHelper.ToRadians(value); }
		}
		public TaskRotateByAngle() { }
		public TaskRotateByAngle(float p_Angle)
		{
			Angle = p_Angle;
		}
		protected override void Do(Sprite on, Microsoft.Xna.Framework.GameTime at)
		{
			on.Rotation += (Angle * (float)at.ElapsedGameTime.TotalSeconds);
		}
		public override Task copy()
		{
			return new TaskRotateByAngle(m_Angle);
		}
	}
}
