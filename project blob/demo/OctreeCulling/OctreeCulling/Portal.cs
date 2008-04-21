using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OctreeCulling
{
	class Portal
	{
		private BoundingBox _boundingBox;
		public BoundingBox BoundingBox
		{
			get { return _boundingBox; }
			set { _boundingBox = value; }
		}

		private List<int> _connectedSectors;
		public List<int> ConnectedSectors
		{
			get { return _connectedSectors; }
			set { _connectedSectors = value; }
		}

		public Portal()
		{

		}
	}
}
