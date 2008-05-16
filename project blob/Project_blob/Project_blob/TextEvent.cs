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
    public class TextEvent : EventTrigger
	{

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

		private string m_message;
		public string Message
		{
			get { return m_message; }
			set { m_message = value; }
		}



		public TextEvent()
		{
		
		}

		public TextEvent(string p_Message)
		{
			m_message = p_Message;
		}

		#region EventTrigger Members

		public bool PerformEvent(PhysicsPoint p)
		{
			try
			{
				GameplayScreen.TextEvent = m_message;
				GameplayScreen.TextEventHit = true;
				
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		#endregion
	}
}
