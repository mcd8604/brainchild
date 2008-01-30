using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskRotateWithTarget : Task
	{
		private float m_Offset = 0f;
		public float Offset
		{
			get { return m_Offset; }
			set { m_Offset = value; }
		}
		private Sprite m_Target = null;
		public Sprite Target
		{
			get { return m_Target; }
			set { m_Target = value; }
		}
		public TaskRotateWithTarget() { }
		public TaskRotateWithTarget(Sprite p_Target) {
			Target = p_Target;
		}
		public TaskRotateWithTarget(Sprite p_Target, float p_Offset) {
			Target = p_Target;
			Offset = p_Offset;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			on.Rotation = Target.Rotation + Offset;
		}
		public override Task copy()
		{
			return new TaskRotateWithTarget(m_Target, m_Offset);
		}
	}
}
