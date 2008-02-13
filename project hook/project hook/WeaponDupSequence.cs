using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class WeaponDupSequence : Weapon
	{

		Weapon weapon = null;
		int repeats = 1;
		int count = 0;
		float recycleDelay = 100;
		public float RecycleDelay
		{ get { return recycleDelay; } set { recycleDelay = value; } }

		public void setWeapon(Weapon w)
		{
			weapon = w;
		}
		public void setRepeats(int r)
		{
			repeats = r;
		}

		public WeaponDupSequence(){ }
		public WeaponDupSequence(Ship p_Ship):base(p_Ship) { }

		public override void CreateShot()
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

		public override void Fire(Ship who)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override IList<Shot> changeShotType(Shot type)
		{
			return weapon.changeShotType(type);
		}

		public override IList<Shot> getShots()
		{
			return weapon.getShots();
		}

	}
}
