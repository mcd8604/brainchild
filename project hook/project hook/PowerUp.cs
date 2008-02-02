using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PowerUp : Collidable
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

		public PowerUp(int size, int value)
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

				m_Back[a].Animation.CurrentFrame = a * 5;
				m_Back[a].Animation.StartAnimation();

			}
			Enabled = false;
			Faction = Factions.PowerUp;
			Texture = TextureLibrary.getGameTexture("DNA", "");
			Height = size;
			Width = size;
			Radius = size * 0.5f;
			Amount = value;
			Task = new TaskStationary();
			Damage = 0;
			Health = float.NaN;

		}

		public PowerUp(int size, int value, Vector2 at)
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
			Center = at;
			Faction = Factions.PowerUp;
			Texture = TextureLibrary.getGameTexture("DNA", "");
			Height = size;
			Width = size;
			Radius = size * 0.5f;
			Amount = value;
			Task = new TaskStationary();
			Damage = 0;
			Health = float.NaN;

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
					Enabled = false;
				}
			}
		}
	}
}
