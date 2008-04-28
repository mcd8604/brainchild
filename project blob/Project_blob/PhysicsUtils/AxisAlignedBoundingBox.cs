using Microsoft.Xna.Framework;

public class AxisAlignedBoundingBox
{

	internal Vector3 Max;
	internal Vector3 Min;
	private bool Valid;

	public AxisAlignedBoundingBox()
	{
		Min = Max = Vector3.Zero;
		Valid = false;
	}

	public AxisAlignedBoundingBox(Vector3 minPt, Vector3 maxPt)
	{
		Min = minPt;
		Max = maxPt;
		Valid = true;
	}

	public AxisAlignedBoundingBox(AxisAlignedBoundingBox cloneFrom)
	{
		Max = cloneFrom.Max;
		Min = cloneFrom.Min;
		Valid = cloneFrom.Valid;
	}

	public void clear()
	{
		Min = Max = Vector3.Zero;
		Valid = false;
	}

	public void expandToInclude(Vector3 pt)
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

	public void expandToInclude(AxisAlignedBoundingBox bb)
	{
		if (Valid)
		{
			expandToInclude(bb.Max);
			expandToInclude(bb.Min);
		}
		else
		{
			Max = bb.Max;
			Min = bb.Min;
			Valid = true;
		}
	}

	public bool contains(Vector3 pt)
	{
		if (Valid)
		{
			return pt.X >= Min.X && pt.X <= Max.X && pt.Y >= Min.Y && pt.Y <= Max.Y && pt.Z >= Min.Z && pt.Z <= Max.Z;
		}
		return false;
	}

	public bool contains(AxisAlignedBoundingBox bb)
	{
		if (Valid)
		{
			return Min.X <= bb.Min.X && Max.X >= bb.Max.X && Min.Y <= bb.Min.Y && Max.Y >= bb.Max.Y && Min.Z <= bb.Min.Z && Max.Z >= bb.Max.Z;
		}
		return false;
	}

	public bool intersects(AxisAlignedBoundingBox box)
	{
		if (Valid)
		{
			if ((Max.X < box.Min.X) || (Min.X > box.Max.X))
			{
				return false;
			}
			if ((Max.Y < box.Min.Y) || (Min.Y > box.Max.Y))
			{
				return false;
			}
			return ((Max.Z >= box.Min.Z) && (Min.Z <= box.Max.Z));

		}
		return false;
	}

	public bool lineIntersects(Vector3 pt1, Vector3 pt2)
	{
		if (Valid)
		{
			if (pt2.X < Min.X && pt1.X < Min.X)
			{
				return false;
			}
			if (pt2.X > Max.X && pt1.X > Max.X)
			{
				return false;
			}
			if (pt2.Y < Min.Y && pt1.Y < Min.Y)
			{
				return false;
			}
			if (pt2.Y > Max.Y && pt1.Y > Max.Y)
			{
				return false;
			}
			if (pt2.Z < Min.Z && pt1.Z < Min.Z)
			{
				return false;
			}
			if (pt2.Z > Max.Z && pt1.Z > Max.Z)
			{
				return false;
			}
			if ((pt1.X > Min.X && pt1.X < Max.X && pt1.Y > Min.Y && pt1.Y < Max.Y && pt1.Z > Min.Z && pt1.Z < Max.Z) ||
				(pt2.X > Min.X && pt2.X < Max.X && pt2.Y > Min.Y && pt2.Y < Max.Y && pt2.Z > Min.Z && pt2.Z < Max.Z))
			{
				return true;
			}

			//check?

			return true;

		}
		return false;
	}

}