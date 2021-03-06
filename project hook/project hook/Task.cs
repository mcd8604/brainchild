using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal abstract class Task
	{

		/// <summary>
		/// Returns if this task has been completed.
		/// </summary>
		internal virtual bool IsComplete(Sprite on) { return false; }

		/// <summary>
		/// Request that this Task take action on a Sprite.
		/// </summary>
		/// <param name="on">The Sprite for this Task to effect.</param>
		/// <param name="at">The Current GameTime.</param>
		internal virtual void Update(Sprite on, GameTime at)
		{
			if (on.Enabled)
			{
				Do(on, at);
			}
		}

		internal virtual void Update(ICollection<Sprite> on, GameTime at)
		{
			foreach (Sprite s in on)
			{
				Update(s, at);
			}
		}

		protected virtual void Do(Sprite on, GameTime at)
		{
			throw new NotImplementedException();
		}

		internal abstract Task copy();
		internal virtual IEnumerable<Task> getSubTasks()
		{
			return null;
		}

		internal virtual void reset()
		{
			if (getSubTasks() != null)
			{
				foreach (Task t in getSubTasks())
				{
					t.reset();
				}
			}
		}

	}
}
