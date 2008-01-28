using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class WeaponStraight : Weapon
	{

		private float m_LastAngle = float.NaN;

		// the angle that the shot is to be fired at
		protected float m_Angle = 0;
		public virtual float Angle
		{
			get
			{
				return m_Angle;
			}
			set
			{
				m_Angle = value;
			}
		}
		public virtual float AngleDegrees
		{
			get
			{
				return MathHelper.ToDegrees(Angle);
			}
			set
			{
				Angle = MathHelper.ToRadians(value);
			}
		}

		public Vector2 m_Position = Vector2.Zero;

		Task m_ShotTask;

		public WeaponStraight() { }

		public WeaponStraight(Shot p_Shot, float p_Delay, float p_Speed, float p_Angle)
			: base(p_Shot, p_Delay, p_Speed)
		{
			Angle = p_Angle;
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
					task.addTask(new TaskRotateToAngle(thisAngle));
					m_ShotTask = task;

					m_LastAngle = thisAngle;
					m_LastSpeed = Speed;
				}

				m_Shots[m_NextShot].Enabled = true;
				m_Shots[m_NextShot].Center = who.Center + m_Position;
				m_Shots[m_NextShot].Faction = who.Faction;
				m_Shots[m_NextShot].Task = m_ShotTask;

				base.CreateShot(who);
			}
		}

	}
}
