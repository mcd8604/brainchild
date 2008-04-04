using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OctreeCulling
{
    class CameraManager
    {
        private static volatile CameraManager _instance;
        private static object _syncRoot = new Object();

        private SortedDictionary<string, Camera> _cameras;

        private Camera _activeCamera;
        public Camera ActiveCamera
        {
            get { return _activeCamera; }
        }

        public CameraManager()
        {
            _cameras = new SortedDictionary<string,Camera>();
        }

        /*! Returns singleton instance of the SceneManager */
        public static CameraManager getSingleton
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new CameraManager();
                    }
                }

                return _instance;
            }
        }

        public void Update(GameTime gameTime)
        {
            _activeCamera.Update(gameTime);
        }

        public void AddCamera(string cameraName, Camera camera)
        {
            if(!_cameras.ContainsKey(cameraName))
            {
                _cameras.Add(cameraName, camera);
            }
        }

        public void SetActiveCamera(string cameraName)
        {
            if(_cameras.ContainsKey(cameraName))
            {
                _activeCamera = _cameras[cameraName];
            }
        }

        public Camera GetCamera(string cameraName)
        {
            if (_cameras.ContainsKey(cameraName))
            {
                return _cameras[cameraName];
            }
            else
            {
                return null;
            }
        }
    }
}
