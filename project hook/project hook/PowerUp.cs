using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class PowerUp : Collidable
	{
		private static readonly List<Sprite> m_PowerUps = new List<Sprite>();
		private static int nextPowerUp = 0;
		private const int MAX_POWERUPS = 20;
		private const int SIZE = 50;

		private const int RAND_HEALTH_PERCENT_CHANCE = 6;
		private const int RAND_SHIELD_PERCENT_CHANCE = 5;
		private const int RAND_WEAPON_PERCENT_CHANCE = 20;

		internal static List<Sprite> iniPowerups()
		{
			for (int a = 0; a < MAX_POWERUPS; a++)
			{
				m_PowerUps.Add(new PowerUp());
			}

			return m_PowerUps;
		}

		internal static void DisplayPowerUp(int size, int value, Vector2 at, PowerType power)
		{
			PowerUp p = m_PowerUps[nextPowerUp] as PowerUp;
			if (power == PowerType.Random)
			{
				p.Type = getRandomType();
			}
			else
			{
				p.Type = power;
			}

			if (p.Type != PowerType.None)
			{
				p.Center = at;
				//p.Height = SIZ;
				//p.Width = size;
				//p.Radius = size * 0.5f;
				p.Amount = value;
				p.Health = float.NaN;
				p.Alpha = 155;
#if !FINAL
				p.Name = "Power Up " + p.Texture.Name;
#endif
				p.Enabled = true;

				//World.m_World.AddSprite(p);
				nextPowerUp = (nextPowerUp + 1) % MAX_POWERUPS;
			}
		}


		protected int m_Amount;
		internal int Amount
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

		internal override int Width
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

		internal override int Height
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

		internal override Vector2 Center
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

		internal enum PowerType
		{
			Random = -1,
			None = 0,
			Weapon = 1,
			Health = 2,
			Shield = 3
		}

		protected PowerType m_Type;
		internal PowerType Type
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
					Texture = TextureLibrary.getGameTexture("DNA");
				}
				else if (m_Type == PowerType.Health)
				{
					Texture = TextureLibrary.getGameTexture("cross");
				}
				else if (m_Type == PowerType.Shield)
				{
					Texture = TextureLibrary.getGameTexture("Shield");
				}
			}
		}

		protected int m_BackCount = 4;

		protected Sprite[] m_Back;

		private PowerUp()
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
				m_Back[a].Z = Depth.GameLayer.Ships - 0.002f;
				m_Back[a].Animation.CurrentFrame = a * 5;
				m_Back[a].Animation.StartAnimation();

			}

			Enabled = false;
			Faction = Factions.PowerUp;
			Height = SIZE;
			Width = SIZE;
			Radius = SIZE * 0.5f;
			Amount = 0;
			Damage = 0;
			Health = float.NaN;
			Z = Depth.GameLayer.Ships - 0.02f;
			Task = new TaskStationary();
#if !FINAL
			Name = "Power Up - Not Assigned";
#endif
		}

		private PowerUp(int size, int value)
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
			Texture = TextureLibrary.getGameTexture("DNA");
			Height = size;
			Width = size;
			Radius = size * 0.5f;
			Amount = value;
			Task = new TaskStationary();
			Damage = 0;
			Health = float.NaN;
			Z += 0.0001f;
#if !FINAL
			Name = "Power Up " + Texture.Name;
#endif
		}

		private PowerUp(int size, int value, Vector2 at)
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
			Type = getRandomType();
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
#if !FINAL
			Name = "Power Up " + Texture.Name;
#endif
		}

		private PowerUp(int size, int value, PowerType p_type, Vector2 at)
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
#if !FINAL
			Name = "Power Up " + Texture.Name;
#endif
		}


		private static PowerType getRandomType()
		{
			int val = Game.Random.Next(100);

			if (val < RAND_WEAPON_PERCENT_CHANCE)
			{
				return PowerType.Weapon;
			}
			else if (val < RAND_WEAPON_PERCENT_CHANCE + RAND_SHIELD_PERCENT_CHANCE)
			{
				return PowerType.Shield;
			}
			else if (val < RAND_WEAPON_PERCENT_CHANCE + RAND_SHIELD_PERCENT_CHANCE + RAND_HEALTH_PERCENT_CHANCE)
			{
				return PowerType.Health;
			}
			else
			{
				return PowerType.None;
			}
		}

		internal override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
		{
			base.Draw(p_SpriteBatch);
		}
		internal override void RegisterCollision(Collidable p_Other)
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
