using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	/*
	* Description: 
	*              
	* 
	* TODO:
	*  1. be able to fire a shot
	*  
	*/
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
		protected int m_Speed;
		public int Speed
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
		protected int m_ShotNumber;
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
		private float m_Degree;
		public float Degree
		{
			get
			{
				return m_Degree;
			}
			set
			{
				m_Degree = value;
			}
		}
		#endregion // End of variables and Properties Region

		public Weapon(Ship p_Ship, int p_Strength, int p_Delay, int p_Speed, GameTexture p_Shot, float p_Degree)
		{
			Ship = p_Ship;
			Strength = p_Strength;
			Delay = p_Delay;
			Speed = p_Speed;
			Shot = p_Shot;
			Degree = p_Degree;

			ShotNumber = 0;
		}

		//this function will creat a Shot at the current location
		public virtual Sprite CreateShot(GameTime p_GameTime, Vector2 p_Position)
		{
			if (p_GameTime.TotalGameTime.TotalMilliseconds < m_LastShot + m_Delay)
			{
				return null;
			}

			Shot t_Shot = new Shot(m_Ship.Name + m_ShotNumber, p_Position, 75, 30, m_Shot, 255f, true,
								  m_Degree, Depth.MidGround.Top, Ship.Faction, -1, null, 50, null, 10, 10);

			//adds all the stuff that was in Game1
			//i just moved it over here.
			t_Shot.setAnimation("FireBall", 10);

			Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.Base, t_Shot);
			dic.Add(PathStrategy.ValueKeys.Start, p_Position);
			dic.Add(PathStrategy.ValueKeys.Speed, m_Speed);
			dic.Add(PathStrategy.ValueKeys.Degree, m_Degree);
			t_Shot.Path = new Path(Paths.Shot, dic);

			t_Shot.Animation.StartAnimation();

			//gets the current time in milliseconds
			m_LastShot = p_GameTime.TotalGameTime.TotalMilliseconds;
			++m_ShotNumber;

			Console.WriteLine(m_ShotNumber);

			return t_Shot;
		}
	}
}