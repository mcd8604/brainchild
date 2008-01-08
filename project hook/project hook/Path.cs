using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class Path
	{
		Paths m_Strategy;
		PathStrategy m_Path;

        public Path(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values)
		{
			m_Strategy = p_Strategy;

			if (m_Strategy == Paths.Follow)
			{
				m_Path = new FollowPath(p_Values);
			}
            else if (m_Strategy == Paths.Line)
            {
                m_Path = new PathLine(p_Values);
            }
            else if (m_Strategy == Paths.Shot)
            {
                m_Path = new ShotPath(p_Values);
            }
			else if (m_Strategy == Paths.Bother)
			{
				m_Path = new BotherPath(p_Values);
			}
          //  return m_Path;
		}

		public void CalculateMovement(GameTime p_gameTime){

            m_Path.CalculateMovement(p_gameTime);
		}

        public bool isDone()
        {
           return m_Path.isDone;
        }

		public enum Paths
		{
			Follow,
			Line,
            Shot,
			Bother

		}

	}
}
