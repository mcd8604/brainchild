using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskWait : Task
	{
		public delegate bool BoolFunction();

		private BoolFunction m_Until = null;
		public BoolFunction Until
		{ get { return m_Until; } set { m_Until = value; } }

		public TaskWait() { }
		public TaskWait(BoolFunction p_Until)
		{
			m_Until = p_Until;
		}
		public override bool IsComplete(Sprite on)
		{
			return m_Until.Invoke();
		}
		protected override void Do(Sprite on, GameTime at)
		{}
		public override Task copy()
		{
			return new TaskWait(m_Until);
		}
		public override void reset()
		{}
	}
}
