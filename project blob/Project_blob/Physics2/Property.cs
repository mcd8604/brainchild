using System;

namespace Physics2
{
	public class Property
	{
		internal float minimum;
		/// <summary>
		/// The minimum value this property is allowed, corresponding to a target of 0;
		/// This value is considered to be a constant by the physics engine.
		/// </summary>
		public float Minimum
		{
			get
			{
				return minimum;
			}
			set
			{
				minimum = value;
			}
		}

		internal float maximum;
		/// <summary>
		/// The maximum value this property is allowed, corresponding to a target of 1;
		/// This value is considered to be a constant by the physics engine.
		/// </summary>
		public float Maximum
		{
			get
			{
				return maximum;
			}
			set
			{
				maximum = value;
			}
		}

		internal float origin;
		/// <summary>
		/// The default value for this property, corresponding to a target of 0.5;
		/// This value is considered to be a constant by the physics engine.
		/// </summary>
		public float Origin
		{
			get
			{
				return origin;
			}
			set
			{
				origin = value;
			}
		}

		internal float delta = 1f;
		/// <summary>
		/// The maximum rate of change for this property;
		/// </summary>
		public float Delta
		{
			get
			{
				return delta;
			}
			set
			{
				delta = value;
			}
		}

		internal float target = 0.5f;
		/// <summary>
		/// The target value for this property at this moment, as indicated by the user, if applicable, as a ratio from 0 to 1.
		/// </summary>
		public float Target
		{
			get
			{
				return target;
			}
			set
			{
				if (value < 0f || value > 1f)
				{
					throw new ArgumentOutOfRangeException("Target must be between 0 and 1, given " + value);
				}
				target = value;
			}
		}

		/// <summary>
		/// The current position, as limited by delta, as a value between 0 and 1.
		/// </summary>
		internal float current = 0.49f;
		internal float value;
		internal bool changed = true;

	}
}
