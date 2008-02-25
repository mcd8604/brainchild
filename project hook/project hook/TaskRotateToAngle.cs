using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskRotateToAngle : Task, TaskIAngle
	{
		private float m_Angle = 0f;
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
		internal TaskRotateToAngle() { }
		internal TaskRotateToAngle(float p_Angle)
		{
			Angle = p_Angle;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			on.Rotation = Angle;
		}
		internal override Task copy()
		{
			return new TaskRotateToAngle(m_Angle);
		}
	}
}
