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
	public class Sprite
	{

		protected int m_MaxYValue_Screen = 768;
		public int MaxScreenSize
		{
			get
			{
				return m_MaxYValue_Screen;
			}
			set
			{
				m_MaxYValue_Screen = value;
			}
		}

		#region Variables and Properties
		/// <summary>
		/// This is the alpha byte value of the sprite.
		/// </summary>
		public byte Alpha
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
		public VisualEffect Animation
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

		/// <summary>
		/// A point representing the center of the sprite on the screen.
		/// </summary>
		public virtual Vector2 Center
		{
			get
			{
				return new Vector2(Position.X + (Width * 0.5f), Position.Y + (Height * 0.5f));
			}
			set
			{
				m_Position.X = value.X - (Width * 0.5f);
				m_Position.Y = value.Y - (Height * 0.5f);
			}
		}

		protected Color m_Color = Color.White;
		/// <summary>
		/// The color of the sprite, the default white uses the texture directly.
		/// </summary>
		public Color Color
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
		public Rectangle Destination
		{
			get
			{
				return new Rectangle((int)(Position.X + (Width * 0.5f)), (int)(Position.Y + (Height * 0.5f)), Width, Height);
			}
		}

		protected int m_Height = 0;
		/// <summary>
		/// This is the height of the the sprite.
		/// </summary>
		public virtual int Height
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

		protected String m_Name = "Unnamed Sprite";
		/// <summary>
		/// The identifying name of the sprite
		/// </summary>
		public String Name
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

		protected Task m_Task = null;
		/// <summary>
		/// The Task for this sprite.
		/// </summary>
		public Task Task
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
		public List<Sprite> Parts
		{
			get
			{
				return m_Parts;
			}
		}

		protected Vector2 m_Position = Vector2.Zero;
		/// <summary>
		/// This is the postion that the sprite is displayed on the screen.
		/// This vector will be modified to move the sprite around the screen.
		/// </summary>
		public virtual Vector2 Position
		{
			get
			{
				return m_Position;
			}
			set
			{
				m_Position = value;
			}
		}

		protected float m_Rotation = 0f;
		/// <summary>
		/// This will determine the amount of rotation applied to a sprite.
		/// </summary>
		public float Rotation
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
		public float RotationDegrees
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
		public List<Sprite> SpritesToBeAdded
		{
			get
			{
				return m_SpritesToBeAdded;
			}
		}

		protected Vector2 m_StartPosition = Vector2.Zero;
		/// <summary>
		/// The start position of the sprite.
		/// </summary>
		public Vector2 StartPosition
		{
			get
			{
				return m_StartPosition;
			}
			protected set
			{
				m_StartPosition = value;
			}

		}

		protected GameTexture m_Texture = null;
		/// <summary>
		/// This is the texture that the sprite will display
		/// The GameTexture is retrieved from the TextureLibrary object
		/// </summary>
		public GameTexture Texture
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
		public Boolean ToBeRemoved
		{
			get
			{
				return m_ToBeRemoved;
			}
			set
			{
				m_ToBeRemoved = value;
			}
		}

		/// <summary>
		/// The Transparency of the sprite, as a float between 0 and 1, where 1 is completely opaque.
		/// </summary>
		public float Transparency
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
		public virtual int Width
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
		public bool Enabled
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
		public float Z
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

		private SpriteBlendMode m_BlendMode = SpriteBlendMode.AlphaBlend;
		public SpriteBlendMode BlendMode
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

		// Why does it have a 'Scale' and Height / Width ??
		private float m_Scale = -1.0f;
		public float Scale
		{
			get
			{
				return m_Scale;
			}
			set
			{
				m_Scale = value;
			}
		}
		#endregion // End of variables and Properties Region

		public Sprite()
		{ }

		public Sprite(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture)
		{
			m_Name = p_Name;
			StartPosition = p_Position;
			Position = StartPosition;
			Height = p_Height;
			Width = p_Width;
			Texture = p_Texture;
		}


		//This is a constructor that has full parameters!
		public Sprite(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, byte p_Alpha, bool p_Visible, float p_Rotation, float p_Z)
		{
			m_Name = p_Name;
			StartPosition = p_Position;
			Position = StartPosition;
			Height = p_Height;
			Width = p_Width;
			Texture = p_Texture;
			Alpha = p_Alpha;
			Enabled = p_Visible;
			Rotation = p_Rotation;
			Z = p_Z;
		}

		public Sprite(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Visible, float p_Rotation, float p_Z)
		{
			m_Name = p_Name;
			StartPosition = p_Position;
			Position = StartPosition;
			Height = p_Height;
			Width = p_Width;
			Texture = p_Texture;
			Transparency = p_Transparency;
			Enabled = p_Visible;
			Rotation = p_Rotation;
			Z = p_Z;
		}

		//sets the anmmation for the object.
		public void setAnimation(string p_Animation, int p_FramesPerSecond)
		{
			m_Animation = new VisualEffect(p_Animation, this, p_FramesPerSecond);
			m_Animation.StopAnimation();
		}

		//sets the anmmation for the object.
		public void setAnimation(string p_Animation, int p_FramesPerSecond, int p_Cycles)
		{
			m_Animation = new VisualEffect(p_Animation, this, p_FramesPerSecond, p_Cycles);
			m_Animation.StopAnimation();
		}

		//This will draw the sprite to the screen
		public virtual void Draw(SpriteBatch p_SpriteBatch)
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
					if (Scale >= 0)
					{
						p_SpriteBatch.Draw(m_Texture.Texture, Position, m_Texture.StartPosition, m_Color, m_Rotation, Texture.Center, Scale, SpriteEffects.None, m_Z);
					}
					else
					{

						p_SpriteBatch.Draw(m_Texture.Texture, Destination, m_Texture.StartPosition, m_Color, m_Rotation, Texture.Center, SpriteEffects.None, m_Z);
					}
				}
			}
		}

		//This update method should be overidden 
		public virtual void Update(GameTime p_Time)
		{
			if (m_Task != null)
			{
				m_Task.Update(this, p_Time);
			}
			if (m_Animation != null)
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

			if ((this.Position.Y > (m_MaxYValue_Screen * 1.75) || this.Position.Y < (0 - (m_MaxYValue_Screen * .75))))
			{
				if(this is Shot)
				{
					((Shot)this).CheckShip();
				}
				else
				{
					this.ToBeRemoved = true;
					if (this is Ship)
						//if (((Ship)this).Faction != Collidable.Factions.Player)
							((Ship)this).Health = -1;
				}
			}
		}

		public static bool isToBeRemoved(Sprite spr)
		{
			return spr.m_ToBeRemoved;
		}

		/// <summary>
		/// Add a sprite to the list of parts
		/// </summary>
		/// <param name="p_Sprite">The Sprite to add</param>
		public void attachSpritePart(Sprite p_Sprite)
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
		public void addSprite(Sprite p_Sprite)
		{
			if (m_SpritesToBeAdded == null)
			{
				m_SpritesToBeAdded = new List<Sprite>();
			}

			m_SpritesToBeAdded.Add(p_Sprite);
		}
		public void addSprites(IEnumerable<Sprite> p_Sprites)
		{
			if (m_SpritesToBeAdded == null)
			{
				m_SpritesToBeAdded = new List<Sprite>();
			}

			m_SpritesToBeAdded.AddRange(p_Sprites);
		}
	}
}
