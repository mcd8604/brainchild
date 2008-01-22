using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PathGroup : Path
	{

		private List<Path> m_list = new List<Path>();
		// What's the pupose of this? It's not used? Who wrote it?
		private List<Path> List
		{
			get
			{
				return m_list;
			}
			set
			{
				m_list = value;
			}
		}

		public PathGroup() { }

		public void AddPath(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values)
		{
			AddPath(new Path(p_Strategy, p_Values));
		}

		public void AddPath(Path p_path)
		{
			m_list.Add(p_path);
		}

		public void AddPaths(List<Path> p_paths)
		{
			m_list.AddRange(p_paths);
		}

		public override void CalculateMovement(GameTime p_gameTime)
		{
			foreach (Path p in m_list)
			{
				p.CalculateMovement(p_gameTime);
			}
		}

		public override bool isDone()
		{
			return m_list.TrueForAll(Path.isDone);
		}

		public override void Set()
		{
			foreach (Path p in m_list)
			{
				p.Set();
			}
		}

	}
}
