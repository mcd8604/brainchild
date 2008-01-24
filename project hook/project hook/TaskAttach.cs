using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskAttach : Task
	{
		private Sprite m_Target = null;
		public Sprite Target {
			get {
				return m_Target;
			}
			set {
				m_Target = value;
			}
		}
		public TaskAttach() { }
		public TaskAttach(Sprite p_Target)
		{
			Target = p_Target;
		}
		public override bool Complete
		{
			get { return true; }
		}
		public override void Update(Sprite on, GameTime at)
		{
			if (on.Enabled)
			{
				on.Center = m_Target.Center;
			}
		}
	}
}
