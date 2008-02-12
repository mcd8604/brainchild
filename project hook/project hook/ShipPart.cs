using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class ShipPart : Ship
	{
		private bool m_TransfersDamage = false;
		public bool TransfersDamage
		{
			get
			{
				return m_TransfersDamage;
			}
			set
			{
				m_TransfersDamage = value;
			}
		}

		private Ship m_ParentShip = null;
		public Ship ParentShip
		{
			get
			{
				return m_ParentShip;
			}
			set
			{
				m_ParentShip = value;
			}
		}

		public ShipPart() { }

		public override void RegisterCollision(Collidable p_Other)
		{
			if (m_TransfersDamage)
			{
				if (p_Other is Tail)
				{
					p_Other.RegisterCollision(m_ParentShip);
				}
				else
				{
					m_ParentShip.RegisterCollision(p_Other);
				}
			}
			else
			{
				base.RegisterCollision(p_Other);
			}			
		}
	}
}
