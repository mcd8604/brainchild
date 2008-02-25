using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class TaskRotateAroundTarget : Task
	{
		private float m_OffsetAngle = 0f;
		internal float OffsetAngle
		{
			get
			{
				return m_OffsetAngle;
			}
			set
			{
				m_OffsetAngle = value;
			}
		}
		internal float OffsetAngleDegrees
		{
			get
			{
				return MathHelper.ToDegrees(m_OffsetAngle);
			}
			set
			{
				m_OffsetAngle = MathHelper.ToRadians(value);
			}
		}

		private float m_OffsetDistance = 0f;
		internal float OffsetDistance
		{
			get
			{
				return m_OffsetDistance;
			}
			set
			{
				m_OffsetDistance = value;
			}
		}

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

		internal TaskRotateAroundTarget() { }
		internal TaskRotateAroundTarget(Sprite p_Target)
		{
			Target = p_Target;
		}
		internal TaskRotateAroundTarget(Sprite p_Target, float p_OffsetDistance)
		{
			Target = p_Target;
			OffsetDistance = p_OffsetDistance;
		}
		internal TaskRotateAroundTarget(Sprite p_Target, float p_OffsetDistance, int p_OffsetAngleDegrees)
		{
			Target = p_Target;
			OffsetDistance = p_OffsetDistance;
			OffsetAngleDegrees = p_OffsetAngleDegrees;
		}
		internal TaskRotateAroundTarget(Sprite p_Target, float p_OffsetDistance, float p_OffsetAngle)
		{
			Target = p_Target;
			OffsetDistance = p_OffsetDistance;
			OffsetAngle = p_OffsetAngle;
		}

		protected override void Do(Sprite on, GameTime at)
		{
			Vector2 newPos = m_Target.Center;
			newPos.X += OffsetDistance * (float)Math.Cos(m_Target.Rotation + m_OffsetAngle);
			newPos.Y += OffsetDistance * (float)Math.Sin(m_Target.Rotation + m_OffsetAngle);
			on.Center = newPos;
		}

		internal override Task copy()
		{
			return new TaskRotateAroundTarget(Target, OffsetDistance, OffsetAngle);
		}
	}
}
