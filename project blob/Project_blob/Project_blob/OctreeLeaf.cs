/*  Author: Josh Wilson
 * 
 *  Nathan Levesque for his tutorial on Octree:
 *      http://rhysyngsun.spaces.live.com/Blog/cns!FBBD62480D87119D!129.entry
 * */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using Engine;

namespace Project_blob
{
    class OctreeLeaf
    {
        private const int _maxobjects = 4;

        private List<Drawable> _containedObjects;
        public List<Drawable> ContainedObjects
        {
            get { return _containedObjects; }
            set { _containedObjects = value; }
        }

        private List<OctreeLeaf> _childLeaves;
        public List<OctreeLeaf> ChildLeaves
        {
            get { return _childLeaves; }
            //set { _childLeaves = value; }
        }

        private BoundingBox _containerBox;
        public BoundingBox ContainerBox
        {
            get { return _containerBox; }
            set { _containerBox = value; }
        }

        private BoundingSphere _containerSphere;
        public BoundingSphere ContainerSphere
        {
            get { return _containerSphere; }
            set { _containerSphere = value; }
        }

        public OctreeLeaf(BoundingBox box)
        {
            _containedObjects = new List<Drawable>();
            _childLeaves = new List<OctreeLeaf>();
            _containerBox = box;
            _containerSphere = BoundingSphere.CreateFromBoundingBox(_containerBox);
        }

        protected void Split()
        {
            Vector3 half = (ContainerBox.Max - ContainerBox.Min) / 2;

			if ( half == Vector3.Zero ) {
				throw new Exception();
			}

            Vector3 halfx = Vector3.UnitX * half;
            Vector3 halfy = Vector3.UnitY * half;
            Vector3 halfz = Vector3.UnitZ * half;

            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min, ContainerBox.Min + half)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfx, ContainerBox.Max - half + halfx)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfz, ContainerBox.Min + half + halfz)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfx + halfz, ContainerBox.Max - halfy)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfy, ContainerBox.Max - halfx - halfz)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfy + halfx, ContainerBox.Max - halfz)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfy + halfz, ContainerBox.Max - halfx)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + half, ContainerBox.Max)));
        }

        public void Distribute()
        {
            if (_containedObjects.Count > _maxobjects)
            {
                Split();

                foreach (OctreeLeaf leaf in ChildLeaves)
                {
                    for (int i = ContainedObjects.Count - 1; i >= 0; --i)
                    {
						BoundingBox box = _containedObjects[i].GetBoundingBox();

                        if (leaf.ContainerBox.Contains(_containedObjects[i].GetBoundingBox()) == ContainmentType.Contains)
                        //if ((leaf.ContainerBox.Contains(_containedObjects[i].GetBoundingBox()) == ContainmentType.Contains) ||
                        //    (leaf.ContainerBox.Contains(_containedObjects[i].GetBoundingBox()) == ContainmentType.Intersects))
                        {
                            leaf.ContainedObjects.Add(ContainedObjects[i]);
                            _containedObjects.Remove(ContainedObjects[i]);
                        }
                    }
                }

				//_containedObjects.Clear();

                foreach (OctreeLeaf leaf in ChildLeaves)
                {
                    leaf.Distribute();
                }
            }
        }

        public void DrawVisible(GameTime gameTime)
        {
            BoundingFrustum frustum = CameraManager.getSingleton.ActiveCamera.Frustum;
            BoundingSphere sphere = CameraManager.getSingleton.ActiveCamera.BoundingSphere;

            foreach (Drawable obj in _containedObjects)
            {
                switch (frustum.Contains(obj.GetBoundingBox()))
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        {
                            SceneManager.getSingleton.Display.AddToBeDrawn(obj);
                            break;
                        }
                    case ContainmentType.Disjoint:
                        break;

                }

                //obj.Draw(gameTime);
                //SceneManager.getSingleton.Display.AddToBeDrawn(obj);
                //SceneManager.getSingleton.Drawn += 1;
            }
            foreach (OctreeLeaf leaf in ChildLeaves)
            {
                ContainmentType typeSphere = frustum.Contains(leaf.ContainerSphere);
                switch (typeSphere)
                {
                    case ContainmentType.Contains:
                        {
                            leaf.Draw(gameTime);
                        }
                        break;
                    case ContainmentType.Intersects:
                        {
                            ContainmentType type = frustum.Contains(leaf.ContainerBox);

                            switch (type)
                            {
                                case ContainmentType.Contains:
                                    {
                                        leaf.Draw(gameTime);
                                    }
                                    break;

                                case ContainmentType.Intersects:
                                    {
                                        leaf.DrawVisible(gameTime);
                                    }
                                    break;

                                case ContainmentType.Disjoint:
                                    {
                                        //Bounding box is culled.
                                    }
                                    break;
                            }
                        }
                        break;
                    case ContainmentType.Disjoint:
                        {
                            //Bounding sphere is culled.
                        }
                        break;
                }

                //ContainmentType type = frustum.Contains(leaf.ContainerBox);

                //switch (type)
                //{
                //    case ContainmentType.Contains:
                //        {
                //            leaf.Draw(gameTime);
                //        }
                //        break;

                //    case ContainmentType.Intersects:
                //        {
                //            leaf.DrawVisible(gameTime);
                //        }
                //        break;

                //    case ContainmentType.Disjoint:
                //        {
                //            //Bounding box is culled.
                //        }
                //        break;
                //}
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Drawable obj in _containedObjects)
            {
                //obj.Draw(gameTime);
                SceneManager.getSingleton.Display.AddToBeDrawn(obj);
            }
            foreach (OctreeLeaf leaf in ChildLeaves)
            {
                leaf.Draw(gameTime);
            }
        }
    }
}
