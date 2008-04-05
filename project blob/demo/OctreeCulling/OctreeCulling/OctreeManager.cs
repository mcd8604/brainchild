using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OctreeCulling
{
    class OctreeManager
    {
        private static volatile OctreeManager _instance;
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
            set { _sceneObjectCount = value; }
        }

        private bool _cull = false;
        public bool Cull
        {
            get { return _cull; }
            set { _cull = value; }
        }

        /// <summary>
        /// The root of the scene graph
        /// </summary>
        //private static OctreeLeaf _root;
        //public static OctreeLeaf Root
        //{
        //    get { return _root; }
        //}

        private Octree _octree;
        public Octree Octree
        {
            get { return _octree; }
            set { _octree = value; }
        }

        public OctreeManager()
        {
            _octree = new Octree();
        }

        /*! Returns singleton instance of the OctreeManager */
        public static OctreeManager getSingleton
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new OctreeManager();
                    }
                }

                return _instance;
            }
        }

        public void Draw(GameTime gameTime)
        {
            _drawn = 0;
            _culled = 0;

            if (_cull)
            {
                //Draw will be replaced with Culling Draw
                //_root.CullDraw(gameTime);
                _octree.DrawVisible(gameTime);
            }
            else
            {
                //_root.Draw(gameTime);
                _octree.Draw(gameTime);
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
