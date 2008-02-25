using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskRemove : Task
	{
		private bool m_Explode = false;

		internal TaskRemove() { }
		internal TaskRemove(bool p_ShouldExplode) { m_Explode = p_ShouldExplode; }
		protected override void Do(Sprite on, GameTime at)
		{
			if (m_Explode && on is Collidable && World.isSpriteVisible(on))
			{
				((Collidable)on).SpawnDeathEffect(on.Center);
			}
			on.Enabled = false;
			on.ToBeRemoved = true;
		}
		internal override Task copy()
		{
			return new TaskRemove();
		}
	}
}
