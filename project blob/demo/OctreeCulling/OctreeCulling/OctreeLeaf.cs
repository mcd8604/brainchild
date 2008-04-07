/*  Author: Josh Wilson
 * 
 *  Nathan Levesque for his tutorial on Octree:
 *      http://rhysyngsun.spaces.live.com/Blog/cns!FBBD62480D87119D!129.entry
 * */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OctreeCulling
{
    class OctreeLeaf
    {
        private const int _maxobjects = 1;

        private List<SceneObject> _containedObjects;
        public List<SceneObject> ContainedObjects
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
            _containedObjects = new List<SceneObject>();
            _childLeaves = new List<OctreeLeaf>();
            _containerBox = box;
            _containerSphere = BoundingSphere.CreateFromBoundingBox(_containerBox);
        }

        protected void Split()
        {
            Vector3 half = (ContainerBox.Max - ContainerBox.Min) / 2;
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
                //for (int i = ContainedObjects.Count - 1; i >= 0; --i)
                //{
                //    foreach (OctreeLeaf leaf in ChildLeaves)
                //    {
                //        if (leaf.ContainerBox.Contains(ContainedObjects[i].GetBoundingBoxTransformed()) == ContainmentType.Contains)
                //        //if (leaf.ContainerBox.Contains(ContainedObjects[i].BoundingBox) == ContainmentType.Contains)
                //        {
                //            leaf.ContainedObjects.Add(ContainedObjects[i]);
                //            _containedObjects.Remove(ContainedObjects[i]);
                //            break;
                //        }
                //    }
                //}

                foreach (OctreeLeaf leaf in ChildLeaves)
                {
                    for (int i = ContainedObjects.Count - 1; i >= 0; --i)
                    {
                        if ((leaf.ContainerBox.Contains(ContainedObjects[i].GetBoundingBoxTransformed()) == ContainmentType.Contains) ||
                            (leaf.ContainerBox.Contains(ContainedObjects[i].GetBoundingBoxTransformed()) == ContainmentType.Intersects))
                        //if (leaf.ContainerBox.Contains(ContainedObjects[i].BoundingBox) == ContainmentType.Contains)
                        {
                            leaf.ContainedObjects.Add(ContainedObjects[i]);
                            _containedObjects.Remove(ContainedObjects[i]);
                            //break;
                        }
                    }
                }

                foreach (OctreeLeaf leaf in ChildLeaves)
                {
                    leaf.Distribute();
                }
            }
        }

        public void DrawVisible(GameTime gameTime)
        {
            //BoundingFrustum frustum = CameraManager.getSingleton.ActiveCamera.Frustum;
            BoundingFrustum frustum = CameraManager.getSingleton.GetCamera("test").Frustum;

            foreach (SceneObject obj in _containedObjects)
            {
                obj.Draw(gameTime);
            }
            foreach (OctreeLeaf leaf in ChildLeaves)
            {
                //Add functionality to test the containment type of the bounding box with the frustum.
                //If the entire box is inside the frustum the call Draw as everything needs to be drawn.

                //if (leaf.ContainerBox.Intersects(frustum))
                //    leaf.DrawVisible(gameTime);

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
        }

        public void Draw(GameTime gameTime)
        {
            foreach (SceneObject obj in _containedObjects)
            {
                obj.Draw(gameTime);
            }
            foreach (OctreeLeaf leaf in ChildLeaves)
            {
                leaf.Draw(gameTime);
            }
        }
    }
}
