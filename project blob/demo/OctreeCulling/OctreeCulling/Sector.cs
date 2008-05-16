using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        private int _sectorNumber;
        public int SectorNumber
        {
            get { return _sectorNumber; }
            set { _sectorNumber = value; }
        }

        public VertexPositionColor[] BoundingFrustumDrawData = new VertexPositionColor[8];
        public int[] BoundingFrustumIndex = new int[24];

        public Sector(int sectorNum)
        {
            _sectorObjects = new List<SceneObject>();
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

        private int drawingPortal;

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
                            //Drawvisible on connected sector
                            foreach (int sectorNum in portal.ConnectedSectors)
                            {
                                if (SceneManager.getSingleton.PortalScene.Sectors.ContainsKey(sectorNum) &&
                                    (sectorNum != SceneManager.getSingleton.PortalScene.CurrSector) &&
                                    (sectorNum != _sectorNumber) &&
                                    (sectorNum != SceneManager.getSingleton.PortalScene.PreviousRecursiveSector))
                                {
									BoundingFrustum newFrustum;

                                    //Create new frustum from portal
									if (type == ContainmentType.Contains)
									{
										newFrustum = CreatePortalFrustum(portal);
										//Frustum newFrustum = CreatePortalFrustum(portal);
									}
									else
									{
										newFrustum = CreateClippedPortalFrustum(portal);
									}

                                    drawingPortal = sectorNum;
                                    drawFrustum(newFrustum);
                                    _drawPortal = true;

									int prev = SceneManager.getSingleton.PortalScene.PreviousRecursiveSector;
									int curr = SceneManager.getSingleton.PortalScene.CurrentRecursiveSector;

                                    SceneManager.getSingleton.PortalScene.PreviousRecursiveSector = SceneManager.getSingleton.PortalScene.CurrentRecursiveSector;
                                    SceneManager.getSingleton.PortalScene.CurrentRecursiveSector = sectorNum;

                                    SceneManager.getSingleton.PortalScene.Sectors[sectorNum].DrawVisible(gameTime, newFrustum);

									SceneManager.getSingleton.PortalScene.CurrentRecursiveSector = curr;
									SceneManager.getSingleton.PortalScene.PreviousRecursiveSector = prev;
									//int temp = SceneManager.getSingleton.PortalScene.PreviousRecursiveSector;
									//SceneManager.getSingleton.PortalScene.PreviousRecursiveSector = SceneManager.getSingleton.PortalScene.CurrentRecursiveSector;
									//SceneManager.getSingleton.PortalScene.CurrentRecursiveSector = temp;
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
        //public void DrawVisible(GameTime gameTime, Frustum frustum)
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
                            //Drawvisible on connected sector
                            foreach (int sectorNum in portal.ConnectedSectors)
                            {
                                if (SceneManager.getSingleton.PortalScene.Sectors.ContainsKey(sectorNum) &&
                                    (sectorNum != SceneManager.getSingleton.PortalScene.CurrSector) &&
                                    (sectorNum != SceneManager.getSingleton.PortalScene.CurrentRecursiveSector) &&//_sectorNumber) &&
                                    (sectorNum != SceneManager.getSingleton.PortalScene.PreviousRecursiveSector))
                                {
									BoundingFrustum newFrustum;
									if (type == ContainmentType.Contains)
									{
										//Create new frustum from portal
										newFrustum = CreatePortalFrustum(portal);
										//Frustum newFrustum = CreatePortalFrustum(portal);
									}
									else
									{
										newFrustum = CreateClippedPortalFrustum(portal);
									}

                                    drawingPortal = sectorNum;
                                    drawFrustum(newFrustum);
                                    _drawPortal = true;

									int prev = SceneManager.getSingleton.PortalScene.PreviousRecursiveSector;
									int curr = SceneManager.getSingleton.PortalScene.CurrentRecursiveSector;

                                    SceneManager.getSingleton.PortalScene.PreviousRecursiveSector = SceneManager.getSingleton.PortalScene.CurrentRecursiveSector;
                                    SceneManager.getSingleton.PortalScene.CurrentRecursiveSector = sectorNum;

                                    SceneManager.getSingleton.PortalScene.Sectors[sectorNum].DrawVisible(gameTime, newFrustum);

									SceneManager.getSingleton.PortalScene.CurrentRecursiveSector = curr;
									SceneManager.getSingleton.PortalScene.PreviousRecursiveSector = prev;
									//int temp = SceneManager.getSingleton.PortalScene.PreviousRecursiveSector;
									//SceneManager.getSingleton.PortalScene.PreviousRecursiveSector = SceneManager.getSingleton.PortalScene.CurrentRecursiveSector;
									//SceneManager.getSingleton.PortalScene.CurrentRecursiveSector = temp;
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

		private BoundingFrustum CreatePortalFrustum(Portal portal)
        //private Frustum CreatePortalFrustum(Portal portal)
        {
            #region nonused code
            /*
			////Create new frustum from portal
			BoundingBox box = portal.GetBoundingBoxTransformed();

			float fieldOfView, aspectRatio;//, nearPlane;
            Vector3 v1, v2, v3, v4;
            bool rotate = false;

            float test1 = box.Max.X - box.Min.X;
            float test2 = box.Max.Z - box.Min.Z;

            if (test2 > test1)
            {
                v1 = new Vector3(box.Min.X, CameraManager.getSingleton.GetCamera("test").Position.Y, box.Max.Z);
                v2 = new Vector3(box.Min.X, CameraManager.getSingleton.GetCamera("test").Position.Y, box.Min.Z);
                //v1 = new Vector3(box.Min.X, box.Max.Y, box.Max.Z);
                //v2 = new Vector3(box.Min.X, box.Max.Y, box.Min.Z);
                rotate = true;
            }
            else
            {
                v1 = new Vector3(box.Max.X, CameraManager.getSingleton.GetCamera("test").Position.Y, box.Min.Z);
                v2 = new Vector3(box.Min.X, CameraManager.getSingleton.GetCamera("test").Position.Y, box.Min.Z);
                //v1 = new Vector3(box.Max.X, box.Max.Y, box.Min.Z);
                //v2 = new Vector3(box.Min.X, box.Max.Y, box.Min.Z);
            }

            v1 = v1 - CameraManager.getSingleton.GetCamera("test").Position;
            v2 = v2 - CameraManager.getSingleton.GetCamera("test").Position;

            //v1 = (new Vector3(box.Max.X, box.Max.Y, box.Min.Z)) - CameraManager.getSingleton.GetCamera("test").Position;
            //v2 = (new Vector3(box.Min.X, box.Max.Y, box.Min.Z)) - CameraManager.getSingleton.GetCamera("test").Position;

            //Vector3 temp = portal.Position - CameraManager.getSingleton.GetCamera("test").Position;
            //nearPlane = temp.Length();

			//if (v1.Length() < v2.Length())
			//    nearPlane = v1.Length();
			//else
			//    nearPlane = v2.Length();

			v1 = Vector3.Normalize(v1);
			v2 = Vector3.Normalize(v2);

			fieldOfView = (float)Math.Acos(Vector3.Dot(v1, v2));
			//fieldOfView = (float)Math.Acos(Vector3.Dot(new Vector3(box.Max.X, box.Max.Y, box.Min.Z),
			//                                           new Vector3(box.Min.X, box.Max.Y, box.Min.Z)));

            //if (CameraManager.getSingleton.GetCamera("test").FieldOfView < fieldOfView)
            //    fieldOfView = CameraManager.getSingleton.GetCamera("test").FieldOfView;

            if (fieldOfView < 0.005)
                fieldOfView = 0.01f;

            if (test2 > test1)
            {
                v1 = new Vector3(portal.Position.X, portal.Position.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position;//CameraManager.getSingleton.GetCamera("test").Position.Y
                v2 = new Vector3(portal.Position.X, portal.Position.Y, box.Min.Z) - CameraManager.getSingleton.GetCamera("test").Position;
            }
            else
            {
                v1 = new Vector3(box.Max.X, portal.Position.Y, portal.Position.Z) - CameraManager.getSingleton.GetCamera("test").Position;
                v2 = new Vector3(box.Min.X, portal.Position.Y, portal.Position.Z) - CameraManager.getSingleton.GetCamera("test").Position;
            }
            v1 = Vector3.Normalize(v1);
            v2 = Vector3.Normalize(v2);

            v3 = new Vector3(portal.Position.X, box.Max.Y, portal.Position.Z) - CameraManager.getSingleton.GetCamera("test").Position;
            v4 = new Vector3(portal.Position.X, box.Min.Y, portal.Position.Z) - CameraManager.getSingleton.GetCamera("test").Position;
            v3 = Vector3.Normalize(v3);
            v4 = Vector3.Normalize(v4);
            Plane near = CameraManager.getSingleton.GetCamera("test").Frustum.Near;
            Vector3 pt1, pt2, pt3;
            if (test2 > test1)
            {
                pt1 = box.Min;
                pt2 = new Vector3(box.Min.X, box.Min.Y, box.Max.Z);
                pt3 = new Vector3(box.Min.X, box.Max.Y, box.Max.Z);
            }
            else
            {
                pt1 = box.Min;
                pt2 = new Vector3(box.Max.X, box.Min.Y, box.Min.Z);
                pt3 = new Vector3(box.Max.X, box.Max.Y, box.Min.Z);
            }
            near = new Plane(pt1, pt2, pt3);
            Vector3 top, bottom, left, right;
            top = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(v3, PlaneIntersectPt(near, v3));
            bottom = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(v4, PlaneIntersectPt(near, v4));
            left = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(v2, PlaneIntersectPt(near, v2));
            right = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(v1, PlaneIntersectPt(near, v1));

            if (rotate)
            {
                aspectRatio = (box.Max.Z - box.Min.Z) /
                              (box.Max.Y - box.Min.Y);
            }
            else
            {
                aspectRatio = (box.Max.X - box.Min.X) /
                              (box.Max.Y - box.Min.Y);
            }

            //aspectRatio = (box.Max.X - box.Min.X) /
            //              (box.Max.Y - box.Min.Y);
            float leftX, rightX;
            if (test2 > test1)
            {
                leftX = left.Z;
                rightX = right.Z;
            }
            else
            {
                leftX = left.X;
                rightX = right.X;
            }
             * */
            #endregion

            Vector3 nearDistance = portal.Position - CameraManager.getSingleton.GetCamera("test").Position; 

            float minScaleX, maxScaleX;
            if (portal.Scale.Z > portal.Scale.X)
            {
                minScaleX = -portal.Scale.Z;
                maxScaleX = portal.Scale.Z;
            }
            else
            {
                minScaleX = -portal.Scale.X;
                maxScaleX = portal.Scale.X;
            }
            Matrix projection = Matrix.CreatePerspectiveOffCenter(minScaleX, maxScaleX, -portal.Scale.Y, portal.Scale.Y,//v2.X, v1.X, v4.Y, v3.Y,
                nearDistance.Length(),//CameraManager.getSingleton.GetCamera("test").NearPlane,
                CameraManager.getSingleton.GetCamera("test").FarPlane);
            //Matrix projection = Matrix.CreatePerspectiveOffCenter(leftX, rightX, bottom.Y, top.Y,//v2.X, v1.X, v4.Y, v3.Y,
            //    nearDistance.Length(),//CameraManager.getSingleton.GetCamera("test").NearPlane,
            //    CameraManager.getSingleton.GetCamera("test").FarPlane);

            //Matrix projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView,
            //    aspectRatio,
            //    CameraManager.getSingleton.GetCamera("test").NearPlane,//nearPlane,
            //    CameraManager.getSingleton.GetCamera("test").FarPlane);

            //Vector3 target = portal.Position;// -CameraManager.getSingleton.GetCamera("test").Position;

			Matrix view = Matrix.CreateLookAt(CameraManager.getSingleton.GetCamera("test").Position,
				portal.Position, Vector3.Up);

			BoundingFrustum newFrustum = new BoundingFrustum(Matrix.Multiply(view, projection));

            #region nonused code
            /*
			Vector3 tl, tr, bl, br;
			Vector3 ntl, ntr, nbl, nbr, ftl, ftr, fbl, fbr;

			tr = Vector3.Normalize(new Vector3(box.Max.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			br = Vector3.Normalize(new Vector3(box.Max.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			bl = Vector3.Normalize(new Vector3(box.Min.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			tl = Vector3.Normalize(new Vector3(box.Min.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);

            //Plane far = new Plane(new Vector4(0, 0, -1, 
            //    (CameraManager.getSingleton.GetCamera("test").Position.Z + CameraManager.getSingleton.GetCamera("test").FarPlane)));
            Plane far = CameraManager.getSingleton.GetCamera("test").Frustum.Far;
            Vector3[] corners = new Vector3[8];
            corners = CameraManager.getSingleton.GetCamera("test").Frustum.GetCorners();
            Vector3 frustumTL, frustumTR, frustumBL, frustumBR;
            frustumTR = Vector3.Normalize(corners[4] - corners[0]);
            frustumTL = Vector3.Normalize(corners[5] - corners[1]);
            frustumBL = Vector3.Normalize(corners[6] - corners[2]);
            frustumBR = Vector3.Normalize(corners[7] - corners[3]);

            float test1 = Vector3.Dot(far.Normal, frustumTR);
            float test2 = Vector3.Dot(far.Normal, tr);
            if (test1 > test2)
            {
                tr = frustumTR;
            }
            test1 = Vector3.Dot(far.Normal, frustumTL);
            test2 = Vector3.Dot(far.Normal, tl);
            if (test1 > test2)
            {
                tl = frustumTL;
            }
            test1 = Vector3.Dot(far.Normal, frustumBL);
            test2 = Vector3.Dot(far.Normal, bl);
            if (test1 > test2)
            {
                bl = frustumBL;
            }
            test1 = Vector3.Dot(far.Normal, frustumBR);
            test2 = Vector3.Dot(far.Normal, br);
            if (test1 > test2)
            {
                br = frustumBR;
            }


            ftl = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(tl, PlaneIntersectPt(far, tl));
            ftr = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(tr, PlaneIntersectPt(far, tr));
            fbl = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(bl, PlaneIntersectPt(far, bl));
            fbr = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(br, PlaneIntersectPt(far, br));

			ntl = CameraManager.getSingleton.GetCamera("test").Position + (new Vector3(box.Min.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			ntr = CameraManager.getSingleton.GetCamera("test").Position + (new Vector3(box.Max.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			nbl = CameraManager.getSingleton.GetCamera("test").Position + (new Vector3(box.Min.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			nbr = CameraManager.getSingleton.GetCamera("test").Position + (new Vector3(box.Max.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
            //ftl = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(tl, CameraManager.getSingleton.GetCamera("test").FarPlane);
            //ftr = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(tr, CameraManager.getSingleton.GetCamera("test").FarPlane);
            //fbl = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(bl, CameraManager.getSingleton.GetCamera("test").FarPlane);
            //fbr = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(br, CameraManager.getSingleton.GetCamera("test").FarPlane);

			Frustum newFrustum = new Frustum(ntl, ntr, nbl, nbr, ftl, ftr, fbl, fbr);
            */
            #endregion

            return newFrustum;
        }

		private BoundingFrustum CreateClippedPortalFrustum(Portal portal)
		{
			BoundingBox box = portal.GetBoundingBoxTransformed();
			Vector3 min = Vector3.Transform(box.Min, CameraManager.getSingleton.GetCamera("test").View);
			Vector3 max = Vector3.Transform(box.Max, CameraManager.getSingleton.GetCamera("test").View);

			return CameraManager.getSingleton.GetCamera("test").Frustum;


			//Vector3 nearDistance = portal.Position - CameraManager.getSingleton.GetCamera("test").Position;

			//float minScaleX, maxScaleX;
			//if (portal.Scale.Z > portal.Scale.X)
			//{
			//    minScaleX = -portal.Scale.Z;
			//    maxScaleX = portal.Scale.Z;
			//}
			//else
			//{
			//    minScaleX = -portal.Scale.X;
			//    maxScaleX = portal.Scale.X;
			//}
			//Matrix projection = Matrix.CreatePerspectiveOffCenter(minScaleX, maxScaleX, -portal.Scale.Y, portal.Scale.Y,
			//    nearDistance.Length(),
			//    CameraManager.getSingleton.GetCamera("test").FarPlane);

			//Matrix view = Matrix.CreateLookAt(CameraManager.getSingleton.GetCamera("test").Position,
			//    portal.Position, Vector3.Up);

			//BoundingFrustum newFrustum = new BoundingFrustum(Matrix.Multiply(view, projection));
			/*
			Vector3 nearDistance = portal.Position - CameraManager.getSingleton.GetCamera("test").Position;
			float test1, test2;
			Plane portalPlane;
			Vector3[] corners = new Vector3[8];
			Vector3 pt1, pt2, pt3;
			BoundingBox box = portal.GetBoundingBoxTransformed();
			test1 = box.Max.X - box.Min.X;
			test2 = box.Max.Z - box.Min.Z;
			
			if(test1 > test2)
			{
				pt1 = new Vector3(box.Min.X, box.Min.Y, box.Min.Z);
				pt2 = new Vector3(box.Max.X, box.Min.Y, box.Min.Z);
				pt3 = new Vector3(box.Max.X, box.Max.Y, box.Min.Z);
			}
			else
			{
				pt1 = new Vector3(box.Min.X, box.Min.Y, box.Min.Z);
				pt2 = new Vector3(box.Min.X, box.Min.Y, box.Max.Z);
				pt3 = new Vector3(box.Min.X, box.Max.Y, box.Max.Z);
			}
			portalPlane = new Plane(pt1, pt2, pt3);
			
			corners = CameraManager.getSingleton.GetCamera("test").Frustum.GetCorners();
			Vector3 frustumTL, frustumTR, frustumBL, frustumBR;
			frustumTR = Vector3.Normalize(corners[4] - corners[0]);
			frustumTL = Vector3.Normalize(corners[5] - corners[1]);
			frustumBL = Vector3.Normalize(corners[6] - corners[2]);
			frustumBR = Vector3.Normalize(corners[7] - corners[3]);

			float tlIntersect, trIntersect, blIntersect, brIntersect;
			
			tlIntersect = PlaneIntersectPt(portalPlane, frustumTL);
			trIntersect = PlaneIntersectPt(portalPlane, frustumTR);
			blIntersect = PlaneIntersectPt(portalPlane, frustumBL);
			brIntersect = PlaneIntersectPt(portalPlane, frustumBR);

			if (tlIntersect < 0 || trIntersect < 0 || blIntersect < 0 || brIntersect < 0)
			{
				return CameraManager.getSingleton.GetCamera("test").Frustum;
			}
			else
			{
				Vector3 tlIntersectPt, trIntersectPt, blIntersectPt, brIntersectPt;
				tlIntersectPt = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(frustumTL, tlIntersect);
				trIntersectPt = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(frustumTR, trIntersect);
				blIntersectPt = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(frustumBL, blIntersect);
				brIntersectPt = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(frustumBR, brIntersect);

				float minX, maxX, minY, maxY;
				if (nearDistance.Z < 0)
				{
					if (tlIntersectPt.X < box.Max.X)
					{
						maxX = tlIntersectPt.X;
					}
					else
					{
						maxX = box.Max.X;
					}

					if (trIntersectPt.X > box.Min.X)
					{
						minX = trIntersectPt.X;
					}
					else
					{
						minX = box.Min.X;
					}
				}
				else
				{
					if (trIntersectPt.X < box.Max.X)
					{
						maxX = trIntersectPt.X;
					}
					else
					{
						maxX = box.Max.X;
					}

					if (tlIntersectPt.X > box.Min.X)
					{
						minX = tlIntersectPt.X;
					}
					else
					{
						minX = box.Min.X;
					}
				}

				if (trIntersectPt.Y < box.Max.Y)
				{
					maxY = trIntersectPt.Y;
				}
				else
				{
					maxY = box.Max.Y;
				}

				if (brIntersectPt.Y > box.Min.Y)
				{
					minY = brIntersectPt.Y;
				}
				else
				{
					minY = box.Min.Y;
				}

				Vector3 min, max;

				min = new Vector3(minX, minY, pt1.Z);
				max = new Vector3(maxX, maxY, pt1.Z);

				Vector3 centerPt = Vector3.Divide((max - min), 2);
				centerPt = min + centerPt;
				//nearDistance = centerPt - CameraManager.getSingleton.GetCamera("test").Position;

				float minScaleX, maxScaleX, minScaleY, maxScaleY;
				maxScaleX = (max.X - min.X) / 2;
				minScaleX = -maxScaleX;
				maxScaleY = (max.Y - min.Y) / 2;
				minScaleY = -maxScaleY;

				Matrix projection = Matrix.CreatePerspectiveOffCenter(minScaleX, maxScaleX, minScaleY, maxScaleY,
					nearDistance.Length(), //centerPt.Length(),
					CameraManager.getSingleton.GetCamera("test").FarPlane);

				Matrix view = Matrix.CreateLookAt(CameraManager.getSingleton.GetCamera("test").Position,
					centerPt, Vector3.Up);

				BoundingFrustum newFrustum = new BoundingFrustum(Matrix.Multiply(view, projection));

				return newFrustum;
			}
			 * */
		}

        private float PlaneIntersectPt(Plane p, Vector3 ray)
        {
            float denom = Vector3.Dot(p.Normal, ray);
			if (denom == 0)
			{
				return -1;
			}
			else
			{
				float nom = Vector3.Dot(p.Normal, CameraManager.getSingleton.GetCamera("test").Position) + p.D;
				float t = -(nom / denom);
				if (t >= 0)
				{
					return t;
				}
				else
				{
					return -1;
				}
			}
        }

        private void drawFrustum(BoundingFrustum frustum)
        //private void drawFrustum(Frustum frustum)
        {
            Vector3[] frustumPoints = new Vector3[8];
            frustumPoints = frustum.GetCorners();

            Color color;
            if (drawingPortal == 1)
                color = Color.Blue;
            else if (drawingPortal == 2)
                color = Color.Green;
            else if (drawingPortal == 3)
                color = Color.Brown;
            else if (drawingPortal == 4)
                color = Color.Orange;
            else
                color = Color.Black;

            BoundingFrustumDrawData = new VertexPositionColor[8]
            {
                new VertexPositionColor(frustumPoints[0], color),// Color.Blue),
                new VertexPositionColor(frustumPoints[1], color),// Color.Blue),
                new VertexPositionColor(frustumPoints[2], color),// Color.Blue),
                new VertexPositionColor(frustumPoints[3], color),// Color.Blue),
                new VertexPositionColor(frustumPoints[4], color),// Color.Blue),
                new VertexPositionColor(frustumPoints[5], color),// Color.Blue),
                new VertexPositionColor(frustumPoints[6], color),// Color.Blue),
                new VertexPositionColor(frustumPoints[7], color),// Color.Blue),
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

            ////Draw temporary frustum
            BasicEffect basicEffect = SceneManager.getSingleton.PortalScene.Sectors[1]._sectorObjects[0].Effect;
            GraphicsDeviceManager graphics = SceneManager.getSingleton.PortalScene.Sectors[1]._sectorObjects[0].Graphics;

            basicEffect.World = Matrix.Identity;
            basicEffect.View = CameraManager.getSingleton.ActiveCamera.View;
            basicEffect.Projection = CameraManager.getSingleton.ActiveCamera.Projection;
            basicEffect.VertexColorEnabled = true;

            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                // Begin the current pass
                pass.Begin();

                graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList, BoundingFrustumDrawData,
                    0, 8, BoundingFrustumIndex, 0, 12);

                //graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                //    PrimitiveType.LineList, CameraManager.getSingleton.ActiveCamera.BoundingFrustumDrawData,
                //    0, 8, CameraManager.getSingleton.ActiveCamera.BoundingFrustumIndex, 0, 12);

                // End the current pass
                pass.End();
            }
            basicEffect.End(); 
        }
    }
}
