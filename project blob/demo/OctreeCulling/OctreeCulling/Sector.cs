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
							//BoundingFrustum newFrustum = CreatePortalFrustum(portal);
							Frustum newFrustum = CreatePortalFrustum(portal);

                            drawFrustum(newFrustum);
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

		//public void DrawVisible(GameTime gameTime, BoundingFrustum frustum)
		public void DrawVisible(GameTime gameTime, Frustum frustum)
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
							//BoundingFrustum newFrustum = CreatePortalFrustum(portal);
							Frustum newFrustum = CreatePortalFrustum(portal);

                            drawFrustum(newFrustum);
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

		//private BoundingFrustum CreatePortalFrustum(Portal portal)
		private Frustum CreatePortalFrustum(Portal portal)
        {
			////Create new frustum from portal
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

			BoundingFrustum newFrustumTest = new BoundingFrustum(Matrix.Multiply(view, projection));



			Vector3 tl, tr, bl, br;
			Vector3 ntl, ntr, nbl, nbr, ftl, ftr, fbl, fbr;

			tr = Vector3.Normalize(new Vector3(box.Max.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			br = Vector3.Normalize(new Vector3(box.Max.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			bl = Vector3.Normalize(new Vector3(box.Min.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			tl = Vector3.Normalize(new Vector3(box.Min.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);

			ntl = CameraManager.getSingleton.GetCamera("test").Position + (new Vector3(box.Min.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			ntr = CameraManager.getSingleton.GetCamera("test").Position + (new Vector3(box.Max.X, box.Max.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			nbl = CameraManager.getSingleton.GetCamera("test").Position + (new Vector3(box.Min.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			nbr = CameraManager.getSingleton.GetCamera("test").Position + (new Vector3(box.Max.X, box.Min.Y, box.Max.Z) - CameraManager.getSingleton.GetCamera("test").Position);
			ftl = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(tl, CameraManager.getSingleton.GetCamera("test").FarPlane);
			ftr = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(tr, CameraManager.getSingleton.GetCamera("test").FarPlane);
			fbl = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(bl, CameraManager.getSingleton.GetCamera("test").FarPlane);
			fbr = CameraManager.getSingleton.GetCamera("test").Position + Vector3.Multiply(br, CameraManager.getSingleton.GetCamera("test").FarPlane);

			Frustum newFrustum = new Frustum(ntl, ntr, nbl, nbr, ftl, ftr, fbl, fbr);

            return newFrustum;
        }

		//private void drawFrustum(BoundingFrustum frustum)
		private void drawFrustum(Frustum frustum)
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

            ////Draw temporary frustum
            //BasicEffect basicEffect = SceneManager.getSingleton.PortalScene.Sectors[1]._sectorObjects[0].Effect;
            //GraphicsDeviceManager graphics = SceneManager.getSingleton.PortalScene.Sectors[1]._sectorObjects[0].Graphics;

            //basicEffect.World = Matrix.Identity;
            //basicEffect.View = CameraManager.getSingleton.ActiveCamera.View;
            //basicEffect.Projection = CameraManager.getSingleton.ActiveCamera.Projection;
            //basicEffect.VertexColorEnabled = true;

            //basicEffect.Begin();
            //foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            //{
            //    // Begin the current pass
            //    pass.Begin();

            //    graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
            //        PrimitiveType.LineList, CameraManager.getSingleton.GetCamera("test").BoundingFrustumDrawData,
            //        0, 8, CameraManager.getSingleton.GetCamera("test").BoundingFrustumIndex, 0, 12);

            //    //graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
            //    //    PrimitiveType.LineList, CameraManager.getSingleton.ActiveCamera.BoundingFrustumDrawData,
            //    //    0, 8, CameraManager.getSingleton.ActiveCamera.BoundingFrustumIndex, 0, 12);

            //    // End the current pass
            //    pass.End();
            //}
            //basicEffect.End(); 
        }
    }
}
