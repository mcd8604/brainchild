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
		public override bool Complete
		{
			get
			{
				return false;
			}
		}
		public override void Update(Sprite on, GameTime at)
		{
			m_Tasks[m_Current].Update(on, at);
			if (m_Tasks[m_Current].Complete)
			{
				m_Current = (m_Current + 1) % m_Tasks.Count;
			}
		}
	}
}