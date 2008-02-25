using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class SpawnPoint : Sprite
	{
		private Sprite m_SpawnObj;
		internal Sprite SpawnObj
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

		internal enum SpawnType { dist, time }

		private SpawnType m_Type = SpawnType.dist;
		internal SpawnType Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				m_Type = value;
			}
		}

		private int m_Count = int.MaxValue;
		internal int Count
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

		private float m_CurTime;

		private float m_Delay;
		internal float Delay
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

		private int m_MinX;
		internal int MinX
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
		internal int MaxX
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

		private float m_LastPos;

		private float m_LastTime;

		internal SpawnPoint()
		{
			m_LastPos = 0;
			m_CurIndex = 0;
		}

		internal SpawnPoint(int p_Count, float p_Delay)
		{
			Count = p_Count;
			Delay = p_Delay;
			m_LastPos = 0;
			m_LastTime = 0;
			m_CurTime = 0;
		}

		internal SpawnPoint(int count, float delay, Collidable p_SpawnObj)
		{
			Count = count;
			m_Delay = delay;
			m_LastPos = 0;
			m_LastTime = 0;
			m_CurTime = 0;
		}

		internal override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);

			m_CurTime += (float)p_Time.ElapsedGameTime.TotalSeconds;
			//if (m_Type == SpawnType.dist)
			//{
			//    if (m_LastPos + m_Delay <= World.m_Position.Distance && m_CurIndex < m_Count)
			//    {
			//        m_LastPos = World.m_Position.Distance;

			//        ++m_CurIndex;

			//        addSprite(m_SpawnObj.copy());
			//    }
			//}
			//else if (m_Type == SpawnType.time)
			//{
			//    Console.WriteLine((m_LastTime + m_Delay) + "   " + m_CurTime);
			//    if (m_LastTime + m_Delay <= m_CurTime && m_CurIndex < m_Count)
			//    {
			//        m_LastTime = m_CurTime;

			//        ++m_CurIndex;

			//        addSprite(m_SpawnObj.copy());
			//    }
			//}
			if (((m_LastPos + m_Delay <= World.Position.Distance && m_Type == SpawnType.dist) || (m_LastTime + m_Delay <= m_CurTime && m_Type == SpawnType.time)) && m_CurIndex < m_Count)
			{
				m_LastPos = World.Position.Distance;
				m_LastTime = m_CurTime;
				++m_CurIndex;

				addSprite(m_SpawnObj.copy());
			}
		}
	}
}
