using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class WeaponSequence : Weapon
	{

		private List<Weapon> weapons = new List<Weapon>();
		private int currentWeapon = 0;
		private float recycleDelay = 100;
		internal float RecycleDelay
		{ get { return recycleDelay; } set { recycleDelay = value; } }

		internal void addWeapon(Weapon w)
		{
			weapons.Add(w);
		}

		internal WeaponSequence() { }
		internal WeaponSequence(Ship p_Ship) : base(p_Ship) { }

		internal override void CreateShot()
		{
			if (m_Cooldown <= 0 && weapons.Count > 0)
			{
				weapons[currentWeapon].Fire(m_Ship);
				if (m_Ship.ShootAnimation != null)
				{
					m_Ship.ShootAnimation.StartAnimation();
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

		internal override void Fire(Ship who)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		internal override IList<Shot> changeShotType(Shot type)
		{
			List<Shot> ret = new List<Shot>();
			foreach (Weapon w in weapons)
			{
				ret.AddRange(w.changeShotType(type));
			}
			return ret;
		}

		internal override IList<Shot> getShots()
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
