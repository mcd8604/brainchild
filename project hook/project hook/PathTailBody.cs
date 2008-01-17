using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PathTailBody : PathStrategy
	{

		Tail m_TailEnd;
		Collidable m_Ship;
		ArrayList m_BodySprites;
		int m_NumberOfBody;

		public PathTailBody(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			m_TailEnd = (Tail)m_Values[ValueKeys.Target];
			m_Ship = (Collidable)m_Values[ValueKeys.End];
			m_BodySprites = (ArrayList)m_Values[ValueKeys.Base];
			if (m_BodySprites == null || m_BodySprites.Count == 0)
			{
				throw new ArgumentException("PathTailBody Base must be an ArrayList of Sprites");
			}
			m_NumberOfBody = m_BodySprites.Count + 1;
		}

		public override void CalculateMovement(GameTime p_GameTime)
		{
			int i = 1;
			foreach (Sprite s in m_BodySprites)
			{
				s.Center = Vector2.Lerp(m_Ship.Center, m_TailEnd.Center, ((float)i++) / ((float)m_NumberOfBody));
			}
		}
	}
}
