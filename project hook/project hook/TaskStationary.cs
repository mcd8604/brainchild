using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskStationary : Task
	{
		public TaskStationary() { }
		protected override void Do(Sprite on, GameTime at)
		{
			Vector2 temp = on.Center;
			temp.Y += World.Position.Speed * (float)at.ElapsedGameTime.TotalSeconds;
			on.Center = temp;
		}
		public override Task copy()
		{
			return new TaskStationary();
		}
	}
}
