using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class SpawnPoint:Collidable
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

		private Ship m_SpriteInfo;
		public Ship getSpriteInfo
		{
			get
			{
				return m_SpriteInfo;
			}
			set
			{
				m_SpriteInfo = value;
                Ship p_Ship = (Ship)value;
                for (int a = 0; a < Count; a++)
                {
                    m_Spawned.Add(new Ship(p_Ship.Name, p_Ship.Position, p_Ship.Height, p_Ship.Width, p_Ship.Texture, p_Ship.Alpha,
                                         false, p_Ship.Rotation, p_Ship.Z, p_Ship.Faction, (int) (p_Ship.MaxHealth), (int)(p_Ship.MaxShield),
                                          p_Ship.DamageEffect, p_Ship.Radius));
                    m_Spawned[a].ToBeRemoved = true;
                }
			}
		}

		private List<Ship> m_Spawned;
					

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

        private string m_SpawnAnimation;
        public string SpawnAnimation
        {
            get
            {
                return m_SpawnAnimation;
            }
            set
            {
                m_SpawnAnimation = value;
            }
        }
        private int m_FPS;
        public int FPS
        {
            get
            {
                return m_FPS;
            }
            set
            {
                m_FPS = value;
            }
        }

        public SpawnPoint(int p_Count, int p_Delay, string p_Animation, int p_Fps)
            :base()
        {
            Count = p_Count;
            Delay = p_Delay;
            m_CurTime = 0;
            base.Bound = Boundings.Square;
            SpawnAnimation = p_Animation;
            FPS = p_Fps;
        }

		public SpawnPoint(int count, int delay,String p_Name, Vector2 p_Position, int p_Height, 
						  int p_Width, GameTexture p_Texture, float p_Alpha,Boolean p_Visible, float p_Rotation, 
						  float p_zBuff, Factions p_Faction, int p_Health, 
						  GameTexture p_DamageEffect, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Rotation, p_zBuff, p_Faction, p_Health, p_DamageEffect, p_Radius)
		{
			
			Count = count;
			m_Delay = delay;
			m_CurTime = 0;
			base.Bound = Boundings.Square;
		}

		public void setShips(String p_Name, Vector2 p_Position, int p_Height,
						  int p_Width, GameTexture p_Texture, float p_Alpha, Boolean p_Visible, float p_Rotation,
						  float p_zBuff, Factions p_Faction, int p_Health, int p_Shield,
						  GameTexture p_DamageEffect, float p_Radius)
		{
			for (int a = 0; a < Count; a++)
			{
				m_Spawned.Add( new Ship(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha,
									 false, p_Rotation, p_zBuff, p_Faction, p_Health, p_Shield,
									  TextureLibrary.getGameTexture("Explosion", "3"), 50));
				m_Spawned[a].ToBeRemoved = true;
			}
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

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
		{
			base.Draw(p_SpriteBatch);
		}
	}
}
