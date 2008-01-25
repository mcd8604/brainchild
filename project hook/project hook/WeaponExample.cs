using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class WeaponExample : Weapon
	{

		private float m_LastAngle = 0f;
		private float m_LastSpeed = 0f;

		public override float Angle
		{
			get
			{
				return base.Angle;
			}
			set
			{
				base.Angle = value;
			}
		}

		public override float Speed
		{
			get
			{
				return base.Speed;
			}
			set
			{
				base.Speed = value;
			}
		}

		public override string ShotName
		{
			get
			{
				return base.ShotName;
			}
			set
			{
				base.ShotName = value;
				for (int i = 0; i < 10; i++)
				{
					Shot t_Shot = new Shot("WeaponExampleShot", Vector2.Zero, 30, 75, m_Texture, 1, false,
									  0f, Depth.GameLayer.Shot, Collidable.Factions.None, -1, null, 15, 10);
					t_Shot.Bound = Collidable.Boundings.Diamond;
					m_Shots.Add(t_Shot);
				}
			}
		}

		Task m_ShotTask;

		public WeaponExample() { }

		public WeaponExample(string p_ShotName, float p_Damage, float p_Delay, float p_Speed, float p_Angle)
			: base(p_ShotName, p_Damage, p_Delay, p_Speed, p_Angle)
		{
			TaskParallel task = new TaskParallel();
			task.addTask(new TaskStraightAngle(p_Angle, p_Speed));
			task.addTask(new TaskRotateAngle(p_Angle));
			m_ShotTask = task;

			// temp
			for (int i = 0; i <= (int)(2500f / m_Speed); i++)
			{
				Shot t_Shot = new Shot("WeaponExampleShot", Vector2.Zero, 30, 75, m_Texture, 1, false,
								  0f, Depth.GameLayer.Shot, Collidable.Factions.None, -1, null, 15, 10);
				t_Shot.Bound = Collidable.Boundings.Diamond;
				m_Shots.Add(t_Shot);
			}
		}

		public override void CreateShot(Ship who)
		{
			if (m_Cooldown <= 0)
			{
				float thisAngle = (who.Rotation + Angle);
				if (m_LastAngle != thisAngle || m_LastSpeed != Speed)
				{
					TaskParallel task = new TaskParallel();
					task.addTask(new TaskStraightAngle(thisAngle, Speed));
					task.addTask(new TaskRotateAngle(thisAngle));
					m_ShotTask = task;
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
