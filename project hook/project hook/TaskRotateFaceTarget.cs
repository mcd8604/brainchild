using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class TaskRotateFaceTarget : Task
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


		public TaskRotateFaceTarget() { }
		public TaskRotateFaceTarget(Sprite p_Target) {
			Target = p_Target;
		}
		public TaskRotateFaceTarget(Sprite p_Target, float p_Offset) {
			Target = p_Target;
			Offset = p_Offset;
		}

		public override bool Complete
		{
			get { return false; }
		}
		public override void Update(Sprite on, Microsoft.Xna.Framework.GameTime at)
		{
			on.Rotation = (float)Math.Atan2(Target.Center.Y - on.Center.Y, Target.Center.X - on.Center.X) + Offset;
		}

	}
}
