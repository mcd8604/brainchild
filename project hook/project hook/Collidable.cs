using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	/// <summary>
	/// A Subclass of Sprite, representing those sprites that are checked for collision
	/// </summary>
	public class Collidable : Sprite
	{
		#region Variables and Properties
		//which faction does this sprite belong to
		public enum Factions
		{
			Player,
			PowerUp,
			Enemy,
			Environment,
			None,
			Blood,
			ClearWall
		}

		/// <summary>
		/// Set of available mathimatical bounding areas a collidable may have for collision detection.
		/// </summary>
		public enum Boundings
		{
			Circle,
			Diamond,
			Square,
			Rectangle,
			Triangle
		}

		protected Boundings m_Bound = Boundings.Circle;
		public Boundings Bound
		{
			get
			{
				return m_Bound;
			}
			set
			{
				m_Bound = value;
			}
		}

		protected Factions m_Faction = Factions.None;
		public Factions Faction
		{
			get
			{
				return m_Faction;
			}
			set
			{
				m_Faction = value;
				if (m_Parts != null)
				{
					foreach (Sprite s in m_Parts)
					{
						if (s is Collidable)
						{
							Collidable c = s as Collidable;
							c.Faction = value;
						}
					}
				}
			}
		}

		protected float m_MaxHealth = float.NaN;
		public float MaxHealth
		{
			get
			{
				return m_MaxHealth;
			}
			set
			{
				m_MaxHealth = value;
				m_Health = m_MaxHealth;
			}
		}

		//this is how much health this sprite has
		protected float m_Health = 0;
		public float Health
		{
			get
			{
				return m_Health;
			}
			set
			{
				m_Health = value;
			}
		}

		protected bool m_Grabbable = true;
		public bool Grabbable
		{
			set
			{
				m_Grabbable = value;
			}
			get
			{
				return m_Grabbable;
			}
		}

		protected Collidable m_Drop = null;
		public Collidable Drop
		{
			get { return m_Drop; }
			set { m_Drop = value; }
		}

		protected float m_DestructionScore = 100;
		public float DestructionScore
		{
			get { return m_DestructionScore; }
			set { m_DestructionScore = value; }
		}

		protected SpriteParticleSystem m_DamageEffect = null;
		public SpriteParticleSystem DamageEffect
		{
			get
			{
				return m_DamageEffect;
			}
		}

		public void setDamageEffect(String p_DamageEffectTextureName, int p_Tag)
		{
			m_DamageEffect = new ExplosionSpriteParticleSystem(
#if !FINAL
				Name + "_DamageEffectParticleSystem",
#endif
				p_DamageEffectTextureName, p_Tag, 1);
			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.IsDead));
			EffectTask.addTask(new TaskTimer(1f));
			EffectTask.addTask(new TaskRemove());
			m_DamageEffect.Task = EffectTask;
			addSprite(m_DamageEffect);
		}
		public void setDamageEffect(String p_DamageEffectTextureName, int p_Tag, String p_DamageEffectAnimationName, int p_AnimationFPS)
		{
			m_DamageEffect = new ExplosionSpriteParticleSystem(
#if !FINAL
				Name + "_DamageEffectParticleSystem",
#endif
				p_DamageEffectTextureName, p_Tag, p_DamageEffectAnimationName, p_AnimationFPS, 1);
			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.IsDead));
			EffectTask.addTask(new TaskTimer(1f));
			EffectTask.addTask(new TaskRemove());
			m_DamageEffect.Task = EffectTask;
			addSprite(m_DamageEffect);
		}

		protected SpriteParticleSystem m_DeathEffect = null;
		public SpriteParticleSystem DeathEffect
		{
			get
			{
				return m_DeathEffect;
			}
		}

		public void setDeathEffect(String p_DeathEffectTextureName, int p_Tag)
		{
			m_DeathEffect = new ExplosionSpriteParticleSystem(
#if !FINAL
				Name + "_DeathEffectParticleSystem",
#endif
				p_DeathEffectTextureName, p_Tag, 1);
			m_DeathEffect.MaxLifetime = 1.0f;
			m_DeathEffect.MinInitialSpeed = 10;
			m_DeathEffect.MaxInitialSpeed = 100;
			m_DeathEffect.MinNumParticles = 100;
			m_DeathEffect.MaxNumParticles = 200;
			m_DeathEffect.MinScale = 1f;
			m_DeathEffect.MaxScale = 1f;
			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.IsDead));
			EffectTask.addTask(new TaskTimer(2f));
			EffectTask.addTask(new TaskRemove());
			m_DeathEffect.Task = EffectTask;
			addSprite(m_DeathEffect);
		}
		public void setDeathEffect(String p_DeathEffectTextureName, int p_Tag, String p_DeathEffectAnimationName, int p_AnimationFPS)
		{
			m_DeathEffect = new ExplosionSpriteParticleSystem(
#if !FINAL
				Name + "_DeathEffectParticleSystem",
#endif
				p_DeathEffectTextureName, p_Tag, p_DeathEffectAnimationName, p_AnimationFPS, 1);
			m_DeathEffect.MaxLifetime = 1.0f;
			m_DeathEffect.MinInitialSpeed = 10;
			m_DeathEffect.MaxInitialSpeed = 100;
			m_DeathEffect.MinNumParticles = 100;
			m_DeathEffect.MaxNumParticles = 200;
			m_DeathEffect.MinScale = 1f;
			m_DeathEffect.MaxScale = 1f;
			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.IsDead));
			EffectTask.addTask(new TaskTimer(2f));
			EffectTask.addTask(new TaskRemove());
			m_DeathEffect.Task = EffectTask;
			addSprite(m_DeathEffect);
		}

		//this is the radius used for collision detection
		protected float m_Radius = 100f;
		public virtual float Radius
		{
			get
			{
				return m_Radius;
			}
			set
			{
				m_Radius = value;
			}
		}

		// The damage that should be incurred on a collision against this collidable, in damage per second
		protected float m_Damage = 100;
		public float Damage
		{
			get
			{
				return m_Damage;
			}
			set
			{
				m_Damage = value;
			}
		}

		protected Queue<Collidable> didCollide = new Queue<Collidable>();

		#endregion // End of variables and Properties Region

		public Collidable()
		{
#if !FINAL
			Name = "Unnamed Collidable";
#endif

		}
		public Collidable(Collidable p_Collidable)
		{

			if (p_Collidable.Animation != null)
			{
				Animation = new VisualEffect(p_Collidable.Animation, this);
			}
			BlendMode = p_Collidable.BlendMode;
			Bound = p_Collidable.Bound;
			Color = p_Collidable.Color;
			Damage = p_Collidable.Damage;
			Enabled = p_Collidable.Enabled;
			Faction = p_Collidable.Faction;
			Height = p_Collidable.Height;
			MaxHealth = p_Collidable.MaxHealth;
			Health = p_Collidable.Health;
#if !FINAL
			Name = p_Collidable.Name;
#endif
			Center = p_Collidable.Center;
			Radius = p_Collidable.Radius;
			Rotation = p_Collidable.Rotation;
			RotationDegrees = p_Collidable.RotationDegrees;
			if (!p_Collidable.Sized)
			{
				Scale = p_Collidable.Scale;
			}
			if (p_Collidable.Task != null)
			{
				Task = p_Collidable.Task.copy();
			}
			Texture = p_Collidable.Texture;
			//ToBeRemoved = p_Collidable.ToBeRemoved;
			Transparency = p_Collidable.Transparency;
			Width = p_Collidable.Width;
			Z = p_Collidable.Z;

			if (p_Collidable.DamageEffect != null)
			{
				setDamageEffect(p_Collidable.DamageEffect.TextureName, p_Collidable.DamageEffect.TextureTag);
			}
			if (p_Collidable.DeathEffect != null)
			{
				setDeathEffect(p_Collidable.DeathEffect.TextureName, p_Collidable.DeathEffect.TextureTag);
			}
		}
		public Collidable(
#if !FINAL
			String p_Name,
#endif
			Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Enabled,
							float p_Rotation, float p_Z, Factions p_Faction, float p_MaxHealth, float p_Radius)
			: base(
#if !FINAL
			p_Name,
#endif
			p_Position, p_Height, p_Width, p_Texture, p_Transparency, p_Enabled, p_Rotation, p_Z)
		{
			Faction = p_Faction;
			MaxHealth = p_MaxHealth;
			Radius = p_Radius;

		}


		internal override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			while (didCollide.Count > 0 && !float.IsNaN(Health) && Health > 0)
			{
				Collidable DC = didCollide.Dequeue();
				float d;
				if (DC is Shot)
				{
					d = DC.Damage;
				}
				else if (float.IsNaN(DC.Health))
				{
					d = DC.Damage * (float)p_Time.ElapsedGameTime.TotalSeconds;
				}
				else
				{
					d = (float)Math.Min(DC.Damage * p_Time.ElapsedGameTime.TotalSeconds, DC.Health);
				}
				if (d > 0)
				{
					takeDamage(d, DC);
				}
				World.m_Score.evaluateCollision(this, DC, d, Health <= 0);
			}

			if (World.isSpriteNotVisible(this) && (Center.Y > (Game.graphics.GraphicsDevice.Viewport.Height * 1.1) || Center.Y < (0 - (Game.graphics.GraphicsDevice.Viewport.Height * .5))))
			{
				if (this is Shot)
				{
					this.Enabled = false;
					this.ToBeRemoved = ((Shot)this).CheckShip();
				}
				else
				{
					bool collect = true;
					if (this.Task is TaskParallel)
					{
						foreach (Task t in ((TaskParallel)this.Task).getSubTasks())
						{
							if (t is TaskAttach && ((TaskAttach)t).Target is Tail)
								collect = false;
						}
					}

					if ((this.Faction == Factions.Enemy || this.Faction == Factions.Blood || this.Faction == Factions.Player) && !(this is Tail) && collect)
					{
						//if (((Ship)this).Faction != Collidable.Factions.Player)
						((Collidable)this).Health = 0;
					}
					else if (this.Faction == Factions.PowerUp)
					{
						Enabled = false;
					}
				}
			}


			if (!(this is Shot) && !(this is PlayerShip) && !ToBeRemoved)
			{
				m_ToBeRemoved = IsDead();
			}
		}

		public virtual Boolean IsDead()
		{
			if (MaxHealth == float.NaN)
			{
				return false;
			}
			else if (Health <= 0)
			{
				return true;
			}
			return false;
		}

		protected virtual void takeDamage(float damage, Collidable from)
		{
			Health = MathHelper.Clamp(Health - damage, 0, MaxHealth);
			if (Health <= 0)
			{
				SpawnDeathEffect(Center);
				if (m_Parts != null)
				{
					foreach (Sprite s in m_Parts)
					{
						if (s is Collidable)
						{
							((Collidable)s).Health = 0;
						}
					}
				}
				//Dispose();
			}
			else if (damage > 0)
			{
				SpawnDamageEffect(Vector2.Lerp(Center, from.Center, 0.5f));
			}
		}

		public virtual void RegisterCollision(Collidable p_Other)
		{

			if ((Faction != Factions.Blood && p_Other.Faction != Factions.Blood) && (Faction != Factions.ClearWall && p_Other.Faction != Factions.ClearWall))
			{

				if (p_Other.Faction == Factions.Environment)
				{
					Center = Collision.GetMinNonCollidingCenter(this, p_Other);
				}
				else
				{
					if (!float.IsNaN(Health))
					{
						didCollide.Enqueue(p_Other);
					}
				}

			}
		}

		internal void SpawnDamageEffect(Vector2 where)
		{
			if (m_DamageEffect != null)
			{
				m_DamageEffect.AddParticles(where);
			}
		}

		internal void SpawnDeathEffect(Vector2 where)
		{
			if (m_DeathEffect != null)
			{
				m_DeathEffect.AddParticles(where);
			}
		}

		public override Sprite copy()
		{
			return new Collidable(this);
		}

		internal override void Draw(SpriteBatch p_SpriteBatch)
		{
			base.Draw(p_SpriteBatch);
		}

		public void captured()
		{
			if (Animation != null)
			{
				Animation.StopAnimation();
			}
		}
	}
}
