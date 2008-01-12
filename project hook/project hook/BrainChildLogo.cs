using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class BrainChildLogo : Menu
	{
		int m_Delay;
		double m_Created;
		double m_Time;

		public BrainChildLogo()
			: base()
		{
			//change to so texture that is is made for our title screen
			m_BackgroundName = "virus";
		}

		protected override void Init()
		{
			base.Init();
			m_Created = 0;
			m_Time = 0;

			//60fps*5sec=300mil sec
			m_Delay = 300;
		}

		public override void checkKeys(KeyHandler keyhandler)
		{
			if (keyhandler.IsActionPressed(KeyHandler.Actions.Pause) || m_Time >= m_Created + m_Delay)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.RITLogo);
			}

			++m_Time;
		}
	}
}