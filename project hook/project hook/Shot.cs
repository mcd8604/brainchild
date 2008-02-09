using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{

	/// <summary>
	/// Description: 
	/// 
	/// TODO:
	/// 1. move across the screen
	/// 
	/// </summary>
	public class Shot : Collidable
	{
		public bool DestroyedOnCollision = true;

		public Ship m_Ship = null;

		public Shot()
		{
			Enabled = false;
			Name = "Unnamed Shot";
			Z = Depth.GameLayer.Shot;
		}
		public Shot(Shot p_Shot)
		{
			if (p_Shot.Animation != null)
			{
				Animation = new VisualEffect(p_Shot.Animation.Name, this, p_Shot.Animation.FramesPerSecond);
			}
			BlendMode = p_Shot.BlendMode;
			Bound = p_Shot.Bound;
			Center = p_Shot.Center;
			//CollisonEffect = p_Shot.CollisonEffect;
			Color = p_Shot.Color;
			Damage = p_Shot.Damage;
			esps = p_Shot.esps;
			//DamageEffect = p_Shot.DamageEffect;
			Enabled = p_Shot.Enabled;
			Faction = p_Shot.Faction;
			Health = p_Shot.Health;
			Height = p_Shot.Height;
			MaxHealth = p_Shot.MaxHealth;
			Name = p_Shot.Name;
			Radius = p_Shot.Radius;
			Rotation = p_Shot.Rotation;
			Task = p_Shot.Task;
			Texture = p_Shot.Texture;
			Transparency = p_Shot.Transparency;
			Width = p_Shot.Width;
			Z = p_Shot.Z;
		}
		public Shot(String p_Name, Vector2 p_Center, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Enabled,
							float p_Rotation, float p_Z, Factions p_Faction, float p_Health, float p_Radius, float p_Damage)
			: base(p_Name, p_Center, p_Height, p_Width, p_Texture, p_Transparency, p_Enabled, p_Rotation, p_Z, p_Faction, p_Health, p_Radius)
		{
			Damage = p_Damage;
			Center = p_Center;
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
			if (Enabled)
			{
				if (Task == null || Task.IsComplete(this))
				{
					Enabled = false;
				}
				else
				{
					if (m_esps != null)
					{
						createShotTrail();
					}
				}
			}
		}

		/*protected void checkTask(Task t)
		{
			if (t is TaskSeekTarget)
			{
				createShotTrail();
			}
			else
			{
				if (t.getSubTasks() != null)
				{
					foreach (Task subT in t.getSubTasks())
					{
						checkTask(subT);
					}
				}
			}
		}*/

		private ExplosionSpriteParticleSystem m_esps;
		public ExplosionSpriteParticleSystem esps
		{
			get
			{
				return m_esps;
			}
			set
			{
				if(value != null) {
					m_esps = new ExplosionSpriteParticleSystem(value.Name, value.TextureName, value.TextureTag, 10);
					esps.MinNumParticles = value.MinNumParticles;
					esps.MaxNumParticles = value.MaxNumParticles;
					esps.MinLifetime = value.MinLifetime;
					esps.MaxLifetime = value.MaxLifetime;
					esps.MinScale = value.MinScale;
					esps.MaxScale = value.MaxScale;
					esps.MinInitialSpeed = value.MinInitialSpeed;
					esps.MaxInitialSpeed = value.MaxInitialSpeed;
					esps.AnimationName = value.AnimationName;
					esps.AnimationFPS = value.AnimationFPS;

					TaskQueue EffectTask = new TaskQueue();
					EffectTask.addTask(new TaskWait(this.CheckShip));
					EffectTask.addTask(new TaskTimer(1f));
					EffectTask.addTask(new TaskRemove());
					m_esps.Task = EffectTask;

					addSprite(m_esps);
				}
			}
		}

		private void initTrail()
		{
			esps = new ExplosionSpriteParticleSystem(this.Name + "_ParticleSystem", this.Texture.Name, "1", 10);
			esps.MinNumParticles = 1;
			esps.MaxNumParticles = 1;
			esps.MinLifetime = 1f;
			esps.MaxLifetime = 1f;
			esps.MinScale = 0.05f;
			esps.MaxScale = 0.05f;
			esps.MinInitialSpeed = 10;
			esps.MaxInitialSpeed = 10;

			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.CheckShip));
			EffectTask.addTask(new TaskTimer(1f));
			EffectTask.addTask(new TaskRemove());
			esps.Task = EffectTask;

			addSprite(esps);
		}

		protected void createShotTrail()
		{
			if (m_esps == null)
			{
				initTrail();
			}
			m_esps.AddParticles(this.Center);
		}

		public override Boolean IsDead()
		{
			return false;

		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (!(p_Other is Shot) && !(p_Other is Tail) && p_Other.Faction != Factions.Blood && p_Other.Faction != Factions.PowerUp)
			{
				//Vector2 midPoint = new Vector2(Center.X - p_Other.Center.X, Center.Y - p_Other.Center.Y);
				//addSprite(new Sprite(Name + "Effect", midPoint, 25, 25, CollisonEffect, 100, true, 0.0f, Depth.GameLayer.Explosion));
				Enabled = !DestroyedOnCollision;
			}

			if (p_Other.Faction == Factions.Environment)
				ToBeRemoved = CheckShip();
		}

		public bool CheckShip()
		{
			return (m_Ship != null && m_Ship.IsDead());
		}
	}
}
