using System;
using System.Collections.Generic;
using System.Text;

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




		//this function will creat a Shot at the current location
		public void CreatShot()
		{
			
		}
	}
}
