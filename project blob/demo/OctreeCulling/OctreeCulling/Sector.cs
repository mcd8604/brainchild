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

		private List<Portal> _portals;
		public List<Portal> Portals
		{
			get { return _portals; }
			set { _portals = value; }
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
            _portals = new List<Portal>();
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

            foreach (Portal portal in _portals)
            {
                ContainmentType type = frustum.Contains(portal.GetBoundingBoxTransformed());

                switch (type)
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        {
                            //Create new frustum from portal
                            BoundingFrustum newFrustum;

                            //Drawvisible on connected sector
                            foreach (int sectorNum in portal.ConnectedSectors)
                            {
                                //sector.DrawVisible(gameTime, newFrustum);
                            }
                        }
                        break;

                    case ContainmentType.Disjoint:
                        {
                            //Portal is not visible.
                        }
                        break;
                }
            }
        }

        public void DrawVisible(GameTime gameTime, BoundingFrustum frustum)
        {
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

            foreach (Portal portal in _portals)
            {
                ContainmentType type = frustum.Contains(portal.GetBoundingBoxTransformed());

                switch (type)
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        {
                            //Create new frustum from portal
                            BoundingBox box = portal.GetBoundingBoxTransformed();
                            float fieldOfView, aspectRatio, nearPlane;
                            Vector3 v1, v2;

                            v1 = box.Max - CameraManager.getSingleton.GetCamera("test").Position;
                            v2 = box.Min - CameraManager.getSingleton.GetCamera("test").Position;

                            if (v1.Length() < v2.Length())
                                nearPlane = v1.Length();
                            else
                                nearPlane = v2.Length();

                            v1 = Vector3.Normalize(v1);
                            v2 = Vector3.Normalize(v2);
                            
                            fieldOfView = (float)Math.Acos(Vector3.Dot(v1, v2));

                            aspectRatio = (box.Max.X - box.Min.X) /
                                          (box.Max.Y - box.Min.Y);
                                                        
                            Matrix projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, 
                                aspectRatio, 
                                nearPlane, 
                                CameraManager.getSingleton.GetCamera("test").FarPlane);

                            BoundingFrustum newFrustum = new BoundingFrustum(
                                Matrix.Multiply(CameraManager.getSingleton.GetCamera("test").View, projection));

                            //Drawvisible on connected sector
                            foreach (int sectorNum in portal.ConnectedSectors)
                            {
                                if (SceneManager.getSingleton.PortalScene.Sectors.ContainsKey(sectorNum))
                                {
                                    SceneManager.getSingleton.PortalScene.Sectors[sectorNum].DrawVisible(gameTime, newFrustum);
                                }
                                //sector.DrawVisible(gameTime, newFrustum);
                            }
                        }
                        break;

                    case ContainmentType.Disjoint:
                        {
                            //Portal is not visible.
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

        public void AddPortalToSector(Portal portal)
        {
            _portals.Add(portal);

            //May not need to do this checking and merging if portals aren't used in the bounding boxes
            // of the sector
            //if (_containerBox.Min == _containerBox.Max)
            //{
            //    _containerBox = obj.GetBoundingBoxTransformed();
            //}
            //else
            //{
            //    _containerBox = BoundingBox.CreateMerged(_containerBox, obj.GetBoundingBoxTransformed());
            //}
        }
    }
}
