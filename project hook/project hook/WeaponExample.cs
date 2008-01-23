using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class WeaponExample : Weapon
	{

		private Dictionary<PathStrategy.ValueKeys, Object> dic;

		public WeaponExample(Ship p_Ship, string p_ShotName, int p_Damage, int p_Delay, float p_Speed, float p_Angle)
			: base(p_Ship, p_ShotName, p_Damage, p_Delay, p_Speed, p_Angle)
		{
			dic = new Dictionary<PathStrategy.ValueKeys, object>();
		}

		public override Sprite CreateShot()
		{
			if (m_Cooldown > 0)
			{
				return null;
			}

			Shot t_Shot = new Shot(m_Ship.Name + m_ShotNumber++, m_Ship.Center, 30, 75, m_Texture, 255f, true,
								  m_Angle, Depth.MidGround.Top, m_Ship.Faction, -1, null, 15, 10);

			t_Shot.Bound = Collidable.Boundings.Diamond;

			t_Shot.setAnimation(m_ShotName, 10);
			t_Shot.Animation.StartAnimation();

			dic[PathStrategy.ValueKeys.Base] = t_Shot;
			dic[PathStrategy.ValueKeys.Speed] = m_Speed;
			dic[PathStrategy.ValueKeys.Angle] = m_Angle;
			dic[PathStrategy.ValueKeys.Rotation] = true;
			t_Shot.Path = new Path(Paths.Straight, dic);

			m_Cooldown = m_Delay;

			return t_Shot;
		}

	}
}
