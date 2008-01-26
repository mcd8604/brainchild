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

		private Collidable m_EnemyCaught;
		public Collidable EnemyCaught
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

		private float m_TailAttackDelay;
		public float TailAttackDelay
		{
			get
			{
				return m_TailAttackDelay;
			}
		}

		private Vector2 m_TailSpeed = Vector2.Zero;

		private double m_LastTailAttack = 0;

		public enum TailState
		{
			Ready,
			Attacking,
			Returning,
			Throwing,
			None
		}

		private TailState m_TailState = TailState.None;
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
		private Task m_AttackTask;
		private Task m_ReturnTask;
		private Task m_ThrowTask;
		private Task m_ReleaseTask;

		public Sprite m_TargetObject;
		

		public Tail(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible,
					float p_Degree, float p_Z, Factions p_Faction, float p_Health, GameTexture p_DamageEffect, float p_Radius,
					Ship p_AttachShip, float p_TailAttackDelay, ICollection<Sprite> p_BodySprites)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z, p_Faction, p_Health, p_DamageEffect, p_Radius)
		{

			TaskParallel temp = new TaskParallel();
			temp.addTask(new TaskTether(p_AttachShip));
			temp.addTask(new TaskRotateFaceTarget(p_AttachShip, (float)Math.PI));
			m_NormalTask = temp;

			Task = m_NormalTask;

			temp = new TaskParallel();
			temp.addTask(new TaskSeekTarget(p_AttachShip, 1000f, 25f));
			temp.addTask(new TaskRotateFaceTarget(p_AttachShip, (float)Math.PI));
			m_ReturnTask = temp;

			this.PlayerShip = (PlayerShip)p_AttachShip;
			m_TailTarget = new Vector2(-1, -1);
			m_EnemyCaught = null;
			m_TailAttackDelay = p_TailAttackDelay;
			m_BodySprites = p_BodySprites;
			m_BodyTask = new TaskLerp(p_AttachShip, this);

			StateOfTail = TailState.Ready;
		}

		public void TailAttack()
		{
			//attack with tail
			if (m_EnemyCaught == null && StateOfTail == TailState.Ready && m_LastTailAttack >= m_TailAttackDelay)
			{
				TaskParallel temp = new TaskParallel();
				temp.addTask(new TaskSeekPoint(m_TargetObject.Center, 1600f));
				temp.addTask(new TaskTimer(0.25f));
				temp.addTask(new TaskRotateFacePoint(m_TargetObject.Center));
				m_AttackTask = temp;

				Task = m_AttackTask;

				StateOfTail = TailState.Attacking;
				m_LastTailAttack = 0;
				Sound.Play("slap");
			}
			//throw enemy
			else if (m_EnemyCaught != null && StateOfTail == TailState.Ready && m_LastTailAttack >= m_TailAttackDelay)
			{
				Vector2 goal = Center - m_TargetObject.Center;

				TaskQueue res = new TaskQueue();
				TaskParallel temp = new TaskParallel();
				temp.addTask(new TaskSeekPoint(Vector2.Add(Center, Vector2.Divide(goal, 2.75f)), 1200f));
				temp.addTask(new TaskRotateFacePoint(m_TargetObject.Center));
				res.addTask(temp);
				temp = new TaskParallel();
				temp.addTask(new TaskSeekPoint(Vector2.Add(Center, Vector2.Divide(Vector2.Negate(goal), 2.75f)), 1600f));
				temp.addTask(new TaskRotateFacePoint(m_TargetObject.Center));
				res.addTask(temp);
				m_ThrowTask = res;

				Task = m_ThrowTask;

				float releaseAngle = (float)Math.Atan2(m_TargetObject.Center.Y - m_EnemyCaught.Center.Y, m_TargetObject.Center.X - m_EnemyCaught.Center.X);
				temp = new TaskParallel();
				temp.addTask(new TaskStraightAngle( releaseAngle, 600f));
				temp.addTask(new TaskRotateToAngle(releaseAngle));
				m_ReleaseTask = temp;

				StateOfTail = Tail.TailState.Throwing;
				m_LastTailAttack = 0;
			}
		}

		[Obsolete]
		public void TailReturned()
		{
			m_TailTarget.X = -1f;
			m_TailTarget.Y = -1f;

			Task = m_NormalTask;

			StateOfTail = TailState.Ready;
		}

		public void EnemyShoot()
		{
			Ship temp = m_EnemyCaught as Ship;
			if (temp != null)
			{
				temp.shoot();
			}
		}

		public override void RegisterCollision(Collidable p_Other)
		{
			//base.RegisterCollision(p_Other);
			if (((p_Other.Faction == Factions.Enemy || p_Other.Faction == Factions.Blood) && m_EnemyCaught == null && m_TailState == TailState.Attacking) && ( !(p_Other is Ship) || ((Ship)p_Other).Shield <= 0))
			{
				m_EnemyCaught = p_Other;
				m_EnemyCaught.Faction = Factions.Player;

				TaskParallel temp = new TaskParallel();
				temp.addTask(new TaskAttach(this));
				temp.addTask(new TaskRotateFaceTarget(m_TargetObject));
				m_EnemyCaught.Task = temp;
			}

			if (m_TailState == TailState.Throwing)
			{
				EnemyCaught.Task = m_ReleaseTask;
				EnemyCaught = null;
			}
			StateOfTail = Tail.TailState.Returning;
			Task = m_ReturnTask;
		}

		public override void Update(GameTime p_Time)
		{
			switch (StateOfTail)
			{
				case TailState.Attacking:

					if (Task.IsComplete(this))
					{
						StateOfTail = Tail.TailState.Returning;
						Task = m_ReturnTask;
					}
					break;
				case TailState.Throwing:
					if (Task.IsComplete(this))
					{
						StateOfTail = Tail.TailState.Returning;
						Task = m_ReturnTask;

						if (EnemyCaught != null)
						{
							EnemyCaught.Task = m_ReleaseTask;
							EnemyCaught = null;
						}
					}
					break;
				case TailState.Returning:

					if (Task.IsComplete(this))
					{
						StateOfTail = TailState.Ready;
						Task = m_NormalTask;
					}
					break;
			}

			m_BodyTask.Update(m_BodySprites, p_Time);
			m_LastTailAttack += (float)p_Time.ElapsedGameTime.TotalSeconds;
			if ( m_EnemyCaught != null && m_EnemyCaught.IsDead())
			{
				m_EnemyCaught = null;
			}
			base.Update(p_Time);
		}
	}
}