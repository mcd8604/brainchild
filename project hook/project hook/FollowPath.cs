using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class FollowPath:PathStrategy
	{
        Sprite m_Base; 
        Sprite m_Target;

		public FollowPath(Dictionary<ValueKeys, Object> p_Values)
			:base(p_Values){
                 m_Base = (Sprite)m_Values[ValueKeys.Base];
                 m_Target = (Sprite)m_Values[ValueKeys.Target];
		}

        public override void CalculateMovement(GameTime p_gameTime)
		{
			Vector2 basePos = m_Base.Center;
			basePos.X = m_Target.Center.X;
			basePos.Y = m_Target.Center.Y;
			m_Base.Center = basePos;
		
		}
	}
}
