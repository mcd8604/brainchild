using System;
using System.Collections.Generic;
using System.Text;
using Physics2;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Drawing.Design;

namespace Project_blob {
	[Serializable]
	public class DynamicModel : StaticModel {

		private List<Task> m_Tasks;
		[Editor(typeof(MultiTypeCollectionEditor), typeof(UITypeEditor))]
		public List<Task> Tasks {
			get {
				return m_Tasks;
			}
			set {
				m_Tasks = value;
			}
		}

		private bool m_HasSprings = false;
		public bool HasSprings {
			get {
				return m_HasSprings;
			}
			set {
				m_HasSprings = value;
			}
		}

		public DynamicModel() { }

		public DynamicModel(StaticModel p_Model)
			: base(p_Model) {
			m_Tasks = new List<Task>();
		}


		/// <summary>
		/// Deserialization constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctxt"></param>
		public DynamicModel(SerializationInfo info, StreamingContext ctxt) { }

		//public DynamicModel(string p_Name, string fileName, string audioName, List<short> rooms)
		//: base(p_Name, fileName, audioName, rooms) { }

		public DynamicModel(string p_Name, string fileName, string audioName, string p_TextureName, List<short> rooms)
			: base(p_Name, fileName, audioName, p_TextureName, rooms) { }

	}
}
