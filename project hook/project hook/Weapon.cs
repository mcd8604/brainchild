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

		// this is how long the delay is between shots in seconds
		protected float m_Delay = 100;
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

		// this will hold the time of the last shot
		protected float m_Cooldown = 0;
		public virtual float Cooldown
		{
			get
			{
				return m_Cooldown;
			}
			set { m_Cooldown += value; }
		}

		// this is how fast the shot will travel, pixels per second
		protected float m_Speed = 1000;
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

		public Shot ShotType
		{
			set
			{
				m_Shots = new List<Shot>();
				for (int i = 0; i < (int)Math.Ceiling((((Math.Sqrt(Math.Pow(Game.graphics.GraphicsDevice.Viewport.Height, 2) + Math.Pow(Game.graphics.GraphicsDevice.Viewport.Width, 2))) / m_Speed) / m_Delay)); i++)
				{
					m_Shots.Add(new Shot(value));
				}
			}
		}

		protected IList<Shot> m_Shots;
		protected int m_NextShot = 0;

		#endregion // End of variables and Properties Region

		public Weapon() {}

		public Weapon(Shot p_Shot, float p_Delay, float p_Speed)
		{
			Delay = p_Delay;
			Speed = p_Speed;
			ShotType = p_Shot;
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