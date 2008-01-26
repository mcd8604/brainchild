using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	/*
	* Description: 
	*              
	* 
	* TODO:
	*  1. move across the screen
	*  
	*/
	public class Shot : Collidable
	{

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
				//ToBeRemoved = true;
			}
		}

		public override Boolean IsDead()
		{
			return false;

		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if(!(p_Other is Shot)){
				Vector2 midPoint = new Vector2(Center.X - p_Other.Center.X, Center.Y - p_Other.Center.Y);
				//addSprite(new Sprite(Name + "Effect", midPoint, 25, 25, CollisonEffect, 100, true, 0.0f, Depth.GameLayer.Explosion));

				//ToBeRemoved = true;
				Enabled = false;
			}

		}
	}
}
