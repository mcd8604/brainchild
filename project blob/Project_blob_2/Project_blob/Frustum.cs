using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob
{
    class Frustum
    {
        private Plane[] frustumPlanes = new Plane[6];
        public Plane[] FrustumPlanes
        {
            get { return frustumPlanes; }
            set { frustumPlanes = value; }
        }

		private Vector3[] corners = new Vector3[8];
		public Vector3[] Corners
		{
			get { return corners; }
			set { corners = value; }
		}

        public Frustum()
        {

        }

        public Frustum(Vector3 ntl, Vector3 ntr, Vector3 nbl, Vector3 nbr, Vector3 ftl, Vector3 ftr, Vector3 fbl, Vector3 fbr)
        {
			frustumPlanes[0] = new Plane(ftl, ntl, ntr);
			frustumPlanes[1] = new Plane(fbr, nbr, nbl);
			frustumPlanes[2] = new Plane(fbl, nbl, ntl);
			frustumPlanes[3] = new Plane(fbr, ntr, nbr);
			frustumPlanes[4] = new Plane(nbr, ntr, ntl);
			frustumPlanes[5] = new Plane(fbl, ftl, ftr);

			corners[0] = ntl;
			corners[1] = ntr;
			corners[2] = nbr;
			corners[3] = nbl;
			corners[4] = ftl;
			corners[5] = ftr;
			corners[6] = fbr;
			corners[7] = fbl;
        }

        public ContainmentType Contains(BoundingBox box)
        {
            int numIn = 0;

            for (int p = 0; p < 6; ++p)
            {
				if (frustumPlanes[p].Intersects(box) == PlaneIntersectionType.Back)
				{
					return ContainmentType.Disjoint;
				}
				else
				{
					++numIn;
				}
            }

			if (numIn == 6)
			{
				return ContainmentType.Contains;
			}
			else
			{
				return ContainmentType.Intersects;
			}
        }

		public Vector3[] GetCorners()
		{
			return Corners;
		}
    }
}
