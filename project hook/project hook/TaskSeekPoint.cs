using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskSeekPoint : Task
	{
		private Vector2 m_Goal = Vector2.Zero;
		internal Vector2 Goal
		{
			get
			{
				return m_Goal;
			}
			set
			{
				m_Goal = value;
			}
		}
		private float m_Speed = 0f;
		internal float Speed
		{
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
			}
		}
		private float m_CloseEnough = 0f;
		internal float CloseEnough
		{
			get
			{
				return m_CloseEnough;
			}
			set
			{
				m_CloseEnough = value;
			}
		}
		internal TaskSeekPoint() { }
		internal TaskSeekPoint(Vector2 p_Goal, float p_Speed)
		{
			Goal = p_Goal;
			Speed = p_Speed;
		}
		internal TaskSeekPoint(Vector2 p_Goal, float p_Speed, float p_CloseEnough)
		{
			Goal = p_Goal;
			Speed = p_Speed;
			CloseEnough = p_CloseEnough;
		}
		internal override bool IsComplete(Sprite on)
		{
			Vector2 temp = m_Goal - on.Center;
			return (Math.Abs(temp.X) <= CloseEnough && Math.Abs(temp.Y) <= CloseEnough);
		}
		protected override void Do(Sprite on, GameTime at)
		{
			Vector2 temp = m_Goal - on.Center;
			if (Math.Abs(temp.X) <= CloseEnough && Math.Abs(temp.Y) <= CloseEnough)
			{
				return;
			}
			Vector2 temp2 = Vector2.Multiply(Vector2.Normalize(temp), (float)(Speed * (at.ElapsedGameTime.TotalSeconds)));
			if (Math.Abs(temp2.X) > Math.Abs(temp.X))
			{
				temp2.X = temp.X;
			}
			if (Math.Abs(temp2.Y) > Math.Abs(temp.Y))
			{
				temp2.Y = temp.Y;
			}
			on.Center = Vector2.Add(on.Center, temp2);
		}
		internal override Task copy()
		{
			return new TaskSeekPoint(m_Goal, m_Speed, m_CloseEnough);
		}
	}
}
