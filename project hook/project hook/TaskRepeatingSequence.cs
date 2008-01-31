using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskRepeatingSequence : Task
	{
		private List<Task> m_Tasks = new List<Task>();
		private int m_Current = 0;
		public TaskRepeatingSequence() { }
		public TaskRepeatingSequence(Task p_Task)
		{
			addTask(p_Task);
		}
		public TaskRepeatingSequence(IEnumerable<Task> p_Tasks)
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
		protected override void Do(Sprite on, GameTime at)
		{
			m_Tasks[m_Current].Update(on, at);
			if (m_Tasks[m_Current].IsComplete(on))
			{
				m_Current = (m_Current + 1) % m_Tasks.Count;
			}
		}
		public override Task copy()
		{
			List<Task> newTasks = new List<Task>();
			foreach (Task t in m_Tasks)
			{
				newTasks.Add(t.copy());
			}
			return new TaskRepeatingSequence(newTasks);
		}
		public override IEnumerable<Task> getSubTasks()
		{
			return m_Tasks;
		}
	}
}