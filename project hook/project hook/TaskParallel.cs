using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskParallel : Task
	{
		private List<Task> m_Tasks = new List<Task>();
		internal TaskParallel() { }
		internal TaskParallel(Task p_Task)
		{
			addTask(p_Task);
		}
		internal TaskParallel(IEnumerable<Task> p_Tasks)
		{
			addTasks(p_Tasks);
		}
		internal void addTask(Task t)
		{
			m_Tasks.Add(t);
		}
		internal void addTasks(IEnumerable<Task> t)
		{
			m_Tasks.AddRange(t);
		}
		internal override bool IsComplete(Sprite on)
		{
			foreach (Task t in m_Tasks)
			{
				if (t.IsComplete(on))
				{
					return true;
				}
			}
			return false;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			foreach (Task t in m_Tasks)
			{
				t.Update(on, at);
			}
		}
		internal override Task copy()
		{
			List<Task> newTasks = new List<Task>();
			foreach (Task t in m_Tasks)
			{
				newTasks.Add(t.copy());
			}
			return new TaskParallel(newTasks);
		}
		internal override IEnumerable<Task> getSubTasks()
		{
			return m_Tasks;
		}
	}
}
