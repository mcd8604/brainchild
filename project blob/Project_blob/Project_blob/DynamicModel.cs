using System;
using System.Collections.Generic;
using System.Text;
using Physics2;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Drawing.Design;

namespace Project_blob
{
	[Serializable]
	public class DynamicModel : StaticModel
	{

		private List<Task> m_Tasks;
		[Editor(typeof(MultiTypeCollectionEditor), typeof(UITypeEditor))]
		public List<Task> Tasks
		{
			get
			{
				return m_Tasks;
			}
			set
			{
				m_Tasks = value;
			}
		}

		public DynamicModel() { }

		public DynamicModel(StaticModel p_Model)
			: base(p_Model)
		{
			m_Tasks = new List<Task>();
		}


		/// <summary>
		/// Deserialization constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctxt"></param>
		public DynamicModel(SerializationInfo info, StreamingContext ctxt) { }

		//public DynamicModel(String p_Name, String fileName, String audioName, List<short> rooms)
			//: base(p_Name, fileName, audioName, rooms) { }

		public DynamicModel(String p_Name, String fileName, String audioName, String p_TextureName, List<short> rooms)
            : base(p_Name, fileName, audioName, p_TextureName, rooms) { }

	}
}
