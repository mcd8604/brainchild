using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{

	public enum ListModes { Continuous, Once, Repeat, Random };

	public class PathList
	{

		private List<Path> m_list = new List<Path>();
        private int m_current = 0;

        private bool m_done = false;
        public bool isDone()
        {
            return m_done;
        }

        private ListModes m_mode = ListModes.Once;
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
				if (m_current >= m_list.Count)
				{
					return null;
				}
				else
				{
					return m_list[m_current];
				}
			}
			set
			{
                if (value == null)
                {
                    //throw new Exception("error");
                }
                else
                {
                    if (m_current >= m_list.Count)
                    {
                        m_list.Add(value);
                        m_done = false;
                    }
                    else
                    {
                        m_list[m_current] = value;
                    }
                    m_list[m_current].Set();
                }
				
			}
		}

		public PathList() {}

		public PathList(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values) {
			m_list.Add(new Path(p_Strategy, p_Values));
			CurrentPath.Set();
		}

		public PathList(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values, ListModes p_mode) {
			m_list.Add(new Path(p_Strategy, p_Values));
			m_mode = p_mode;
			CurrentPath.Set();
		}

		public PathList(Path p_path) {
			m_list.Add(p_path);
			CurrentPath.Set();
		}

		public PathList(Path p_path, ListModes p_mode) {
			m_list.Add(p_path);
			m_mode = p_mode;
			CurrentPath.Set();
		}

		public PathList(List<Path> p_paths)
		{
			m_list.AddRange(p_paths);
			CurrentPath.Set();
		}

		public PathList(List<Path> p_paths, ListModes p_mode)
		{
			m_list.AddRange(p_paths);
			m_mode = p_mode;
			CurrentPath.Set();
		}

		public void AddPath(Paths p_Strategy, Dictionary<PathStrategy.ValueKeys, Object> p_Values) {
			AddPath(new Path(p_Strategy, p_Values));
		}

		public void AddPath(Path p_path)
		{
			if (m_current >= m_list.Count)
			{
				m_list.Add(p_path);
				m_done = false;
				CurrentPath.Set();
			}
			else
			{
				m_list.Add(p_path);
			}
		}

		public void AddPaths(List<Path> p_paths)
		{
			if (m_current >= m_list.Count)
			{
				m_list.AddRange(p_paths);
				m_done = false;
				CurrentPath.Set();
			}
			else
			{
				m_list.AddRange(p_paths);
			}
		}

        public void CalculateMovement(GameTime p_gameTime)
        {

            if ( m_current < m_list.Count && (!m_done || m_mode == ListModes.Continuous))
            {
                m_list[m_current].CalculateMovement(p_gameTime);

                if (m_list[m_current].isDone())
                {
                    switch (m_mode)
                    {
                        case ListModes.Continuous:
                            if (m_current < m_list.Count - 1) {
                                m_current++;
                                m_list[m_current].Set();
                            } else {
                                m_done = true;
                            }
                            break;
                        case ListModes.Once:
                            m_current++;
                            if (m_current >= m_list.Count)
                            {
                                m_done = true;
                            }
                            else
                            {
                                m_list[m_current].Set();
                            }
                            break;
                        case ListModes.Repeat:
                            m_current = (m_current + 1) % m_list.Count;
                            CurrentPath.Set();
                            break;
                        case ListModes.Random:
                            m_current = new Random().Next(0, m_list.Count);
                            CurrentPath.Set();
                            break;
                        default:
                            m_done = true;
                            break;
                    }
                }
            }
        }

		public void reset()
		{
			m_current = 0;
			m_done = false;
		}

	}
}
