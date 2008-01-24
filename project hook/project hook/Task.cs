using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public abstract class Task
	{

		/// <summary>
		/// Returns if this task has been completed.
		/// </summary>
		public abstract bool Complete
		{
			get;
		}

		/// <summary>
		/// Request that this Task take action on a Sprite.
		/// </summary>
		/// <param name="on">The Sprite for this Task to effect.</param>
		/// <param name="at">The Current GameTime.</param>
		public abstract void Update(Sprite on, GameTime at);

		public virtual void Update(ICollection<Sprite> on, GameTime at)
		{
			foreach (Sprite s in on)
			{
				Update(s, at);
			}
		}

		/// <summary>
		/// Check if a task has been completed.
		/// </summary>
		/// <param name="t">The task to check.</param>
		/// <returns>If the task has been completed.</returns>
		public static bool isComplete(Task t)
		{
			return t.Complete;
		}

	}
}
