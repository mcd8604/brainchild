using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskWaitFor : Task
	{
		public delegate bool BoolFunction(Sprite s);

		private BoolFunction m_Until = null;
		public BoolFunction Until
		{ get { return m_Until; } set { m_Until = value; } }

		public TaskWaitFor() { }
		public TaskWaitFor(BoolFunction p_Until)
		{
			m_Until = p_Until;
		}
		public override bool IsComplete(Sprite on)
		{
			return m_Until.Invoke(on);
		}
		protected override void Do(Sprite on, GameTime at)
		{ }
		public override Task copy()
		{
			return new TaskWaitFor(m_Until);
		}
		public override void reset()
		{ }
	}
}
