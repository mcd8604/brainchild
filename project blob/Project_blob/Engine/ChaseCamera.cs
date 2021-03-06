#region File Description
//-----------------------------------------------------------------------------
// ChaseCamera.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace Engine
{
	public class ChaseCamera : Camera
	{
		#region Chased object properties (set externally each frame)

		/// <summary>
		/// Position of object being chased.
		/// </summary>
		public Vector3 ChasePosition
		{
			get { return chasePosition; }
			set { chasePosition = value; }
		}
		private Vector3 chasePosition;

		/// <summary>
		/// Direction the chased object is facing.
		/// </summary>
		public Vector3 ChaseDirection
		{
			get { return chaseDirection; }
            set
            {
                if (chaseDirection != Vector3.Zero)
                    chaseDirection = value;
            }
		}
		private Vector3 chaseDirection = Vector3.Forward;

		private Vector3 m_ClimbNormal;
		public Vector3 ClimbNormal
		{
			set { m_ClimbNormal = value; }
			get { return m_ClimbNormal; }
		}

		#endregion

		#region Desired camera positioning (set when creating camera or changing view)

		private bool m_Climbing = false;
		public bool Climbing
		{
			get { return m_Climbing; }
			set { m_Climbing = value; }
		}

		/// <summary>
		/// Desired camera position in the chased object's coordinate system.
		/// </summary>
		public float DesiredPositionOffset
		{
			get { return desiredPositionOffset; }
			set { desiredPositionOffset = value; }
		}
		private float desiredPositionOffset = 10f;

		/// <summary>
		/// Desired camera position in world space.
		/// </summary>
		public Vector3 DesiredPosition
		{
			get
			{
				// Ensure correct value even if update has not been called this frame
				//UpdateWorldPositions();

				return desiredPosition;
			}

			set
			{
				desiredPosition = value;
			}

		}
		private Vector3 desiredPosition;

		public Vector2 UserOffset
		{
			get
			{
				return m_UserOffset;
			}
			set
			{
				m_UserOffset = value;
			}
		}
		private Vector2 m_UserOffset;

		/// <summary>
		/// Look at point in the chased object's coordinate system.
		/// </summary>
		public Vector3 LookAtOffset
		{
			get { return lookAtOffset; }
			set { lookAtOffset = value; }
		}
		private Vector3 lookAtOffset = new Vector3(0, 2.8f, 0);

		/// <summary>
		/// Look at point in world space.
		/// </summary>
		public Vector3 LookAt
		{
			get
			{
				// Ensure correct value even if update has not been called this frame
				//UpdateWorldPositions();

				return lookAt;
			}
		}
		private Vector3 lookAt;

		#endregion

		#region Camera physics (typically set when creating camera)

		/// <summary>
		/// Physics coefficient which controls the influence of the camera's position
		/// over the spring force. The stiffer the spring, the closer it will stay to
		/// the chased object.
		/// </summary>
		public float Stiffness
		{
			get { return stiffness; }
			set { stiffness = value; }
		}
		private float stiffness = 1800.0f;

		/// <summary>
		/// Physics coefficient which approximates internal friction of the spring.
		/// Sufficient damping will prevent the spring from oscillating infinitely.
		/// </summary>
		public float Damping
		{
			get { return damping; }
			set { damping = value; }
		}
		private float damping = 600.0f;

		/// <summary>
		/// Mass of the camera body. Heaver objects require stiffer springs with less
		/// damping to move at the same rate as lighter objects.
		/// </summary>
		public float Mass
		{
			get { return mass; }
			set { mass = value; }
		}
		private float mass = 50.0f;

		#endregion

		#region Current camera properties (updated by camera physics)

		/// <summary>
		/// Velocity of camera.
		/// </summary>
		public Vector3 Velocity
		{
			get { return velocity; }
		}
		private Vector3 velocity;

		#endregion

		#region Methods

		/// <summary>
		/// Rebuilds object space values in world space. Invoke before publicly
		/// returning or privately accessing world space values.
		/// </summary>
		//private void UpdateWorldPositions()
		//{
		//    // Construct a matrix to transform from object space to worldspace
		//    Matrix transform = Matrix.Identity;
		//    transform.Forward = ChaseDirection;
		//    transform.Up = Up;
		//    transform.Right = Vector3.Cross(Up, ChaseDirection);

		//    // Calculate desired camera properties in world space
		//    desiredPosition = ChasePosition +
		//        Vector3.TransformNormal(DesiredPositionOffset, transform);
		//    lookAt = ChasePosition +
		//        Vector3.TransformNormal(LookAtOffset, transform);
		//}

		/// <summary>
		/// Rebuilds camera's view and projection matricies.
		/// </summary>
		public override void UpdateMatrices()
		{
			View = Matrix.CreateLookAt(this.Position, this.LookAt, this.Up);
			Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView,
				AspectRatio, NearPlane, FarPlane);
			Frustum = new BoundingFrustum(Matrix.Multiply(View, Projection));

			BoundingSphere = BoundingSphere.CreateFromFrustum(Frustum);
		}

		/// <summary>
		/// Forces camera to be at desired position and to stop moving. The is useful
		/// when the chased object is first created or after it has been teleported.
		/// Failing to call this after a large change to the chased object's position
		/// will result in the camera quickly flying across the world.
		/// </summary>
		public void Reset()
		{
			//UpdateWorldPositions();

			// Stop motion
			velocity = Vector3.Zero;

			// Force desired position
			Position = desiredPosition;

            chaseDirection = Vector3.Forward;

			UpdateMatrices();
		}

		/// <summary>
		/// Animates the camera from its current position towards the desired offset
		/// behind the chased object. The camera's animation is controlled by a simple
		/// physical spring attached to the camera and anchored to the desired position.
		/// </summary>
		public override void Update(GameTime gameTime)
		{
#if DEBUG
            if (chaseDirection == Vector3.Zero)
            {
                throw new Exception("Chase Direction set to Vector3.Zero. This should never happen and will break physics.");
            }
			if (gameTime == null)
				throw new ArgumentNullException("gameTime");
#endif

			//UpdateWorldPositions();
			if (Climbing)
			{
				desiredPosition = chasePosition + (m_ClimbNormal * desiredPositionOffset * 2);
				desiredPosition -= chaseDirection;
			}
			else
			{
				desiredPosition = chasePosition - (Vector3.Normalize(chaseDirection) * DesiredPositionOffset);
				desiredPosition.Y += 5f;
				Vector3 rightVector = Vector3.Cross(desiredPosition - chasePosition, Up);
				desiredPosition += rightVector * m_UserOffset.X;
				desiredPosition.Y += (m_UserOffset.Y * 2);
			}

			lookAt = ChasePosition;



			//m_UserOffset = Vector2.Multiply(m_UserOffset,.9f);

			//if(Vector3.Dot(lookAt - desiredPosition,Vector3.Up) < 35)
			//{
			//    desiredPosition.Y = ChasePosition.Y + 5;
			//}
			//if (Vector3.Dot(lookAt - desiredPosition, Vector3.Down) < 35)
			//{
			//    desiredPosition.Y = ChasePosition.Y - 5;
			//}

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
			base.Update(gameTime);
		}

		#endregion
	}
}
