using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
	public static class Player
	{


		private static Body playerObject = null;
		public static Body Player
		{
			set
			{
				playerObject = value;
			}
			get
			{
				return playerObject;
			}
		}


		// Cling
		static Property cling = new Property();
		public static Property Cling
		{
			get
			{
				return cling;
			}
		}


		// Traction
		static Property traction = new Property();
		public static Property Traction
		{
			get
			{
				return traction;
			}
		}

		// Resilience
		static Property resilience = new Property();
		public static Property Resilience
		{
			get
			{
				return resilience;
			}
		}

		// Volume
		static Property volume = new Property();
		public static Property Volume
		{
			get
			{
				return volume;
			}
		}





		internal static void update(float time)
		{
			update(cling, time);
			update(traction, time);
			update(resilience, time);
			update(volume, time);
		}

		private static void update(Property p, float time)
		{

			if (p.target != p.current)
			{
				float diff = p.current - p.target;
				float delta = p.delta * time;
				if (Math.Abs(diff) < Math.Abs(delta))
				{
					p.current = p.target;
				}
				else
				{
					if (diff > 0f)
					{
						p.current += delta;
					}
					else
					{
						p.current -= delta;
					}
				}
			}

			if (p.current > 0.5f)
			{
				p.value = p.origin + ((p.current - 0.5f) * (p.maximum - p.origin));
			}
			else
			{
				p.value = p.minimum + (p.current * (p.origin - p.minimum));
			}

		}

	}

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

		internal float delta;
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
		internal float current = 0.5;
		internal float value;
		

		internal float last; //?
		
	}
}
