using System;
using System.Collections.Generic;
using System.Text;

namespace Project_blob {
	[Serializable]
	class StaticModelSpeed : StaticModel {
		private float m_Speed = 1.0f;
		public float Speed {
			get {
				return m_Speed;
			}
			set {
				m_Speed = value;
			}
		}

		public StaticModelSpeed() { }

		public StaticModelSpeed(StaticModel p_Model)
			: base(p_Model) { }

		public StaticModelSpeed(string p_Name, string fileName, string audioName, string p_TextureName, List<short> rooms)
			: base(p_Name, fileName, audioName, p_TextureName, rooms) { }

	}
}
