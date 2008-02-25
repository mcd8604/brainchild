using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	internal class WeaponSimple : Weapon
	{
		private Task m_ShotTask;
		internal Task ShotTask { get { return m_ShotTask; } set { m_ShotTask = value; } }

		internal WeaponSimple(Ship p_Ship) : base(p_Ship) { }

		internal WeaponSimple(Ship p_Ship, Shot p_Shot, float p_Delay, float p_Speed, Task p_ShotTask)
			: base(p_Ship, p_Shot, p_Delay, p_Speed)
		{
			ShotTask = p_ShotTask;
		}

		internal override void Fire(Ship who)
		{
			m_Shots[m_NextShot].Enabled = true;
			m_Shots[m_NextShot].Center = who.Center;
			m_Shots[m_NextShot].Faction = who.Faction;
			m_Shots[m_NextShot].Task = m_ShotTask;

			m_Shots[m_NextShot].m_Ship = who;
		}
	}
}
