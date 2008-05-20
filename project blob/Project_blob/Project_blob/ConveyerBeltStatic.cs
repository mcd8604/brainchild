using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Project_blob {

	internal class ConveyerBeltStatic : StaticModel {
		private Vector3 m_Direction;
		public Vector3 Direction {
			get {
				return m_Direction;
			}
			set {
				m_Direction = value;
			}
		}

		private float m_Speed;
		public float Speed {
			get {
				return m_Speed;
			}
			set {
				m_Speed = value;
			}
		}

		internal float Dist = 0;

		public ConveyerBeltStatic() { }

		public ConveyerBeltStatic(StaticModel p_Model)
			: base(p_Model) {
			m_Direction = Vector3.Forward;
			m_Speed = 1f;
		}

		/// <summary>
		/// Deserialization constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctxt"></param>
		public ConveyerBeltStatic(SerializationInfo info, StreamingContext ctxt) { }

		//public ConveyerBeltStatic(string p_Name, string fileName, string audioName, List<short> rooms) : base(p_Name, fileName, audioName, rooms) { }

		public ConveyerBeltStatic(string p_Name, string fileName, string audioName, string p_TextureName, List<short> rooms)
			: base(p_Name, fileName, audioName, p_TextureName, rooms) { }

		public override void DrawMe(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Graphics.Effect effect, bool gameMode) {
			TextureOffsetX = -1 * this.Speed * TextureScaleX * (m_Direction.X + m_Direction.Z) * Dist;
			base.DrawMe(graphicsDevice, effect, gameMode);
		}

	}
}
