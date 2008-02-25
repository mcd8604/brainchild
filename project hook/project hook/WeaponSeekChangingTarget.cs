using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class WeaponSeekChangingTarget : WeaponSeek
	{

		private Collidable.Factions m_Faction;
		private Sprite noTarget;
		private List<Sprite> pos;

		internal Collidable.Factions Faction
		{
			get { return m_Faction; }
			set { m_Faction = value; }
		}

		internal WeaponSeekChangingTarget(Ship p_Ship, Shot p_Shot, float p_Delay, float p_Speed, Collidable.Factions factionToFind)
			: base(p_Ship, p_Shot, p_Delay, p_Speed)
		{
			m_Faction = factionToFind;
			pos = new List<Sprite>();
			noTarget = new Sprite();
		}

		internal override void Fire(Ship who)
		{
			List<Sprite> list = World.getAllSprites();

			pos.Clear();

			for (int a = 0; a < list.Count; a++)
			{
				if (list[a] is Collidable)
				{
					Collidable c = (Collidable)list[a];
					if (c.Faction == Faction && !(c is Shot))
					{
						pos.Add(list[a]);
					}
				}
			}

			if (pos.Count == 0)
			{
				Target = noTarget;
				float x = who.Center.X;
				Vector2 vect = Target.Center;
				vect.X = x;
				Target.Center = vect;
			}
			else
			{
				int index = Game.Random.Next(pos.Count);
				Target = pos[index];
			}
			base.Fire(who);
		}
	}
}
