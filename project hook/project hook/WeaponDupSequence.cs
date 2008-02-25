using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class WeaponDupSequence : Weapon
	{

		private Weapon weapon = null;
		private int repeats = 1;
		private int count = 0;
		private float recycleDelay = 100;
		internal float RecycleDelay
		{ get { return recycleDelay; } set { recycleDelay = value; } }

		internal void setWeapon(Weapon w)
		{
			weapon = w;
		}
		internal void setRepeats(int r)
		{
			repeats = r;
		}

		internal WeaponDupSequence() { }
		internal WeaponDupSequence(Ship p_Ship) : base(p_Ship) { }

		internal override void CreateShot()
		{
			if (m_Cooldown <= 0 && weapon != null)
			{
				weapon.Fire(m_Ship);
				if (m_Ship.ShootAnimation != null)
				{
					m_Ship.ShootAnimation.StartAnimation();
				}
				weapon.m_NextShot = (weapon.m_NextShot + 1) % weapon.m_Shots.Count;
				count++;
				if (count >= repeats)
				{
					count = 0;
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
			return weapon.changeShotType(type);
		}

		internal override IList<Shot> getShots()
		{
			return weapon.getShots();
		}

	}
}
