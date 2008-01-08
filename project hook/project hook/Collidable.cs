using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	/*
	* Description: this class is a basic structure for all sprites that
	*              will be able to collide with other sprites.
	* 
	* TODO:
	*  1. 
	*  2. 
	*  
	*/
	public class Collidable:Sprite
	{
		#region Variables and Properties
		//which faction does this sprite belong to
		public enum Factions
		{
			Player,
			Enemy,
			Environment
		}

		private Factions m_Faction;
		public Factions Faction
		{
			get
			{
				return m_Faction;
			}
			set
			{
				m_Faction = value;
			}
		}

		//this is how much health this sprite has
		private int m_Health;
		public int Health
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

		//this is the speed of the sprite
		private int m_Speed;
		public int Speed
		{
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
			}
		}

		//this is what effect will be shown on the sprite when it takes damage
		private GameTexture m_DamageEffect;
		public GameTexture DamageEffect
		{
			get
			{
				return m_DamageEffect;
			}
			set
			{
				m_DamageEffect = value;
			}
		}

		//this is the sprite for teh damage effect
		private Visual m_DamageSprite;

		//this is the radius used for collision detection
		private float m_Radius;
		public float Radius
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
		#endregion // End of variables and Properties Region

        public Collidable(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, 
							float p_Degree, float p_Z, Factions p_Faction, int p_Health, Path p_Path, int p_Speed, GameTexture p_DamageEffect, float p_Radius)
            : base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z)
        {
			Faction = p_Faction;
			Health = p_Health;
			Path = p_Path;
			Speed = p_Speed;
			DamageEffect = p_DamageEffect;
			Radius = p_Radius;
        }


		//public virtual void CheckCollision(Collidable p_Sprite)
		//{
			//to do:	send it's self and p_Sprite to the gameCollision class, and check for a collision.
			//			if there is a collision do some action.  no action at this point.
		//}

		public virtual void RegisterCollision(Collidable p_Other, GameTime gameTime)
		{
			if (m_DamageEffect != null)
			{
               
				m_DamageSprite = new Visual(Name + "_DamageSprite", Position, 100, 100, m_DamageEffect, 50f, true, 0, Depth.MidGround.Top, gameTime, 1.1f);
                m_DamageSprite.setAnimation(m_DamageEffect.Name, 10);
                m_DamageSprite.Animation.StartAnimation();
                Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();

                dic.Add(PathStrategy.ValueKeys.Target,this);
                dic.Add(PathStrategy.ValueKeys.Base, m_DamageSprite);
                m_DamageSprite.Path = new Path(Path.Paths.Follow, dic);
				attachSpritePart(m_DamageSprite);
			}
		}

		public override void Draw(SpriteBatch p_SpriteBatch)
		{
			base.Draw(p_SpriteBatch);

			if (m_DamageSprite != null)
			{
				m_DamageSprite.Draw(p_SpriteBatch);
			}
		}
	}
}
