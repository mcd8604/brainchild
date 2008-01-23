using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{

	// TODO: Extract Weapon out as a superclass, of which this is a subclass, say, exampleweapon

	public class Weapon
	{
		#region Variables and Properties
		//this is a ref to what ship this weapon is a psrt of.
		protected Ship m_Ship = null;
		public Ship Ship
		{
			get
			{
				return m_Ship;
			}
			set
			{
				m_Ship = value;
			}
		}

		//the strength of the shot fired
		protected int m_Damage = 0;
		public int Damage
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

		//this is how long the delay is between shots in millisecnds
		protected int m_Delay = 1000;
		public int Delay
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

		//this is how fast the shot will travel
		protected float m_Speed = 100;
		public float Speed
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
		public string ShotName
		{
			get
			{
				return m_ShotName;
			}
			set
			{
				m_ShotName = value;
			}
		}

		//this is used to keep track of how many shots have been fired by this weapon
		// Why?
		protected int m_ShotNumber = 0;
		public int ShotNumber
		{
			get
			{
				return m_ShotNumber;
			}
			set
			{
				m_ShotNumber = value;
			}
		}

		//this will hold the time of the last shot
		protected double m_Cooldown = 0;
		public double LastShot
		{
			get
			{
				return m_Cooldown;
			}
			set
			{
				m_Cooldown = value;
			}
		}

		//the angle that the shot is to be fired at
		protected float m_Angle = 0;
		public float Angle
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

		protected GameTexture m_Texture = null;

		#endregion // End of variables and Properties Region

		public Weapon(Ship p_Ship, string p_ShotName, int p_Damage, int p_Delay, float p_Speed, float p_Angle)
		{
			m_Ship = p_Ship;
			m_ShotName = p_ShotName;
			m_Damage = p_Damage;
			m_Delay = p_Delay;
			m_Speed = p_Speed;
			m_Angle = p_Angle;
			m_Texture = TextureLibrary.getGameTexture(m_ShotName, "");
		}

		//this function will create a Shot at the current location
		// Only a single shot? 
		public virtual Sprite CreateShot()
		{
			return null;
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
		}

		public virtual Sprite CreateShot(Vector2 target)
		{
			return null;
		}

		public void Update(GameTime p_Time)
		{
			m_Cooldown -= p_Time.ElapsedGameTime.TotalMilliseconds;
		}
	}
}