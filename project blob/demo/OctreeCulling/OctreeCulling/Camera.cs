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
        private Quaternion _rotation;
        public Quaternion Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// World Matrix of the camera.
        /// </summary>
        private Matrix _world = Matrix.Identity;
        public Matrix World
        {
            get { return _world; }
            set { _world = value; }
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
        /// Perspective field of view.
        /// </summary>
        private float _fieldOfView = MathHelper.ToRadians(45.0f);
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set
            {
                _fieldOfView = value;
                //UpdateProjection();
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
                //UpdateProjection();
            }
        }

        /// <summary>
        /// Distance to the far clipping plane.
        /// </summary>
        private float _farPlane = 20.0f;//3500.0f;
        public float FarPlane
        {
            get { return _farPlane; }
            set
            {
                _farPlane = value;
                //UpdateProjection();
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
        private Vector3 _up = Vector3.Up;
        public Vector3 Up
        {
            get { return _up; }
            set { _up = value; }
        }

        /// <summary>
        /// Camera's look at point in world space.
        /// </summary>
        private Vector3 _lookAt;
        public Vector3 LookAt
        {
            get { return _lookAt; }
            set { _lookAt = value; }
        }

        //private float _zoom;
        //public float Zoom
        //{
        //    get { return _zoom; }
        //    set { _zoom = value; }
        //}

        //private Matrix _scale;
        //public Matrix Scale
        //{
        //    get { return _scale; }
        //    set { _scale = value; }
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

        //public virtual void Rotate(Vector3 axis, float angle)
        //{

        //}

        //public virtual void Translate(Vector3 offset)
        //{

        //}

        //public virtual void Update()
        //{

        //}

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

        #region old camera code
        /*
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
        /// Perspective field of view.
        /// </summary>
        private float _fieldOfView = MathHelper.ToRadians(45.0f);
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set 
            { 
                _fieldOfView = value;
                //UpdateProjection();
            }
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
                //UpdateProjection();
            }
        }

        /// <summary>
        /// Distance to the far clipping plane.
        /// </summary>
        private float _farPlane = 20.0f;//3500.0f;
        public float FarPlane
        {
            get { return _farPlane; }
            set 
            { 
                _farPlane = value;
                //UpdateProjection();
            }
        }

        /// <summary>
        /// Camera's Up vector.
        /// </summary>
        private Vector3 _up = Vector3.Up;
        public Vector3 Up
        {
            get { return _up; }
            set { _up = value; }
        }

        /// <summary>
        /// Camera's look at point in world space.
        /// </summary>
        private Vector3 _lookAt;
        public Vector3 LookAt
        {
            get { return _lookAt; }
            set { _lookAt = value; }
        }

        /// <summary>
        /// Amount the camera is zoomed in or out
        /// </summary>
        private float _cameraZoom;
        public float CameraZoom
        {
            get { return _cameraZoom; }
            set { _cameraZoom = value; }
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
        /// Matrix containing coordinates of the camera.
        /// </summary>
        private Matrix _view;
        public Matrix View
        {
            get { return _view; }
            set { _view = value; }
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

        private VertexPositionColor[] _boundingFrustumDrawData;
        public VertexPositionColor[] BoundingFrustumDrawData
        {
            get { return _boundingFrustumDrawData; }
            set { _boundingFrustumDrawData = value; }
        }

        private int[] _boundingFrustumIndex;
        public int[] BoundingFrustumIndex
        {
            get { return _boundingFrustumIndex; }
            set { _boundingFrustumIndex = value; }
        }

        //Screen's Aspect ratio
        private float _aspectRatio = 0.0f;
        public float AspectRatio
        {
            get { return _aspectRatio; }
            set { _aspectRatio = value; }
        }

        //Speed of the Camera's rotation
        private float _rotationSpeed = 0.05f;
        public float RotationSpeed
        {
            get { return _rotationSpeed; }
            set { _rotationSpeed = value; }
        }

        //Speed of the Camera's forward movement
        private float _forwardSpeed = 0.05f;
        public float ForwardSpeed
        {
            get { return _forwardSpeed; }
            set { _forwardSpeed = value; }
        }

        //Amount that the camera will turn about the y-axis
        private float _yaw = 0.0f;
        public float Yaw
        {
            get { return _yaw; }
            set 
            {
                _yaw = value;
                //if (_yaw >= MathHelper.Pi * 2)
                //    _yaw = MathHelper.ToRadians(0.0f);
                //else if (_yaw <= -MathHelper.Pi * 2)
                //    _yaw = MathHelper.ToRadians(0.0f);
            }
        }

        //Amount that the camera will turn about the x-axis
        private float _pitch = 0.0f;
        public float Pitch
        {
            get { return _pitch; }
            set 
            { 
                _pitch = value;
                //if (_pitch >= MathHelper.ToRadians(75))
                //    _pitch = MathHelper.ToRadians(75);
                //else if (_pitch <= MathHelper.ToRadians(-75))
                //    _pitch = MathHelper.ToRadians(-75);
            }
        }

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

        public virtual void MoveForward()
        {

        }

        public virtual void MoveBack()
        {

        }

        public virtual void MoveUp()
        {

        }

        public virtual void MoveDown()
        {

        }

        public virtual void StrafeLeft()
        {

        }

        public virtual void StrafeRight()
        {

        }

        public virtual void RotateCamera()
        {

        }

        protected virtual void UpdateProjection()
        {

        }

        public virtual void CreateBoundingFrustrumWireFrame()
        {
            Vector3[] frustumPoints = new Vector3[8];
            frustumPoints = _frustum.GetCorners();

            BoundingFrustumDrawData = new VertexPositionColor[8]
            {
                new VertexPositionColor(Position + frustumPoints[0], Color.Blue),
                new VertexPositionColor(Position + frustumPoints[1], Color.Blue),
                new VertexPositionColor(Position + frustumPoints[2], Color.Blue),
                new VertexPositionColor(Position + frustumPoints[3], Color.Blue),
                new VertexPositionColor(Position + frustumPoints[4], Color.Blue),
                new VertexPositionColor(Position + frustumPoints[5], Color.Blue),
                new VertexPositionColor(Position + frustumPoints[6], Color.Blue),
                new VertexPositionColor(Position + frustumPoints[7], Color.Blue),
                //new VertexPositionColor(Position + new Vector3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Min.Z), Color.Red),
                //new VertexPositionColor(Position + new Vector3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Min.Z), Color.Red),
                //new VertexPositionColor(Position + new Vector3(BoundingBox.Max.X, BoundingBox.Min.Y, BoundingBox.Max.Z), Color.Red),
                //new VertexPositionColor(Position + new Vector3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Max.Z), Color.Red),
                //new VertexPositionColor(Position + new Vector3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Min.Z), Color.Red),
                //new VertexPositionColor(Position + new Vector3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Min.Z), Color.Red),
                //new VertexPositionColor(Position + new Vector3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z), Color.Red),
                //new VertexPositionColor(Position + new Vector3(BoundingBox.Min.X, BoundingBox.Max.Y, BoundingBox.Max.Z), Color.Red)
            };

            BoundingFrustumIndex = new int[24];
            BoundingFrustumIndex[0] = 0;
            BoundingFrustumIndex[1] = 1;
            BoundingFrustumIndex[2] = 1;
            BoundingFrustumIndex[3] = 2;
            BoundingFrustumIndex[4] = 2;
            BoundingFrustumIndex[5] = 3;
            BoundingFrustumIndex[6] = 3;
            BoundingFrustumIndex[7] = 0;

            BoundingFrustumIndex[8] = 4;
            BoundingFrustumIndex[9] = 5;
            BoundingFrustumIndex[10] = 5;
            BoundingFrustumIndex[11] = 6;
            BoundingFrustumIndex[12] = 6;
            BoundingFrustumIndex[13] = 7;
            BoundingFrustumIndex[14] = 7;
            BoundingFrustumIndex[15] = 4;

            BoundingFrustumIndex[16] = 0;
            BoundingFrustumIndex[17] = 4;
            BoundingFrustumIndex[18] = 1;
            BoundingFrustumIndex[19] = 5;
            BoundingFrustumIndex[20] = 2;
            BoundingFrustumIndex[21] = 6;
            BoundingFrustumIndex[22] = 3;
            BoundingFrustumIndex[23] = 7;
        }
         * */
        #endregion
    }
}
