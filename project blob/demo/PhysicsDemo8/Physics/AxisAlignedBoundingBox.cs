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
				if (pt.Y <Min.Y)
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

	}
}