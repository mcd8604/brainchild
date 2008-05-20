using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using Engine;

namespace Project_blob
{
    public class SceneManager
    {
        private static volatile SceneManager _instance;
        private static object _syncRoot = new Object();

#if DEBUG
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
#endif

        private int _sceneObjectCount;
        public int SceneObjectCount
        {
            get { return _sceneObjectCount; }
        }

        private bool _cull = false;
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
            Octree = 0,
            Portal = 1
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

        private PortalScene _portalScene;
        public PortalScene PortalScene
        {
            get { return _portalScene; }
            set { _portalScene = value; }
        }

        public SceneManager()
        {
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
#if DEBUG
            _drawn = 0;
            _culled = 0;
#endif

            if (_graphType == SceneGraphType.Octree)
            {
                if (_cull)
                {
                    BoundingFrustum frustum = CameraManager.getSingleton.ActiveCamera.Frustum;

                    if ((frustum.Contains(_octree.ContainerBox) == ContainmentType.Contains) ||
                        (frustum.Contains(_octree.ContainerBox) == ContainmentType.Intersects))
                    {
                        //Draw will be replaced with Culling Draw
                        _octree.DrawVisible(gameTime);
                    }
                }
                else
                {
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

        public void BuildOctree(ref List<Drawable> scene)
        {
            _sceneObjectCount = scene.Count;

            _octree = new Octree();

            if (_graphType == SceneGraphType.Octree)
            {
                _octree.Distribute(ref scene);
            }
        }

        public void BuildPortalScene(List<Drawable> scene)
        {
            _sceneObjectCount = scene.Count;

            if (_graphType == SceneGraphType.Portal)
            {
                _portalScene.DistributeDrawableObjects(scene);
            }
        }

        public void BuildPortalScene(List<Drawable> scene, List<Portal> portals)
        {
            _sceneObjectCount = scene.Count;
            
            _portalScene = new PortalScene();

            if (_graphType == SceneGraphType.Portal)
            {
                _portalScene.DistributeDrawableObjects(scene);

                _portalScene.DistributePortals(portals);
            }
        }
    }
}
