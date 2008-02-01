using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class SpawnPoint
	{
		private Sprite m_Target = null;
		public Sprite Target
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

		private Collidable m_SpawnObj;
		public Collidable SpawnObj
		{
			get
			{
				return m_SpawnObj;
			}
			set
			{
				m_SpriteInfo = value;
				//Ship p_Ship = (Ship)value;
				//for (int a = 0; a < Count; a++)
				//{
				//    m_Spawned.Add(new Ship(p_Ship.Name, p_Ship.Position, p_Ship.Height, p_Ship.Width, p_Ship.Texture, p_Ship.Alpha,
				//                         false, p_Ship.Rotation, p_Ship.Z, p_Ship.Faction, (int) (p_Ship.MaxHealth), (int)(p_Ship.MaxShield),
				//                          p_Ship.DamageEffect, p_Ship.Radius));
				//    m_Spawned[a].ToBeRemoved = true;
				//}
			}
		}
					
		private int m_Count = 0;
		public int Count
		{
			get
			{
				return m_Count;
			}

			set			
			{
			    m_Count = value;
                m_Spawned = new List<Ship>(m_Count);
			}
		}

		private int m_Delay;
        private int Delay
        {
            get
            {
                return m_Delay;
            }
            set
            {
                m_Delay = value;
            }

        }
		private int m_CurTime;

        public SpawnPoint(int p_Count, int p_Delay)
        {
            Count = p_Count;
            Delay = p_Delay;
            m_CurTime = 0;
        }

		public SpawnPoint(int count, int delay, Collidable p_SpawnObj)
		{			
			Count = count;
			m_Delay = delay;
			m_CurTime = 0;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			m_CurTime += p_Time.ElapsedGameTime.Milliseconds;
			if (m_CurTime >= m_Delay)
			{
				m_CurTime = 0;
				for (int a = 0; a < Count; a++)
				{
					if (m_Spawned[a].ToBeRemoved == true)
					{
						
						Ship s = m_Spawned[a];
						s.ToBeRemoved = false;
						s.Center = Center;
						s.Enabled = true;

						if (Target == null)
						{
							s.Task = new TaskStraightVelocity(new Vector2(0, 100));
						}
						else
						{
							s.Task = new TaskSeekPoint(Target.Center, 100);
						}
						
						s.setAnimation(SpawnAnimation, FPS);
						s.Animation.StartAnimation();
						addSprite(s);
						a = Count;
					}
				}
			}	
		}
	}
}
