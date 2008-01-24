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
	public class Collidable : Sprite
	{
		#region Variables and Properties
		//which faction does this sprite belong to
		public enum Factions
		{
			Player,
			Enemy,
			Environment
		}

		/// <summary>
		/// Set of available mathimatical bounding areas a collidable may have for collision detection.
		/// </summary>
		public enum Boundings
		{
			Circle,
			Diamond,
			Square
		}

		private Boundings m_Bound = Boundings.Circle;
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
		private float m_Speed;
		public float Speed
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
		private Sprite m_DamageSprite;

		private SpriteParticleSystem m_DamageParticleSystem;
		public SpriteParticleSystem DamageParticleSystem
		{
			get
			{
				return m_DamageParticleSystem;
			}

			set
			{
				m_DamageParticleSystem = value;
			}
		}

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

		private GameTexture m_CollisonEffect;
		public GameTexture CollisonEffect
		{
			get
			{
				return m_CollisonEffect;
			}
			set
			{
				m_CollisonEffect = value;
			}
		}
		#endregion // End of variables and Properties Region

		public Collidable(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Enabled,
							float p_Rotation, float p_Z, Factions p_Faction, int p_Health, GameTexture p_DamageEffect, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Transparency, p_Enabled, p_Rotation, p_Z)
		{
			Faction = p_Faction;
			Health = p_Health;
			DamageEffect = p_DamageEffect;
			if (DamageEffect != null)
			{
				DamageParticleSystem = new ExplosionSpriteParticleSystem(Name + "_BloodParticleSystem", Position, DamageEffect.Width, DamageEffect.Height, DamageEffect, 255.0f, true, 0, this.Z, 1);
				DamageParticleSystem.setAnimation("Explosion", 10);
				DamageParticleSystem.Animation.StartAnimation();
				addSprite(DamageParticleSystem);
			}
			Radius = p_Radius;
		}


		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
			ToBeRemoved = AmIDead();
		}

		public virtual Boolean AmIDead()
		{
			if (Health == int.MinValue)
			{
				return false;

			}
			else if (Health <= 0)
			{
				return true;
			}
			return false;
		}

		//public virtual void CheckCollision(Collidable p_Sprite)
		//{
		//to do:	send it's self and p_Sprite to the gameCollision class, and check for a collision.
		//			if there is a collision do some action.  no action at this point.
		//}

		public virtual void RegisterCollision(Collidable p_Other)
		{
			if (DamageEffect != null)
			{

				/*m_DamageSprite = new Sprite(Name + "_DamageSprite", Position, 100, 100, m_DamageEffect, 255f, true, 0, Depth.MidGround.Top);
                m_DamageSprite.setAnimation(m_DamageEffect.Name, 100, 1);
                m_DamageSprite.Animation.StartAnimation();
                Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();

                dic.Add(PathStrategy.ValueKeys.Target,this);
                dic.Add(PathStrategy.ValueKeys.Base, m_DamageSprite);
                m_DamageSprite.Path = new Path(Path.Paths.Follow, dic);
				attachSpritePart(m_DamageSprite);*/

				//DamageParticleSystem.Direction = v2.;
				//DamageParticleSystem.AddParticles(Collision.getMidpoint(this,p_Other));
				DamageParticleSystem.AddParticles(Vector2.Lerp(this.Center, p_Other.Center, 0.5f));
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
