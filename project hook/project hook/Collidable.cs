using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	/*
	* Description: this class is a basic structure for all sprites that
	*              will be able to collide with other sprites.
	* 
	* TODO:
	*  1. check for collisions
	*  2. 
	*  
	*/
	class Collidable:Sprite
	{
		//which faction does this sprite belong to
		private int m_Faction;
		public int Faction
		{
			get
			{
				return m_Faction;
			}
			set
			{
				m_Faction = value;
			}
		}

		//this is how much health this sprite has
		private int m_Health;
		public int Health
		{
			get
			{
				return m_Health;
			}
			set
			{
				m_Health = value;
			}
		}

		//this is the path that the sprite will fallow
		private Path m_Path;
		public Path Path
		{
			get
			{
				return m_Path;
			}
			set
			{
				m_Path = value;
			}
		}

		//this is the speed of the sprite
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

		//this is what effect will be shown on the sprite when it takes damage
		private GameTexture m_DamageEffect;
		public GameTexture DamageEffect
		{
			get
			{
				return m_DamageEffect;
			}
			set
			{
				m_DamageEffect = value;
			}
		}

		//this is the radius used for collision detection
		private float m_Radius;
		public float Radius
		{
			get
			{
				return m_Radius;
			}
			set
			{
				m_Radius = value;
			}
		}
	}
}
