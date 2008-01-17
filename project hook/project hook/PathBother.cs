using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PathBother : PathStrategy
	{
		Tail m_Base;
		Collidable m_BotherTarget;
		Vector2 m_PullScaler = new Vector2(.85f, .85f);
		Vector2 m_TailSpeed = new Vector2(0, 0);

        private float m_Friction = .025f;

		public PathBother(Dictionary<ValueKeys, Object> p_Values)
			: base(p_Values)
		{
			m_BotherTarget = (Collidable)m_Values[ValueKeys.Target];
			m_Base = (Tail)m_Values[ValueKeys.Base];


		}

		public override void CalculateMovement(GameTime p_gameTime)
		{
			double distance = Math.Sqrt(Math.Pow((m_Base.Position.X - m_BotherTarget.Position.X), 2) + (Math.Pow((m_Base.Position.Y - m_BotherTarget.Position.Y), 2)));
			if (distance > 150)
			{
				m_TailSpeed = m_PullScaler*(m_BotherTarget.Position - m_Base.Position);
			}
			else
			{
				if (m_TailSpeed.Y > 0)
					m_TailSpeed.Y = (float)MathHelper.Max(m_TailSpeed.Y - (m_Friction), 0);
				else if (m_TailSpeed.Y < 0)
					m_TailSpeed.Y = (float)MathHelper.Min(m_TailSpeed.Y + (m_Friction), 0);

				if (m_TailSpeed.X > 0)
					m_TailSpeed.X = (float)MathHelper.Max(m_TailSpeed.X - (m_Friction), 0);
				else if (m_TailSpeed.X < 0)
					m_TailSpeed.X = (float)MathHelper.Min(m_TailSpeed.X + (m_Friction), 0);
			}

			Vector2 temp = m_Base.Position;
			temp.X += (float)(m_TailSpeed.X * p_gameTime.ElapsedGameTime.TotalSeconds);
			temp.Y += (float)(m_TailSpeed.Y * p_gameTime.ElapsedGameTime.TotalSeconds);
			m_Base.Position = temp;
		}
	}
}