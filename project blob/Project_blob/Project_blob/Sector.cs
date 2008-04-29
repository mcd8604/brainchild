using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine;

namespace Project_blob
{
    class Sector
    {
        private List<Drawable> _sectorObjects;
        public List<Drawable> SectorObjects
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

        private int _sectorNumber;
        public int SectorNumber
        {
            get { return _sectorNumber; }
            set { _sectorNumber = value; }
        }

        //public VertexPositionColor[] BoundingFrustumDrawData = new VertexPositionColor[8];
        //public int[] BoundingFrustumIndex = new int[24];

        public Sector(int sectorNum)
        {
            _sectorObjects = new List<Drawable>();
            _portals = new List<Portal>();
            _sectorNumber = sectorNum;
        }

        //Used in testing
        private bool _drawPortal;
        public bool DrawPortal
        {
            get { return _drawPortal; }
            set { _drawPortal = value; }
        }

        public void DrawVisible(GameTime gameTime)
        {
            //BoundingFrustum frustum = CameraManager.getSingleton.ActiveCamera.Frustum;
            BoundingFrustum frustum = CameraManager.getSingleton.GetCamera("test").Frustum;

            foreach (Drawable obj in _sectorObjects)
            {
                ContainmentType type = frustum.Contains(obj.GetBoundingBox());

                switch (type)
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        {
                            //obj.Draw(gameTime);
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
                            BoundingFrustum newFrustum = CreatePortalFrustum(portal);

                            //drawFrustum(newFrustum);
                            _drawPortal = true;

                            //Drawvisible on connected sector
                            foreach (int sectorNum in portal.ConnectedSectors)
                            {
                                if (SceneManager.getSingleton.PortalScene.Sectors.ContainsKey(sectorNum) &&
                                    (sectorNum != SceneManager.getSingleton.PortalScene.CurrSector) && 
                                    (sectorNum != _sectorNumber))
                                {
                                    SceneManager.getSingleton.PortalScene.Sectors[sectorNum].DrawVisible(gameTime, newFrustum);
                                }
                            }
                        }
                        break;

                    case ContainmentType.Disjoint:
                        {
                            //Portal is not visible.
                            _drawPortal = false;
                        }
                        break;
                }
            }
        }

        public void DrawVisible(GameTime gameTime, BoundingFrustum frustum)
        {
            foreach (Drawable obj in _sectorObjects)
            {
                ContainmentType type = frustum.Contains(obj.GetBoundingBox());

                switch (type)
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        {
                            //obj.Draw(gameTime);
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
                            BoundingFrustum newFrustum = CreatePortalFrustum(portal);

                            //drawFrustum(newFrustum);
                            _drawPortal = true;

                            //Drawvisible on connected sector
                            foreach (int sectorNum in portal.ConnectedSectors)
                            {
                                if (SceneManager.getSingleton.PortalScene.Sectors.ContainsKey(sectorNum) &&
                                    (sectorNum != SceneManager.getSingleton.PortalScene.CurrSector) && 
                                    (sectorNum != _sectorNumber))
                                {
                                    SceneManager.getSingleton.PortalScene.Sectors[sectorNum].DrawVisible(gameTime, newFrustum);
                                }
                            }
                        }
                        break;

                    case ContainmentType.Disjoint:
                        {
                            //Portal is not visible.
                            _drawPortal = false;
                        }
                        break;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Drawable obj in _sectorObjects)
            {
                //obj.Draw(gameTime);
            }
        }

