using System;
using System.Collections.Generic;
using System.Text;

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
				m_Score += p_Damage;
				if (p_Killed)
				{
					m_Score += 100f;
				}
			}
		}

		public override string ToString()
		{
			return "Score: " + Convert.ToInt32(m_Score).ToString();
		}

	}
}
