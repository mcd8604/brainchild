using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class GateTrigger : Collidable
	{
		private Collidable m_Gate;
		public Collidable Gate
		{
			set
			{
				m_Gate = value;
			}
			get
			{
				return m_Gate;
			}
		}

		public GateTrigger()
		{
		}
		public GateTrigger(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture,
							float p_Alpha, bool p_Visible, float p_Rotation, float p_zBuff, Factions p_Faction,
							int p_Health, float p_Radius, Collidable p_Gate)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Rotation, p_zBuff,
					p_Faction, p_Health, p_Radius)
		{
			Gate = p_Gate;
		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (p_Other.Faction == Factions.Player)
			{
				base.RegisterCollision(p_Other);
				World.m_Position.Speed = 80;
				m_Gate.ToBeRemoved = true;
			}
		}
	}
}
