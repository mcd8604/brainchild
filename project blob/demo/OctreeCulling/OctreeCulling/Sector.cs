using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OctreeCulling
{
    class Sector
    {
        private List<SceneObject> _sectorObjects;
        public List<SceneObject> SectorObjects
        {
            get { return _sectorObjects; }
            set { _sectorObjects = value; }
        }

        private BoundingBox _containerBox;
        public BoundingBox ContainerBox
        {
            get { return _containerBox; }
            set { _containerBox = value; }
        }

        public Sector()
        {
            _sectorObjects = new List<SceneObject>();
        }

        public void DrawVisible(GameTime gameTime)
        {
            //BoundingFrustum frustum = CameraManager.getSingleton.ActiveCamera.Frustum;
            BoundingFrustum frustum = CameraManager.getSingleton.GetCamera("test").Frustum;

            foreach (SceneObject obj in _sectorObjects)
            {
                ContainmentType type = frustum.Contains(obj.GetBoundingBoxTransformed());

                switch (type)
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        {
                            obj.Draw(gameTime);
                        }
                        break;

                    case ContainmentType.Disjoint:
                        {
                            //Object is culled.
                        }
                        break;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (SceneObject obj in _sectorObjects)
            {
                obj.Draw(gameTime);
            }
        }

        public void AddObjectToSector(SceneObject obj)
        {
            _sectorObjects.Add(obj);

            if (_containerBox.Min == _containerBox.Max)
            {
                _containerBox = obj.GetBoundingBoxTransformed();
            }
            else
            {
                _containerBox = BoundingBox.CreateMerged(_containerBox, obj.GetBoundingBoxTransformed());
            }
        }
    }
}
