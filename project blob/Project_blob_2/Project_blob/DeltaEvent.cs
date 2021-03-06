using System;
using Microsoft.Xna.Framework;
using Project_blob.GameState;
using System.ComponentModel;
using Physics2;

namespace Project_blob
{
	[Serializable]
	public class DeltaEvent : EventTrigger
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

		private Vector3 deltaPosition;
		public Vector3 DeltaPosition
		{
			get
			{
				return deltaPosition;
			}
			set
			{
				deltaPosition = value;
			}
		}
		private Vector3 deltaVelocity;
		public Vector3 DeltaVelocity
		{
			get
			{
				return deltaVelocity;
			}
			set
			{
				deltaVelocity = value;
			}
		}
		private Vector3 deltaForce;
		public Vector3 DeltaForce
		{
			get
			{
				return deltaForce;
			}
			set
			{
				deltaForce = value;
			}
		}

		public DeltaEvent() { }
		public DeltaEvent(Vector3 Force)
		{
			DeltaForce = Force;
			DeltaVelocity = Vector3.Zero;
			DeltaPosition = Vector3.Zero;
		}

		public DeltaEvent(Vector3 Force, Vector3 Velocity)
		{
			DeltaForce = Force;
			DeltaVelocity = Velocity;
			DeltaPosition = Vector3.Zero;
		}

		public DeltaEvent(Vector3 Force, Vector3 Velocity, Vector3 Position)
		{
			DeltaForce = Force;
			DeltaVelocity = Velocity;
			DeltaPosition = Position;
		}

        public bool PerformEvent( PhysicsPoint point )
		{
			foreach (PhysicsPoint p in point.ParentBody.getPoints())
			{
				p.NextPosition += DeltaPosition;
				p.NextVelocity += DeltaVelocity;
				p.ForceNextFrame += DeltaForce;
			}
            return true;
		}
	}
}
