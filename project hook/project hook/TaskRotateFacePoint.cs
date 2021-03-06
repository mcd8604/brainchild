using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskRotateFacePoint : Task
	{
		private float m_Offset = 0f;
		internal float Offset
		{
			get { return m_Offset; }
			set { m_Offset = value; }
		}
		private Vector2 m_Point;
		internal Vector2 Point
		{
			get { return m_Point; }
			set { m_Point = value; }
		}
		internal TaskRotateFacePoint() { }
		internal TaskRotateFacePoint(Vector2 p_Point)
		{
			Point = p_Point;
		}
		internal TaskRotateFacePoint(Vector2 p_Point, float p_Offset)
		{
			Point = p_Point;
			Offset = p_Offset;
		}
		protected override void Do(Sprite on, GameTime at)
		{
			on.Rotation = (float)Math.Atan2(Point.Y - on.Center.Y, Point.X - on.Center.X) + Offset;
		}
		internal override Task copy()
		{
			return new TaskRotateFacePoint(m_Point, m_Offset);
		}
	}
}
