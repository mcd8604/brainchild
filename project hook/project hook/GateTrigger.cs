using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class GateTrigger : Collidable
	{
		protected List<Sprite> m_Gates;
		public List<Sprite> Gates
		{
			set
			{
				m_Gates = value;
			}
			get
			{
				return m_Gates;
			}
		}

		protected Collidable m_Guardian = null;
		public Collidable Guardian
		{
			get
			{
				return m_Guardian;
			}
			set
			{
				m_Guardian = value;
			}
		}

		protected List<Sprite> m_Walls;
		public List<Sprite> Walls
		{
			set
			{
				m_Walls = value;
			}
			get
			{
				return m_Walls;
			}
		}

		protected bool m_EndGate = true;
		public bool EndGate
		{
			get
			{
				return m_EndGate;
			}
			set
			{
				m_EndGate = value;
			}
		}

		public GateTrigger()
		{
		}
		public GateTrigger(
#if !FINAL
			String p_Name,
#endif
			Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Rotation, float p_zBuff, Factions p_Faction, int p_Health, float p_Radius, List<Sprite> p_Gates)
			: base(
#if !FINAL
			p_Name,
#endif
			p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Rotation, p_zBuff, p_Faction, p_Health, p_Radius)
		{
			Gates = p_Gates;
		}

		public override void RegisterCollision(Collidable p_Other)
		{
#if DEBUG && VERBOSE
			Game.Out.WriteLine( "The Trigger has been hit by " + p_Other + "!" );
#endif
			if (p_Other.Faction == Factions.Player &&
				World.Position.Speed == 0 &&
				(m_Guardian == null || m_Guardian.IsDead()))
			{
				base.RegisterCollision(p_Other);
				if (m_EndGate)
				{
					World.Position.setSpeed(80);
				}
				foreach (Sprite gate in m_Gates)
				{
					gate.Enabled = false;
					gate.ToBeRemoved = true;
				}
				foreach (Sprite wall in m_Walls)
				{
					wall.Enabled = false;
					wall.ToBeRemoved = true;
				}
				m_Enabled = false;
				m_ToBeRemoved = true;
			}
		}
	}
}
