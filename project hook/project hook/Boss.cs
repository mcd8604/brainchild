using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class Boss : Ship
	{
		public Boss() { }

		public override void RegisterCollision(Collidable p_Other)
		{
#if DEBUG
			Console.WriteLine("The Trigger has been hit by " + p_Other + "!");
#endif
			if (World.Position.Speed == 0 && p_Other.Faction == Factions.Player && !(p_Other is Tail))
			{
				base.RegisterCollision(p_Other);
				World.Position.setSpeed(80);
				ToBeRemoved = true;
			}
		}
	}
}
