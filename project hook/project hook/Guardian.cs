using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class Guardian : Ship
	{
		protected GateTrigger m_Trigger;
		public GateTrigger Trigger
		{
			get
			{
				return m_Trigger;
			}
			set
			{
				m_Trigger = value;
				m_Trigger.HasGuardian = true;
			}
		}

		public Guardian()
		{
		}

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);

			if (this.ToBeRemoved)
			{
				m_Trigger.HasGuardian = false;
			}
		}
	}
}
