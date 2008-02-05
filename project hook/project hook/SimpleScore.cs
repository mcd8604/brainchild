using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class SimpleScore
	{

		protected float m_Score = 0;
		public float Score
		{
			get { return m_Score; }
		}

		public void evaluateCollision(Collidable p_Target, Collidable p_Attacker, float p_Damage, bool p_Killed)
		{

			if (p_Target.Faction == Collidable.Factions.Environment || p_Attacker.Faction == Collidable.Factions.Environment)
			{
				if (p_Target.Faction == Collidable.Factions.Player || p_Attacker.Faction == Collidable.Factions.Player)
				{
					m_Score += 0;
				}
			}

			if (p_Attacker.Faction == Collidable.Factions.Player)
			{
				m_Score += p_Damage * 0.1f;
				if (p_Killed)
				{
					m_Score += p_Target.DestructionScore;

					Vector2 at = p_Target.Center;
					at.Y -= 50;
					TextSprite Kill = new TextSprite(p_Target.DestructionScore.ToString(), at, Microsoft.Xna.Framework.Graphics.Color.Yellow, Depth.HUDLayer.Background);
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

		public override string ToString()
		{
			return "Score: " + Convert.ToInt32(m_Score).ToString();
		}

	}
}
