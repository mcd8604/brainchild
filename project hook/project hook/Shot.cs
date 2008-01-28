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

		//private ArrayList<Shot> blurShots;

		public Shot() {
			Enabled = false;
			Name = "Unnamed Shot";
			Z = Depth.GameLayer.Shot;
		}
		public Shot(Shot p_Shot)
		{
			Animation = p_Shot.Animation;
			BlendMode = p_Shot.BlendMode;
			Bound = p_Shot.Bound;
			Center = p_Shot.Center;
			CollisonEffect = p_Shot.CollisonEffect;
			Color = p_Shot.Color;
			Damage = p_Shot.Damage;
			DamageEffect = p_Shot.DamageEffect;
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
							float p_Rotation, float p_Z, Factions p_Faction, float p_Health, GameTexture p_DamageEffect, float p_Radius, float p_Damage)
			: base(p_Name, p_Center, p_Height, p_Width, p_Texture, p_Transparency, p_Enabled, p_Rotation, p_Z, p_Faction, p_Health, p_DamageEffect, p_Radius)
		{
			Damage = p_Damage;
			Center = p_Center;
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			if (Task == null || Task.IsComplete(this))
			{
				Enabled = false;
			}
		}

		public override Boolean IsDead()
		{
			return false;

		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if(!(p_Other is Shot) && !(p_Other is Tail) && p_Other.Faction!=Factions.Blood){
				//Vector2 midPoint = new Vector2(Center.X - p_Other.Center.X, Center.Y - p_Other.Center.Y);
				//addSprite(new Sprite(Name + "Effect", midPoint, 25, 25, CollisonEffect, 100, true, 0.0f, Depth.GameLayer.Explosion));
				Enabled = false;
			}
		}
	}
}
