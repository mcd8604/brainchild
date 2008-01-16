using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class PathStrategy
	{
      
         public enum ValueKeys
        {
            Start,   //The starting point, Vector2
            End,     //The Ending point 
            Degree,  //The Degree of rotation, float 
            Base,    //The that will recieve this information, Sprite 
            Target,  //The target sprite, Sprite
            Speed,   //The scalar speed of the movement 
            Velocity,//The vector speed of the movement
            Duration //The duration of the movement.
        }

        protected Dictionary<ValueKeys, Object> m_Values;
        public Dictionary<ValueKeys, Object> Values
		{
			get
			{
				return m_Values;
			}
			set
			{
				m_Values = value;
			}

		}

        protected bool m_Done = false;
        public bool isDone
        {
            get
            {
                return m_Done;
            }
        }

        public PathStrategy(Dictionary<ValueKeys, Object> p_Values)
		{
			m_Values = p_Values;
		}

		public virtual void CalculateMovement(GameTime p_gameTime){

		}
	}
}
