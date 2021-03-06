using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;
using Physics2;

namespace Project_blob
{
	[Serializable]
	public class WarpEvent : EventTrigger
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

		private Vector3 _moveToPos;
		public Vector3 MoveToPos
		{
			get
			{
				return _moveToPos;
			}
			set
			{
				_moveToPos = value;
			}
		}
		private Vector3 _moveToVel;
		public Vector3 MoveToVel
		{
			get
			{
				return _moveToVel;
			}
			set
			{
				_moveToVel = value;
			}
		}

		public WarpEvent() { }

		public WarpEvent(float xPos, float yPos, float zPos, float xVel, float yVel, float zVel)
		{
			_moveToPos = new Vector3(xPos, yPos, zPos);
			_moveToVel = new Vector3(xVel, yVel, zVel);
		}

		public bool PerformEvent(PhysicsPoint point)
		{
			Vector3 diff = _moveToPos - point.ParentBody.getCenter();

			foreach (PhysicsPoint p in point.ParentBody.getPoints())
			{
				p.NextPosition = p.ExternalPosition + diff;
				p.NextVelocity = _moveToVel;
			}

            return true;
		}
	}
}
