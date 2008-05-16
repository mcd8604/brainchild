using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OctreeCulling
{
    class TestCamera : Camera
    {
        private float _yaw = 0.0f;
        private float _pitch = 0.0f;

        public TestCamera(Viewport viewport)
        {
            Position = Vector3.Zero;
            NearPlane = 0.1f;
            FarPlane = 2000.0f;
            AspectRatio = viewport.AspectRatio;
            RotationSpeed = 0.05f;
            TranslationSpeed = 0.05f;

            UpdateMatrices();

            //Creates the graphical view of the Frustum trapezoid.
            CreateBoundingFrustrumWireFrame();
        }

        /// <summary>
        /// Set the position in 3d space.
        /// </summary>
        /// <param name="newPosition"></param>
        public override void SetPosition(Vector3 newPosition)
        {
            Position = newPosition;
        }

        /// <summary>
        /// Set the point in 3d space where the camera is looking.
        /// </summary>
        /// <param name="newReference"></param>
        public void SetCameraReference(Vector3 newReference)
        {
            CameraReference = newReference;
        }

        /// <summary>
        /// Move the camera in 3d space.
        /// </summary>
        /// <param name="move"></param>
        public override void Translate(Vector3 move)
        {
            Matrix forwardMovement = Matrix.CreateRotationY(_yaw);
            Vector3 v = new Vector3(0, 0, 0);
            v = Vector3.Transform(move, forwardMovement);

            Position += new Vector3(v.X, v.Y, v.Z);
            //Position = v;
        }

        /// <summary>
        /// Rotate around the Y, Default Up axis.  Usually called Yaw.
        /// </summary>
        /// <param name="angle">Angle in degrees to rotate the camera.</param>
        public override void RotateY(float angle)
        {
            angle = MathHelper.ToRadians(angle);
            //_yaw += angle;
            if (_yaw >= MathHelper.Pi * 2)
                _yaw = MathHelper.ToRadians(0.0f);
            else if (_yaw <= -MathHelper.Pi * 2)
                _yaw = MathHelper.ToRadians(0.0f);
            _yaw += angle;
        }

        /// <summary>
        /// Rotate the camera around the X axis.  Usually called pitch.
        /// </summary>
        /// <param name="angle">Angle in degrees to rotate the camera.</param>
        public override void RotateX(float angle)
        {
            angle = MathHelper.ToRadians(angle);
            _pitch += angle;
            if (_pitch >= MathHelper.ToRadians(75))
                _pitch = MathHelper.ToRadians(75);
            else if (_pitch <= MathHelper.ToRadians(-75))
                _pitch = MathHelper.ToRadians(-75);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMatrices();

            CreateBoundingFrustrumWireFrame();
        }

        public override void UpdateMatrices()
        {
            Vector3 cameraPosition = Position;
            Matrix rotationMatrix = Matrix.CreateRotationY(_yaw);
            Matrix pitchMatrix = Matrix.Multiply(Matrix.CreateRotationX(_pitch), rotationMatrix);
            Vector3 transformedReference = Vector3.Transform(CameraReference, pitchMatrix);
			//Vector3 cameraLookat = cameraPosition + transformedReference;
			LookAt = cameraPosition + transformedReference;

			View = Matrix.CreateLookAt(cameraPosition, LookAt, Vector3.Up);//cameraLookat

            Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearPlane, FarPlane);

            Frustum = new BoundingFrustum(Matrix.Multiply(View, Projection));

            BoundingSphere = BoundingSphere.CreateFromFrustum(Frustum);
        }

        public override void CreateBoundingFrustrumWireFrame()
        {
            Vector3[] frustumPoints = new Vector3[8];
            frustumPoints = Frustum.GetCorners();

            BoundingFrustumDrawData = new VertexPositionColor[8]
            {
                //new VertexPositionColor(Position + frustumPoints[0], Color.Blue),
                //new VertexPositionColor(Position + frustumPoints[1], Color.Blue),
                //new VertexPositionColor(Position + frustumPoints[2], Color.Blue),
                //new VertexPositionColor(Position + frustumPoints[3], Color.Blue),
                //new VertexPositionColor(Position + frustumPoints[4], Color.Blue),
                //new VertexPositionColor(Position + frustumPoints[5], Color.Blue),
                //new VertexPositionColor(Position + frustumPoints[6], Color.Blue),
                //new VertexPositionColor(Position + frustumPoints[7], Color.Blue),

                new VertexPositionColor(frustumPoints[0], Color.Blue),
                new VertexPositionColor(frustumPoints[1], Color.Blue),
                new VertexPositionColor(frustumPoints[2], Color.Blue),
                new VertexPositionColor(frustumPoints[3], Color.Blue),
                new VertexPositionColor(frustumPoints[4], Color.Blue),
                new VertexPositionColor(frustumPoints[5], Color.Blue),
                new VertexPositionColor(frustumPoints[6], Color.Blue),
                new VertexPositionColor(frustumPoints[7], Color.Blue),
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
        
        #region Quaternion code. Not used.
        /*
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="viewport">Graphics Device Viewport</param>
        public TestCamera(Viewport viewport)
        {
            Position = new Vector3(0.0f, 0.0f, 0.0f);

            Rotation = Quaternion.Identity;

            NearPlane = 0.1f;

            FarPlane = 2000.0f;

            AspectRatio = viewport.AspectRatio;

            RotationSpeed = 0.0005f;
            TranslationSpeed = 0.05f;

            Update();
        }

        /// <summary>
        /// Rotates the Camera around the given axis with the given angle
        /// </summary>
        /// <param name="axis">Axis to rotate the camera on.</param>
        /// <param name="angle">Angle to rotate the camera.</param>
        public override void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(Rotation));

            Rotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * Rotation);

            Update();
        }

        /// <summary>
        /// Moves the Camera with the given offset.
        /// </summary>
        /// <param name="offset">Vector3 Offset to move the camera</param>
        public override void Translate(Vector3 offset)
        {
            Position += Vector3.Transform(offset, Matrix.CreateFromQuaternion(Rotation));

            Update();
        }

        /// <summary>
        /// Updates the camera
        /// </summary>
        public override void Update()
        {
            // Computes the camera matrices.
            World = Matrix.Identity;
            View = Matrix.Invert(Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position));
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FieldOfView), 
                AspectRatio, NearPlane, FarPlane);

            // Computes the bounding frustum.
            Frustum = new BoundingFrustum(View * Projection);

            //Creates the graphical view of the Frustum trapezoid.
            CreateBoundingFrustrumWireFrame();
        }
         * */
        #endregion
    }
}
