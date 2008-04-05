using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
    class Octree : OctreeLeaf
    {
        public Octree()
            : base(new BoundingBox())
        {
        }

        public void Bounds()
        {
            foreach (SceneObject obj in ContainedObjects)
            {
                //ContainerBox = BoundingBox.CreateMerged(ContainerBox, obj.BoundingBox);
                ContainerBox = BoundingBox.CreateMerged(ContainerBox, obj.GetBoundingBoxTransformed());
            }
        }

        public void Distribute(ref List<SceneObject> scene)
        {
            ContainedObjects = scene;
            Bounds();
            base.Distribute();
        }

        //public void DrawVisible(GameTime gameTime)
        //{
        //    base.DrawVisible(gameTime);
        //}
    }
}
