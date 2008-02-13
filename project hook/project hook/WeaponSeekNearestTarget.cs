using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class WeaponSeekNearestTarget : Weapon
	{

		private Sprite m_LastTarget = null;

		public Vector2 m_Position = Vector2.Zero;

		Task m_SeekTask;
		Task m_StraightTask;
        Task m_ShotTask;


		private Collidable.Factions m_Faction;
		public Collidable.Factions Faction
		{
			get { return m_Faction; }
			set { m_Faction = value; }
		}

		public WeaponSeekNearestTarget(Ship p_Ship, Shot p_Shot, float p_Delay, float p_Speed, Collidable.Factions factionToSeek)
			: base(p_Ship, p_Shot, p_Delay, p_Speed)
		{
			m_Faction = factionToSeek;

			TaskParallel task = new TaskParallel();
			task.addTask(new TaskStraightAngle(MathHelper.PiOver2, Speed));
			task.addTask(new TaskRotateToAngle(MathHelper.PiOver2));
			m_StraightTask = task;
		}

		public override void Fire(Ship who)
		{
			List<Sprite> list = World.m_World.getSpriteList();

			Sprite target = null;
			float d = float.MaxValue;

			foreach (Sprite s in list)
			{
				if (s is Collidable && !(s is Shot) && ((Collidable)s).Faction == Faction)
				{
					float t = Vector2.DistanceSquared(who.Center, s.Center);
					if (t < d)
					{
						target = s;
						t = d;
					}
				}
			}

			if (target != null)
			{
				if (m_LastTarget != target || m_LastSpeed != Speed)
				{
					TaskParallel task = new TaskParallel();
					task.addTask(new TaskSeekTarget(target, Speed));
					task.addTask(new TaskRotateFaceTarget(target));
					m_SeekTask = task;

					m_LastTarget = target;
					m_LastSpeed = Speed;
				}

				m_Shots[m_NextShot].Enabled = true;
				m_Shots[m_NextShot].Center = who.Center + m_Position;
				m_Shots[m_NextShot].Faction = who.Faction;
				m_Shots[m_NextShot].Task = m_SeekTask;

				m_Shots[m_NextShot].m_Ship = who;
			}
			else
			{
				if (m_LastSpeed != Speed)
				{
					TaskParallel task = new TaskParallel();
					task.addTask(new TaskStraightAngle(MathHelper.PiOver2, Speed));
					task.addTask(new TaskRotateToAngle(MathHelper.PiOver2));
                    m_StraightTask = task;

					//m_LastAngle = thisAngle;
					m_LastSpeed = Speed;
				}
			
				m_Shots[m_NextShot].Enabled = true;
				m_Shots[m_NextShot].Center = who.Center + m_Position;
				m_Shots[m_NextShot].Faction = who.Faction;
				m_Shots[m_NextShot].Task = m_StraightTask;

				m_Shots[m_NextShot].m_Ship = who;
			}
		}
	}
}
