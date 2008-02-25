using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskSequence : Task
	{
		private List<Task> m_Tasks = new List<Task>();
		private int m_Current = 0;
		internal TaskSequence() { }
		internal TaskSequence(Task p_Task)
		{
			addTask(p_Task);
		}
		internal TaskSequence(IEnumerable<Task> p_Tasks)
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
			return m_Current >= m_Tasks.Count;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			if (m_Current < m_Tasks.Count)
			{
				m_Tasks[m_Current].Update(on, at);
				if (m_Tasks[m_Current].IsComplete(on))
				{
					m_Current++;
				}
			}
		}
		internal override Task copy()
		{
			List<Task> newTasks = new List<Task>();
			foreach (Task t in m_Tasks)
			{
				newTasks.Add(t.copy());
			}
			return new TaskSequence(newTasks);
		}
		internal override IEnumerable<Task> getSubTasks()
		{
			return m_Tasks;
		}
		internal override void reset()
		{
			m_Current = 0;
			if (m_Tasks != null)
			{
				foreach (Task t in m_Tasks)
				{
					t.reset();
				}
			}
		}
	}
}
