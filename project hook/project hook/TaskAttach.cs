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
		protected override void Do(Sprite on, GameTime at)
		{
				on.Center = m_Target.Center;
		}
		public override Task copy()
		{
			return new TaskAttach(m_Target);
		}
	}
}
