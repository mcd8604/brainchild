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
		#region Variables and Properties
		//how much damage will be done by this shot
		private int m_Damage;
		public int Damage
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


		#endregion // End of variables and Properties Region

		public Shot(String p_Name, Vector2 p_Center, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible,
							float p_Rotation, float p_Z, Factions p_Faction, int p_Health, GameTexture p_DamageEffect, float p_Radius, int p_Damage)
			: base(p_Name, Vector2.Zero, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Rotation, p_Z, p_Faction, p_Health, p_DamageEffect, p_Radius)
		{
			Damage = p_Damage;
			Center = p_Center;
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			if (PathList.isDone())
			{
				Enabled = false;
				ToBeRemoved = true;
			}
		}

		public override Boolean AmIDead()
		{
			return false;

		}

		public override void RegisterCollision(Collidable p_Other)
		{
			base.RegisterCollision(p_Other);
			if(!(p_Other is Shot)){
			Vector2 midPoint = new Vector2(Center.X - p_Other.Center.X, Center.Y - p_Other.Center.Y);
			addSprite(new Sprite(Name + "Effect", midPoint, 25, 25, CollisonEffect, 100, true, 0.0f, Depth.MidGround.Top));

			ToBeRemoved = true;
			Enabled = false;
			}

		}

		//public override void Update(float p_Elapsed)
		//{
		//    //
		//    //
		//    //
		//    if(m_Faction==
		//}
	}
}
