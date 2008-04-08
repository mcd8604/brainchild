using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Project_blob
{
    class SceneManager
    {
        private static volatile SceneManager _instance;
        private static object _syncRoot = new Object();

        private int _drawn = 0;
        public int Drawn
        {
            get { return _drawn; }
            set { _drawn = value; }
        }

        private int _culled = 0;
        public int Culled
        {
            get { return _culled; }
            set { _culled = value; }
        }

        private int _sceneObjectCount;
        public int SceneObjectCount
        {
            get { return _sceneObjectCount; }
        }

        private bool _cull = true;
        public bool Cull
        {
            get { return _cull; }
            set { _cull = value; }
        }

        private Octree _octree;
        public Octree Octree
        {
            get { return _octree; }
            set { _octree = value; }
        }

        public enum SceneGraphType
        {
            Octree = 0
        }

        private SceneGraphType _graphType = 0;
        public SceneGraphType GraphType
        {
            get { return _graphType; }
            set { _graphType = value; }
        }

        private Display _display = null;
        public Display Display
        {
            get { return _display; }
            set { _display = value; }
        }

        public SceneManager()
        {
            _octree = new Octree();
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

        public void UpdateVisibleDrawables(GameTime gameTime)
        {
            _drawn = 0;
            _culled = 0;

            if (_graphType == SceneGraphType.Octree)
            {
                if (_cull)
                {
                    //Draw will be replaced with Culling Draw
                    _octree.DrawVisible(gameTime);
                }
                else
                {
                    _octree.Draw(gameTime);
                }
            }
        }

        public void BuildOctree(ref List<Drawable> scene)
        {
            _sceneObjectCount = scene.Count;

            if (_graphType == SceneGraphType.Octree)
            {
                _octree.Distribute(ref scene);
            }
        }

        //public void AddObject(SceneObject sceneObject)
        //{
        //    SceneObjectNode node = new SceneObjectNode(sceneObject);
        //    _root.AddNode(node);
        //    _sceneObjectCount += 1;
        //}
    }
}
