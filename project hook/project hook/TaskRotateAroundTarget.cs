using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskRotateAroundTarget : Task
	{
		private Vector2 m_Offset = Vector2.Zero;
		public Vector2 Offset
		{
			get
			{
				return m_Offset;
			}
			set
			{
				m_Offset = value;
			}
		}

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

		public TaskRotateAroundTarget() { }
		public TaskRotateAroundTarget(Sprite p_Target)
		{
			Target = p_Target;
		}
		public TaskRotateAroundTarget(Sprite p_Target, Vector2 p_Offset)
		{
			Target = p_Target;
			m_Offset = p_Offset;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			float radius = Vector2.Distance(Vector2.Zero, m_Offset);
			Vector2 newPos = new Vector2();
			newPos.X = radius * (float)Math.Cos(MathHelper.ToRadians(m_Target.RotationDegrees));
			newPos.Y = radius * (float)Math.Sin(MathHelper.ToRadians(m_Target.RotationDegrees));
			on.Center = m_Target.Center + newPos;
		}

		public override Task copy()
		{
			return new TaskRotateAroundTarget(m_Target);
		}
	}
}
