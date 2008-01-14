using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TailBodyPath:PathStrategy
	{

		Tail m_TailEnd;
		ArrayList m_BodySprites;
		Collidable m_Ship;
		int m_NumberOfBody;

		public TailBodyPath(Dictionary<ValueKeys, Object> p_Values)
			:base(p_Values)
		{
			m_TailEnd = (Tail)m_Values[ValueKeys.Target];
			m_BodySprites = (ArrayList)m_Values[ValueKeys.Base];
			m_Ship = (Collidable)m_Values[ValueKeys.End];
			if (m_BodySprites.Count > 0 && m_BodySprites != null)
				m_NumberOfBody = m_BodySprites.Count+1;
		}

        public override void CalculateMovement(GameTime p_GameTime)
        {
			Vector2 distance = new Vector2(m_TailEnd.Center.X - m_Ship.Center.X, m_TailEnd.Center.Y - m_Ship.Center.Y);
			Vector2 tick = new Vector2(distance.X / m_NumberOfBody, distance.Y / m_NumberOfBody);
			foreach (Sprite s in m_BodySprites)
			{
				int index = m_BodySprites.IndexOf(s);
				Vector2 temp = new Vector2((index + 1) * tick.X, (index + 1) * tick.Y);
				s.Center = temp;
			}
        }
	}
}
