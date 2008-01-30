using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskQueue : Task
	{
		private Queue<Task> m_Tasks = new Queue<Task>();
		public TaskQueue() { }
		public TaskQueue(Task p_Task)
		{
			addTask(p_Task);
		}
		public void addTask(Task t)
		{
			m_Tasks.Enqueue(t);
		}
		public override bool IsComplete(Sprite on)
		{
			return m_Tasks.Count == 0;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			if (m_Tasks.Count > 0)
			{
				m_Tasks.Peek().Update(on, at);
				if (m_Tasks.Peek().IsComplete(on))
				{
					m_Tasks.Dequeue();
				}
			}
		}
		public override Task copy()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
