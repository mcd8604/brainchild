using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	[Obsolete]
	class TaskSpecialTailAttack : Task
	{

		Task m_AttackTask;
		Task m_ReturnTask;

		public TaskSpecialTailAttack(Vector2 p_AttackTarget, float p_AttackSpeed,
										Sprite p_ReturnTo, float p_ReturnSpeed)
		{
			m_AttackTask = new TaskSeekPoint(p_AttackTarget, p_AttackSpeed);
			m_ReturnTask = new TaskSeekTarget(p_ReturnTo, p_ReturnSpeed);
		}

		public TaskSpecialTailAttack( Vector2 p_AttackTarget, float p_AttackSpeed, float p_AttackMaxDuration,
										Sprite p_ReturnTo, float p_ReturnSpeed )
		{
			TaskParallel temp = new TaskParallel();
			temp.addTask(new TaskSeekPoint(p_AttackTarget, p_AttackSpeed));
			temp.addTask(new TaskTimer(p_AttackMaxDuration));
			temp.addTask(new TaskRotateFacePoint(p_AttackTarget));
			m_AttackTask = temp;
			temp = new TaskParallel();
			temp.addTask(new TaskSeekTarget(p_ReturnTo, p_ReturnSpeed));
			temp.addTask(new TaskRotateFaceTarget(p_ReturnTo, (float)Math.PI));
			m_ReturnTask = temp;
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

				if (tail.StateOfTail == Tail.TailState.Returning)
				{

					m_ReturnTask.Update(on, at);

					// If the tail is returning, reverse the direction it's pointing
					// Opposite the direction of travel, i.e. away from the ship
					//m_Base.Rotation += (float)Math.PI;

					if (m_ReturnTask.Complete)
					{
						tail.TailReturned();
					}
				}

				if (tail.StateOfTail == Tail.TailState.Attacking)
				{
					m_AttackTask.Update(on, at);

					if (m_AttackTask.Complete)
					{
						tail.StateOfTail = Tail.TailState.Returning;
					}
				}

			}
#if DEBUG
			else
			{
				throw new Exception("TaskSpecialTailAttack assigned to something other than a Tail object");
			}
#endif
		}

	}
}
