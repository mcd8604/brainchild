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

		protected IList<Shot> m_Shots = new List<Shot>();
		protected int m_NextShot = 0;

		#endregion // End of variables and Properties Region

		public Weapon() { }

		public Weapon(string p_ShotName, float p_Damage, float p_Delay, float p_Speed, float p_Angle)
		{
			m_ShotName = p_ShotName;
			m_Damage = p_Damage;
			m_Delay = p_Delay;
			m_Speed = p_Speed;
			m_Angle = p_Angle;
		}

		//this function will create a Shot at the current location
		// Only a single shot? 
		public abstract void CreateShot( Ship who );
			//if (m_Cooldown > 0 )
			//{
			//    return null;
			//}

			//Shot t_Shot = new Shot(m_Ship.Name + m_ShotNumber++, m_Ship.Center, 30, 75, TextureLibrary.getGameTexture(m_ShotName, ""), 255f, true,
			//                      m_Angle, Depth.MidGround.Top, m_Ship.Faction, -1, null, 50f, null, 10, 10);

			////adds all the stuff that was in Game1
			////i just moved it over here.
			//t_Shot.setAnimation(m_ShotName, 10);

			//Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			//dic.Add(PathStrategy.ValueKeys.Base, t_Shot);
			//dic.Add(PathStrategy.ValueKeys.Speed, m_Speed);
			//dic.Add(PathStrategy.ValueKeys.Angle, m_Angle);
			//dic.Add(PathStrategy.ValueKeys.Rotation, true);
			//t_Shot.Path = new Path(Paths.Straight, dic);

			//t_Shot.Animation.StartAnimation();

			////gets the current time in milliseconds
			//m_Cooldown = m_Delay;

			////Console.WriteLine(m_ShotNumber);

			//return t_Shot;

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