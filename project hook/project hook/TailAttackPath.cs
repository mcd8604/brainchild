using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class TailAttackPath : PathStrategy
    {
        Path m_CurrentPath;
        Vector2 m_Start;
        Vector2 m_End;
        Tail m_Base;
        PlayerShip m_PlayerShip;
        float m_Speed;
        bool changed = false;

        public TailAttackPath(Dictionary<ValueKeys, Object> p_Values)
            : base(p_Values)
        {
            m_Start = (Vector2)m_Values[ValueKeys.Start];
            m_End = (Vector2)m_Values[ValueKeys.End];
            m_Base = (Tail)m_Values[ValueKeys.Base];
            m_Speed = (float)m_Values[ValueKeys.Speed];
            m_PlayerShip = (PlayerShip)m_Values[ValueKeys.Target];
            Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
            dic.Add(PathStrategy.ValueKeys.End, (Vector2)m_Values[ValueKeys.End]);
            dic.Add(PathStrategy.ValueKeys.Start, (Vector2)m_Values[ValueKeys.Start]);
            dic.Add(PathStrategy.ValueKeys.Base, (Collidable)m_Values[ValueKeys.Base]);
            dic.Add(PathStrategy.ValueKeys.Speed, (float)m_Values[ValueKeys.Speed]);
            m_CurrentPath = new Path(Path.Paths.Shot, dic);
        }

        public override void CalculateMovement(GameTime p_GameTime)
        {
            m_CurrentPath.CalculateMovement(p_GameTime);
            m_Base.Degree = TrigHelper.TurnToFace(m_Base.Center, m_End, m_Base.Degree, 10);
            if (MathHelper.Distance(m_Base.Center.X, m_End.X) < 10 && MathHelper.Distance(m_Base.Center.Y, m_End.Y) < 10 && !changed)
            {
                Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
                dic.Add(PathStrategy.ValueKeys.End, m_PlayerShip.Center);
                dic.Add(PathStrategy.ValueKeys.Start, m_End);
                dic.Add(PathStrategy.ValueKeys.Base, m_Base);
                dic.Add(PathStrategy.ValueKeys.Speed, (m_Speed * 2.5f));
                m_CurrentPath = new Path(Path.Paths.Shot, dic);
                changed = true;
            }
            if (MathHelper.Distance(m_Base.Center.X, m_PlayerShip.Center.X) < 75 && MathHelper.Distance(m_Base.Center.Y, m_PlayerShip.Center.Y) < 75 && changed)
            {
                m_Base.TailReturned();
            }
        }
    }
}