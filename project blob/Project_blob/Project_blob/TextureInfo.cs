using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Project_blob
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

        public TextureInfo(String texutreName, int p_SortNumber)
        {
            _textureName = texutreName;
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
