using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class Tail : Collidable
	{
		private Vector2 m_TailTarget;
		public Vector2 TailTarget
		{
			get
			{
				return m_TailTarget;
			}
			set
			{
				m_TailTarget = value;
			}
		}

		private Ship m_EnemyCaught;
		public Ship EnemyCaught
		{
			get
			{
				return m_EnemyCaught;
			}
			set
			{
				m_EnemyCaught = value;
			}
		}

		private PlayerShip m_PlayerShip;
		public PlayerShip PlayerShip
		{
			get
			{
				return m_PlayerShip;
			}
			set
			{
				m_PlayerShip = value;
			}
		}

		private double m_TailAttackDelay;
		public double TailAttackDelay
		{
			get
			{
				return m_TailAttackDelay;
			}
			set
			{
				m_TailAttackDelay = value;
			}
		}

		private Vector2 m_TailSpeed = new Vector2(0, 0);

		private double m_LastTailAttack = 0;

		private bool m_TailReturned = true;

		public enum TailState
		{
			Neutral,
			Attacking,
			Returning
		}

		private TailState m_TailState = TailState.Neutral;
		public TailState StateOfTail
		{
			get
			{
				return m_TailState;
			}
			set
			{
				m_TailState = value;
			}
		}

		private ArrayList m_BodySprites = new ArrayList();

		public Tail(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible,
					float p_Degree, float p_Z, Factions p_Faction, int p_Health, int p_Speed, GameTexture p_DamageEffect, float p_Radius,
					Ship p_AttachShip, double p_TailAttackDelay, ArrayList p_BodySprites)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z, p_Faction, -1, null, p_Speed, p_DamageEffect, p_Radius)
		{
			Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.Target, p_AttachShip);
			this.PlayerShip = (PlayerShip)p_AttachShip;
			dic.Add(PathStrategy.ValueKeys.Base, this);
			this.Path = new Path(Paths.Tether, dic);
			m_TailTarget = new Vector2(-1, -1);
			m_EnemyCaught = null;
			m_TailAttackDelay = p_TailAttackDelay;
			m_BodySprites = p_BodySprites;

			dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.End, m_PlayerShip);
			dic.Add(PathStrategy.ValueKeys.Base, m_BodySprites);
			dic.Add(PathStrategy.ValueKeys.Target, this);
			((Sprite)m_BodySprites[0]).Path = new Path(Paths.TailBody, dic);
		}

		public void TailAttack(Vector2 p_Target, GameTime p_GameTime)
		{
			//attack with tail
			if (m_EnemyCaught == null && p_GameTime.TotalGameTime.TotalMilliseconds >= m_LastTailAttack + m_TailAttackDelay)
			{
				m_TailTarget = p_Target;
				Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
				dic.Add(PathStrategy.ValueKeys.Base, this);
				dic.Add(PathStrategy.ValueKeys.Speed, 2000f);
				dic.Add(PathStrategy.ValueKeys.Target, this.PlayerShip);
				dic.Add(PathStrategy.ValueKeys.End, p_Target);
				dic.Add(PathStrategy.ValueKeys.Duration, 0.25f);
				this.Path = new Path(Paths.TailAttack, dic);
				//gets the current time in milliseconds
				m_LastTailAttack = p_GameTime.TotalGameTime.TotalMilliseconds;
				m_TailReturned = false;
				Sound.Play("slap");
			}
			//throw enemy
			else if (m_EnemyCaught != null && m_TailReturned && p_GameTime.TotalGameTime.TotalMilliseconds >= m_LastTailAttack + m_TailAttackDelay)
			{
				Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
				dic.Add(PathStrategy.ValueKeys.Base, this);
				dic.Add(PathStrategy.ValueKeys.Speed, 1000f);
				dic.Add(PathStrategy.ValueKeys.End, m_EnemyCaught);
				dic.Add(PathStrategy.ValueKeys.Target, p_Target);
				this.Path = new Path(Paths.Throw, dic);
				m_EnemyCaught = null;
				m_LastTailAttack = p_GameTime.TotalGameTime.TotalMilliseconds;
			}
		}

		public void TailReturned()
		{
			m_TailTarget.X = -1f;
			m_TailTarget.Y = -1f;
			Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.Target, this.PlayerShip);
			dic.Add(PathStrategy.ValueKeys.Base, this);
			this.Path = new Path(Paths.Tether, dic);
			m_TailReturned = true;
		}

		public override void RegisterCollision(Collidable p_Other)
		{
			//base.RegisterCollision(p_Other);
			if (p_Other.Faction == Factions.Enemy && m_EnemyCaught == null && m_TailState == TailState.Attacking)
			{
				TailReturned();
				Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
				dic.Add(PathStrategy.ValueKeys.Target, p_Other);
				dic.Add(PathStrategy.ValueKeys.Base, this);
				m_EnemyCaught = (Ship)p_Other;
				m_EnemyCaught.Faction = Factions.Player;
				m_EnemyCaught.Path = new Path(Paths.TailAttach, dic);
			}
		}

		private void UpdateEnemyCaught()
		{
			if (m_EnemyCaught != null)
			{
				m_EnemyCaught.Position = this.Position;
				m_EnemyCaught.Rotation = this.Rotation;
			}
		}
	}
}