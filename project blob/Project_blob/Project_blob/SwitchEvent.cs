using System;
using System.Collections.Generic;
using System.Text;
using Project_blob.GameState;
using Physics2;
using System.ComponentModel;
using System.Drawing.Design;

namespace Project_blob
{
	/// <summary>
	/// Event that applies a Task to a list
	/// of Bodies when triggered
	/// </summary>
	[Serializable]
    public class SwitchEvent : EventTrigger
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

        List<EventTrigger> m_Events = new List<EventTrigger>();
		[Editor(typeof(MultiTypeCollectionEditor), typeof(UITypeEditor))]
		public List<EventTrigger> Events
		{
			get
			{
				return m_Events;
			}
			set
			{
				m_Events = value;
			}
		}

		List<DynamicModel> m_Models = new List<DynamicModel>();
		[Editor(typeof(MultiTypeCollectionEditor), typeof(UITypeEditor))]
		public List<DynamicModel> Models
		{
			get
			{
				return m_Models;
			}
			set
			{
				m_Models = value;
			}
		}

		[NonSerialized]
		List<Body> m_Bodies = new List<Body>();
		[Editor(typeof(MultiTypeCollectionEditor), typeof(UITypeEditor))]
		public List<Body> Bodies
		{
			get
			{
				return m_Bodies;
			}
			set
			{
				m_Bodies = value;
			}
		}

		/*private Task m_Task;
		public Task Task
		{
			get
			{
				return m_Task;
			}
			set
			{
				m_Task = value;
				m_Task.run = false;
			}
		}*/

        public SwitchEvent() { }

        public SwitchEvent(List<EventTrigger> p_Events)
        {
            m_Events = p_Events;
        }

        public bool PerformEvent( PhysicsPoint point )
        {
			foreach (Body b in m_Bodies)
			{
				foreach (Task t in b.getTasks())
				{
					t.run = true;
				}
			}
			bool partialSuccess = false;
            foreach (EventTrigger e in m_Events)
            {
                if ( e.PerformEvent( point ) )
                {
                    partialSuccess = true;
                }
            }
			return true;
        }
    }
}
