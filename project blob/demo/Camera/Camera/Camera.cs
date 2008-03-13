/*  Author: Josh Wilson
 * 
 *  Credits: Matt Christian's Camera Tutorial at 
 *           http://matt.insidegamer.org/xnatutorials.aspx
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

namespace Camera
{
    class Camera
    {
        //Speed of the Camera's rotation
        public float rotationSpeed = 0.05f;

        //Speed of the Camera's forward movement
        private float forwardSpeed = 0.05f;

        //Amount that the camera will turn
        public float turnAmt;

        //Position and reference vectors
        private Vector3 position;
        private Vector3 transRef;
        private Vector3 cameraRef;
        private Vector3 lookAt;

        //Screen's Aspect ratio
        private float aspectRatio = 0.0f;

        //Contains the Camera' Rotation Matrix
        private Matrix cameraRotation;

        //Projection Matrix
        private Matrix projection;

        //View Matrix
        private Matrix view;

        /// <summary>
        /// Camera Constructor
        /// </summary>
        /// <param name="graphics"></param>
        public Camera(GraphicsDeviceManager graphics)
        {
            //Start aiming forward (no turn)
            turnAmt = 0;

            //Look down the Z-axis by default
            lookAt = transRef = new Vector3(0.0f, 0.0f, 1.0f);

            //Starting position of the camera
            position = new Vector3(0.0f, 0.0f, 0.0f);

            //Direction camera points without rotations applied
            cameraRef = new Vector3(0.0f, 0.0f, 1.0f);

            //Aspect ratio of screen
            aspectRatio = graphics.GraphicsDevice.Viewport.Width / graphics.GraphicsDevice.Viewport.Height;

            //Initialize our camera rotation to identity
            cameraRotation = Matrix.Identity;

            //Create a general view matrix from start position and original lookat
            view = Matrix.CreateLookAt(position, lookAt, Vector3.Up);

            //Create general projection matrix for the screen
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                    aspectRatio, 0.01f, 10000.0f);
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
            position.X += currPos.X;
            position.Y += currPos.Y;
            position.Z += currPos.Z;

            //Update our lookAt Matrix
            lookAt = position + transRef;
            view = Matrix.CreateLookAt(position, lookAt, Vector3.Up);
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
        public Matrix GetProjectionMatrix()
        {
            return projection;
        }

        /// <summary>
        /// Returns Camera View Matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetViewMatrix()
        {
            //Get the newest view
            view = Matrix.CreateLookAt(position, lookAt, Vector3.Up);

            return view;
        }

        /// <summary>
        /// Rotates Camera Around the Y-Axis
        /// </summary>
        public void RotateCamera()
        {
            //Figure out rotation about Y
            cameraRotation = Matrix.CreateRotationY(turnAmt);

            //Calculate transform between constant reference position and our rotation
            transRef = Vector3.Transform(cameraRef, cameraRotation);

            //Look at the angle reference + position offset
            lookAt = transRef + position;

            //Create view matrix for update
            view = Matrix.CreateLookAt(position, lookAt, Vector3.Up);
        }
    }
}
