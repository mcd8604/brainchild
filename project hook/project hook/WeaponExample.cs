using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class WeaponExample : Weapon
	{

		public WeaponExample(Ship p_Ship, string p_ShotName, int p_Damage, int p_Delay, float p_Speed, float p_Angle)
			: base(p_Ship, p_ShotName, p_Damage, p_Delay, p_Speed, p_Angle)
		{
			TaskParallel task = new TaskParallel();
			task.addTask(new TaskStraightAngle(p_Angle, p_Speed));
			task.addTask(new TaskRotateAngle(p_Angle));

			m_Shots = new List<Shot>();
			// temp
			for (int i = 0; i <= (int)(2500f / m_Speed); i++)
			{
				Shot t_Shot = new Shot(m_Ship.Name + m_ShotNumber++, Vector2.Zero, 30, 75, m_Texture, 1, false,
								  m_Angle, Depth.MidGround.Top, m_Ship.Faction, -1, null, 15, 10);
				t_Shot.Bound = Collidable.Boundings.Diamond;
				t_Shot.Task = task;
				m_Shots.Add(t_Shot);
			}
		}

		public override void CreateShot()
		{
			if (m_Cooldown <= 0)
			{
				m_Shots[m_NextShot].Enabled = true;
				m_Shots[m_NextShot].Center = m_Ship.Center;

				m_Shots[m_NextShot].setAnimation(m_ShotName, 10);
				m_Shots[m_NextShot].Animation.StartAnimation();

				m_Cooldown = m_Delay;

				m_NextShot = (m_NextShot + 1) % m_Shots.Count;
			}
		}

		//public override void Update(GameTime p_Time)
		//{
		//    base.Update(p_Time);
		//    foreach (Shot s in m_Shots)
		//    {
		//        if (s.Enabled)
		//        {
		//            s.Update(p_Time);
		//        }
		//    }
		//}

		//public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
		//{
		//    foreach (Shot s in m_Shots)
		//    {
		//        if (s.Enabled)
		//        {
		//            s.Draw(p_SpriteBatch);
		//        }
		//    }
		//}

	}
}
