using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class BasicCamera
    {
        private Vector3 _position;
        public Vector3 Postiion
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Where the camera is looking at
        /// </summary>
        private Vector3 _target;
        public Vector3 Target
        {
            get { return _target; }
            set { _target = value; }
        }

        private Vector3 _up;
        public Vector3 Up
        {
            get { return _up; }
            set { _up = value; }
        }

        private float _fieldOfView = MathHelper.ToRadians(45.0f);
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set { _fieldOfView = value; }
        }

        private float _aspectRatio;
        public float AspectRatio
        {
            get { return _aspectRatio; }
            set { _aspectRatio = value; }
        }

        private float _nearPlane;
        public float NearPlane
        {
            get { return _nearPlane; }
            set { _nearPlane = value; }
        }

        private float _farPlane;
        public float FarPlane
        {
            get { return _farPlane; }
            set { _farPlane = value; }
        }

        private Matrix _view;
        public Matrix View
        {
            get { return _view; }
            set { _view = value; }
        }

        private Matrix _projection;
        public Matrix Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }

        private BoundingFrustum _frustum;
        public BoundingFrustum Frustum
        {
            get { return _frustum; }
            set { _frustum = value; }
        }

        public BasicCamera()
        {

        }

        public void Update(GameTime gameTime)
        {
            UpdateMatrices();
            
        }

        public void UpdateMatrices()
        {

            View = Matrix.CreateLookAt(_position, _target, _up);

            Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearPlane, FarPlane);

            _frustum = new BoundingFrustum(Matrix.Multiply(View, Projection));
        }
    }
}
