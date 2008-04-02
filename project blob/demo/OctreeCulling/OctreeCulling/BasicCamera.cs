using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OctreeCulling
{
    class BasicCamera : Camera
    {
        //reference vectors
        private Vector3 transRef;
        private Vector3 cameraRef;

        //Contains the Camera' Rotation Matrix
        private Matrix cameraRotation;

        //Amount that the camera will turn about the z-axis
        private float _roll = 0.0f;
        public float Roll
        {
            get { return _roll; }
            set { _roll = value; }
        }

        /// <summary>
        /// Camera Constructor
        /// </summary>
        public BasicCamera()
        {
            //Start aiming forward (no turn)
            Yaw = 0;

            //Look down the Z-axis by default
            LookAt = transRef = new Vector3(0.0f, 0.0f, 1.0f);

            //Starting position of the camera
            Position = new Vector3(0.0f, 0.0f, 0.0f);

            //Direction camera points without rotations applied
            cameraRef = new Vector3(0.0f, 0.0f, 1.0f);

            //Aspect ratio of screen
            //aspectRatio = graphics.GraphicsDevice.Viewport.Width / graphics.GraphicsDevice.Viewport.Height;
            AspectRatio = 4.0f/3.0f;

            //Initialize our camera rotation to identity
            cameraRotation = Matrix.Identity;

            //Create a general view matrix from start position and original lookat
            View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);

            //Create general projection matrix for the screen
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                    AspectRatio, 0.01f, 10000.0f);

            UpdatePosition(Position);
        }

        /// <summary>
        /// Update Camera Position
        /// </summary>
        /// <param name="newPos"></param>
        private void UpdatePosition(Vector3 newPos)
        {
            //Create a new rotation matrix about the Y-Axis
            Matrix yRotation = Matrix.CreateRotationY(Yaw);

            Vector3 currPos = Vector3.Transform(newPos, yRotation);

            //Set position of our camera to the v vector's new values
            Position += currPos;
            //position.X += currPos.X;
            //position.Y += currPos.Y;
            //position.Z += currPos.Z;

            //Update our lookAt Matrix
            LookAt = Position + transRef;
            View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);

            Frustum = new BoundingFrustum(Matrix.Multiply(View, Projection));

            CreateBoundingFrustrumWireFrame();
        }

        /// <summary>
        /// Move camera along the Z-Axis using forwardSpeed
        /// </summary>
        public override void MoveForward()
        {
            //Create a new vector to calculate speeds in certain directions
            Vector3 v = new Vector3(0.0f, 0.0f, ForwardSpeed);

            UpdatePosition(v);
        }
        
        /// <summary>
        /// Move camera along the Z-Axis using -forwardSpeed
        /// </summary>
        public override void MoveBack()
        {
            //Create a new vector to calculate speeds in certain directions
            Vector3 v = new Vector3(0.0f, 0.0f, -ForwardSpeed);

            UpdatePosition(v);
        }

        /// <summary>
        /// Move camera along the X-Axis using forwardSpeed
        /// </summary>
        public override void StrafeLeft()
        {
            //Create a new vector to calculate speeds in certain directions
            Vector3 v = new Vector3(ForwardSpeed, 0.0f, 0.0f);

            UpdatePosition(v);
        }

        /// <summary>
        /// Move camera along the X-Axis using -forwardSpeed
        /// </summary>
        public override void StrafeRight()
        {
            //Create a new vector to calculate speeds in certain directions
            Vector3 v = new Vector3(-ForwardSpeed, 0.0f, 0.0f);

            UpdatePosition(v);
        }

        public override void MoveUp()
        {
            Vector3 v = new Vector3(0.0f, ForwardSpeed, 0.0f);

            UpdatePosition(v);
        }

        public override void MoveDown()
        {
            Vector3 v = new Vector3(0.0f, -ForwardSpeed, 0.0f);

            UpdatePosition(v);
        }

        /// <summary>
        /// Returns Camera Projection Matrix
        /// </summary>
        /// <returns></returns>
        //public Matrix GetProjectionMatrix()
        //{
        //    return Projection;
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

        #region These rotations not needed. Incorporated into RotateCamera
        /// <summary>
        /// Rotates Camera Around the Y-Axis
        /// </summary>
        //public void RotateCameraY()
        //{
        //    //Figure out rotation about Y
        //    cameraRotation = Matrix.CreateRotationY(_yaw);

        //    //Calculate transform between constant reference position and our rotation
        //    transRef = Vector3.Transform(cameraRef, cameraRotation);

        //    //Look at the angle reference + position offset
        //    LookAt = transRef + Position;

        //    //Create view matrix for update
        //    View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);
        //}

        /// <summary>
        /// Rotates Camera Around the X-Axis
        /// </summary>
        //public void RotateCameraX()
        //{
        //    //Figure out rotation about Y
        //    //cameraRotation = Matrix.CreateRotationY(turnAmt);
        //    cameraRotation = Matrix.CreateRotationX(_pitch);

        //    //Calculate transform between constant reference position and our rotation
        //    transRef = Vector3.Transform(cameraRef, cameraRotation);

        //    //Look at the angle reference + position offset
        //    LookAt = transRef + Position;

        //    //Create view matrix for update
        //    View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);
        //}
        #endregion

        /// <summary>
        /// Rotates Camera Around the respective axises with yaw, pitch, roll
        /// </summary>
        public override void RotateCamera()
        {
            //Figure out rotation about x, y, z
            //cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, _roll);

            Matrix rotationMatrix = Matrix.CreateRotationY(Yaw);
            Matrix pitchMatrix = Matrix.Multiply(Matrix.CreateRotationX(Pitch), rotationMatrix);
            transRef = Vector3.Transform(cameraRef, pitchMatrix);

            //Calculate transform between constant reference position and our rotation
            //transRef = Vector3.Transform(cameraRef, cameraRotation);

            //Look at the angle reference + position offset
            LookAt = transRef + Position;

            //Create view matrix for update
            View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);

            Frustum = new BoundingFrustum(Matrix.Multiply(View, Projection));

            CreateBoundingFrustrumWireFrame();
        }

        public override void Reset()
        {
            //Look down the Z-axis by default
            LookAt = transRef = new Vector3(0.0f, 0.0f, 1.0f);

            //Starting position of the camera
            Position = new Vector3(0.0f, 0.0f, 0.0f);

            //Direction camera points without rotations applied
            cameraRef = new Vector3(0.0f, 0.0f, 1.0f);

            //Initialize our camera rotation to identity
            cameraRotation = Matrix.Identity;

            //Create a general view matrix from start position and original lookat
            View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);

            Frustum = new BoundingFrustum(Matrix.Multiply(View, Projection));

            CreateBoundingFrustrumWireFrame();
        }
    }
}
