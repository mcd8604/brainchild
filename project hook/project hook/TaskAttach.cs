using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskAttach : Task
	{
		private Sprite m_Target = null;
		internal Sprite Target
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
		internal TaskAttach() { }
		internal TaskAttach(Sprite p_Target)
		{
			Target = p_Target;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			on.Center = m_Target.Center;
		}
		internal override Task copy()
		{
			return new TaskAttach(m_Target);
		}
	}
}
