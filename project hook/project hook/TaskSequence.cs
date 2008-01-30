using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class TaskSequence : Task
	{
		private List<Task> m_Tasks = new List<Task>();
		private int m_Current = 0;
		public TaskSequence() { }
		public TaskSequence(Task p_Task)
		{
			addTask(p_Task);
		}
		public TaskSequence(IEnumerable<Task> p_Tasks)
		{
			addTasks(p_Tasks);
		}
		public void addTask(Task t)
		{
			m_Tasks.Add(t);
		}
		public void addTasks(IEnumerable<Task> t)
		{
			m_Tasks.AddRange(t);
		}
		public override bool IsComplete(Sprite on)
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
		public override Task copy()
		{
			List<Task> newTasks = new List<Task>();
			foreach (Task t in m_Tasks)
			{
				newTasks.Add(t.copy());
			}
			return new TaskSequence(newTasks);
		}
		public override IEnumerable<Task> getSubTasks()
		{
			return m_Tasks;
		}
	}
}
