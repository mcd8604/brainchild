using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	[Obsolete]
	class PathTailAttack : PathStrategy
	{
		Tail m_Base;

		Path m_AttackPath;
		Path m_ReturnPath;

		public PathTailAttack(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			m_Base = (Tail)m_Values[ValueKeys.Base];
			float m_Speed = (float)m_Values[ValueKeys.Speed];
			PlayerShip m_PlayerShip = (PlayerShip)m_Values[ValueKeys.Target];
			Vector2 m_End = (Vector2)m_Values[ValueKeys.End];

			Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.Base, m_Base);
			dic.Add(PathStrategy.ValueKeys.Speed, m_Speed);
			dic.Add(PathStrategy.ValueKeys.End, m_End);
			dic.Add(PathStrategy.ValueKeys.Duration, (float)m_Values[ValueKeys.Duration]);
			m_AttackPath = new Path(Paths.Seek, dic);

			Dictionary<PathStrategy.ValueKeys, object> dic2 = new Dictionary<PathStrategy.ValueKeys, object>();
			dic2.Add(PathStrategy.ValueKeys.Base, m_Base);
			dic2.Add(PathStrategy.ValueKeys.Speed, m_Speed / 2.0f);
			dic2.Add(PathStrategy.ValueKeys.Target, m_PlayerShip);
			m_ReturnPath = new Path(Paths.Seek, dic2);

			m_Base.StateOfTail = Tail.TailState.Attacking;
			m_AttackPath.Set();
		}

		public override void CalculateMovement(GameTime p_GameTime)
		{

			if (m_Base.StateOfTail == Tail.TailState.Returning)
			{

				m_ReturnPath.CalculateMovement(p_GameTime);

				// If the tail is returning, reverse the direction it's pointing
				// Opposite the direction of travel, i.e. away from the ship
				m_Base.Rotation += (float)Math.PI;

				if (m_ReturnPath.isDone())
				{
					m_Base.TailReturned();
				}
			}

			if (m_Base.StateOfTail == Tail.TailState.Attacking)
			{

				m_AttackPath.CalculateMovement(p_GameTime);

				if (m_AttackPath.isDone())
				{
					m_ReturnPath.Set();
					m_Base.StateOfTail = Tail.TailState.Returning;
				}
			}

		}
	}
}