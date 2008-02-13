using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PowerUp : Collidable
	{
		protected Random rand = new Random();

		protected int m_Amount;
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

		public enum PowerType
		{
			Weapon = 1,
			Health = 2,
			Shield = 3
		}

		protected PowerType m_Type;
		public PowerType Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				m_Type = value;

				if (m_Type == PowerType.Weapon)
				{
					Texture = TextureLibrary.getGameTexture("DNA", "");
				}
				else if (m_Type == PowerType.Health)
				{
					Texture = TextureLibrary.getGameTexture("cross", "");
				}
				else if (m_Type == PowerType.Shield)
				{
					Texture = TextureLibrary.getGameTexture("Shield", "");
				}
			}
		}

		protected int m_BackCount = 4;

		protected Sprite[] m_Back;

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
			Z += 0.0001f;
			Name = "Power Up " + Texture.Name;
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
				m_Back[a].Z = Depth.GameLayer.TailBody;

				attachSpritePart(m_Back[a]);

				m_Back[a].Animation.CurrentFrame = a * 5;
				m_Back[a].Animation.StartAnimation();

			}

			Center = at;
			randomType();
			Faction = Factions.PowerUp;
			Height = size;
			Width = size;
			Radius = size * 0.5f;
			Amount = value;
			Task = new TaskStationary();
			Damage = 0;
			Health = float.NaN;
			Z = Depth.GameLayer.PlayerShip;
			Alpha = 200;
			Name = "Power Up " + Texture.Name;
		}

		public PowerUp(int size, int value, PowerType p_type, Vector2 at)
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

			Center = at;
			Faction = Factions.PowerUp;
			Type = p_type;
			Height = size;
			Width = size;
			Radius = size * 0.5f;
			Amount = value;
			Task = new TaskStationary();
			Damage = 0;
			Health = float.NaN;
			Z = Depth.GameLayer.PlayerShip;
			Alpha = 200;
			Name = "Power Up " + Texture.Name;
		}


		void randomType()
		{
			int val = rand.Next(100) + 1;
			PowerType pt = PowerType.Weapon;

			if (val <= 50)
			{
				if (val >= 0 && val <= 30)
				{
					pt = PowerType.Weapon;
				}
				else if (val >= 31 && val <= 40)
				{
					pt = PowerType.Health;
				}
				else if (val >= 41 && val <= 50)
				{
					pt = PowerType.Shield;
				}
			}
			else
			{
				m_Enabled = false;
				m_ToBeRemoved = true;
			}
			//Type = (PowerType)val acted weird;
			Type = pt;
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
					ToBeRemoved = true;
				}
			}
		}
	}
}
