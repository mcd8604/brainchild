using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class SpawnPoint : Sprite
	{
		private Collidable m_SpawnObj;
		public Collidable SpawnObj
		{
			get
			{
				return m_SpawnObj;
			}
			set
			{
				m_SpawnObj = value;
			}
		}
					
		private int m_Count;
		public int Count
		{
			get
			{
				return m_Count;
			}

			set			
			{
			    m_Count = value;
			}
		}
		private int m_CurIndex;

		private float m_Delay;
        public float Delay
        {
            get
            {
                return m_Delay;
            }
            set
            {
                m_Delay = value;
            }
        }

		private List<Task> m_SpawnTasks;
		public List<Task> SpawnTasks
		{
			get
			{
				return m_SpawnTasks;
			}
			set
			{
				m_SpawnTasks = value;
			}
		}

		private int m_MinX;
		public int MinX
		{
			get
			{
				return m_MinX;
			}
			set
			{
				m_MinX = value;
			}
		}

		private int m_MaxX;
		public int MaxX
		{
			get
			{
				return m_MaxX;
			}
			set
			{
				m_MaxX = value;
			}
		}

		private Random m_Random = new Random();

		private float m_LastPos;

		public SpawnPoint()
		{
			m_LastPos = 0;
			m_CurIndex = 0;
		}

        public SpawnPoint(int p_Count, int p_Delay)
        {
            Count = p_Count;
            Delay = p_Delay;
			m_LastPos = 0;
        }

		public SpawnPoint(int count, int delay, Collidable p_SpawnObj)
		{			
			Count = count;
			m_Delay = delay;
			m_LastPos = 0;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);
			if (m_LastPos + m_Delay <= World.m_Position.Distance && m_CurIndex<m_Count)
			{
				m_LastPos = World.m_Position.Distance;
				++m_CurIndex;
				
				Collidable t_Collidable = new Collidable(m_SpawnObj);

				t_Collidable.Task = m_SpawnObj.Task.copy();

				this.addSprite(t_Collidable);
			}	
		}
	}
}
