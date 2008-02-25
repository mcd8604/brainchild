using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskFire : Task
	{
		internal TaskFire() { }
		protected override void Do(Sprite on, GameTime at)
		{
			Ship temp = on as Ship;
			if (temp != null)
			{
				temp.shoot();
			}
#if DEBUG
			else
			{
				throw new Exception("TaskFire assigned to a non-ship object");
			}
#endif
		}
		internal override Task copy()
		{
			return new TaskFire();
		}
	}
}