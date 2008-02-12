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
                p_Other.RegisterCollision(m_ParentShip);
			}
			else
			{
				base.RegisterCollision(p_Other);
			}			
		}
	}
}
