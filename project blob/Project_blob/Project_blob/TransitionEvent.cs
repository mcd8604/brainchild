using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;
using Physics2;
using System.ComponentModel;

namespace Project_blob {
	[Serializable]
	public class TransitionEvent : EventTrigger {

		private int m_NumTriggers = -1;
		public int NumTriggers {
			get {
				return m_NumTriggers;
			}
			set {
				m_NumTriggers = value;
			}
		}

		private bool m_Solid = false;
		public bool Solid {
			get {
				return m_Solid;
			}
			set {
				m_Solid = value;
			}
		}

		private float m_CoolDown = 1f;
		public float CoolDown {
			get {
				return m_CoolDown;
			}
			set {
				m_CoolDown = value;
			}
		}

		private string _area;
		[TypeConverter(typeof(TypeConverterArea))]
		public string Area {
			get {
				return _area;
			}
			set {
				_area = value;
				_position = Level.GetArea(value).StartPosition;
			}
		}
		private Vector3 _position;
		public Vector3 Position {
			get {
				return _position;
			}
			set {
				_position = value;
			}
		}


		public TransitionEvent() { }

		public TransitionEvent(string area) {
			_area = area;
		}

		public TransitionEvent(string area, float xPos, float yPos, float zPos) {
			_area = area;
			_position = new Vector3(xPos, yPos, zPos);
		}

		public bool PerformEvent(PhysicsPoint p) {
			GameplayScreen.SetChangeArea(_area, _position);
			return true;
		}
	}
}
