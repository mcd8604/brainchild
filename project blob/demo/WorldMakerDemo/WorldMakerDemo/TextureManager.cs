using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WorldMakerDemo
{
    class TextureManager
    {
        // Objects to ensure singleton design
        private static volatile TextureManager _instance;
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
            if (_textures.ContainsKey(textureName))
            {
                return _textures[textureName];
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
        }
    }
}
