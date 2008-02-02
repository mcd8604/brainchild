using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskRemove : Task
	{
		public TaskRemove() { }
		protected override void Do(Sprite on, GameTime at)
		{
			on.Enabled = false;
			on.ToBeRemoved = true;
		}
		public override Task copy()
		{
			return new TaskRemove();
		}
	}
}
