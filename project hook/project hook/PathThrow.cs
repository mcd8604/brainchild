using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PathThrow : PathStrategy
	{
		Path m_CurrentPath;
		Vector2 m_Target;
		Sprite m_EnemyCaught;
		Tail m_Base;
		Vector2 m_Distance;
		Vector2 m_BackPos;

		public PathThrow(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			m_Base = (Tail)m_Values[ValueKeys.Base];
			m_Target = (Vector2)m_Values[ValueKeys.Target];
			m_EnemyCaught = (Sprite)m_Values[ValueKeys.End];

			m_Distance = Vector2.Subtract(m_Base.Center, m_Target);
			m_BackPos = Vector2.Add(m_Base.Center, Vector2.Divide(m_Distance, 2.75f));

			Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.Base, m_Base);
			dic.Add(PathStrategy.ValueKeys.End, m_BackPos);
			dic.Add(PathStrategy.ValueKeys.Speed, 2500f);
			m_CurrentPath = new Path(Paths.Seek, dic);
			m_Base.StateOfTail = Tail.TailState.Neutral;
		}

		public override void CalculateMovement(GameTime p_GameTime)
		{
			m_CurrentPath.CalculateMovement(p_GameTime);
			m_Base.Rotation = TrigHelper.TurnToFace(m_Base.Center, m_Target, m_Base.Rotation, 10);

			if (m_CurrentPath.isDone())
			{
				if (m_Base.StateOfTail != Tail.TailState.Returning)
				{
					Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
					dic.Add(PathStrategy.ValueKeys.Base, m_Base);
					dic.Add(PathStrategy.ValueKeys.End, Vector2.Add(m_Base.Center, Vector2.Multiply(m_Distance, -1)));
					dic.Add(PathStrategy.ValueKeys.Speed, 2500f);
					m_CurrentPath = new Path(Paths.Seek, dic);
					m_Base.StateOfTail = Tail.TailState.Returning;
				}
				else
				{
					Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
					dic.Add(PathStrategy.ValueKeys.Base, m_EnemyCaught);
					dic.Add(PathStrategy.ValueKeys.Speed, 1000f);
					dic.Add(PathStrategy.ValueKeys.End, m_Target);
					m_EnemyCaught.PathList = new PathList(Paths.Straight, dic, ListModes.Continuous);
					m_Base.TailReturned();
				}
			}
		}
	}
}