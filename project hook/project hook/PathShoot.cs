using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PathShoot : PathStrategy
	{
		Ship m_Base;

		public PathShoot(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			m_Base = (Ship)m_Values[ValueKeys.Base];
			m_Done = true;
		}

		public override void CalculateMovement(GameTime p_gameTime)
		{
			m_Base.shoot();
		}
	}
}
