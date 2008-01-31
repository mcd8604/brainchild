using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskStraightAngle : Task
	{
		private float m_Angle = 0f;
		public float Angle
		{
			get
			{
				return m_Angle;
			}
			set
			{
				m_Angle = value;
			}
		}
		public float AngleDegrees
		{
			get
			{
				return MathHelper.ToDegrees(Angle);
			}
			set
			{
				Angle = MathHelper.ToRadians(value);
			}
		}

		private float m_Speed = 0f;
		public float Speed {
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
			}
		}
		public TaskStraightAngle() { }
		public TaskStraightAngle(float p_Angle, float p_Speed)
		{
			Angle = p_Angle;
			Speed = p_Speed;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			on.Center = new Vector2(on.Center.X + (m_Speed * (float)Math.Cos(m_Angle) * (float)at.ElapsedGameTime.TotalSeconds), on.Center.Y + (m_Speed * (float)Math.Sin(m_Angle) * (float)at.ElapsedGameTime.TotalSeconds));
		}
		public override Task copy()
		{
			return new TaskStraightAngle(m_Angle, m_Speed);
		}
	}
}
