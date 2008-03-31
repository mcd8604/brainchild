using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OctreeCulling
{
    class ChaseCamera : Camera
    {
        #region Spring Physics Properties (Should go in a ChaseCamera class that extends Camera)
        /// <summary>
        /// Velocity of camera. Used for spring physics
        /// </summary>
        private Vector3 velocity;
        public Vector3 Velocity
        {
            get { return velocity; }
        }

        /// <summary>
        /// Physics coefficient which controls the influence of the camera's position
        /// over the spring force. The stiffer the spring, the closer it will stay to
        /// the chased object.
        /// </summary>
        private float stiffness = 1800.0f;
        public float Stiffness
        {
            get { return stiffness; }
            set { stiffness = value; }
        }

        /// <summary>
        /// Physics coefficient which approximates internal friction of the spring.
        /// Sufficient damping will prevent the spring from oscillating infinitely.
        /// </summary>
        private float damping = 600.0f;
        public float Damping
        {
            get { return damping; }
            set { damping = value; }
        }

        /// <summary>
        /// Mass of the camera body. Heaver objects require stiffer springs with less
        /// damping to move at the same rate as lighter objects.
        /// </summary>
        private float mass = 50.0f;
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }


        /// <summary>
        /// Desired camera position in the chased object's coordinate system.
        /// </summary>
        private Vector3 desiredPositionOffset = new Vector3(0, 2.0f, 2.0f);
        public Vector3 DesiredPositionOffset
        {
            get { return desiredPositionOffset; }
            set { desiredPositionOffset = value; }
        }

        /// <summary>
        /// Desired camera position in world space.
        /// </summary>
        private Vector3 desiredPosition;
        public Vector3 DesiredPosition
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return desiredPosition;
            }
        }

        /// <summary>
        /// Position of object being chased.
        /// </summary>
        private Vector3 chasePosition;
        public Vector3 ChasePosition
        {
            get { return chasePosition; }
            set { chasePosition = value; }
        }

        /// <summary>
        /// Direction the chased object is facing.
        /// </summary>
        private Vector3 chaseDirection;
        public Vector3 ChaseDirection
        {
            get { return chaseDirection; }
            set { chaseDirection = value; }
        }

        #endregion

        #region probably will not need
        /// <summary>
        /// 
        /// </summary>
        private Vector2 cameraAngle;
        public Vector2 CameraAngle
        {
            get { return cameraAngle; }
            set { cameraAngle = value; }
        }
        public float CameraAngleX
        {
            get { return cameraAngle.X; }
            set { cameraAngle.X = value; }
        }
        public float CameraAngleY
        {
            get { return cameraAngle.Y; }
            set { cameraAngle.Y = value; }
        }

        private float camMulti;
        public float CamMulti
        {
            get { return camMulti; }
            set { camMulti = value; }
        }

        #endregion

        //Speed of the Camera's rotation
        public float rotationSpeed = 0.05f;

        //Speed of the Camera's forward movement
        private float forwardSpeed = 0.05f;

        //Amount that the camera will turn
        public float turnAmt;

        private Vector3 _transRef;
        public Vector3 TransRef
        {
            get { return _transRef; }
            set { _transRef = value; }
        }

        private Vector3 _cameraRef;
        public Vector3 CameraRef
        {
            get { return _cameraRef; }
            set { _cameraRef = value; }
        }

        //Contains the Camera' Rotation Matrix
        private Matrix _cameraRotation;
        public Matrix CameraRotation
        {
            get { return _cameraRotation; }
            set { _cameraRotation = value; }
        }

        /// <summary>
        /// Look at point in the chased object's coordinate system.
        /// </summary>
        private Vector3 _lookAtOffset = new Vector3(0, 2.8f, 0);
        public Vector3 LookAtOffset
        {
            get { return _lookAtOffset; }
            set { _lookAtOffset = value; }
        }

        /// <summary>
        /// Camera Constructor
        /// </summary>
        //public Camera(GraphicsDeviceManager graphics)
        public ChaseCamera()
        {
            //Start aiming forward (no turn)
            turnAmt = 0;

            //Look down the Z-axis by default
            LookAt = _transRef = new Vector3(0.0f, 0.0f, 1.0f);

            //Starting position of the camera
            Position = new Vector3(0.0f, 0.0f, 0.0f);

            //Direction camera points without rotations applied
            _cameraRef = new Vector3(0.0f, 0.0f, 1.0f);

            //Aspect ratio of screen
            //aspectRatio = graphics.GraphicsDevice.Viewport.Width / graphics.GraphicsDevice.Viewport.Height;
            AspectRatio =  4.0f / 3.0f;

            //Initialize our camera rotation to identity
            _cameraRotation = Matrix.Identity;

            //Create a general view matrix from start position and original lookat
            View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);

            //Create general projection matrix for the screen
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                    AspectRatio, 0.01f, 10000.0f);
        }

        /// <summary>
        /// Update Camera Position
        /// </summary>
        /// <param name="newPos"></param>
        private void UpdatePosition(Vector3 newPos)
        {
            //Create a new rotation matrix about the Y-Axis
            Matrix yRotation = Matrix.CreateRotationY(turnAmt);

            Vector3 currPos = Vector3.Transform(newPos, yRotation);

            //Set position of our camera to the v vector's new values
            Position += currPos;
            //Position.X += currPos.X;
            //Position.Y += currPos.Y;
            //Position.Z += currPos.Z;

            //Update our lookAt Matrix
            LookAt = Position + _transRef;
            View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);
        }

        
        /// <summary>
        /// Move camera along the Z-Axis using forwardSpeed
        /// </summary>
        public void MoveForward()
        {
            //Create a new vector to calculate speeds in certain directions
            Vector3 v = new Vector3(0.0f, 0.0f, forwardSpeed);

            UpdatePosition(v);
        }
        
        /// <summary>
        /// Move camera along the Z-Axis using -forwardSpeed
        /// </summary>
        public void MoveBack()
        {
            //Create a new vector to calculate speeds in certain directions
            Vector3 v = new Vector3(0.0f, 0.0f, -forwardSpeed);

            UpdatePosition(v);
        }

        /// <summary>
        /// Move camera along the X-Axis using forwardSpeed
        /// </summary>
        public void StrafeLeft()
        {
            //Create a new vector to calculate speeds in certain directions
            Vector3 v = new Vector3(forwardSpeed, 0.0f, 0.0f);

            UpdatePosition(v);
        }

        /// <summary>
        /// Move camera along the X-Axis using -forwardSpeed
        /// </summary>
        public void StrafeRight()
        {
            //Create a new vector to calculate speeds in certain directions
            Vector3 v = new Vector3(-forwardSpeed, 0.0f, 0.0f);

            UpdatePosition(v);
        }

        /// <summary>
        /// Returns Camera Projection Matrix
        /// </summary>
        /// <returns></returns>
        //public Matrix GetProjectionMatrix()
        //{
        //    return projection;
        //}

        /// <summary>
        /// Returns Camera View Matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetViewMatrix()
        {
            //Get the newest view
            View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);

            return View;
        }

        /// <summary>
        /// Rotates Camera Around the Y-Axis
        /// </summary>
        public void RotateCamera()
        {
            //Figure out rotation about Y
            _cameraRotation = Matrix.CreateRotationY(turnAmt);

            //Calculate transform between constant reference position and our rotation
            _transRef = Vector3.Transform(_cameraRef, _cameraRotation);

            //Look at the angle reference + position offset
            LookAt = _transRef + Position;

            //Create view matrix for update
            View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);
        }

        /// <summary>
        /// Rebuilds object space values in world space. Invoke before publicly
        /// returning or privately accessing world space values.
        /// </summary>
        private void UpdateWorldPositions()
        {
            // Construct a matrix to transform from object space to worldspace
            Matrix transform = Matrix.Identity;
            transform.Forward = ChaseDirection;
            transform.Up = Up;
            transform.Right = Vector3.Cross(Up, ChaseDirection);

            // Calculate desired camera properties in world space
            desiredPosition = ChasePosition +
                Vector3.TransformNormal(DesiredPositionOffset, transform);
            LookAt = ChasePosition +
                Vector3.TransformNormal(LookAtOffset, transform);
        }

        /// <summary>
        /// Rebuilds camera's view and projection matricies.
        /// </summary>
        private void UpdateMatrices()
        {
            View = Matrix.CreateLookAt(this.Position, this.LookAt, this.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView,
                AspectRatio, NearPlane, FarPlane);
        }

        /// <summary>
        /// Forces camera to be at desired position and to stop moving. The is useful
        /// when the chased object is first created or after it has been teleported.
        /// Failing to call this after a large change to the chased object's position
        /// will result in the camera quickly flying across the world.
        /// </summary>
        public override void Reset()
        {
            UpdateWorldPositions();

            // Stop motion
            velocity = Vector3.Zero;

            // Force desired position
            Position = desiredPosition;

            UpdateMatrices();
        }

        /// <summary>
        /// Animates the camera from its current position towards the desired offset
        /// behind the chased object. The camera's animation is controlled by a simple
        /// physical spring attached to the camera and anchored to the desired position.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            UpdateWorldPositions();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate spring force
            Vector3 stretch = Position - desiredPosition;
            Vector3 force = -stiffness * stretch - damping * velocity;

            // Apply acceleration
            Vector3 acceleration = force / mass;
            velocity += acceleration * elapsed;

            // Apply velocity
            Position += velocity * elapsed;

            UpdateMatrices();
        }
    }
}
