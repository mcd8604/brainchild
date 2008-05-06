/*  Author: Josh Wilson
 * 
 *  Credits: Matt Christian's Camera Tutorial at 
 *           http://matt.insidegamer.org/xnatutorials.aspx
 * 
 *           Chase Camera Sample from XNA Creators Club at
 *           http://creators.xna.com/Headlines/developmentaspx/archive/2007/01/01/Chase-Camera-Sample.aspx
 * 
 *           GameEngine Framework Tutorials at
 *           http://roecode.wordpress.com/xna-gameengine-development-series/
 * 
 *  Description: Provides a general Camera class.
 * 
 * */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Engine
{
    public class Camera
    {
        /// <summary>
        /// Postition of the camera.
        /// </summary>
        private Vector3 _position = Vector3.Zero;
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                UpdateMatrices();
            }
        }

        /// <summary>
        /// Where the camera is looking at
        /// </summary>
        private Vector3 _target = Vector3.UnitZ;
        public Vector3 Target
        {
            get { return _target; }
            set
            {
                _target = value;
                UpdateMatrices();
            }
        }

        /// <summary>
        /// Camera's Up vector.
        /// </summary>
        private Vector3 _up = Vector3.Up;
        public Vector3 Up
        {
            get { return _up; }
            set
            {
                _up = value;
                UpdateMatrices();
            }
        }

        /// <summary>
        /// Perspective field of view.
        /// </summary>
        private float _fieldOfView = MathHelper.ToRadians(45.0f);
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set
            {
                _fieldOfView = value;
                UpdateMatrices();
            }
        }

        /// <summary>
        /// Aspect Ratio of the camera.
        /// </summary>
        private float _aspectRatio = 4f / 3f;
        public float AspectRatio
        {
            get { return _aspectRatio; }
            set
            {
                _aspectRatio = value;
                UpdateMatrices();
            }
        }

        /// <summary>
        /// Distance to the near clipping plane.
        /// </summary>
        private float _nearPlane = 1f;
        public float NearPlane
        {
            get { return _nearPlane; }
            set
            {
                _nearPlane = value;
                UpdateMatrices();
            }
        }

        /// <summary>
        /// Distance to the far clipping plane.
        /// </summary>
        private float _farPlane = 1000.0f;
        public float FarPlane
        {
            get { return _farPlane; }
            set
            {
                _farPlane = value;
                UpdateMatrices();
            }
        }

        /// <summary>
        /// Matrix containing coordinates of the camera.
        /// </summary>
        private Matrix _view;
        public Matrix View
        {
            get { return _view; }
            set { _view = value; }
        }

        /// <summary>
        /// The projection matrix, what can be seen.
        /// </summary>
        private Matrix _projection;
        public Matrix Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }

        /// <summary>
        /// The trapezoid that contains everything that the camera can see.
        /// </summary>
        private BoundingFrustum _frustum;
        public BoundingFrustum Frustum
        {
            get { return _frustum; }
            set { _frustum = value; }
        }

        private BoundingSphere _boundingSphere;
        public BoundingSphere BoundingSphere
        {
            get { return _boundingSphere; }
            set { _boundingSphere = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Camera()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void UpdateMatrices()
        {

        }
    }
}
