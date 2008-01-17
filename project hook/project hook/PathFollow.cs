using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class PathFollow : PathStrategy
	{
		Sprite m_Base;
		Sprite m_Target;

		public PathFollow(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			m_Base = (Sprite)m_Values[ValueKeys.Base];
			m_Target = (Sprite)m_Values[ValueKeys.Target];
		}

		public override void CalculateMovement(GameTime p_gameTime)
		{
			m_Base.Center = m_Target.Center;
		}
	}
}
