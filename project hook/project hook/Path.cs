using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{

    public enum Paths
    {
        Bother,
        Follow,
        Line,
        Seek,
        Shot,
        Straight,
        TailAttach,
        TailAttack,
        TailBody,
        Tether,
        Throw
    }

    public class Path
    {
        Paths m_Strategy;
        PathStrategy m_Path;

        public Path(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values)
        {
            m_Strategy = p_Strategy;

            switch (m_Strategy)
            {
                case Paths.Bother:
                    m_Path = new PathBother(p_Values);
                    break;
                case Paths.Follow:
                    m_Path = new PathFollow(p_Values);
                    break;
                case Paths.Line:
                    m_Path = new PathLine(p_Values);
                    break;
                case Paths.Seek:
                    m_Path = new PathSeek(p_Values);
                    break;
                case Paths.Shot:
                    m_Path = new PathShot(p_Values);
                    break;
                case Paths.Straight:
                    m_Path = new PathStraight(p_Values);
                    break;
                case Paths.TailAttach:
                    m_Path = new PathTailAttach(p_Values);
                    break;
                case Paths.TailAttack:
                    m_Path = new PathTailAttack(p_Values);
                    break;
                case Paths.TailBody:
                    m_Path = new PathTailBody(p_Values);
                    break;
                case Paths.Tether:
                    m_Path = new PathTether(p_Values);
                    break;
                case Paths.Throw:
                    m_Path = new PathThrow(p_Values);
                    break;
            }
        }

        public void CalculateMovement(GameTime p_gameTime)
        {
            m_Path.CalculateMovement(p_gameTime);
        }

        public bool isDone()
        {
            return m_Path.isDone;
        }

        public void Set()
        {
            m_Path.Set();
        }
    }
}
