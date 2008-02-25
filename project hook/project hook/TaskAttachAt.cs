using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskAttachAt : Task
	{
		private Sprite m_Target = null;
		internal Sprite Target
		{
			get
			{
				return m_Target;
			}
			set
			{
				m_Target = value;
			}
		}

		private Vector2 m_Offset;
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

		internal TaskAttachAt() { }
		internal TaskAttachAt(Sprite p_Target, Vector2 p_Offset)
		{
			Target = p_Target;
			m_Offset = p_Offset;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			on.Center = m_Target.Center + m_Offset;
		}
		internal override Task copy()
		{
			return new TaskAttachAt(m_Target, m_Offset);
		}
	}
}
