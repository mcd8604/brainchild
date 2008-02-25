using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskLerp : Task
	{

		private Sprite m_From = null;
		internal Sprite From
		{
			get
			{
				return m_From;
			}
			set
			{
				m_From = value;
			}
		}
		private Sprite m_To = null;
		internal Sprite To
		{
			get
			{
				return m_To;
			}
			set
			{
				m_To = value;
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


		internal TaskLerp() { }
		internal TaskLerp(Sprite p_From, Sprite p_To)
		{
			From = p_From;
			To = p_To;
		}

		internal override void Update(Sprite on, GameTime at)
		{
			throw new NotImplementedException("The method or operation is not supported.");
		}

		internal override void Update(ICollection<Sprite> on, GameTime at)
		{
			float i = 1;
			float div = on.Count + 1;
			foreach (Sprite s in on)
			{
				s.Center = Vector2.Lerp(m_From.Center + m_Offset, m_To.Center, i++ / div);
			}
		}
		internal override Task copy()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
