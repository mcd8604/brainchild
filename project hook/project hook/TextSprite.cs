using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
    class TextSprite : Sprite
    {

        private SpriteFont m_Font;
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

        protected String m_Text;
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
        /*
        private Color m_Color;
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
        */
        
        protected Vector2 m_Center;
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
        

        private Vector2 m_Origin;

        protected bool sized = false;
        
        protected Vector2 m_Scale = new Vector2(1, 1);
        public Vector2 Scale
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
                return m_Height;
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
                return m_Width;
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

        public TextSprite(String p_Text, Vector2 p_Center)
            : base("String: " + p_Text, p_Center, 0, 0, null, 1f, true, 0, 0f)
        {
            setFont("Courier New");
            Text = p_Text;
            Center = p_Center;
            Color = Color.Black;
        }

        public TextSprite(String p_Text, Vector2 p_Center, Color p_Color)
            : base("String: " + p_Text, p_Center, 0, 0, null, 1f, true, 0, 0f)
        {
            setFont("Courier New");
            Text = p_Text;
            Center = p_Center;
            Color = p_Color;
        }

        public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z)
            : base("String: " + p_Text, p_Center, 0, 0, null, 1f, true, 0, p_Z)
        {
            setFont("Courier New");
            Text = p_Text;
            Center = p_Center;
            Color = p_Color;
        }

        public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency)
            : base("String: " + p_Text, p_Center, 0, 0, null, p_Transparency, true, 0, p_Z)
        {
            setFont("Courier New");
            Text = p_Text;
            Center = p_Center;
            Color = p_Color;
        }

        public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation)
            : base("String: " + p_Text, p_Center, 0, 0, null, p_Transparency, true, p_Rotation, p_Z)
        {
            setFont("Courier New");
            Text = p_Text;
            Center = p_Center;
            Color = p_Color;
        }

        public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation, Vector2 p_Scale)
            : base("String: " + p_Text, p_Center, 0, 0, null, p_Transparency, true, p_Rotation, p_Z)
        {
            setFont("Courier New");
            Text = p_Text;
            Center = p_Center;
            Color = p_Color;
            Scale = p_Scale;
        }

        public TextSprite(String p_Text, Vector2 p_Center, Color p_Color, float p_Z, float p_Transparency, float p_Rotation, int p_Height, int p_Width)
            : base("String: " + p_Text, p_Center, p_Height, p_Width, null, p_Transparency, true, p_Rotation, p_Z)
        {
            setFont("Courier New");
            Text = p_Text;
            Center = p_Center;
            Color = p_Color;
            Height = p_Height;
            Width = p_Width;
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            if (Visible)
            {
                p_SpriteBatch.DrawString(Font, Text, Center, Color, base.Rotation, m_Origin, Scale, SpriteEffects.None, base.Z);
            }
        }

    }
}
