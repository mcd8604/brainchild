using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskStationaryBackground : Task
	{
		internal TaskStationaryBackground() { }
		protected override void Do(Sprite on, GameTime at)
		{
			Vector2 temp = on.Center;
			temp.Y += World.Position.BackgroundSpeed * (float)at.ElapsedGameTime.TotalSeconds;
			on.Center = temp;
		}
		internal override Task copy()
		{
			return new TaskStationaryBackground();
		}
	}
}
