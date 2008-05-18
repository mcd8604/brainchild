using System;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Player
	{

		private Body playerBody = null;
		/// <summary>
		/// The Body controlled by the player.
		/// </summary>
		public Body PlayerBody
		{
			set
			{
				playerBody = value;
			}
			get
			{
				return playerBody;
			}
		}

		private Property cling = new Property();
		/// <summary>
		/// The ability to stick to surfaces.
		/// </summary>
		public Property Cling
		{
			get
			{
				return cling;
			}
		}

		private Property traction = new Property();
		/// <summary>
		/// The friction opposing sliding along a surface.
		/// </summary>
		public Property Traction
		{
			get
			{
				return traction;
			}
		}

		private Property resilience = new Property();
		/// <summary>
		/// The resistance to deformation.
		/// </summary>
		public Property Resilience
		{
			get
			{
				return resilience;
			}
		}

		private Property volume = new Property();
		/// <summary>
		/// The internal pressure.
		/// </summary>
		public Property Volume
		{
			get
			{
				return volume;
			}
		}

#if DEBUG
		public bool DEBUG_MoveModeFlag = false;
#endif

		public void move(Vector2 input, Vector3 reference)
		{
			moveInput = input;
			refPos = reference;
			moveflag = true;
		}

		private bool moveflag = false;
		private Vector2 moveInput;
		private Vector3 refPos;

		private float twist;
		/// <summary>
		/// The force applied to rotate the blob by the player's input on the ground.
		/// </summary>
		public float Twist
		{
			get
			{
				return twist;
			}
			set
			{
				twist = value;
			}
		}

		private float airTwist;
		/// <summary>
		/// The force applied to rotate the blob by the player's input while in the air.
		/// </summary>
		public float AirTwist
		{
			get
			{
				return airTwist;
			}
			set
			{
				airTwist = value;
			}
		}

		private float drift;
		/// <summary>
		/// The force applied to drift the blob by the player's input while on the ground.
		/// </summary>
		public float Drift
		{
			get
			{
				return -drift;
			}
			set
			{
				drift = -value;
			}
		}

		private float airDrift;
		/// <summary>
		/// The force applied to rotate the blob by the player's input while in the air.
		/// </summary>
		public float AirDrift
		{
			get
			{
				return -airDrift;
			}
			set
			{
				airDrift = -value;
			}
		}

		private bool touching = false;
		/// <summary>
		/// Is the blob touching something?
		/// </summary>
		public bool Touching
		{
			get
			{
				return touching;
			}
		}

		/// <summary>
		/// fake jump trigger
		/// </summary>
		public void jump() { jumpflag = true; }

		private bool jumpflag = false;

		private float maxJumpWork;
		/// <summary>
		/// Maximum force to apply when jumping off of something.
		/// </summary>
		public float MaxJumpWork
		{
			get
			{
				return maxJumpWork;
			}
			set
			{
				maxJumpWork = value;
			}
		}

		private float airJumpWork;
		/// <summary>
		/// Force to apply when jumping in mid air.
		/// </summary>
		public float AirJumpWork
		{
			get
			{
				return airJumpWork;
			}
			set
			{
				airJumpWork = value;
			}
		}

		private Vector3 jumpVector;

		public Vector3 Normal
		{
			get
			{
				return jumpVector;
			}
		}

		internal void update(float time)
		{
			update(cling, time);
			update(traction, time);
			update(resilience, time);
			update(volume, time);

			#region Jump
			jumpVector = Util.Zero;
			foreach (PhysicsPoint p in PlayerBody.points)
			{
				if (p.LastCollision != null)
				{
					jumpVector += p.LastCollision.Normal;
				}
			}

			if (jumpVector != Util.Zero)
			{
				touching = true;
				jumpVector = Vector3.Normalize(jumpVector);
			}
			else
			{
				touching = false;
			}

			if (jumpflag)
			{
				jumpflag = false;

				Vector3 JumpForce = Vector3.Up * (airJumpWork / time);

				if (touching)
				{
					if (playerBody is BodyPressure)
					{
						JumpForce += jumpVector * (MathHelper.Clamp(maxJumpWork - ((BodyPressure)playerBody).Volume, maxJumpWork * 0.5f, maxJumpWork) / time);
					}
					else
					{
						JumpForce += jumpVector * (maxJumpWork / time);
					}
				}

				// Fake Fake Jump:
				foreach (PhysicsPoint p in playerBody.points)
				{
					p.ForceThisFrame += JumpForce;
				}
			}
			#endregion

			#region Movement
			if (moveflag)
			{
				moveflag = false;
				Vector3 CurrentPlayerCenter = playerBody.getCenter();

				Vector3 Up = Vector3.Up;
				if (DEBUG_MoveModeFlag && jumpVector != Util.Zero)
				{
					Up = jumpVector;
				}

				Vector3 Horizontal = Vector3.Normalize(Vector3.Cross(CurrentPlayerCenter - refPos, Up));
				Vector3 Run = Vector3.Normalize(Vector3.Cross(Horizontal, Up));

				Vector3 n = Vector3.Normalize((Horizontal * moveInput.Y) + (Run * moveInput.X));
				Vector3 n2 = Vector3.Normalize(Vector3.Cross(n, Up));
				float m = MathHelper.Clamp((float)Math.Sqrt((moveInput.X * moveInput.X) + (moveInput.Y * moveInput.Y)), 0, 1);

				float ClingEffect = MathHelper.Clamp(Cling.value * 0.5f, 2, 4);

				float force = m / time;

				if (touching)
				{
					Vector3 currentDrift = n2 * (force * drift);
					float twistMult = (force * twist * ClingEffect);
					foreach (PhysicsPoint p in playerBody.points)
					{
						p.ForceThisFrame += currentDrift + Vector3.Normalize(Vector3.Cross(p.CurrentPosition - CurrentPlayerCenter, n)) * twistMult;
					}
				}
				else
				{
					Vector3 currentDrift = n2 * (force * airDrift);
					float twistMult = (force * airTwist * ClingEffect);
					foreach (PhysicsPoint p in playerBody.points)
					{
						p.ForceThisFrame += currentDrift + Vector3.Normalize(Vector3.Cross(p.CurrentPosition - CurrentPlayerCenter, n)) * twistMult;
					}

				}
			}
			#endregion

			if (cling.value > 0)
			{
				foreach (PhysicsPoint p in playerBody.points)
				{
					if (p.LastCollision != null)
					{
						p.ForceThisFrame -= p.LastCollision.Normal * (cling.value * p.LastCollision.getMaterial().Cling);
						p.LastCollision = null;
					}
				}
			}

			if (resilience.changed)
			{
				foreach (Spring s in playerBody.springs)
				{
					s.Force = resilience.value;
				}
			}

			if (volume.changed && playerBody is BodyPressure)
			{
				((BodyPressure)playerBody).IdealVolume = volume.value;
			}
		}

		private void update(Property p, float time)
		{
			if (p.target != p.current)
			{
				float diff = p.current - p.target;
				float delta = p.delta * time;
				if (Math.Abs(diff) < Math.Abs(delta))
				{
					p.current = p.target;
				}
				else
				{
					if (diff < 0f)
					{
						p.current += delta;
					}
					else
					{
						p.current -= delta;
					}
				}
				p.changed = true;

				if (p.current > 0.5f)
				{
					p.value = p.origin + (((p.current - 0.5f) * 2) * (p.maximum - p.origin));
				}
				else
				{
					p.value = p.minimum + ((p.current * 2) * (p.origin - p.minimum));
				}
			}
			else
			{
				p.changed = false;
			}
		}
	}
}
