using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Project_blob
{
	[Serializable]
	class ConveyerBeltDynamic : DynamicModel
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

		internal float Dist = 0;

		public ConveyerBeltDynamic() { }

		public ConveyerBeltDynamic(StaticModel p_Model)
			: base(p_Model)
		{
			m_Direction = Vector3.Forward;
			m_Speed = 1f;
		}

		public ConveyerBeltDynamic(String p_Name, String fileName, String audioName, String p_TextureName, List<short> rooms)
			: base(p_Name, fileName, audioName, p_TextureName, rooms) { }

		public override void DrawMe(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Graphics.Effect effect, bool gameMode)
		{
			TextureOffsetX = -1 * this.Speed * TextureScaleX * (m_Direction.X + m_Direction.Z) * Dist;
			base.DrawMe(graphicsDevice, effect, gameMode);
		}

	}
}
