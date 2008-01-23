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
			}
		}

		private int m_Delay;
		private int m_CurTime;

		public SpawnPoint(int count, int delay,String p_Name, Vector2 p_Position, int p_Height, 
						  int p_Width, GameTexture p_Texture, float p_Alpha,Boolean p_Visible, float p_Rotation, 
						  float p_zBuff, Factions p_Faction, int p_Health, 
						  GameTexture p_DamageEffect, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Rotation, p_zBuff, p_Faction, p_Health, p_DamageEffect, p_Radius)
		{
			m_Spawned = new List<Ship>(count);
			Count = count;
			m_Delay = delay;
			m_CurTime = 0;
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
						s.Center = this.Center;
						s.Enabled = true;

						PathGroup group2b = new PathGroup();

						

						Dictionary<PathStrategy.ValueKeys, Object> dic2b = new Dictionary<PathStrategy.ValueKeys, object>();
						dic2b.Add(PathStrategy.ValueKeys.Base, s);
						

						if (Target == null)
						{
							dic2b.Add(PathStrategy.ValueKeys.Velocity, new Vector2(0,100));
						}
						else
						{
							dic2b.Add(PathStrategy.ValueKeys.Rotation, true);
							dic2b.Add(PathStrategy.ValueKeys.Speed, 100f);
							dic2b.Add(PathStrategy.ValueKeys.End, Target.Center);
						}
						
						group2b.AddPath(new Path(Paths.Straight, dic2b));

						s.PathList.AddPath(group2b);
						s.setAnimation("bloodcell", 60);
						s.Animation.StartAnimation();
						this.addSprite(s);
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
