using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class TaskFire : Task
	{
		public TaskFire() { }
		public override bool Complete
		{
			get { return true; }
		}
		public override void Update(Sprite on, Microsoft.Xna.Framework.GameTime at)
		{
			if (on.Enabled)
			{
				Ship temp = on as Ship;
				if (temp != null)
				{
					temp.shoot();
				}
#if DEBUG
				else
				{
					throw new Exception("TaskFire assigned to a non-shooting object");
				}
#endif
			}
		}
	}
}