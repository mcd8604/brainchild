using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
	[Serializable]
	public class EventWin : EventTrigger
	{

		private int m_NumTriggers = -1;
		public int NumTriggers
		{
			get
			{
				return m_NumTriggers;
			}
			set
			{
				m_NumTriggers = value;
			}
		}

		private bool m_Solid = false;
		public bool Solid
		{
			get
			{
				return m_Solid;
			}
			set
			{
				m_Solid = value;
			}
        }

        private float m_CoolDown = 1f;
        public float CoolDown
        {
            get
            {
                return m_CoolDown;
            }
            set
            {
                m_CoolDown = value;
            }
        }


		public EventWin()
		{
			
		}

		#region EventTrigger Members

		public bool PerformEvent(PhysicsPoint p)
		{
			try
			{
				GameplayScreen.game.WinFlag = true;
				
			}
			catch (Exception e)
			{
				Log.Out.WriteLine(e);
				return false;
			}
			return true;
		}

		#endregion
	}
}
