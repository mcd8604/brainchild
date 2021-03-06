using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class WeaponSeek : Weapon
	{
		private Sprite m_LastTarget = null;

		private Sprite m_Target = null;
		internal Sprite Target
		{
			get
			{
				return m_Target;
			}
			set
			{
				m_Target = value;
			}
		}

		internal Vector2 m_Position = Vector2.Zero;

		private Task m_ShotTask;

		internal WeaponSeek() { }

		internal WeaponSeek(Ship p_Ship, Shot p_Shot, float p_Delay, float p_Speed)
			: base(p_Ship, p_Shot, p_Delay, p_Speed)
		{ }

		internal override void Fire(Ship who)
		{
			if (m_LastTarget != Target || m_LastSpeed != Speed)
			{
				TaskParallel task = new TaskParallel();
				task.addTask(new TaskSeekTarget(m_Target, Speed));
				task.addTask(new TaskRotateFaceTarget(m_Target));
				m_ShotTask = task;

				m_LastTarget = Target;
				m_LastSpeed = Speed;
			}

			m_Shots[m_NextShot].Enabled = true;
			m_Shots[m_NextShot].Center = who.Center + m_Position;
			m_Shots[m_NextShot].Faction = who.Faction;
			m_Shots[m_NextShot].Task = m_ShotTask;

			m_Shots[m_NextShot].m_Ship = who;
		}
	}
}
