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
		public override bool Complete
		{
			get
			{
				return m_Tasks.Count == 0;
			}

		}
		public override void Update(Sprite on, GameTime at)
		{
			if (m_Tasks.Count > 0)
			{
				m_Tasks.Peek().Update(on, at);
				if (m_Tasks.Peek().Complete)
				{
					m_Tasks.Dequeue();
				}
			}
		}
	}
}
