using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PathTailAttach : PathStrategy
	{

		Tail m_Tail;
		Collidable m_Enemy;

		public PathTailAttach(Dictionary<ValueKeys, Object> p_Values)
			:base(p_Values)
		{
			m_Tail = (Tail)m_Values[ValueKeys.Base];
			m_Enemy = (Collidable)m_Values[ValueKeys.Target];
		}

        public override void CalculateMovement(GameTime p_GameTime)
        {
			m_Enemy.Center = m_Tail.Center;
			m_Enemy.Rotation = m_Tail.Rotation;
        }
	}
}
