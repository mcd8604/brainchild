using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	internal class ShipPart : Ship
	{
		private bool m_TransfersDamage = false;
		internal bool TransfersDamage
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
		internal Ship ParentShip
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

		internal ShipPart() { }

		internal override void RegisterCollision(Collidable p_Other)
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
