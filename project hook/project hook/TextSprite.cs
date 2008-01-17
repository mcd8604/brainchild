using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	class TextSprite : Sprite
	{

		public delegate String StringFunction();

		private SpriteFont m_Font = null;
		protected SpriteFont Font
		{
			get
			{
				return m_Font;
			}
			set
			{
				m_Font = value;
			}

		}
		public void setFont(String value)
		{
			m_Font = TextureLibrary.getFont(value);
		}

		protected String m_Text = "Blank String";
		public String Text
		{
			get
			{
				return m_Text;
			}
			set
			{
				m_Text = value;
				if (Font != null)
				{
					if (sized)
					{
						m_Scale.X = Width / Font.MeasureString(Text).X;
						m_Scale.Y = Height / Font.MeasureString(Text).Y;
					}
					m_Origin = (Font.MeasureString(Text) / 2);
				}

			}
		}

		private StringFunction m_Func = null;
		public StringFunction Func
		{
			set
			{
				m_Func = value;
			}

		}

		protected Vector2 m_Center = Vector2.Zero;
		public override Vector2 Center
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


		protected Vector2 m_Origin = Vector2.Zero;

		protected bool sized = false;

		protected Vector2 m_Scale = Vector2.One;
		public new Vector2 Scale
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

		public override int Height
		{
			get
			{
				if (sized)
				{
					return m_Height;
				}
				else
				{
					return (int)Font.MeasureString(Text).Y;
				}
			}
			set
			{
				m_Height = value;
				if (Font != null)
				{
					m_Scale.Y = value / Font.MeasureString(Text).Y;
					sized = true;
				}
			}
		}

		public override int Width
		{
			get
			{
				if (sized)
				{
					return m_Width;
				}
				else
				{
					return (int)Font.MeasureString(Text).X;
				}
			}
			set
			{
				m_Width = value;
				if (Font != null)
				{
					m_Scale.X = value / Font.MeasureString(Text).X;
					sized = true;
				}
			}
		}

		public override Vector2 Position
		{
			get
			{
				return m_Center - new Vector2(Width / 2f, Height / 2f);
			}
			set
			{
				m_Center = value + new Vector2(Width / 2f, Height / 2f);
			}
		}

		public TextSprite(String p_Text, Vector2 p_Center)
		{
			setFont("Courier New");
			Text = p_Text;
			Name = "String: " + p_Text;
			Center = p_Center;
			Color = Color.Black;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color)
		{
			setFont("Courier New");
			Text = p_Text;
			Name = "String: " + p_Text;
			Center = p_Center;
			Color = p_Color;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z)
		{
			setFont("Courier New");
			Text = p_Text;
			Name = "String: " + p_Text;
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency)
		{
			setFont("Courier New");
			Text = p_Text;
			Name = "String: " + p_Text;
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation)
		{
			setFont("Courier New");
			Text = p_Text;
			Name = "String: " + p_Text;
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
			Rotation = p_Rotation;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation, Vector2 p_Scale)
		{
			setFont("Courier New");
			Text = p_Text;
			Name = "String: " + p_Text;
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
			Rotation = p_Rotation;
			Scale = p_Scale;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation, int p_Height, int p_Width)
		{
			setFont("Courier New");
			Text = p_Text;
			Name = "String: " + p_Text;
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
			Rotation = p_Rotation;
			Height = p_Height;
			Width = p_Width;
		}



		public TextSprite(StringFunction p_Func, Vector2 p_Center)
		{
			setFont("Courier New");
			Func = p_Func;
			Name = "String: " + p_Func.ToString();
			Center = p_Center;
			Color = Color.Black;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color)
		{
			setFont("Courier New");
			Func = p_Func;
			Name = "String: " + p_Func.ToString();
			Center = p_Center;
			Color = p_Color;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color, float p_Z)
		{
			setFont("Courier New");
			Func = p_Func;
			Name = "String: " + p_Func.ToString();
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency)
		{
			setFont("Courier New");
			Func = p_Func;
			Name = "String: " + p_Func.ToString();
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation)
		{
			setFont("Courier New");
			Func = p_Func;
			Name = "String: " + p_Func.ToString();
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
			Rotation = p_Rotation;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation, Vector2 p_Scale)
		{
			setFont("Courier New");
			Func = p_Func;
			Name = "String: " + p_Func.ToString();
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
			Rotation = p_Rotation;
			Scale = p_Scale;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation, int p_Height, int p_Width)
		{
			setFont("Courier New");
			Func = p_Func;
			Name = "String: " + p_Func.ToString();
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
			Rotation = p_Rotation;
			Height = p_Height;
			Width = p_Width;
		}



		public override void Draw(SpriteBatch p_SpriteBatch)
		{
			if (Visible)
			{
				if (m_Func == null)
				{
					p_SpriteBatch.DrawString(Font, Text, Center, Color, base.Rotation, m_Origin, Scale, SpriteEffects.None, base.Z);
				}
				else
				{
					p_SpriteBatch.DrawString(Font, m_Func.Invoke(), Center, Color, base.Rotation, m_Origin, Scale, SpriteEffects.None, base.Z);
				}
			}
		}

	}
}
