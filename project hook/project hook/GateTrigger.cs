using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class GateTrigger : Collidable
	{
		protected List<Sprite> m_Gates;
		internal List<Sprite> Gates
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
		internal Collidable Guardian
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
		internal List<Sprite> Walls
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
		internal bool EndGate
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

		internal GateTrigger() { }
		internal GateTrigger(
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

		internal override void RegisterCollision(Collidable p_Other)
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
					//gate.Enabled = false;
					//gate.ToBeRemoved = true;
					//((Collidable)gate).SpawnDeathEffect(gate.Center);
					TaskQueue q = new TaskQueue();
					q.addTask(new TaskTimer(Vector2.DistanceSquared(this.Center, gate.Center) / 1638400));
					q.addTask(new TaskRemove(true));
					gate.Task = q;
				}
				foreach (Sprite wall in m_Walls)
				{
					//wall.Enabled = false;
					//wall.ToBeRemoved = true;
					//((Collidable)wall).SpawnDeathEffect(wall.Center);
					TaskQueue q = new TaskQueue();
					q.addTask(new TaskTimer(Vector2.DistanceSquared(this.Center, wall.Center) / 1638400));
					q.addTask(new TaskRemove(true));
					wall.Task = q;
				}
				//m_Enabled = false;
				//m_ToBeRemoved = true;
				Task = new TaskRemove(true);
			}
		}
	}
}
