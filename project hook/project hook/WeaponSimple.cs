using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class WeaponSimple : Weapon
	{
		private Task m_ShotTask;
		public Task ShotTask { get { return m_ShotTask; } set { m_ShotTask = value; } }

		public WeaponSimple() { }

		public WeaponSimple(Shot p_Shot, float p_Delay, float p_Speed, Task p_ShotTask)
			: base(p_Shot, p_Delay, p_Speed)
		{
			ShotTask = p_ShotTask;
		}

		public override void Fire(Ship who)
		{
			m_Shots[m_NextShot].Enabled = true;
			m_Shots[m_NextShot].Center = who.Center;
			m_Shots[m_NextShot].Faction = who.Faction;
			m_Shots[m_NextShot].Task = m_ShotTask;

			m_Shots[m_NextShot].m_Ship = who;
		}
	}
}
