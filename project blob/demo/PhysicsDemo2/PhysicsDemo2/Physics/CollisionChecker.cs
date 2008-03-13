using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsDemo2.Physics
{
	public class CollisionChecker
	{
		private static CollisionChecker _instance;
		private static object _syncRoot = new Object();

		//! Instance
		public static CollisionChecker getSingleton
		{
			get
			{
				if (_instance == null)
				{
					lock (_syncRoot)
					{
						if (_instance == null)
							_instance = new CollisionChecker();
					}
				}

				return _instance;
			}
		}

		private CollisionChecker()
		{

		}
	}
}
