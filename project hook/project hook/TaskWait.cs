using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskWait : Task
	{
		internal delegate bool BoolFunction();

		private BoolFunction m_Until = null;
		internal BoolFunction Until
		{ get { return m_Until; } set { m_Until = value; } }

		internal TaskWait() { }
		internal TaskWait(BoolFunction p_Until)
		{
			m_Until = p_Until;
		}
		internal override bool IsComplete(Sprite on)
		{
			return m_Until.Invoke();
		}
		protected override void Do(Sprite on, GameTime at)
		{ }
		internal override Task copy()
		{
			return new TaskWait(m_Until);
		}
		internal override void reset()
		{ }
	}
}
