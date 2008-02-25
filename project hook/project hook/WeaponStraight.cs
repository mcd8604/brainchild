using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class WeaponStraight : Weapon
	{

		private float m_LastAngle = float.NaN;

		// the angle that the shot is to be fired at
		protected float m_Angle = 0;
		internal float Angle
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
		internal float AngleDegrees
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

		internal Vector2 m_Position = Vector2.Zero;

		private Task m_ShotTask;

		internal WeaponStraight() { }
		internal WeaponStraight(Ship p_Ship) : base(p_Ship) { }

		internal WeaponStraight(Ship p_Ship, Shot p_Shot, float p_Delay, float p_Speed, float p_Angle)
			: base(p_Ship, p_Shot, p_Delay, p_Speed)
		{
			Angle = p_Angle;
		}

		internal override void Fire(Ship who)
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

			m_Shots[m_NextShot].m_Ship = who;
		}

	}
}
