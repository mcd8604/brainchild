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

        private bool _cull = true;//false;
        public bool Cull
        {
            get { return _cull; }
            set { _cull = value; }
        }

        /// <summary>
        /// The root of the scene graph
        /// </summary>
        //private static Node _root;
        //public static Node Root
        //{
        //    get { return _root; }
        //}

        private Octree _octree;
        public Octree Octree
        {
            get { return _octree; }
            set { _octree = value; }
        }

        public enum SceneGraphType
        {
            Octree = 0,
            Portal = 1
        }

        private SceneGraphType _graphType = 0;
        public SceneGraphType GraphType
        {
            get { return _graphType; }
            set { _graphType = value; }
        }

        private PortalScene _portalScene;
        public PortalScene PortalScene
        {
            get { return _portalScene; }
            set { _portalScene = value; }
        }

        public SceneManager()
        {
            //_root = new Node();
            _octree = new Octree();
            _portalScene = new PortalScene();
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
            _drawn = 0;
            _culled = 0;

            if (_graphType == SceneGraphType.Octree)
            {
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
            else if (_graphType == SceneGraphType.Portal)
            {
                if (_cull)
                {
                    _portalScene.DrawVisible(gameTime);
                }
                else
                {
                    _portalScene.Draw(gameTime);
                }
            }
        }

        public void Distribute(List<SceneObject> scene)
        {
            _sceneObjectCount = scene.Count;

            if (_graphType == SceneGraphType.Octree)
            {
                _octree.Distribute(scene);
            }
            else if (_graphType == SceneGraphType.Portal)
            {
                _portalScene.DistributeDrawableObjects(scene);
            }
        }

        public void DistributePortals(List<Portal> portals)
        {
            //_sceneObjectCount = portals.Count;

            //if (_graphType == SceneGraphType.Octree)
            //{
            //    _octree.Distribute(portals);
            //}
            //else 
            if (_graphType == SceneGraphType.Portal)
            {
                _portalScene.DistributePortals(portals);
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
