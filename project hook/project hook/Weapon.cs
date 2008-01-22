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
		protected Ship m_Ship;
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
		protected int m_Strength;
		public int Strength
		{
			get
			{
				return m_Strength;
			}
			set
			{
				m_Strength = value;
			}
		}

		//this is how long the delay is between shots in millisecnds
		protected int m_Delay;
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
		protected float m_Speed;
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
		protected GameTexture m_Shot;
		public GameTexture Shot
		{
			get
			{
				return m_Shot;
			}
			set
			{
				m_Shot = value;
			}
		}

		//this is used to keep track of how many shots have been fired by this weapon
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
		protected double m_LastShot = 0;
		public double LastShot
		{
			get
			{
				return m_LastShot;
			}
			set
			{
				m_LastShot = value;
			}
		}

		//the angle that the shot is to be fired at
		private float m_Angle;
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
		#endregion // End of variables and Properties Region

		public Weapon(Ship p_Ship, int p_Strength, int p_Delay, float p_Speed, GameTexture p_Shot, float p_Angle)
		{
			Ship = p_Ship;
			Strength = p_Strength;
			Delay = p_Delay;
			Speed = p_Speed;
			Shot = p_Shot;
			Angle = p_Angle;
		}

		//this function will create a Shot at the current location
		// Only a single shot? 
		public virtual Sprite CreateShot()
		{
			if (m_LastShot < m_Delay)
			{
				return null;
			}

			Shot t_Shot = new Shot(m_Ship.Name + m_ShotNumber++, Ship.Center, 30, 75, m_Shot, 255f, true,
								  m_Angle, Depth.MidGround.Top, Ship.Faction, -1, null, 50f, null, 10, 10);

			//adds all the stuff that was in Game1
			//i just moved it over here.
			t_Shot.setAnimation("FireBall", 10);

			Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.Base, t_Shot);
			dic.Add(PathStrategy.ValueKeys.Speed, m_Speed);
			dic.Add(PathStrategy.ValueKeys.Angle, m_Angle);
			dic.Add(PathStrategy.ValueKeys.Rotation, true);
			t_Shot.Path = new Path(Paths.Straight, dic);

			t_Shot.Animation.StartAnimation();

			//gets the current time in milliseconds
			m_LastShot = 0;

			//Console.WriteLine(m_ShotNumber);

			return t_Shot;
		}

		public void Update(GameTime p_Time)
		{
			m_LastShot += p_Time.ElapsedGameTime.TotalMilliseconds;
		}
	}
}