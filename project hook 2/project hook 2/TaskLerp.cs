using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskLerp : Task
	{

		private Sprite m_From = null;
		public Sprite From
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
		public Sprite To
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


		public TaskLerp() { }
		public TaskLerp(Sprite p_From, Sprite p_To) {
			From = p_From;
			To = p_To;
		}

		public override void Update(Sprite on, GameTime at)
		{
			throw new NotImplementedException("The method or operation is not supported.");
		}

		public override void Update(ICollection<Sprite> on, GameTime at)
		{
			float i = 1;
			float div = on.Count + 1;
			foreach (Sprite s in on)
			{
				s.Center = Vector2.Lerp(m_From.Center, m_To.Center, i++ / div );
			}
		}

	}
}
