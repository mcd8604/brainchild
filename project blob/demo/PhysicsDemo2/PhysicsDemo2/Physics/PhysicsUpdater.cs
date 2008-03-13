using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsDemo2.Physics
{
	public class PhysicsUpdater
	{
		private static PhysicsUpdater _instance;
		private static object _syncRoot = new Object();

		//! Instance
		public static PhysicsUpdater getSingleton
		{
			get
			{
				if (_instance == null)
				{
					lock (_syncRoot)
					{
						if (_instance == null)
							_instance = new PhysicsUpdater();
					}
				}

				return _instance;
			}
		}

		private PhysicsUpdater()
		{

		}
	}
}
