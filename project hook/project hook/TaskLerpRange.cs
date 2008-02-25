using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskLerpRange : Task
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
		private float m_Range = 0f;
		internal float Range
		{
			get
			{
				return m_Range;
			}
			set
			{
				m_Range = value;
			}
		}

		internal TaskLerpRange() { }
		internal TaskLerpRange(Sprite p_From, Sprite p_To, float p_MaxRangeFromFrom)
		{
			From = p_From;
			To = p_To;
			Range = p_MaxRangeFromFrom;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			float value = 1;
			float distance = Vector2.Distance(From.Center, To.Center);
			if (distance > Range)
			{
				value = Range / distance;
			}
			on.Center = Vector2.Lerp(From.Center, To.Center, value);
		}
		internal override Task copy()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
