using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Project_blob
{
	[Serializable]
	class ConveyerBeltStatic : StaticModel
	{
		private Vector3 m_Direction;
		public Vector3 Direction
		{
			get
			{
				return m_Direction;
			}
			set
			{
				m_Direction = value;
			}
		}

		private float m_Speed;
		public float Speed
		{
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
			}
		}

		public ConveyerBeltStatic() { }

		public ConveyerBeltStatic(StaticModel p_Model) : base(p_Model) 
		{
			m_Direction = Vector3.Forward;
			m_Speed = 1f;
		}

		/// <summary>
		/// Deserialization constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctxt"></param>
		public ConveyerBeltStatic(SerializationInfo info, StreamingContext ctxt) { }

		public ConveyerBeltStatic(String p_Name, String fileName, String audioName, List<short> rooms) : base(p_Name, fileName, audioName, rooms) { }

		public ConveyerBeltStatic(String p_Name, String fileName, String audioName, TextureInfo p_TextureKey, List<short> rooms) : base(p_Name, fileName, audioName, p_TextureKey, rooms) { }

	}
}