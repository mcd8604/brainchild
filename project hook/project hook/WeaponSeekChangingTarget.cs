using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class WeaponSeekChangingTarget : WeaponSeek
    {

        private Collidable.Factions m_Faction;

        public Collidable.Factions Faction
        {
            get { return m_Faction; }

            set { m_Faction = value; }

        }

        public WeaponSeekChangingTarget(Shot p_Shot, float p_Delay, float p_Speed,Collidable.Factions factionToFind)
			: base(p_Shot, p_Delay, p_Speed)
		{
            m_Faction = factionToFind;
		}
        
        public override void Fire(Ship who)
        {
            List<Sprite> list = World.m_World.getSpriteList();

            List<Sprite> pos = new List<Sprite>();

            for (int a = 0; a < list.Count; a ++ )
            {
                if (list[a] is Collidable)
                {
                    Collidable c = (Collidable )list[a];
                    if (c.Faction == Faction && !(c is Shot))
                    {
                        pos.Add(list[a]);
                    }
                }
            }

            if (pos.Count == 0)
            {
                Target = new Sprite();
                Target.Position = new Vector2(512, 0);
            }
            else
            {
                Random r = new Random();
                int rt = r.Next(pos.Count);
                Target = pos[rt];
            }
            base.Fire(who);
        }
    }
}
