using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskSpecialTailThrow : Task
	{

		Task m_WindupTask;
		Task m_ThrowTask;
		Task m_ReleaseTask;

		public TaskSpecialTailThrow(Vector2 p_WindupTo, Vector2 p_ThrowTo, float p_TailSpeed, Vector2 p_ThrowAt, float p_ThrownSpeed)
		{
			TaskParallel temp = new TaskParallel();
			temp.addTask(new TaskSeekPoint(p_WindupTo, p_TailSpeed));
			temp.addTask(new TaskRotateFacePoint(p_ThrowAt));
			m_WindupTask = temp;
			temp = new TaskParallel();
			temp.addTask(new TaskSeekPoint(p_ThrowTo, p_TailSpeed));
			temp.addTask(new TaskRotateFacePoint(p_ThrowAt));
			m_ThrowTask = temp;
			temp = new TaskParallel();
			temp.addTask(new TaskSeekPoint(p_ThrowAt, p_ThrownSpeed));
			temp.addTask(new TaskRotateFacePoint(p_ThrowAt));
			m_ReleaseTask = temp;

		}

		public override bool Complete
		{
			get { return false; }
		}

		public override void Update(Sprite on, GameTime at)
		{
			Tail tail = on as Tail;
			if (tail != null)
			{

				if (tail.StateOfTail == Tail.TailState.Neutral)
				{

					m_WindupTask.Update(on, at);

					if (m_WindupTask.Complete)
					{
						tail.StateOfTail = Tail.TailState.Returning;
					}

				}
				else if (tail.StateOfTail == Tail.TailState.Returning)
				{

					m_ThrowTask.Update(on, at);

					if (m_ThrowTask.Complete)
					{
						tail.EnemyCaught.Task = m_ReleaseTask;
						tail.EnemyCaught = null;
						tail.TailReturned();
					}
				}

			}
#if DEBUG
			else
			{
				throw new Exception("TaskSpecialTailThrow assigned to something other than a Tail object");
			}
#endif
		}

	}
}
