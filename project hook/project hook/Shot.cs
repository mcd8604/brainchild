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
#if !FINAL
			Name = "Unnamed Shot";
#endif
			Z = Depth.GameLayer.Shot;
		}
		public Shot(Shot p_Shot)
		{
			if (p_Shot.Animation != null)
			{
				Animation = new VisualEffect(p_Shot.Animation, this);
			}
			BlendMode = p_Shot.BlendMode;
			Bound = p_Shot.Bound;
			Center = p_Shot.Center;
			Color = p_Shot.Color;
			Damage = p_Shot.Damage;
			if (p_Shot.TrailEffect != null)
			{
				TrailEffect = p_Shot.TrailEffect.copy();
			}
			Enabled = p_Shot.Enabled;
			Faction = p_Shot.Faction;
			Health = p_Shot.Health;
			Height = p_Shot.Height;
			MaxHealth = p_Shot.MaxHealth;
#if !FINAL
			Name = p_Shot.Name;
#endif
			Radius = p_Shot.Radius;
			Rotation = p_Shot.Rotation;
			Task = p_Shot.Task;
			Texture = p_Shot.Texture;
			Transparency = p_Shot.Transparency;
			Width = p_Shot.Width;
			Z = p_Shot.Z;
		}
		public Shot(
#if !FINAL
			String p_Name,
#endif
			Vector2 p_Center, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Enabled, float p_Rotation, float p_Z, Factions p_Faction, float p_Health, float p_Radius, float p_Damage)
			: base(
#if !FINAL
			p_Name,
#endif
			p_Center, p_Height, p_Width, p_Texture, p_Transparency, p_Enabled, p_Rotation, p_Z, p_Faction, p_Health, p_Radius)
		{
			Damage = p_Damage;
			Center = p_Center;
		}

		internal override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
			if (Enabled)
			{
				if (Task == null || Task.IsComplete(this))
				{
					Enabled = false;
				}
				else if (m_TrailEffect != null)
				{
					m_TrailEffect.AddParticles(Center);
				}
			}
			else
			{
				if (CheckShip())
					this.ToBeRemoved = true;
			}
		}

		private SpriteParticleSystem m_TrailEffect;
		public SpriteParticleSystem TrailEffect
		{
			get
			{
				return m_TrailEffect;
			}
			set
			{
				if (value != null)
				{
					m_TrailEffect = value;
					TaskQueue EffectTask = new TaskQueue();
					EffectTask.addTask(new TaskWait(this.CheckShip));
					EffectTask.addTask(new TaskTimer(1f));
					EffectTask.addTask(new TaskRemove());
					m_TrailEffect.Task = EffectTask;
					addSprite(m_TrailEffect);
				}
			}
		}

		public override Boolean IsDead()
		{
			return false;
		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (!(p_Other is Shot) && !(p_Other is Tail) && p_Other.Faction != Factions.Blood && p_Other.Faction != Factions.PowerUp)
			{
				Enabled = !DestroyedOnCollision;
			}

			if (p_Other.Faction == Factions.Environment)
			{
				m_ToBeRemoved = CheckShip();
			}
		}

		public bool CheckShip()
		{
			return (m_Ship == null || (m_Ship.IsDead() || m_Ship.ToBeRemoved));
		}
	}
}
