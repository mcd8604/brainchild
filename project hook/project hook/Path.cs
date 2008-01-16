using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{

	public enum Paths
	{
		Follow,
		Line,
		Shot,
		Bother,
		Tether,
		Straight,
		TailAttack,
		TailAttach,
		TailBody,
		Seek
	}

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
			else if (m_Strategy == Paths.Tether)
			{
				m_Path = new TetherPath(p_Values);
			}
			else if (m_Strategy == Paths.Straight)
			{
				m_Path = new PathStraight(p_Values);
			}
			else if (m_Strategy == Paths.TailAttack)
			{
				m_Path = new TailAttackPath(p_Values);
			}
			else if (m_Strategy == Paths.TailAttach)
			{
				m_Path = new TailAttachPath(p_Values);
			}
			else if (m_Strategy == Paths.TailBody)
			{
				m_Path = new TailBodyPath(p_Values);
			}
			else if (m_Strategy == Paths.Seek)
			{
				m_Path = new PathSeek(p_Values);
			}
			//  return m_Path;
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