        public void AddObjectToSector(Drawable obj)
        {
            _sectorObjects.Add(obj);

            if (_containerBox.Min == _containerBox.Max)
            {
                _containerBox = obj.GetBoundingBox();
            }
            else
            {
                _containerBox = BoundingBox.CreateMerged(_containerBox, obj.GetBoundingBox());
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

        private BoundingFrustum CreatePortalFrustum(Portal portal)
        {
            //Create new frustum from portal
            BoundingBox box = portal.GetBoundingBoxTransformed();
           
            float fieldOfView, aspectRatio, nearPlane;
            Vector3 v1, v2;

            //v1 = box.Max - CameraManager.getSingleton.GetCamera("test").Position;
            //v2 = box.Min - CameraManager.getSingleton.GetCamera("test").Position;

            v1 = (new Vector3(box.Max.X, box.Max.Y, box.Min.Z)) - CameraManager.getSingleton.GetCamera("test").Position;
            v2 = (new Vector3(box.Min.X, box.Max.Y, box.Min.Z)) - CameraManager.getSingleton.GetCamera("test").Position;

            Vector3 temp = portal.Position - CameraManager.getSingleton.GetCamera("test").Position;
            nearPlane = temp.Length();
            //if (v1.Length() < v2.Length())
            //    nearPlane = v1.Length();
            //else
            //    nearPlane = v2.Length();

            v1 = Vector3.Normalize(v1);
            v2 = Vector3.Normalize(v2);

            fieldOfView = (float)Math.Acos(Vector3.Dot(v1, v2));
            //fieldOfView = (float)Math.Acos(Vector3.Dot(new Vector3(box.Max.X, box.Max.Y, box.Min.Z),
            //                                           new Vector3(box.Min.X, box.Max.Y, box.Min.Z)));

            aspectRatio = (box.Max.X - box.Min.X) /
                          (box.Max.Y - box.Min.Y);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView,
                aspectRatio,
                nearPlane,
                CameraManager.getSingleton.GetCamera("test").FarPlane);

            //Vector3 target = (((box.Max - box.Min) / 2) + box.Min) - CameraManager.getSingleton.GetCamera("test").Position;
            Vector3 target = portal.Position;// -CameraManager.getSingleton.GetCamera("test").Position;

            Matrix view = Matrix.CreateLookAt(CameraManager.getSingleton.GetCamera("test").Position,
                target, Vector3.Up);

            BoundingFrustum newFrustum = new BoundingFrustum(Matrix.Multiply(view, projection));

            //BoundingFrustum newFrustum = new BoundingFrustum(
            //    Matrix.Multiply(CameraManager.getSingleton.GetCamera("test").View, projection));
             

            //Vector3 topRight, bottomRight, bottomLeft, topLeft;

            //topRight = Vector3.Normalize(box.Max - CameraManager.getSingleton.GetCamera("test").Position);
            //bottomRight = Vector3.Normalize(new Vector3(box.Max.X, box.Min.Y, box.Min.Z) - CameraManager.getSingleton.GetCamera("test").Position);
            //bottomLeft = Vector3.Normalize(box.Min - CameraManager.getSingleton.GetCamera("test").Position);
            //topLeft = Vector3.Normalize(new Vector3(box.Min.X, box.Max.Y, box.Min.Z) - CameraManager.getSingleton.GetCamera("test").Position);

            //BoundingFrustum newFrustum = new BoundingFrustum(
            //newFrustum.

            return newFrustum;
        }

        /* Used for testing
        private void drawFrustum(BoundingFrustum frustum)
        {
            Vector3[] frustumPoints = new Vector3[8];
            frustumPoints = frustum.GetCorners();

            BoundingFrustumDrawData = new VertexPositionColor[8]
            {
                new VertexPositionColor(frustumPoints[0], Color.Blue),
                new VertexPositionColor(frustumPoints[1], Color.Blue),
                new VertexPositionColor(frustumPoints[2], Color.Blue),
                new VertexPositionColor(frustumPoints[3], Color.Blue),
                new VertexPositionColor(frustumPoints[4], Color.Blue),
                new VertexPositionColor(frustumPoints[5], Color.Blue),
                new VertexPositionColor(frustumPoints[6], Color.Blue),
                new VertexPositionColor(frustumPoints[7], Color.Blue),
            };

            BoundingFrustumIndex = new int[24];
            BoundingFrustumIndex[0] = 0;
            BoundingFrustumIndex[1] = 1;
            BoundingFrustumIndex[2] = 1;
            BoundingFrustumIndex[3] = 2;
            BoundingFrustumIndex[4] = 2;
            BoundingFrustumIndex[5] = 3;
            BoundingFrustumIndex[6] = 3;
            BoundingFrustumIndex[7] = 0;

            BoundingFrustumIndex[8] = 4;
            BoundingFrustumIndex[9] = 5;
            BoundingFrustumIndex[10] = 5;
            BoundingFrustumIndex[11] = 6;
            BoundingFrustumIndex[12] = 6;
            BoundingFrustumIndex[13] = 7;
            BoundingFrustumIndex[14] = 7;
            BoundingFrustumIndex[15] = 4;

            BoundingFrustumIndex[16] = 0;
            BoundingFrustumIndex[17] = 4;
            BoundingFrustumIndex[18] = 1;
            BoundingFrustumIndex[19] = 5;
            BoundingFrustumIndex[20] = 2;
            BoundingFrustumIndex[21] = 6;
            BoundingFrustumIndex[22] = 3;
            BoundingFrustumIndex[23] = 7;
        }
         * */
    }
}
