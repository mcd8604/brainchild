using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskComposite : Task
	{
		private List<Task> m_Tasks = new List<Task>();
		internal TaskComposite() { }
		internal TaskComposite(Task p_Task)
		{
			addTask(p_Task);
		}
		internal TaskComposite(IEnumerable<Task> p_Tasks)
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
				if (!t.IsComplete(on))
				{
					return false;
				}
			}
			return true;
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
			throw new Exception("The method or operation is not implemented.");
		}
	}
}