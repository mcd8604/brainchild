using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class SimpleScore
	{

		private float m_Score = 0;
		internal float Score
		{
			get { return m_Score; }
		}

		internal void evaluateCollision(Collidable p_Target, Collidable p_Attacker, float p_Damage, bool p_Killed)
		{
			if (p_Attacker.Faction == Collidable.Factions.Player)
			{
				m_Score += p_Damage * 0.1f;
				if (p_Killed && p_Target.DestructionScore > 0)
				{
					m_Score += p_Target.DestructionScore;

					Vector2 at = p_Target.Center;
					at.Y -= 50;
					TextSprite Kill = new TextSprite(p_Target.DestructionScore.ToString(), at, Microsoft.Xna.Framework.Graphics.Color.Yellow, Depth.HUDLayer.Text);
					Kill.Scale = new Vector2(0.5f, 0.5f);
					TaskParallel Par = new TaskParallel();
					Par.addTask(new TaskStationary());
					TaskSequence Seq = new TaskSequence();
					Seq.addTask(new TaskTimer(1.25f));
					Seq.addTask(new TaskRemove());
					Par.addTask(Seq);
					Kill.Task = Par;

					p_Attacker.addSprite(Kill);

				}
			}
		}

		internal void reset()
		{
			m_Score = 0f;
		}

		internal string ScoreString()
		{
			return "Score: " + Convert.ToInt32(m_Score).ToString();
		}

	}
}
