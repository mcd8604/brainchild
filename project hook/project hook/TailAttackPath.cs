using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class TailAttackPath : PathStrategy
    {
        Path m_CurrentPath;
        Vector2 m_End;
        Tail m_Base;
        PlayerShip m_PlayerShip;
        float m_Speed;

        public TailAttackPath(Dictionary<ValueKeys, Object> p_Values)
            : base(p_Values)
        {
            m_Base = (Tail)m_Values[ValueKeys.Base];
            m_Speed = (float)m_Values[ValueKeys.Speed];
            m_PlayerShip = (PlayerShip)m_Values[ValueKeys.Target];
            m_End = (Vector2)m_Values[ValueKeys.End];
            Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
            dic.Add(PathStrategy.ValueKeys.Base, m_Base);
            dic.Add(PathStrategy.ValueKeys.Speed, m_Speed);
            dic.Add(PathStrategy.ValueKeys.End, m_End);
            dic.Add(PathStrategy.ValueKeys.Duration, (float)m_Values[ValueKeys.Duration]);
            m_CurrentPath = new Path(Paths.Seek, dic);
			m_Base.StateOfTail = Tail.TailState.Attacking;
        }

        public override void CalculateMovement(GameTime p_GameTime)
        {
            m_CurrentPath.CalculateMovement(p_GameTime);

            if (m_Base.StateOfTail == Tail.TailState.Returning)
            {
                // If the tail is returning, reverse the direction it's pointing
                // Opposite the direction of travel, i.e. away from the ship
                m_Base.Rotation += (float)Math.PI;

                if (m_CurrentPath.isDone())
                {
                    m_Base.TailReturned();
                }
            }

            if (m_Base.StateOfTail == Tail.TailState.Attacking && m_CurrentPath.isDone())
            {
                Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
                dic.Add(PathStrategy.ValueKeys.Base, m_Base);
                dic.Add(PathStrategy.ValueKeys.Speed, m_Speed / 2.0f);
                dic.Add(PathStrategy.ValueKeys.Target, m_PlayerShip);
                m_CurrentPath = new Path(Paths.Seek, dic);
                m_Base.StateOfTail = Tail.TailState.Returning;
            }

        }
    }
}