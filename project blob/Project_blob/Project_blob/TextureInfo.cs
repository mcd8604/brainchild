using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Project_blob
{
    //This class stores information about a texture needed to sort it and draw it
    class TextureInfo
    {
        Texture m_Texture;
        public Texture TextureObject
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

        int m_SortNumber;
        public int SortNumber
        {
            get
            {
                return m_SortNumber;
            }
            set
            {
                m_SortNumber = value;
            }
        }

        public TextureInfo(Texture p_Texture, int p_SortNumber)
        {
            m_Texture = p_Texture;
            m_SortNumber = p_SortNumber;
        }
    }
}
