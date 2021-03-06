using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Wintellect.PowerCollections;

namespace project_hook
{
	/// <summary>
	/// This class contains the base information all sprites need.
	/// It also provides default draw and update method.
	/// </summary>
	internal class Sprite
	{
		#region Variables and Properties
		/// <summary>
		/// This is the alpha byte value of the sprite.
		/// </summary>
		internal byte Alpha
		{
			get
			{
				return m_Color.A;
			}
			set
			{
				m_Color = new Color(m_Color.R, m_Color.G, m_Color.B, value);
			}
		}

		protected VisualEffect m_Animation = null;
		/// <summary>
		/// The animation for the sprite.
		/// </summary>
		internal VisualEffect Animation
		{
			get
			{
				return m_Animation;
			}
			set
			{
				m_Animation = value;
			}
		}

		protected Vector2 m_Center = Vector2.Zero;
		/// <summary>
		/// A point representing the center of the sprite on the screen.
		/// </summary>
		internal virtual Vector2 Center
		{
			get
			{
				return m_Center;
			}
			set
			{
				m_Center = value;
			}
		}

		protected Color m_Color = Color.White;
		/// <summary>
		/// The color of the sprite, the default white uses the texture directly.
		/// </summary>
		internal Color Color
		{
			get
			{
				return m_Color;
			}
			set
			{
				m_Color = value;
			}
		}

		/// <summary>
		/// This will create the destination rectangle used to draw the sprite to the screen.
		/// </summary>
		internal Rectangle Destination
		{
			get
			{
				return new Rectangle((int)Center.X, (int)Center.Y, Width, Height);
			}
		}

		protected int m_Height = 0;
		/// <summary>
		/// This is the height of the the sprite.
		/// </summary>
		internal virtual int Height
		{
			get
			{
				return m_Height;
			}
			set
			{
				m_Height = value;
			}
		}
#if !FINAL
		protected String m_Name = "Unnamed Sprite";
		/// <summary>
		/// The identifying name of the sprite
		/// </summary>
		internal String Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}
#endif
		protected Task m_Task = null;
		/// <summary>
		/// The Task for this sprite.
		/// </summary>
		internal Task Task
		{
			get
			{
				return m_Task;
			}
			set
			{
				m_Task = value;
			}
		}

		protected List<Sprite> m_Parts = null;
		/// <summary>
		/// Subsprites that are 'attached' to this sprite
		/// </summary>
		internal List<Sprite> Parts
		{
			get
			{
				return m_Parts;
			}
		}

		/// <summary>
		/// This is the postion that the sprite is displayed on the screen.
		/// This vector will be modified to move the sprite around the screen.
		/// </summary>
		internal virtual Vector2 Position
		{
			get
			{
				return new Vector2(m_Center.X - (Width * 0.5f), m_Center.Y - (Height * 0.5f));
			}
			set
			{
				m_Center.X = value.X + (Width * 0.5f);
				m_Center.Y = value.Y + (Height * 0.5f);
			}
		}

		protected float m_Rotation = 0f;
		/// <summary>
		/// This will determine the amount of rotation applied to a sprite.
		/// </summary>
		internal float Rotation
		{
			get
			{
				return m_Rotation;
			}
			set
			{
				m_Rotation = value;
			}
		}

		/// <summary>
		/// TThe Rotation if the Sprite, in degrees
		/// </summary>
		internal float RotationDegrees
		{
			get
			{
				return MathHelper.ToDegrees(m_Rotation);
			}
			set
			{
				m_Rotation = MathHelper.ToRadians(value);
			}
		}

		protected List<Sprite> m_SpritesToBeAdded = null;
		/// <summary>
		/// A list of seperate sprites to be added to the main list
		/// </summary>
		internal List<Sprite> SpritesToBeAdded
		{
			get
			{
				return m_SpritesToBeAdded;
			}
		}

		protected GameTexture m_Texture = null;
		/// <summary>
		/// This is the texture that the sprite will display
		/// The GameTexture is retrieved from the TextureLibrary object
		/// </summary>
		internal GameTexture Texture
		{
			get
			{
				return m_Texture;
			}
			set
			{
				m_Texture = value;
			}
		}

		protected Boolean m_ToBeRemoved = false;
		/// <summary>
		/// Mark this sprite for removal
		/// </summary>
		internal Boolean ToBeRemoved
		{
			get
			{
				return m_ToBeRemoved;
			}
			set
			{
				m_ToBeRemoved = value;
				if (m_Parts != null)
				{
					foreach (Sprite s in m_Parts)
					{
						s.ToBeRemoved = value;
					}
				}
			}
		}

		/// <summary>
		/// The Transparency of the sprite, as a float between 0 and 1, where 1 is completely opaque.
		/// </summary>
		internal float Transparency
		{
			get
			{
				return m_Color.A / 255.0f;
			}
			set
			{
				m_Color = new Color(m_Color.R, m_Color.G, m_Color.B, (byte)MathHelper.Clamp(255 * value, 0, 255));
			}
		}

		protected int m_Width = 0;
		/// <summary>
		/// This is the width of the sprite that will be displayed
		/// </summary>
		internal virtual int Width
		{
			get
			{
				return m_Width;
			}
			set
			{
				m_Width = value;
			}
		}

		protected bool m_Enabled = true;
		/// <summary>
		/// This will determine if the sprite is currently active.
		/// </summary>
		internal bool Enabled
		{
			get
			{
				return m_Enabled;
			}
			set
			{
				m_Enabled = value;
			}
		}

