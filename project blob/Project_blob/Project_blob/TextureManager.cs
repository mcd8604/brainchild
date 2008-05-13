using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob
{
    public class TextureManager
    {
        // Objects to ensure singleton design
        /*private static volatile TextureManager _instance;
        private static object _syncRoot = new Object();

        // Dictionary of 2D textures
        private static Dictionary<String, Texture2D> _textures;

        /// <summary>
        /// Constructor
        /// </summary>
        private TextureManager()
        {
            _textures = new Dictionary<String, Texture2D>();
        }

        //! Instance
        public static TextureManager getSingleton
        {
            get {
                if (_instance == null) {
                    lock (_syncRoot) {
                        if (_instance == null)
                            _instance = new TextureManager();
                    }
                }

                return _instance;
            }
        }

        public Texture2D GetTexture(String textureName)
        {
            if( textureName != null ) {
                if( _textures.ContainsKey( textureName ) ) {
                    return _textures[textureName];
                }
            }
            return null;
        }

        public void AddTexture(String textureName, Texture2D texture)
        {
            if (!_textures.ContainsKey(textureName))
            {
                _textures.Add(textureName, texture);
            }
        }

        public void RemoveTexture(String textureName)
        {
            if (_textures.ContainsKey(textureName))
            {
                _textures.Remove(textureName);
            }
        }*/

		private static List<Texture2D> m_TextureList = new List<Texture2D>();
		public static List<Texture2D> TextureList
		{
			get
			{
				return m_TextureList;
			}
		}

		public static void AddTexture(Texture2D texture)
		{
			if (!m_TextureList.Contains(texture))
			{
				m_TextureList.Add(texture);
			}
		}

		public static Texture2D GetTexture(int textureID)
		{
			return m_TextureList[textureID];
        }

        public static Texture2D GetTexture(String textureName)
        {
			foreach (Texture2D t in m_TextureList)
			{
                if (t.Name.Equals(textureName))
                {
                    return t;
                }
            }
            throw new Exception(textureName + " Texture Not Found");
            return null;
        }

		public static int GetTextureID(String textureName)
		{
			if (textureName != null)
			{
				foreach (Texture2D t in m_TextureList)
				{
					if (t.Name.Equals(textureName))
					{
						return m_TextureList.IndexOf(t);
					}
				}
            }
            throw new Exception(textureName + " Texture Not Found");
			return -1;
        }

        public static string[] GetTextureNames()
        {
            string[] textureNames = new string[m_TextureList.Count];
            for(int i = 0; i < m_TextureList.Count; i++) 
            {
                textureNames[i] = m_TextureList[i].Name;
            }
            return textureNames;
        }

        public static void ClearTextures()
        {
            m_TextureList.Clear();
        }
    }
}
