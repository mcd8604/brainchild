using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace WorldMakerDemo
{
    //This class stores information about a texture needed to sort it and draw it
    [Serializable]
    public class TextureInfo
    {
        private String _textureName;
        public String TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
        }

        [NonSerialized]
        Texture2D m_Texture;
        public Texture2D TextureObject
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

        public TextureInfo(Texture2D p_Texture, int p_SortNumber)
        {
            m_Texture = p_Texture;
            m_SortNumber = p_SortNumber;
        }
    }

    [Serializable]
    public class TextureInfoComparer : IComparer<TextureInfo>
    {
        public int Compare(TextureInfo x, TextureInfo y)
        {
            int retInt = 0;
            if (x.SortNumber < y.SortNumber)
                retInt = -1;
            else if (x.SortNumber > y.SortNumber)
                retInt = 1;

            return retInt;
        }
    }
}
