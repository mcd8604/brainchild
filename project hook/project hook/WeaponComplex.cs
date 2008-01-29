using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class WeaponComplex : Weapon
	{

		private Task m_ShotTask;
		public Task ShotTask
		{
			get
			{
				return m_ShotTask;
			}
			set
			{
				m_ShotTask = value;
			}
		}


		public override void Fire(Ship who)
		{
			if (m_Shots[m_NextShot].Task != m_ShotTask)
			{
				//m_Shots[m_NextShot].Task = m_ShotTask.Copy();
			}

			m_Shots[m_NextShot].Enabled = true;
			m_Shots[m_NextShot].Center = who.Center;
			m_Shots[m_NextShot].Faction = who.Faction;
		}

	}
}
