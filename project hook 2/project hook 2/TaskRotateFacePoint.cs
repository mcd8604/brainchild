using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class TaskRotateFacePoint : Task
	{
		private float m_Offset = 0f;
		public float Offset
		{
			get { return m_Offset; }
			set { m_Offset = value; }
		}
		private Vector2 m_Point;
		public Vector2 Point
		{
			get { return m_Point; }
			set { m_Point = value; }
		}
		public TaskRotateFacePoint() { }
		public TaskRotateFacePoint(Vector2 p_Point) {
			Point = p_Point;
		}
		public TaskRotateFacePoint(Vector2 p_Point, float p_Offset) {
			Point = p_Point;
			Offset = p_Offset;
		}
		protected override void Do(Sprite on,GameTime at)
		{
			on.Rotation = (float)Math.Atan2(Point.Y - on.Center.Y, Point.X - on.Center.X) + Offset;
		}

	}
}
