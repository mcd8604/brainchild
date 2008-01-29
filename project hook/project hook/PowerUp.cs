using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PowerUp:Collidable
	{
		private int m_Amount;
		public int Amount
		{
			get
			{
				return m_Amount;
			}
			set
			{
				m_Amount = value;
			}
		}

		public override int Width
		{
			get
			{
				return m_Width;
			}
			set
			{
				for (int a = 0; a < m_BackCount; a++)
				{
					m_Back[a].Width = value;
				}
				m_Width = value / 2;
			}

		}

		public override int Height
		{
			get
			{
				return m_Height;
			}
			set
			{
				for (int a = 0; a < m_BackCount; a++)
				{
					m_Back[a].Height = value;
				}
				m_Height = value / 2;
			}
		}

		public override Vector2 Center
		{
			get
			{
				return base.Center;
			}
			set
			{
				for (int a = 0; a < m_BackCount; a++)
				{
					m_Back[a].Center = value;
				}
				base.Center = value;
			}

		}

		private int m_BackCount = 4;

		private Sprite[] m_Back;



		public PowerUp(Collidable p_Base, WorldPosition p_Pos  )
			: base()
		{
			m_Back = new Sprite[m_BackCount];

			for (int a = 0; a < m_BackCount; a++)
			{
				m_Back[a] = new Sprite();
				m_Back[a].setAnimation("energyball", 30);
				m_Back[a].BlendMode = Microsoft.Xna.Framework.Graphics.SpriteBlendMode.Additive;

				m_Back[a].Task = new TaskAttach(this);
				m_Back[a].Alpha = 65;

				attachSpritePart(m_Back[a]);

				m_Back[a].Animation.CurrentFrame = a*5 ;
				m_Back[a].Animation.StartAnimation();
				
			}
			


			this.Center = p_Base.Center;
			this.Faction = Factions.PowerUp;
			this.Texture = TextureLibrary.getGameTexture("DNA", "");
			this.Radius = p_Base.Radius / 2;
			Amount = (int)this.Radius;
			TaskStraightVelocity straightVelocity = new TaskStraightVelocity();
			Vector2 v = new Vector2(0, p_Pos.Speed);
			straightVelocity.Velocity = v;
			this.Task = straightVelocity;

			this.Damage = 0;

		}


		public PowerUp()
			:base()
		{
			
		TaskStraightVelocity straightVelocity = new TaskStraightVelocity();
					 Vector2 v = new Vector2(0,0);
					straightVelocity.Velocity = v;
					this.Task = straightVelocity;
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
		{
			base.Draw(p_SpriteBatch);
		}

		public override void RegisterCollision(Collidable p_Other)
		{

			if (p_Other.Faction == Factions.Player)
			{
				if (p_Other is PlayerShip)
				{
					this.m_Enabled = false;
				}
			}
		}
	}
}
