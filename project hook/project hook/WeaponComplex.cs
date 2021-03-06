using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class WeaponComplex : Weapon
	{

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

		// the angle that the shot is to be fired at
		protected float m_Offset = 0;
		internal float Offset
		{
			get
			{
				return m_Offset;
			}
			set
			{
				m_Offset = value;
			}
		}
		internal float OffsetDegrees
		{
			get
			{
				return MathHelper.ToDegrees(Offset);
			}
			set
			{
				Offset = MathHelper.ToRadians(value);
			}
		}

		private Task m_ShotTask;
		internal Task ShotTask
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

		internal WeaponComplex() { }
		internal WeaponComplex(Ship p_Ship) : base(p_Ship) { }

		internal WeaponComplex(Ship p_Ship, Shot p_Shot, float p_Delay, float p_Speed, Task p_ShotTask)
			: base(p_Ship, p_Shot, p_Delay, p_Speed)
		{
			ShotTask = p_ShotTask;
		}

		internal override void Fire(Ship who)
		{
			if (m_Shots[m_NextShot].Task == null)
			{
				m_Shots[m_NextShot].Task = m_ShotTask.copy();
			}
			else
			{
				m_Shots[m_NextShot].Task.reset();
			}

			ChangeWeaponTaskAngle(m_Shots[m_NextShot].Task, (who.Rotation + Offset));
			ChangeWeaponTaskTarget(m_Shots[m_NextShot].Task, m_Target);

			m_Shots[m_NextShot].Enabled = true;
			m_Shots[m_NextShot].Center = who.Center;
			m_Shots[m_NextShot].Faction = who.Faction;

			m_Shots[m_NextShot].m_Ship = who;
		}
	}
}
