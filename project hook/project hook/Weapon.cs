using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{

	public abstract class Weapon
	{
		#region Variables and Properties

		//the strength of the shot fired
		protected float m_Damage = 0;
		public virtual float Damage
		{
			get
			{
				return m_Damage;
			}
			set
			{
				m_Damage = value;
			}
		}

		//this is how long the delay is between shots in seconds
		protected float m_Delay = 10;
		public virtual float Delay
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

		//this is how fast the shot will travel, pixels per second
		protected float m_Speed = 0;
		public virtual float Speed
		{
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
			}
		}

		//this is the texture of this weapon's shot
		protected string m_ShotName = null;
		public virtual string ShotName
		{
			get
			{
				return m_ShotName;
			}
			set
			{
				m_ShotName = value;
				m_Texture = TextureLibrary.getGameTexture(m_ShotName, "");

				if (m_Shots == null)
				{
					m_Shots = new List<Shot>();
					for (int i = 0; i < 10; i++)
					{
						Shot t_Shot = new Shot("WeaponExampleShot", Vector2.Zero, 30, 75, m_Texture, 1, false,
										  0f, Depth.GameLayer.Shot, Collidable.Factions.None, -1, null, 15, 10);
						t_Shot.Bound = Collidable.Boundings.Diamond;
						m_Shots.Add(t_Shot);
					}
				}
			}
		}

		//this will hold the time of the last shot
		protected float m_Cooldown = 0;
		public virtual float Cooldown
		{
			get
			{
				return m_Cooldown;
			}
		}

		//the angle that the shot is to be fired at
		protected float m_Angle = 0;
		public virtual float Angle
		{
			get
			{
				return m_Angle;
			}
			set
			{
				m_Angle = value;
			}
		}

		public virtual float AngleDegrees
		{
			get
			{
				return MathHelper.ToDegrees(Angle);
			}
			set
			{
				Angle = MathHelper.ToRadians(value);
			}
		}

		protected GameTexture m_Texture = null;

		protected IList<Shot> m_Shots;
		protected int m_NextShot = 0;

		#endregion // End of variables and Properties Region

		public Weapon() {}

		public Weapon(string p_ShotName, float p_Damage, float p_Delay, float p_Speed, float p_Angle)
		{
			m_ShotName = p_ShotName;
			m_Damage = p_Damage;
			m_Delay = p_Delay;
			m_Speed = p_Speed;
			m_Angle = p_Angle;

			m_Shots = new List<Shot>();
			for (int i = 0; i < (int)Math.Ceiling((((Math.Sqrt(Math.Pow( Game.graphics.GraphicsDevice.Viewport.Height, 2) + Math.Pow(Game.graphics.GraphicsDevice.Viewport.Width, 2))) / m_Speed) / m_Delay)); i++)
			{
				Shot t_Shot = new Shot("WeaponShot", Vector2.Zero, 30, 75, m_Texture, 1, false,
								  0f, Depth.GameLayer.Shot, Collidable.Factions.None, -1, null, 15, m_Damage);
				t_Shot.Bound = Collidable.Boundings.Diamond;
				m_Shots.Add(t_Shot);
			}
		}

		//this function will create a Shot at the current location
		// Only a single shot? 
		public abstract void CreateShot( Ship who );

		public void Update(GameTime p_Time)
		{
			m_Cooldown -= (float)p_Time.ElapsedGameTime.TotalSeconds;
		}

		public IList<Shot> getShots()
		{
			return m_Shots;
		}

	}
}