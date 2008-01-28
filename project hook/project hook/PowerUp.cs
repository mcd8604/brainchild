using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class PowerUp:Collidable
	{
		private int m_Amount;
		public int Amount
		{
			get
			{
				return m_Amount;
			}
			set
			{
				m_Amount = value;
			}
		}

		public PowerUp()
			:base()
		{
			
			TaskStraightVelocity straightVelocity = new TaskStraightVelocity();
					 Vector2 v = new Vector2(0,0);
					straightVelocity.Velocity = v;
					this.Task = straightVelocity;
		}

		public override void RegisterCollision(Collidable p_Other)
		{

			if (p_Other.Faction == Factions.Player)
			{
				if (p_Other is PlayerShip)
				{
					this.m_Enabled = false;
				}
			}
		}
	}
}
