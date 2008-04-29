using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project_blob
{
	class Portal
	{
		private BoundingBox _boundingBox;
		public BoundingBox BoundingBox
		{
			get { return _boundingBox; }
			set { _boundingBox = value; }
		}

        private BoundingSphere _boundingSphere;
        public BoundingSphere BoundingSphere
        {
            get { return _boundingSphere; }
            set { _boundingSphere = value; }
        }

		private List<int> _connectedSectors;
		public List<int> ConnectedSectors
		{
			get { return _connectedSectors; }
			set { _connectedSectors = value; }
		}

        private Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Vector3 _scale;
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

		public Portal(Vector3 size, Vector3 position)
		{
            _connectedSectors = new List<int>();

            _scale = size;
            _position = position;

            CreateBoundingBox();
		}

        public BoundingBox GetBoundingBoxTransformed()
        {
            Vector3 min, max;
            min = _boundingBox.Min;
            max = _boundingBox.Max;

            min = Vector3.Transform(_boundingBox.Min, Matrix.CreateTranslation(_position));
            max = Vector3.Transform(_boundingBox.Max, Matrix.CreateTranslation(_position));

            return new BoundingBox(min, max);
        }

        private void CreateBoundingBox()
        {
            _boundingBox = new BoundingBox(new Vector3(-1.0f, -1.0f, -1.0f) * _scale, new Vector3(1.0f, 1.0f, 1.0f) * _scale);
        }

        private void CreateBoundingSphere()
        {
            _boundingSphere = BoundingSphere.CreateFromBoundingBox(_boundingBox);
        }
	}
}
