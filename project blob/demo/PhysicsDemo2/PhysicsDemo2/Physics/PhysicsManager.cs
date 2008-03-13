using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsDemo2.Physics
{
	public class PhysicsManager
	{
		private static PhysicsManager _instance;
		private static object _syncRoot = new Object();

		//! Instance
		public static PhysicsManager getSingleton
		{
			get
			{
				if (_instance == null)
				{
					lock (_syncRoot)
					{
						if (_instance == null)
							_instance = new PhysicsManager();
					}
				}

				return _instance;
			}
		}

		private PhysicsManager()
		{

		}
	}
}
