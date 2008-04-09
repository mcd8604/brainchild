using Microsoft.Xna.Framework;

namespace Physics
{
	public class AxisAlignedBoundingBox
	{

		public Vector3 Max;
		public Vector3 Min;
		private bool Valid;

		public AxisAlignedBoundingBox()
		{
			Min = Max = Vector3.Zero;
			Valid = false;
		}

		public AxisAlignedBoundingBox(ref Vector3 minPt, ref Vector3 maxPt)
		{
			Min = minPt;
			Max = maxPt;
			Valid = true;
		}

		public void clear()
		{
			Min = Max = Vector3.Zero;
			Valid = false;
		}
		public bool contains(ref Vector3 pt)
		{
			if (Valid)
			{
				return pt.X >= Min.X && pt.X <= Max.X && pt.Y >= Min.Y && pt.Y <= Max.Y && pt.Z >= Min.Z && pt.Z <= Max.Z;
			}
			return false;
		}

		public void expandToInclude(ref Vector3 pt)
		{
			if (Valid)
			{
				if (pt.X < Min.X)
				{
					Min.X = pt.X;
				}
				else if (pt.X > Max.X)
				{
					Max.X = pt.X;
				}
				if (pt.Y < Min.Y)
				{
					Min.Y = pt.Y;
				}
				else if (pt.Y > Max.Y)
				{
					Max.Y = pt.Y;
				}
				if (pt.Z < Min.Z)
				{
					Min.Z = pt.Z;
				}
				else if (pt.Z > Max.Z)
				{
					Max.Z = pt.Z;
				}
			}
			else
			{
				Min = Max = pt;
				Valid = true;
			}
		}

		public bool intersects(ref AxisAlignedBoundingBox box)
		{
			return Min.X <= box.Max.X && Max.X >= box.Min.X && Min.Y <= box.Max.Y && Max.Y >= box.Min.Y && Min.Z <= box.Max.Z && Max.Z >= box.Min.Z;
		}

		public bool lineIntersects(ref Vector3 pt1, ref Vector3 pt2)
		{
			if (pt2.X < Min.X && pt1.X < Min.X) return false;
			if (pt2.X > Max.X && pt1.X > Max.X) return false;
			if (pt2.Y < Min.Y && pt1.Y < Min.Y) return false;
			if (pt2.Y > Max.Y && pt1.Y > Max.Y) return false;
			if (pt2.Z < Min.Z && pt1.Z < Min.Z) return false;
			if (pt2.Z > Max.Z && pt1.Z > Max.Z) return false;
			if ((pt1.X > Min.X && pt1.X < Max.X &&
				pt1.Y > Min.Y && pt1.Y < Max.Y &&
				pt1.Z > Min.Z && pt1.Z < Max.Z) ||
			(pt2.X > Min.X && pt2.X < Max.X &&
				pt2.Y > Min.Y && pt2.Y < Max.Y &&
				pt2.Z > Min.Z && pt2.Z < Max.Z))
			{
				return true;
			}

			//check?

			return true;
		}

	}
}