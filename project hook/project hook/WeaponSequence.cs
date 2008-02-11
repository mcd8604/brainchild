using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class WeaponSequence : Weapon
	{

		List<Weapon> weapons = new List<Weapon>();
		int currentWeapon = 0;
		float recycleDelay = 100;
		public float RecycleDelay
		{ get { return recycleDelay; } set { recycleDelay = value; } }

		public void addWeapon(Weapon w)
		{
			weapons.Add(w);
		}

		public WeaponSequence() { }

		public override void CreateShot(Ship who)
		{
			if (m_Cooldown <= 0 && weapons.Count > 0)
			{
				weapons[currentWeapon].Fire(who);
				if (who.ShootAnimation != null)
				{
					who.ShootAnimation.StartAnimation();
				}
				weapons[currentWeapon].m_NextShot = (weapons[currentWeapon].m_NextShot + 1) % weapons[currentWeapon].m_Shots.Count;
				currentWeapon++;
				if (currentWeapon >= weapons.Count)
				{
					currentWeapon = 0;
					m_Cooldown = recycleDelay;
				}
				else
				{
					m_Cooldown = m_Delay;
				}
			}
		}

		public override void Fire(Ship who)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override IList<Shot> changeShotType(Shot type)
		{
			List<Shot> ret = new List<Shot>();
			foreach (Weapon w in weapons)
			{
				ret.AddRange(w.changeShotType(type));
			}
			return ret;
		}

		public override IList<Shot> getShots()
		{
			List<Shot> ret = new List<Shot>();
			foreach (Weapon w in weapons)
			{
				ret.AddRange(w.getShots());
			}
			return ret;
		}

	}
}
