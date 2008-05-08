using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics2
{
	class BoundingFrustum
	{
		private Plane[] planes;
		private Vector3[] corners;

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
		public BoundingFrustum(Vector3 NearTopLeft, Vector3 NearTopRight, Vector3 NearBottomRight, Vector3 NearBottomLeft, Vector3 FarTopLeft, Vector3 FarTopRight, Vector3 FarBottomRight, Vector3 FarBottomLeft)
		{
			planes = new Plane[6];

			planes[0] = new Plane(NearTopRight, NearTopLeft, NearBottomLeft);
			planes[1] = new Plane(FarTopLeft, FarTopRight, FarBottomRight);
			planes[2] = new Plane(NearTopLeft, FarTopLeft, FarBottomLeft);
			planes[3] = new Plane(FarTopRight, NearTopRight, NearBottomRight);
			planes[4] = new Plane(FarTopRight, FarTopLeft, NearTopLeft);
			planes[5] = new Plane(NearBottomRight, NearBottomLeft, FarBottomLeft);

			corners = new Vector3[8];

			corners[0] = NearTopLeft;
			corners[1] = NearTopRight;
			corners[2] = NearBottomRight;
			corners[3] = NearBottomLeft;
			corners[4] = FarTopLeft;
			corners[5] = FarTopRight;
			corners[6] = FarBottomRight;
			corners[7] = FarBottomLeft;
		}

		public ContainmentType Contains(BoundingBox box)
		{
			switch (box.Intersects(planes[0]))
			{
				case PlaneIntersectionType.Front:
					return ContainmentType.Disjoint;

				case PlaneIntersectionType.Intersecting:
					return ContainmentType.Intersects;
			}
			switch (box.Intersects(planes[1]))
			{
				case PlaneIntersectionType.Front:
					return ContainmentType.Disjoint;

				case PlaneIntersectionType.Intersecting:
					return ContainmentType.Intersects;
			}
			switch (box.Intersects(planes[2]))
			{
				case PlaneIntersectionType.Front:
					return ContainmentType.Disjoint;

				case PlaneIntersectionType.Intersecting:
					return ContainmentType.Intersects;
			}
			switch (box.Intersects(planes[3]))
			{
				case PlaneIntersectionType.Front:
					return ContainmentType.Disjoint;

				case PlaneIntersectionType.Intersecting:
					return ContainmentType.Intersects;
			}
			switch (box.Intersects(planes[4]))
			{
				case PlaneIntersectionType.Front:
					return ContainmentType.Disjoint;

				case PlaneIntersectionType.Intersecting:
					return ContainmentType.Intersects;
			}
			switch (box.Intersects(planes[5]))
			{
				case PlaneIntersectionType.Front:
					return ContainmentType.Disjoint;

				case PlaneIntersectionType.Intersecting:
					return ContainmentType.Intersects;
			}

			return ContainmentType.Contains;
		}

		public Vector3[] GetCorners()
		{
			return (Vector3[])corners.Clone();
		}

		public Plane Bottom
		{
			get
			{
				return planes[5];
			}
		}

		public Plane Far
		{
			get
			{
				return planes[1];
			}
		}

		public Plane Left
		{
			get
			{
				return planes[2];
			}
		}

		public Plane Near
		{
			get
			{
				return planes[0];
			}
		}

		public Plane Right
		{
			get
			{
				return planes[3];
			}
		}

		public Plane Top
		{
			get
			{
				return planes[4];
			}
		}
	}
}
