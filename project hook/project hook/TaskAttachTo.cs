using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskAttachTo : Task
	{

		internal delegate Vector2 PositionFunction();

		private PositionFunction m_Position = null;
		internal PositionFunction Position
		{
			get
			{
				return m_Position;
			}
			set
			{
				m_Position = value;
			}
		}

		private Vector2 m_Offset = Vector2.Zero;
		internal Vector2 Offset
		{
			get
			{
				return m_Offset;
			}
			set
			{
				m_Offset = value;
			}
		}

		internal TaskAttachTo() { }
		internal TaskAttachTo(PositionFunction p_Position)
		{
			m_Position = p_Position;
		}
		internal TaskAttachTo(PositionFunction p_Position, Vector2 p_Offset)
		{
			m_Position = p_Position;
			m_Offset = p_Offset;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			on.Center = m_Position.Invoke() + m_Offset;
		}
		internal override Task copy()
		{
			return new TaskAttachTo(m_Position, m_Offset);
		}
	}
}
