using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskRepeatingSequence : Task
	{
		private List<Task> m_Tasks = new List<Task>();
		private int m_Current = 0;
		internal TaskRepeatingSequence() { }
		internal TaskRepeatingSequence(Task p_Task)
		{
			addTask(p_Task);
		}
		internal TaskRepeatingSequence(IEnumerable<Task> p_Tasks)
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
		protected override void Do(Sprite on, GameTime at)
		{
			m_Tasks[m_Current].Update(on, at);
			if (m_Tasks[m_Current].IsComplete(on))
			{
				m_Current = (m_Current + 1) % m_Tasks.Count;
			}
		}
		internal override Task copy()
		{
			List<Task> newTasks = new List<Task>();
			foreach (Task t in m_Tasks)
			{
				newTasks.Add(t.copy());
			}
			return new TaskRepeatingSequence(newTasks);
		}
		internal override IEnumerable<Task> getSubTasks()
		{
			return m_Tasks;
		}
	}
}