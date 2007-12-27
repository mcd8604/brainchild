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
		private Ship m_Ship;
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
		private int m_Strength;
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
		private int m_Delay;
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
		private int m_Speed;
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
		private GameTexture m_Shot;
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
		private int m_ShotNumber;
		public int ShotNumber
		{
			get
			{
				return m_ShotNumber;
			}
			set
			{
				m_ShotNumber=value;
			}
		}

        //this will hold the time of the last shot
        private double m_LastShot = 0;
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
		#endregion // End of variables and Properties Region

		public Weapon(Ship p_Ship, int p_Strength, int p_Delay, int p_Speed, GameTexture p_Shot)
		{
			Ship = p_Ship;
			Strength = p_Strength;
			Delay = p_Delay;
			Speed = p_Speed;
			Shot = p_Shot;
			ShotNumber=0;
		}

		//this function will creat a Shot at the current location
		public Shot CreatShot(GameTime p_GameTime)
		{
            if (p_GameTime.TotalGameTime.TotalMilliseconds >= m_LastShot + m_Delay)
            {
                //creates a temp shot
                Shot t_Shot = new Shot(m_Ship.Name + m_ShotNumber, m_Ship.Center, 75, 30, m_Shot, 100, true,
                                        0, Depth.MidGround.Top, Collidable.Factions.Player, -1, null, 2, null, 5, 10);

                //adds all the stuff that was in Game1
                //i just moved it over here.
                t_Shot.setAnimation("RedShot", 10);

                Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
                dic.Add(PathStrategy.ValueKeys.Start, t_Shot.Center);
                dic.Add(PathStrategy.ValueKeys.End, new Vector2(t_Shot.Center.X, -100));
                dic.Add(PathStrategy.ValueKeys.Duration, -1.0f);
                dic.Add(PathStrategy.ValueKeys.Base, t_Shot);
                t_Shot.Path = new Path(Path.Paths.Shot, dic);

                t_Shot.Animation.StartAnimation();

                //gets the current time in milliseconds
                m_LastShot = p_GameTime.TotalGameTime.TotalMilliseconds;
                ++m_ShotNumber;
                return t_Shot;
            }
            else
            {
                Shot t_Shot = new Shot("no_Shot", m_Ship.Center, 0, 0, null, 0, false, 0, Depth.MidGround.Top, Collidable.Factions.Player,
                                        -1, null, 0, null, 0, 0);

                return t_Shot;
            }
		}
	}
}
