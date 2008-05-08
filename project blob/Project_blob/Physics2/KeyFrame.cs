using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class KeyFrame
	{
		private Vector3 position;
		public Vector3 Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		private float time;
		public float Time
		{
			get
			{
				return time;
			}
			set
			{
				time = value;
			}
		}
	}
}
