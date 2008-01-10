using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class Weapon_SideShot : Weapon
	{

		public Weapon_SideShot(Ship p_Ship, int p_Strength, int p_Delay, int p_Speed, GameTexture p_Shot)
			: base(p_Ship, p_Strength, p_Delay, p_Speed, p_Shot)
		{

			m_Speed = p_Speed;

		}

		public override List<Shot> CreatShot(GameTime p_GameTime)
		{

			if (p_GameTime.TotalGameTime.TotalMilliseconds >= m_LastShot + m_Delay)
			{
				List<Shot> r_Shots = new List<Shot>();

				Shot t_Shot1 = new Shot(m_Ship.Name + m_ShotNumber, m_Ship.Position, 75, 30, m_Shot, 255f, true,
									  -1.50f, Depth.MidGround.Top, Collidable.Factions.Player, -1, null, 50, null, 10, 10);

				Vector2 shot = t_Shot1.Position;
				shot.X = m_Ship.Position.X - 50;
				shot.Y = m_Ship.Position.Y + 25;
				t_Shot1.Position = shot;

				//adds all the stuff that was in Game1
				//i just moved it over here.
				t_Shot1.setAnimation("FireBall", 10);


				Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
				dic.Add(PathStrategy.ValueKeys.Start, t_Shot1.Center);
				dic.Add(PathStrategy.ValueKeys.End, new Vector2(-100, t_Shot1.Center.Y));
				dic.Add(PathStrategy.ValueKeys.Base, t_Shot1);
				dic.Add(PathStrategy.ValueKeys.Speed, (float)500);
				t_Shot1.Path = new Path(Path.Paths.Shot, dic);

				t_Shot1.Animation.StartAnimation();

				//second shot
				Shot t_Shot2 = new Shot(m_Ship.Name + (m_ShotNumber+1), m_Ship.Center, 75, 30, m_Shot, 255f, true,
										1.50f, Depth.MidGround.Top, Collidable.Factions.Player, -1, null, 500, null, 10, 10);

				shot = t_Shot2.Position;
				shot.X = m_Ship.Position.X + 50;
				shot.Y = m_Ship.Position.Y + 25;
				t_Shot2.Position = shot;

				t_Shot2.setAnimation("FireBall", 10);


				dic = new Dictionary<PathStrategy.ValueKeys, object>();
				dic.Add(PathStrategy.ValueKeys.Start, t_Shot2.Center);
				dic.Add(PathStrategy.ValueKeys.End, new Vector2(800, t_Shot2.Center.Y));
				dic.Add(PathStrategy.ValueKeys.Base, t_Shot2);
				dic.Add(PathStrategy.ValueKeys.Speed, (float)500);
				t_Shot2.Path = new Path(Path.Paths.Shot, dic);

				t_Shot2.Animation.StartAnimation();


				//gets the current time in milliseconds
				m_LastShot = p_GameTime.TotalGameTime.TotalMilliseconds;
				++m_ShotNumber;
				r_Shots.Add(t_Shot1);
				r_Shots.Add(t_Shot2);
				return r_Shots;
			}
			else
			{
				List<Shot> t_Shots = new List<Shot>();
				Shot t_Shot = new Shot("no_Shot", m_Ship.Center, 0, 0, null, 0, false, 0, Depth.MidGround.Top, Collidable.Factions.Player,
										-1, null, 0, null, 0, 0);

				return t_Shots;

			}

		}
	}
}