		protected float m_Z = 0f;
		/// <summary>
		/// This is the Z Depth value
		/// </summary>
		internal float Z
		{
			get
			{
				return m_Z;
			}
			set
			{
				m_Z = value;
			}
		}

		protected SpriteBlendMode m_BlendMode = SpriteBlendMode.AlphaBlend;
		internal SpriteBlendMode BlendMode
		{
			get
			{
				return m_BlendMode;
			}
			set
			{
				m_BlendMode = value;
			}
		}

		protected bool m_Sized = true;
		internal bool Sized
		{ get { return m_Sized; } }

		protected Vector2 m_Scale = Vector2.One;
		internal Vector2 Scale
		{
			get
			{
				return m_Scale;
			}
			set
			{
				m_Scale = value;
				m_Sized = false;
			}
		}
		internal float ScaleScalar
		{
			set
			{
				m_Scale = new Vector2(value);
				m_Sized = false;
			}
		}
		#endregion // End of variables and Properties Region

		internal Sprite()
		{ }

		internal Sprite(Sprite p_Sprite)
		{
			throw new NotImplementedException();
		}

		internal Sprite(
#if !FINAL
			String p_Name,
#endif
Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture)
		{
#if !FINAL
			m_Name = p_Name;
#endif
			Height = p_Height;
			Width = p_Width;
			Position = p_Position;
			Texture = p_Texture;
		}


		//This is a constructor that has full parameters!
		internal Sprite(
#if !FINAL
			String p_Name,
#endif
Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, byte p_Alpha, bool p_Visible, float p_Rotation, float p_Z)
		{
#if !FINAL
			m_Name = p_Name;
#endif
			Height = p_Height;
			Width = p_Width;
			Position = p_Position;
			Texture = p_Texture;
			Alpha = p_Alpha;
			Enabled = p_Visible;
			Rotation = p_Rotation;
			Z = p_Z;
		}

		internal Sprite(
#if !FINAL
			String p_Name,
#endif
Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Visible, float p_Rotation, float p_Z)
		{
#if !FINAL
			m_Name = p_Name;
#endif
			Height = p_Height;
			Width = p_Width;
			Position = p_Position;
			Texture = p_Texture;
			Transparency = p_Transparency;
			Enabled = p_Visible;
			Rotation = p_Rotation;
			Z = p_Z;
		}

		//sets the anmmation for the object.
		internal void setAnimation(string p_Animation, int p_FramesPerSecond)
		{
			m_Animation = new VisualEffect(p_Animation, this, p_FramesPerSecond);
			m_Animation.StopAnimation();
		}

		//sets the anmmation for the object.
		internal void setAnimation(string p_Animation, int p_FramesPerSecond, int p_Cycles)
		{
			m_Animation = new VisualEffect(p_Animation, this, p_FramesPerSecond, p_Cycles);
			m_Animation.StopAnimation();
		}

		//This will draw the sprite to the screen
		internal virtual void Draw(SpriteBatch p_SpriteBatch)
		{
			if (m_Enabled)
			{
				if (m_Parts != null)
				{
					foreach (Sprite part in m_Parts)
					{
						part.Draw(p_SpriteBatch);
					}
				}
				if (m_Texture != null)
				{
					if (m_Sized)
					{
						p_SpriteBatch.Draw(m_Texture.Texture, Destination, m_Texture.StartPosition, m_Color, m_Rotation, Texture.Center, SpriteEffects.None, m_Z);
					}
					else
					{
						p_SpriteBatch.Draw(m_Texture.Texture, Center, m_Texture.StartPosition, m_Color, m_Rotation, Texture.Center, Scale, SpriteEffects.None, m_Z);
					}
				}
			}
		}

		//This update method should be overidden 
		internal virtual void Update(GameTime p_Time)
		{
			if (m_Task != null)
			{
				m_Task.Update(this, p_Time);
			}
			if (m_Animation != null && Enabled)
			{
				m_Animation.Update(p_Time);
			}
			if (m_Parts != null)
			{
				m_Parts.RemoveAll(isToBeRemoved);
				foreach (Sprite part in m_Parts)
				{
					part.Update(p_Time);
				}
			}
		}

		internal static bool isToBeRemoved(Sprite spr)
		{
			return spr.m_ToBeRemoved;
		}

		/// <summary>
		/// Add a sprite to the list of parts
		/// </summary>
		/// <param name="p_Sprite">The Sprite to add</param>
		internal void attachSpritePart(Sprite p_Sprite)
		{
			if (m_Parts == null)
			{
				m_Parts = new List<Sprite>();
			}

			m_Parts.Add(p_Sprite);
		}

		/// <summary>
		/// Add a sprite to the 'to be added' list
		/// </summary>
		/// <param name="p_Sprite">The Sprite</param>
		internal void addSprite(Sprite p_Sprite)
		{
			if (m_SpritesToBeAdded == null)
			{
				m_SpritesToBeAdded = new List<Sprite>();
			}

			m_SpritesToBeAdded.Add(p_Sprite);
		}
		internal void addSprites(IEnumerable<Sprite> p_Sprites)
		{
			if (m_SpritesToBeAdded == null)
			{
				m_SpritesToBeAdded = new List<Sprite>();
			}

			m_SpritesToBeAdded.AddRange(p_Sprites);
		}

		internal virtual Sprite copy()
		{
			return new Sprite(this);
		}
#if !FINAL
		public override string ToString()
		{
			return Name + " " + Center + " " + BlendMode + " " + Enabled + " " + ToBeRemoved;
		}
#endif
	}
}
