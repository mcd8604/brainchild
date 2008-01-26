using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class WeaponExample : Weapon
	{

		private float m_LastAngle = float.NaN;
		private float m_LastSpeed = float.NaN;


		public override string ShotName
		{
			get
			{
				return base.ShotName;
			}
			set
			{
				base.ShotName = value;
			}
		}

		Task m_ShotTask;

		public WeaponExample() { }

		public WeaponExample(string p_ShotName, float p_Damage, float p_Delay, float p_Speed, float p_Angle)
			: base(p_ShotName, p_Damage, p_Delay, p_Speed, p_Angle)
		{}

		public override void CreateShot(Ship who)
		{
			if (m_Cooldown <= 0)
			{
				float thisAngle = (who.Rotation + Angle);
				if (m_LastAngle != thisAngle || m_LastSpeed != Speed)
				{
					TaskParallel task = new TaskParallel();
					task.addTask(new TaskStraightAngle(thisAngle, Speed));
					task.addTask(new TaskRotateToAngle(thisAngle));
					m_ShotTask = task;

					m_LastAngle = thisAngle;
					m_LastSpeed = Speed;
				}

				m_Shots[m_NextShot].Enabled = true;
				m_Shots[m_NextShot].Center = who.Center;
				m_Shots[m_NextShot].Faction = who.Faction;
				m_Shots[m_NextShot].Task = m_ShotTask;
				m_Shots[m_NextShot].setAnimation(m_ShotName, 10);
				m_Shots[m_NextShot].Animation.StartAnimation();

				m_Cooldown = m_Delay;

				m_NextShot = (m_NextShot + 1) % m_Shots.Count;
			}
		}

	}
}
