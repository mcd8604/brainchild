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

		private ICollection<Sprite> m_BodySprites = new List<Sprite>();
		private Task m_BodyTask;

		private Task m_NormalTask;

		public Tail(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible,
					float p_Degree, float p_Z, Factions p_Faction, int p_Health, GameTexture p_DamageEffect, float p_Radius,
					Ship p_AttachShip, double p_TailAttackDelay, ICollection<Sprite> p_BodySprites)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z, p_Faction, -1, p_DamageEffect, p_Radius)
		{
			TaskParallel temp = new TaskParallel();
			temp.addTask(new TaskTether(p_AttachShip));
			temp.addTask(new TaskRotateFaceTarget(p_AttachShip, (float)Math.PI));
			m_NormalTask = temp;
			Task = m_NormalTask;
			this.PlayerShip = (PlayerShip)p_AttachShip;
			m_TailTarget = new Vector2(-1, -1);
			m_EnemyCaught = null;
			m_TailAttackDelay = p_TailAttackDelay;
			m_BodySprites = p_BodySprites;
			m_BodyTask = new TaskLerp(p_AttachShip, this);
		}

		public void TailAttack(Vector2 p_Target)
		{
			//attack with tail
			if (m_EnemyCaught == null && m_TailReturned && m_LastTailAttack >= m_TailAttackDelay)
			{
				m_TailTarget = p_Target;

				Task = new TaskSpecialTailAttack(p_Target, 2000f, 0.25f, PlayerShip, 1000f);

				StateOfTail = Tail.TailState.Attacking;
				m_LastTailAttack = 0;
				m_TailReturned = false;
				Sound.Play("slap");
			}
			//throw enemy
			else if (m_EnemyCaught != null && m_TailReturned && m_LastTailAttack >= m_TailAttackDelay)
			{
				Vector2 temp = Center - p_Target;

				Task = new TaskSpecialTailThrow(Vector2.Add(Center, Vector2.Divide(temp, 2.75f)), Vector2.Add(Center, Vector2.Divide(Vector2.Negate(temp), 2.75f)), 2500f, p_Target, 1000f);

				StateOfTail = Tail.TailState.Neutral;
				//m_EnemyCaught = null;
				m_LastTailAttack = 0;
			}
		}

		public void TailReturned()
		{
			m_TailTarget.X = -1f;
			m_TailTarget.Y = -1f;

			Task = m_NormalTask;

			m_TailReturned = true;
		}

		public override void RegisterCollision(Collidable p_Other)
		{
			//base.RegisterCollision(p_Other);
			if (p_Other.Faction == Factions.Enemy && m_EnemyCaught == null && m_TailState == TailState.Attacking && p_Other is Ship)
			{
				TailReturned();

				m_EnemyCaught = (Ship)p_Other;
				m_EnemyCaught.Faction = Factions.Player;

				TaskParallel temp = new TaskParallel();
				temp.addTask(new TaskAttach(this));
				temp.addTask(new TaskRotateWithTarget(this));
				m_EnemyCaught.Task = temp;


				// example:  Reverse enemy weapons
				foreach (Weapon w in m_EnemyCaught.Weapons)
				{
					w.Angle += (float)Math.PI;
				}
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

		public override void Update(GameTime p_Time)
		{
			m_BodyTask.Update(m_BodySprites, p_Time);
			m_LastTailAttack += p_Time.ElapsedGameTime.TotalMilliseconds;
			base.Update(p_Time);
		}
	}
}