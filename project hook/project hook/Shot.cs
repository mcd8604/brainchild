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
	*  1. move across the screen
	*  
	*/
	class Shot:Collidable
	{
		//how much damage will be done by this shot
		private int m_Damage;
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



		
	}
}
