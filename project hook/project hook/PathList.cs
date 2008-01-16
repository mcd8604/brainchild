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
					list[m_current].Set();
				}
			}
		}

		public PathList() {}

		public PathList(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values) {
			list.Add(new Path(p_Strategy, p_Values));
			CurrentPath.Set();
		}

		public PathList(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values, ListModes p_mode) {
			list.Add(new Path(p_Strategy, p_Values));
			m_mode = p_mode;
			CurrentPath.Set();
		}

		public PathList(Path p_path) {
			list.Add(p_path);
			CurrentPath.Set();
		}

		public PathList(Path p_path, ListModes p_mode) {
			list.Add(p_path);
			m_mode = p_mode;
			CurrentPath.Set();
		}

		public PathList(List<Path> p_paths)
		{
			list.AddRange(p_paths);
			CurrentPath.Set();
		}

		public PathList(List<Path> p_paths, ListModes p_mode)
		{
			list.AddRange(p_paths);
			m_mode = p_mode;
			CurrentPath.Set();
		}

		public void AddPath(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values) {
			AddPath(new Path(p_Strategy, p_Values));
		}

		public void AddPath(Path p_path)
		{
			if (m_current >= list.Count)
			{
				list.Add(p_path);
				m_done = false;
				CurrentPath.Set();
			}
			else
			{
				list.Add(p_path);
			}
		}

		public void AddPaths(List<Path> p_paths)
		{
			if (m_current >= list.Count)
			{
				list.AddRange(p_paths);
				m_done = false;
				CurrentPath.Set();
			}
			else
			{
				list.AddRange(p_paths);
			}
		}

		public void CalculateMovement(GameTime p_gameTime)
		{
			if (m_current < list.Count && list[m_current] != null && (!m_done || m_mode == ListModes.Continuous))
			{
				list[m_current].CalculateMovement(p_gameTime);
				if (list[m_current].isDone())
				{
					switch (m_mode)
					{
						case ListModes.Once:
							m_current++;
							if (m_current >= list.Count)
							{
								m_done = true;
							}
							else
							{
								CurrentPath.Set();
							}
							break;
						case ListModes.Repeat:
							m_current = (m_current + 1) % list.Count;
							CurrentPath.Set();
							break;
						case ListModes.Random:
							m_current = new Random().Next(0, list.Count);
							CurrentPath.Set();
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

		public void reset()
		{
			m_current = 0;
			m_done = false;
		}

	}
}
