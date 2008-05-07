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

        public Sector(int sectorNum)
        {
            _sectorObjects = new List<Drawable>();
            _portals = new List<Portal>();
            _sectorNumber = sectorNum;
        }

        public void DrawVisible(GameTime gameTime)
        {
            BoundingFrustum frustum = CameraManager.getSingleton.ActiveCamera.Frustum;

            foreach (Drawable obj in _sectorObjects)
            {
                ContainmentType type = frustum.Contains(obj.GetBoundingBox());

                switch (type)
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        {
                            SceneManager.getSingleton.Display.AddToBeDrawn(obj);
                            SceneManager.getSingleton.Drawn += 1;
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
                //ContainmentType type = frustum.Contains(portal.GetBoundingBoxTransformed());
                ContainmentType type = frustum.Contains(portal.BoundingBox);

                switch (type)
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        {
                            //Create new frustum from portal
							//BoundingFrustum newFrustum = CreatePortalFrustum(portal);
							Frustum newFrustum = CreatePortalFrustum(portal);

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
                        }
                        break;
                }
            }
        }

        public void DrawVisible(GameTime gameTime, Frustum frustum)// BoundingFrustum frustum)
        {
            foreach (Drawable obj in _sectorObjects)
            {
                ContainmentType type = frustum.Contains(obj.GetBoundingBox());

                switch (type)
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        {
                            SceneManager.getSingleton.Display.AddToBeDrawn(obj);
                            SceneManager.getSingleton.Drawn += 1;
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
                //ContainmentType type = frustum.Contains(portal.GetBoundingBoxTransformed());
                ContainmentType type = frustum.Contains(portal.BoundingBox);

                switch (type)
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        {
                            //Create new frustum from portal
							//BoundingFrustum newFrustum = CreatePortalFrustum(portal);
							Frustum newFrustum = CreatePortalFrustum(portal);

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
                        }
                        break;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Drawable obj in _sectorObjects)
            {
                SceneManager.getSingleton.Display.AddToBeDrawn(obj);
                SceneManager.getSingleton.Drawn += 1;
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

		//private BoundingFrustum CreatePortalFrustum(Portal portal)
		private Frustum CreatePortalFrustum(Portal portal)
        {
            //Create new frustum from portal
            //BoundingBox box = portal.GetBoundingBoxTransformed();
            BoundingBox box = portal.BoundingBox;
           
			//float fieldOfView, aspectRatio, nearPlane;
			//Vector3 v1, v2;

			////v1 = box.Max - CameraManager.getSingleton.GetCamera("test").Position;
			////v2 = box.Min - CameraManager.getSingleton.GetCamera("test").Position;

			//v1 = (new Vector3(box.Max.X, box.Max.Y, box.Min.Z)) - CameraManager.getSingleton.ActiveCamera.Position;
			//v2 = (new Vector3(box.Min.X, box.Max.Y, box.Min.Z)) - CameraManager.getSingleton.ActiveCamera.Position;

			//Vector3 temp = portal.Position - CameraManager.getSingleton.ActiveCamera.Position;
			//nearPlane = temp.Length();
			////if (v1.Length() < v2.Length())
			////    nearPlane = v1.Length();
			////else
			////    nearPlane = v2.Length();

			//v1 = Vector3.Normalize(v1);
			//v2 = Vector3.Normalize(v2);

			//fieldOfView = (float)Math.Acos(Vector3.Dot(v1, v2));
			////fieldOfView = (float)Math.Acos(Vector3.Dot(new Vector3(box.Max.X, box.Max.Y, box.Min.Z),
			////                                           new Vector3(box.Min.X, box.Max.Y, box.Min.Z)));

			//aspectRatio = (box.Max.X - box.Min.X) /
			//              (box.Max.Y - box.Min.Y);

			//Matrix projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView,
			//    aspectRatio,
			//    nearPlane,
			//    CameraManager.getSingleton.ActiveCamera.FarPlane);

			////Vector3 target = (((box.Max - box.Min) / 2) + box.Min) - CameraManager.getSingleton.GetCamera("test").Position;
			//Vector3 target = portal.Position;// -CameraManager.getSingleton.GetCamera("test").Position;

			//Matrix view = Matrix.CreateLookAt(CameraManager.getSingleton.ActiveCamera.Position,
			//    target, Vector3.Up);

			//BoundingFrustum newFrustum = new BoundingFrustum(Matrix.Multiply(view, projection));

			Vector3 tl, tr, bl, br;
			Vector3 ntl, ntr, nbl, nbr, ftl, ftr, fbl, fbr;
			Vector3 offset = new Vector3(0.0f, 0.0f, 0.1f);

			tr = Vector3.Normalize(new Vector3(box.Max.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
			br = Vector3.Normalize(new Vector3(box.Max.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
			bl = Vector3.Normalize(new Vector3(box.Min.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
			tl = Vector3.Normalize(new Vector3(box.Min.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);

			ntl = offset + CameraManager.getSingleton.ActiveCamera.Position + (new Vector3(box.Min.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
			ntr = offset + CameraManager.getSingleton.ActiveCamera.Position + (new Vector3(box.Max.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
			nbl = offset + CameraManager.getSingleton.ActiveCamera.Position + (new Vector3(box.Min.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
			nbr = offset + CameraManager.getSingleton.ActiveCamera.Position + (new Vector3(box.Max.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
			ftl = offset + CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(tl, CameraManager.getSingleton.ActiveCamera.FarPlane);
			ftr = offset + CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(tr, CameraManager.getSingleton.ActiveCamera.FarPlane);
			fbl = offset + CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(bl, CameraManager.getSingleton.ActiveCamera.FarPlane);
			fbr = offset + CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(br, CameraManager.getSingleton.ActiveCamera.FarPlane);

			Frustum newFrustum = new Frustum(ntl, ntr, nbl, nbr, ftl, ftr, fbl, fbr);

            return newFrustum;
        }
    }
}
