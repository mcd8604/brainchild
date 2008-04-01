using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace OctreeCulling
{
    class SceneManager
    {
        private static volatile SceneManager _instance;
        private static object _syncRoot = new Object();

        /// <summary>
        /// The root of the scene graph
        /// </summary>
        private static Node _root;
        public static Node Root
        {
            get { return _root; }
        }

        public SceneManager()
        {
            _root = new Node();
        }

        /*! Returns singleton instance of the SceneManager */
        public static SceneManager getSingleton
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new SceneManager();
                    }
                }

                return _instance;
            }
        }

        public void Draw(GameTime gameTime)
        {
            _root.Draw(gameTime);
        }

        public void AddObject(SceneObject sceneObject)
        {
            SceneObjectNode node = new SceneObjectNode(sceneObject);
            _root.AddNode(node);
        }
    }
}
