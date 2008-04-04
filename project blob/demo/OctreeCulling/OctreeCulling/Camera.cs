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

namespace OctreeCulling
{
    public class Camera
    {
        /// <summary>
        /// Postition of the camera.
        /// </summary>
        private Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Quaternion rotation of the camera.
        /// </summary>
        //private Quaternion _rotation;
        //public Quaternion Rotation
        //{
        //    get { return _rotation; }
        //    set { _rotation = value; }
        //}

        /// <summary>
        /// World Matrix of the camera.
        /// </summary>
        //private Matrix _world = Matrix.Identity;
        //public Matrix World
        //{
        //    get { return _world; }
        //    set { _world = value; }
        //}

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
        /// Perspective field of view.
        /// </summary>
        private float _fieldOfView = MathHelper.ToRadians(45.0f);
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set
            {
                _fieldOfView = value;
            }
        }

        /// <summary>
        /// Aspect Ratio of the camera.
        /// </summary>
        private float _aspectRatio = 0.0f;
        public float AspectRatio
        {
            get { return _aspectRatio; }
            set { _aspectRatio = value; }
        }

        /// <summary>
        /// Distance to the near clipping plane.
        /// </summary>
        private float _nearPlane = 0.1f;
        public float NearPlane
        {
            get { return _nearPlane; }
            set
            {
                _nearPlane = value;
            }
        }

        /// <summary>
        /// Distance to the far clipping plane.
        /// </summary>
        private float _farPlane = 20.0f;
        public float FarPlane
        {
            get { return _farPlane; }
            set
            {
                _farPlane = value;
            }
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

        /// <summary>
        /// The Vertex positions of the Frustum used to draw the frustum wireframe
        /// </summary>
        private VertexPositionColor[] _boundingFrustumDrawData;
        public VertexPositionColor[] BoundingFrustumDrawData
        {
            get { return _boundingFrustumDrawData; }
            set { _boundingFrustumDrawData = value; }
        }

        /// <summary>
        /// The frustum indexes used to draw the frustum wireframe
        /// </summary>
        private int[] _boundingFrustumIndex;
        public int[] BoundingFrustumIndex
        {
            get { return _boundingFrustumIndex; }
            set { _boundingFrustumIndex = value; }
        }

        /// <summary>
        /// Rotation speed of the camera.
        /// </summary>
        private float _rotationSpeed;
        public float RotationSpeed
        {
            get { return _rotationSpeed; }
            set { _rotationSpeed = value; }
        }

        /// <summary>
        /// Movement speed of the camera.
        /// </summary>
        private float _translationSpeed;
        public float TranslationSpeed
        {
            get { return _translationSpeed; }
            set { _translationSpeed = value; }
        }

        /// <summary>
        /// Camera's Up vector.
        /// </summary>
        //private Vector3 _up = Vector3.Up;
        //public Vector3 Up
        //{
        //    get { return _up; }
        //    set { _up = value; }
        //}

        /// <summary>
        /// Camera's look at point in world space.
        /// </summary>
        //private Vector3 _lookAt;
        //public Vector3 LookAt
        //{
        //    get { return _lookAt; }
        //    set { _lookAt = value; }
        //}

        /// <summary>
        /// The spot in 3d space where the camera is looking.
        /// </summary>
        private Vector3 _cameraReference = new Vector3(0, 0, 1);
        public Vector3 CameraReference
        {
            get { return _cameraReference; }
            set { _cameraReference = value; }
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

        public virtual void SetPosition(Vector3 newPosition)
        {

        }

        public virtual void Translate(Vector3 move)
        {

        }

        public virtual void RotateX(float angle)
        {

        }

        public virtual void RotateY(float angle)
        {

        }

        public virtual void RotateZ(float angle)
        {

        }

        public virtual void Reset()
        {

        }

        public virtual void CreateBoundingFrustrumWireFrame()
        {
            
        }

        public virtual void UpdateMatrices()
        {

        }
    }
}
