using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine;

namespace Project_blob
{
	internal class Sector
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
							//SceneManager.getSingleton.Display.SetDrawn(obj);
                            obj.Drawn = true;
#if DEBUG
                            ++SceneManager.getSingleton.Drawn;
#endif
						}
						break;

					case ContainmentType.Disjoint:
						{
							//Object is culled.
                            //obj.Drawn = true;
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
							//Drawvisible on connected sector
							foreach (int sectorNum in portal.ConnectedSectors)
							{
								if (SceneManager.getSingleton.PortalScene.Sectors.ContainsKey(sectorNum) &&
									(sectorNum != SceneManager.getSingleton.PortalScene.CurrSector) &&
									(sectorNum != SceneManager.getSingleton.PortalScene.CurrentRecursiveSector) && //_sectorNumber
									(sectorNum != SceneManager.getSingleton.PortalScene.PreviousRecursiveSector))
								{
                                    BoundingFrustum newFrustum;

                                    /*TEST FURTHER!!!
									//Create new frustum from portal
                                    if (type == ContainmentType.Contains)
                                    {
                                        newFrustum = CreatePortalFrustum(portal);
                                        //Frustum newFrustum = CreatePortalFrustum(portal);
                                        //BoundingFrustum newFrustum = CameraManager.getSingleton.ActiveCamera.Frustum;
                                    }
                                    else
                                    {
                                        newFrustum = CreateClippedPortalFrustum(portal);
                                    }
                                     * */

                                    newFrustum = CameraManager.getSingleton.ActiveCamera.Frustum;

									//drawFrustum(newFrustum);
                                                                        
									int prev = SceneManager.getSingleton.PortalScene.PreviousRecursiveSector;
									int curr = SceneManager.getSingleton.PortalScene.CurrentRecursiveSector;

									SceneManager.getSingleton.PortalScene.PreviousRecursiveSector = SceneManager.getSingleton.PortalScene.CurrentRecursiveSector;
									SceneManager.getSingleton.PortalScene.CurrentRecursiveSector = sectorNum;

									SceneManager.getSingleton.PortalScene.Sectors[sectorNum].DrawVisible(gameTime, newFrustum);

									//TEST THIS
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
						}
						break;
				}
			}
		}

		public void DrawVisible(GameTime gameTime, BoundingFrustum frustum)//Frustum frustum)
		{
			foreach (Drawable obj in _sectorObjects)
			{
				ContainmentType type = frustum.Contains(obj.GetBoundingBox());

				switch (type)
				{
					case ContainmentType.Contains:
					case ContainmentType.Intersects:
						{
							//SceneManager.getSingleton.Display.SetDrawn(obj);
                            obj.Drawn = true;
#if DEBUG
                            ++SceneManager.getSingleton.Drawn;
#endif
						}
						break;

					case ContainmentType.Disjoint:
						{
							//Object is culled.
                            //obj.Drawn = false;
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
							//Drawvisible on connected sector
							foreach (int sectorNum in portal.ConnectedSectors)
							{
								if (SceneManager.getSingleton.PortalScene.Sectors.ContainsKey(sectorNum) &&
									(sectorNum != SceneManager.getSingleton.PortalScene.CurrSector) &&
									(sectorNum != SceneManager.getSingleton.PortalScene.CurrentRecursiveSector) &&//_sectorNumber
									(sectorNum != SceneManager.getSingleton.PortalScene.PreviousRecursiveSector))
								{
                                    BoundingFrustum newFrustum;

                                    /*TEST FURTHER!!!
									//Create new frustum from portal
                                    if (type == ContainmentType.Contains)
                                    {
                                        newFrustum = CreatePortalFrustum(portal);
                                        //Frustum newFrustum = CreatePortalFrustum(portal);
                                        //BoundingFrustum newFrustum = CameraManager.getSingleton.ActiveCamera.Frustum;
                                    }
                                    else
                                    {
                                        newFrustum = CreateClippedPortalFrustum(portal);
                                    }
                                    * */

                                    newFrustum = CameraManager.getSingleton.ActiveCamera.Frustum;

									//drawFrustum(newFrustum);

									int prev = SceneManager.getSingleton.PortalScene.PreviousRecursiveSector;
									int curr = SceneManager.getSingleton.PortalScene.CurrentRecursiveSector;

									SceneManager.getSingleton.PortalScene.PreviousRecursiveSector = SceneManager.getSingleton.PortalScene.CurrentRecursiveSector;
									SceneManager.getSingleton.PortalScene.CurrentRecursiveSector = sectorNum;

									SceneManager.getSingleton.PortalScene.Sectors[sectorNum].DrawVisible(gameTime, newFrustum);

									//TEST THIS
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
						}
						break;
				}
			}
		}

		public void Draw(GameTime gameTime)
		{
			foreach (Drawable obj in _sectorObjects)
			{
				//SceneManager.getSingleton.Display.SetDrawn(obj);
                obj.Drawn = true;
#if DEBUG
                ++SceneManager.getSingleton.Drawn;
#endif
			}
		}

		public void AddObjectToSector(Drawable obj)
		{
			_sectorObjects.Add(obj);
            BoundingBox box = obj.GetBoundingBox();

            if (box.Max == box.Min) {
                throw new Exception("Invalid Bounding Box");
            }

			if (_containerBox.Min == _containerBox.Max)
			{
				_containerBox = box;
			}
			else
			{
				_containerBox = BoundingBox.CreateMerged(_containerBox, box);
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
            #region old code
            /*
			//Create new frustum from portal
			//BoundingBox box = portal.GetBoundingBoxTransformed();
			BoundingBox box = portal.BoundingBox;

			float fieldOfView, aspectRatio, nearPlane;
			Vector3 v1, v2;

			//v1 = box.Max - CameraManager.getSingleton.GetCamera("test").Position;
			//v2 = box.Min - CameraManager.getSingleton.GetCamera("test").Position;

			v1 = (new Vector3(box.Max.X, box.Max.Y, box.Min.Z)) - CameraManager.getSingleton.ActiveCamera.Position;
			v2 = (new Vector3(box.Min.X, box.Max.Y, box.Min.Z)) - CameraManager.getSingleton.ActiveCamera.Position;

			Vector3 temp = portal.Position - CameraManager.getSingleton.ActiveCamera.Position;
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
            if (CameraManager.getSingleton.ActiveCamera.FieldOfView < fieldOfView)
                fieldOfView = CameraManager.getSingleton.ActiveCamera.FieldOfView;

			aspectRatio = (box.Max.X - box.Min.X) /
						  (box.Max.Y - box.Min.Y);

			if (fieldOfView <= 0.005) //fieldOfView >= -0.005 && 
				fieldOfView = 0.01f;

			Matrix projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView,
				aspectRatio,
				CameraManager.getSingleton.ActiveCamera.NearPlane, //nearPlane,
				CameraManager.getSingleton.ActiveCamera.FarPlane);

			//Vector3 target = (((box.Max - box.Min) / 2) + box.Min) - CameraManager.getSingleton.GetCamera("test").Position;
			Vector3 target = portal.Position;// -CameraManager.getSingleton.GetCamera("test").Position;

			Matrix view = Matrix.CreateLookAt(CameraManager.getSingleton.ActiveCamera.Position,
				target, Vector3.Up);

			// Crash: StackOverflowException
			BoundingFrustum newFrustum = new BoundingFrustum(Matrix.Multiply(view, projection));
			*/

            /*
            //Vector3 tl, tr, bl, br;
            //Vector3 ntl, ntr, nbl, nbr, ftl, ftr, fbl, fbr;
            //Vector3 offset = new Vector3(0.0f, 0.0f, 0.1f);

            //tr = Vector3.Normalize(new Vector3(box.Max.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
            //br = Vector3.Normalize(new Vector3(box.Max.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
            //bl = Vector3.Normalize(new Vector3(box.Min.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
            //tl = Vector3.Normalize(new Vector3(box.Min.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);

            //ntl = offset + CameraManager.getSingleton.ActiveCamera.Position + (new Vector3(box.Min.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
            //ntr = offset + CameraManager.getSingleton.ActiveCamera.Position + (new Vector3(box.Max.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
            //nbl = offset + CameraManager.getSingleton.ActiveCamera.Position + (new Vector3(box.Min.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
            //nbr = offset + CameraManager.getSingleton.ActiveCamera.Position + (new Vector3(box.Max.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.ActiveCamera.Position);
            //ftl = offset + CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(tl, CameraManager.getSingleton.ActiveCamera.FarPlane);
            //ftr = offset + CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(tr, CameraManager.getSingleton.ActiveCamera.FarPlane);
            //fbl = offset + CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(bl, CameraManager.getSingleton.ActiveCamera.FarPlane);
            //fbr = offset + CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(br, CameraManager.getSingleton.ActiveCamera.FarPlane);

            //Frustum newFrustum = new Frustum(ntl, ntr, nbl, nbr, ftl, ftr, fbl, fbr);
            */
            #endregion

            Vector3 nearDistance = portal.Position - CameraManager.getSingleton.ActiveCamera.Position;

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
            Matrix projection = Matrix.CreatePerspectiveOffCenter(minScaleX, maxScaleX, -portal.Scale.Y, portal.Scale.Y,
                nearDistance.Length(),
                CameraManager.getSingleton.ActiveCamera.FarPlane);

            Matrix view = Matrix.CreateLookAt(CameraManager.getSingleton.ActiveCamera.Position,
                portal.Position, Vector3.Up);

            BoundingFrustum newFrustum = new BoundingFrustum(Matrix.Multiply(view, projection));

            return newFrustum;
		}

        private BoundingFrustum CreateClippedPortalFrustum(Portal portal) {
            #region not fully functional code
            /*
            Vector3 nearDistance = portal.Position - CameraManager.getSingleton.ActiveCamera.Position;
            float test1, test2;
            Plane portalPlane;
            Vector3[] corners = new Vector3[8];
            Vector3 pt1, pt2, pt3;
            BoundingBox box = portal.BoundingBox;
            test1 = box.Max.X - box.Min.X;
            test2 = box.Max.Z - box.Min.Z;
            bool flip = false;
            bool xAxis = false;

            if (test1 > test2) {
                pt1 = new Vector3(box.Min.X, box.Min.Y, box.Min.Z);
                pt2 = new Vector3(box.Max.X, box.Min.Y, box.Min.Z);
                pt3 = new Vector3(box.Max.X, box.Max.Y, box.Min.Z);
                if (nearDistance.Z < 0)
                    flip = true;
            } else {
                xAxis = true;
                pt1 = new Vector3(box.Min.X, box.Min.Y, box.Min.Z);
                pt2 = new Vector3(box.Min.X, box.Min.Y, box.Max.Z);
                pt3 = new Vector3(box.Min.X, box.Max.Y, box.Max.Z);
                if (nearDistance.X > 0)
                    flip = true;
            }
            portalPlane = new Plane(pt1, pt2, pt3);

            corners = CameraManager.getSingleton.ActiveCamera.Frustum.GetCorners();
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

            if (tlIntersect < 0 || trIntersect < 0 || blIntersect < 0 || brIntersect < 0) {
                return CameraManager.getSingleton.ActiveCamera.Frustum;
            } else {
                Vector3 tlIntersectPt, trIntersectPt, blIntersectPt, brIntersectPt;
                tlIntersectPt = CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(frustumTL, tlIntersect);
                trIntersectPt = CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(frustumTR, trIntersect);
                blIntersectPt = CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(frustumBL, blIntersect);
                brIntersectPt = CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(frustumBR, brIntersect);

                if (flip) {
                    Vector3 temp = tlIntersectPt;
                    tlIntersectPt = trIntersectPt;
                    trIntersectPt = temp;
                    temp = blIntersectPt;
                    blIntersectPt = brIntersectPt;
                    brIntersectPt = temp;
                }
                float minX, maxX, minY, maxY;
                Vector3 min, max;
                float scaleX, scaleY;
                if (xAxis) {
                    if (trIntersectPt.Z < box.Max.Z) {
                        maxX = trIntersectPt.Z;
                    } else {
                        maxX = box.Max.Z;
                    }

                    if (tlIntersectPt.Z > box.Min.Z) {
                        minX = tlIntersectPt.Z;
                    } else {
                        minX = box.Min.Z;
                    }

                    max.X = trIntersectPt.X;
                    max.Z = maxX;
                    min.X = trIntersectPt.X;
                    min.Z = minX;

                    scaleX = (max.Z - min.Z) * 0.5f;

                } else {
                    if (trIntersectPt.X < box.Max.X) {
                        maxX = trIntersectPt.X;
                    } else {
                        maxX = box.Max.X;
                    }

                    if (tlIntersectPt.X > box.Min.X) {
                        minX = tlIntersectPt.X;
                    } else {
                        minX = box.Min.X;
                    }

                    max.X = maxX;
                    max.Z = trIntersectPt.Z;
                    min.X = minX;
                    min.Z = trIntersectPt.Z;

                    scaleX = (max.X - min.X) * 0.5f;
                }

                if (trIntersectPt.Y < box.Max.Y || tlIntersectPt.Y < box.Max.Y) {
                    if (tlIntersectPt.Y < trIntersectPt.Y) {
                        maxY = tlIntersectPt.Y;
                    } else {
                        maxY = trIntersectPt.Y;
                    }
                } else {
                    maxY = box.Max.Y;
                }

                if (brIntersectPt.Y > box.Min.Y || blIntersectPt.Y > box.Min.Y) {
                    if (blIntersectPt.Y > brIntersectPt.Y) {
                        minY = blIntersectPt.Y;
                    } else {
                        minY = brIntersectPt.Y;
                    }
                } else {
                    minY = box.Min.Y;
                }

                max.Y = maxY;
                min.Y = minY;

                scaleY = (max.Y - min.Y) * 0.5f;

                Vector3 centerPt = Vector3.Divide((max - min), 2);
                centerPt = min + centerPt;
                float offset = 0.5f;
                Matrix projection = Matrix.CreatePerspectiveOffCenter(-scaleX - offset, scaleX + offset, -scaleY - offset, scaleY + offset,
                    nearDistance.Length(),
                    CameraManager.getSingleton.ActiveCamera.FarPlane);

                Matrix view = Matrix.CreateLookAt(CameraManager.getSingleton.ActiveCamera.Position,
                    centerPt, Vector3.Up);

                BoundingFrustum newFrustum = new BoundingFrustum(Matrix.Multiply(view, projection));

                return newFrustum;
            }
            */
            #endregion

            return CameraManager.getSingleton.ActiveCamera.Frustum;

            #region old code
            /*
            Vector3 nearDistance = portal.Position - CameraManager.getSingleton.ActiveCamera.Position;
            float test1, test2;
            Plane portalPlane;
            Vector3[] corners = new Vector3[8];
            Vector3 pt1, pt2, pt3;
            BoundingBox box = portal.BoundingBox; //portal.GetBoundingBoxTransformed();
            test1 = box.Max.X - box.Min.X;
            test2 = box.Max.Z - box.Min.Z;

            if (test1 > test2)
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

            corners = CameraManager.getSingleton.ActiveCamera.Frustum.GetCorners();
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
                return CameraManager.getSingleton.ActiveCamera.Frustum;
            }
            else
            {
                Vector3 tlIntersectPt, trIntersectPt, blIntersectPt, brIntersectPt;
                tlIntersectPt = CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(frustumTL, tlIntersect);
                trIntersectPt = CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(frustumTR, trIntersect);
                blIntersectPt = CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(frustumBL, blIntersect);
                brIntersectPt = CameraManager.getSingleton.ActiveCamera.Position + Vector3.Multiply(frustumBR, brIntersect);

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
                maxScaleX = (max.X - min.X) * 0.5f;
                minScaleX = -maxScaleX;
                maxScaleY = (max.Y - min.Y) * 0.5f;
                minScaleY = -maxScaleY;

                Matrix projection = Matrix.CreatePerspectiveOffCenter(minScaleX, maxScaleX, minScaleY, maxScaleY,
                    nearDistance.Length(), //centerPt.Length(),
                    CameraManager.getSingleton.ActiveCamera.FarPlane);

                Matrix view = Matrix.CreateLookAt(CameraManager.getSingleton.ActiveCamera.Position,
                    centerPt, Vector3.Up);

                BoundingFrustum newFrustum = new BoundingFrustum(Matrix.Multiply(view, projection));

                return newFrustum;
            }
             * * */
            #endregion
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
                float nom = Vector3.Dot(p.Normal, CameraManager.getSingleton.ActiveCamera.Position) + p.D;
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

		/*TESTING METHOD
		private void drawFrustum(BoundingFrustum frustum)
		//private void drawFrustum(Frustum frustum)
		{
			Vector3[] frustumPoints = new Vector3[8];
			frustumPoints = frustum.GetCorners();

			VertexPositionColor[] BoundingFrustumDrawData = new VertexPositionColor[8]
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

			int[] BoundingFrustumIndex = new int[24];
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
			//Effect basicEffect = SceneManager.getSingleton.Display.CartoonEffect;

			//basicEffect.Parameters["World"].SetValue(Matrix.Identity);
			//basicEffect.Parameters["View"].SetValue(CameraManager.getSingleton.ActiveCamera.View);
			//basicEffect.Parameters["Projection"].SetValue(CameraManager.getSingleton.ActiveCamera.Projection);
			////basicEffect. = true;

			//basicEffect.Begin();
			//foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			//{
			//    // Begin the current pass
			//    pass.Begin();

			//    SceneManager.getSingleton.graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
			//        PrimitiveType.LineList, BoundingFrustumDrawData, 0, 8, BoundingFrustumIndex, 0, 12);

			//    // End the current pass
			//    pass.End();
			//}
			//basicEffect.End();
		}
		 * */
	}
}
