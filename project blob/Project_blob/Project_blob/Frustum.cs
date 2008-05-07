using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob
{
    class Frustum
    {
        //public enum Planes
        //{
        //    TOP = 0,
        //    BOTTOM = 1,
        //    LEFT = 2,
        //    RIGHT = 3,
        //    NEAR = 4,
        //    FAR = 5
        //}

        private Plane[] frustumPlanes = new Plane[8];
        public Plane[] FrustumPlanes
        {
            get { return frustumPlanes; }
            set { frustumPlanes = value; }
        }

        public Frustum()
        {

        }

        public Frustum(Vector3 ntl, Vector3 ntr, Vector3 nbl, Vector3 nbr, Vector3 ftl, Vector3 ftr, Vector3 fbl, Vector3 fbr)
        {
            frustumPlanes[0] = new Plane(ntr, ntl, ftl);
            frustumPlanes[1] = new Plane(nbl, nbr, fbr);
            frustumPlanes[2] = new Plane(ntl, nbl, fbl);
            frustumPlanes[3] = new Plane(nbr, ntr, fbr);
            frustumPlanes[4] = new Plane(ntl, ntr, nbr);
            frustumPlanes[5] = new Plane(ftr, ftl, fbl);

            //pl[TOP].set3Points(ntr, ntl, ftl);
            //pl[BOTTOM].set3Points(nbl, nbr, fbr);
            //pl[LEFT].set3Points(ntl, nbl, fbl);
            //pl[RIGHT].set3Points(nbr, ntr, fbr);
            //pl[NEARP].set3Points(ntl, ntr, nbr);
            //pl[FARP].set3Points(ftr, ftl, fbl);
        }

        public ContainmentType Contains(BoundingBox box)
        {
            int numIn = 8;
            int ptIn = 0;

            //for (int p = 0; p < 6; ++p)
            //{
            //    numIn = 0;

            //    for (int i = 0; i < 8; ++i)
            //    {
            //        if (frustumPlanes[p].Intersects( == PlaneIntersectionType.Back)
            //        {
            //            ptIn = 0;
            //            --numIn;
            //        }
            //    }
            //}

            return ContainmentType.Intersects;
        }
    }
}
