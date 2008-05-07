using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics2
{
    class BoundingFrustum
    {
        private Plane[] planes;

        /// <summary>
        /// No Error checking, no guarantees.
        /// </summary>
        /// <param name="NearTopLeft"></param>
        /// <param name="NearTopRight"></param>
        /// <param name="NearBottomLeft"></param>
        /// <param name="NearBottomRight"></param>
        /// <param name="FarTopLeft"></param>
        /// <param name="FarTopRight"></param>
        /// <param name="FarBottomLeft"></param>
        /// <param name="FarBottomRight"></param>
        public BoundingFrustum(Vector3 NearTopLeft, Vector3 NearTopRight, Vector3 NearBottomLeft, Vector3 NearBottomRight, Vector3 FarTopLeft, Vector3 FarTopRight, Vector3 FarBottomLeft, Vector3 FarBottomRight)
        {
            planes = new Plane[6];

            planes[0] = new Plane(NearTopRight, NearTopLeft, NearBottomLeft);
            planes[1] = new Plane(FarTopLeft, FarTopRight, FarBottomRight);
            planes[2] = new Plane(NearTopLeft, FarTopLeft, FarBottomLeft);
            planes[3] = new Plane(FarTopRight, NearTopRight, NearBottomRight);
            planes[4] = new Plane(FarTopRight, FarTopLeft, NearTopLeft);
            planes[5] = new Plane(NearBottomRight, NearBottomLeft, FarBottomLeft);

        }

        public ContainmentType Contains(BoundingBox box)
        {
            bool flag = false;
            foreach (Plane plane in this.planes)
            {
                switch (box.Intersects(plane))
                {
                    case PlaneIntersectionType.Front:
                        return ContainmentType.Disjoint;

                    case PlaneIntersectionType.Intersecting:
                        flag = true;
                        break;
                }
            }
            if (!flag)
            {
                return ContainmentType.Contains;
            }
            return ContainmentType.Intersects;
        }
    }
}
