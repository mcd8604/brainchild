using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskQueue : Task
	{
		private Queue<Task> m_Tasks = new Queue<Task>();
		internal TaskQueue() { }
		internal TaskQueue(Task p_Task)
		{
			addTask(p_Task);
		}
		internal void addTask(Task t)
		{
			m_Tasks.Enqueue(t);
		}
		internal override bool IsComplete(Sprite on)
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
		internal override Task copy()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
