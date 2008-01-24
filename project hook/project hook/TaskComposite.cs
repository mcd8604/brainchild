using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskComposite : Task
	{
		private List<Task> m_Tasks = new List<Task>();
		public TaskComposite() { }
		public TaskComposite(Task p_Task)
		{
			addTask(p_Task);
		}
		public TaskComposite(IEnumerable<Task> p_Tasks)
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
				return m_Tasks.TrueForAll(isComplete);
			}
		}
		public override void Update(Sprite on, GameTime at)
		{
			foreach (Task t in m_Tasks)
			{
				t.Update(on, at);
			}
		}
	}
}