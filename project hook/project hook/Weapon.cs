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
	class Weapon
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

		//this is how long the delay is between shots
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
		public void CreatShot()
		{
			String t_Name=m_Ship.Name;
			Vector2 t_Position=m_Ship.Position;

			Shot t_Shot = new Shot(t_Name + m_ShotNumber, t_Position, 10, 10, m_Shot, 100f, true, 0f, 0.31f, 2);

			++m_ShotNumber;
		}
	}
}
