using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class FollowPath:PathStrategy
	{
		public FollowPath(Dictionary<String, Object> p_Values)
			:base(p_Values){

		}

		public override void CalculateMovement()
		{
			Sprite t_Base = (Sprite)m_Values["Base"];
			Sprite t_Target = (Sprite)m_Values["Target"];

			Vector2 basePos = t_Base.Center;
			basePos.X = t_Target.Center.X;
			basePos.Y = t_Target.Center.Y;
			t_Base.Center = basePos;
		
		}
	}
}
