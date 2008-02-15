using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	public class TextSprite : Sprite
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
					if (m_Sized)
					{
						m_Scale.X = Width / Font.MeasureString(Text).X;
						m_Scale.Y = Height / Font.MeasureString(Text).Y;
					}
					m_Origin = (Font.MeasureString(Text) * 0.5f);
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

		protected Vector2 m_Origin = Vector2.Zero;

		public override int Height
		{
			get
			{
				if (m_Sized)
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
					m_Sized = true;
				}
			}
		}

		public override int Width
		{
			get
			{
				if (m_Sized)
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
					m_Sized = true;
				}
			}
		}

		public TextSprite(String p_Text, Vector2 p_Center)
		{
			m_Sized = false;
			setFont("Courier New");
			Text = p_Text;
#if !FINAL
			Name = "String: " + p_Text;
#endif
			Center = p_Center;
			Color = Color.Black;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color)
		{
			m_Sized = false;
			setFont("Courier New");
			Text = p_Text;
#if !FINAL
			Name = "String: " + p_Text;
#endif
			Center = p_Center;
			Color = p_Color;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z)
		{
			m_Sized = false;
			setFont("Courier New");
			Text = p_Text;
#if !FINAL
			Name = "String: " + p_Text;
#endif
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency)
		{
			m_Sized = false;
			setFont("Courier New");
			Text = p_Text;
#if !FINAL
			Name = "String: " + p_Text;
#endif
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation)
		{
			m_Sized = false;
			setFont("Courier New");
			Text = p_Text;
#if !FINAL
			Name = "String: " + p_Text;
#endif
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
			Rotation = p_Rotation;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation, Vector2 p_Scale)
		{
			m_Sized = false;
			setFont("Courier New");
			Text = p_Text;
#if !FINAL
			Name = "String: " + p_Text;
#endif
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
			Rotation = p_Rotation;
			Scale = p_Scale;
		}

		public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation, int p_Height, int p_Width)
		{
			m_Sized = false;
			setFont("Courier New");
			Text = p_Text;
#if !FINAL
			Name = "String: " + p_Text;
#endif
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
			m_Sized = false;
			setFont("Courier New");
			Func = p_Func;
#if !FINAL
			Name = "String: " + p_Func.ToString();
#endif
			Center = p_Center;
			Color = Color.Black;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color)
		{
			m_Sized = false;
			setFont("Courier New");
			Func = p_Func;
#if !FINAL
			Name = "String: " + p_Func.ToString();
#endif
			Center = p_Center;
			Color = p_Color;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color, float p_Z)
		{
			m_Sized = false;
			setFont("Courier New");
			Func = p_Func;
#if !FINAL
			Name = "String: " + p_Func.ToString();
#endif
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency)
		{
			m_Sized = false;
			setFont("Courier New");
			Func = p_Func;
#if !FINAL
			Name = "String: " + p_Func.ToString();
#endif
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation)
		{
			m_Sized = false;
			setFont("Courier New");
			Func = p_Func;
#if !FINAL
			Name = "String: " + p_Func.ToString();
#endif
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
			Rotation = p_Rotation;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation, Vector2 p_Scale)
		{
			m_Sized = false;
			setFont("Courier New");
			Func = p_Func;
#if !FINAL
			Name = "String: " + p_Func.ToString();
#endif
			Center = p_Center;
			Color = p_Color;
			Z = p_Z;
			Transparency = p_Transparency;
			Rotation = p_Rotation;
			Scale = p_Scale;
		}

		public TextSprite(StringFunction p_Func, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation, int p_Height, int p_Width)
		{
			m_Sized = false;
			setFont("Courier New");
			Func = p_Func;
#if !FINAL
			Name = "String: " + p_Func.ToString();
#endif
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
			if (Enabled)
			{
				if (m_Func == null)
				{
					p_SpriteBatch.DrawString(Font, Text, Center, Color, Rotation, m_Origin, Scale, SpriteEffects.None, Z);
				}
				else
				{
					p_SpriteBatch.DrawString(Font, m_Func.Invoke(), Center, Color, Rotation, m_Origin, Scale, SpriteEffects.None, Z);
				}


				if (m_Parts != null)
				{
					foreach (Sprite part in m_Parts)
					{
						part.Draw(p_SpriteBatch);
					}
				}
			}
		}

	}
}
