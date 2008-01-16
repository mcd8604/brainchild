using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{

	public enum ListModes { Continuous, Once, Repeat, Random };

	public class PathList
	{

		List<Path> list = new List<Path>();
		int m_current = 0;
		bool m_done = false;
		ListModes m_mode = ListModes.Once;
		public ListModes Mode
		{
			get {
				return m_mode;
			}
			set {
				m_mode = value;
			}
		}

		public Path CurrentPath
		{
			get
			{
				if (m_current >= list.Count)
				{
					return null;
				}
				else
				{
					return list[m_current];
				}
			}
			set
			{
				if (value != null)
				{
					if (m_current >= list.Count)
					{
						list.Add(value);
						m_done = false;
					}
					else
					{
						list[m_current] = value;
					}
				}
			}
		}

		public PathList() {}

		public PathList(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values) {
			list.Add(new Path(p_Strategy, p_Values));
		}

		public PathList(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values, ListModes p_mode) {
			list.Add(new Path(p_Strategy, p_Values));
			m_mode = p_mode;
		}

		public PathList(Path p_path) {
			list.Add(p_path);
		}

		public PathList(Path p_path, ListModes p_mode) {
			list.Add(p_path);
			m_mode = p_mode;
		}

		public PathList(List<Path> p_paths)
		{
			list.AddRange(p_paths);
		}

		public PathList(List<Path> p_paths, ListModes p_mode)
		{
			list.AddRange(p_paths);
			m_mode = p_mode;
		}

		public void AddPath(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values) {
			list.Add(new Path(p_Strategy, p_Values));
		}

		public void AddPath(Path p_path)
		{
			list.Add(p_path);
		}

		public void AddPaths(List<Path> p_paths)
		{
			list.AddRange(p_paths);
		}

		public void CalculateMovement(GameTime p_gameTime)
		{
			if (m_current < list.Count && list[m_current] != null && !m_done)
			{
				list[m_current].CalculateMovement(p_gameTime);
				if (list[m_current].isDone() && m_mode != ListModes.Continuous)
				{
					switch (m_mode)
					{
						case ListModes.Once:
							m_current++;
							if (m_current >= list.Count)
							{
								m_done = true;
							}
							break;
						case ListModes.Repeat:
							m_current++;
							if (m_current >= list.Count)
							{
								m_current = 0;
								foreach (Path p in list)
								{
									p.resetDuration();
								}
							}
							break;
						case ListModes.Random:
							m_current = new Random().Next(0, list.Count);
							list[m_current].resetDuration();
							break;
						default:
							m_done = true;
							break;
					}
				}
			}

		}

		public bool isDone()
		{
			return m_done;
		}

	}
}
