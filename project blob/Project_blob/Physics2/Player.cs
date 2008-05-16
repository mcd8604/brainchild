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

		// Cling
		Property cling = new Property();
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

		// Traction
		Property traction = new Property();
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

		// Resilience
		Property resilience = new Property();
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

		// Volume
		Property volume = new Property();
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

		public bool DEBUG_MoveModeFlag = false;

		public void move(Vector2 input, Vector3 reference)
		{
			moveInput = input;
			refPos = reference;
			moveflag = true;
		}

		private bool moveflag = false;
		private Vector2 moveInput;
		private Vector3 refPos;

		public float Twist = 0.2f;
		public float AirTwist = 0.05f;
		public float Drift = -0.01f;
		public float AirDrift = -0.1f;

		/// <summary>
		/// fake jump trigger
		/// </summary>
		public void jump() { jumpflag = true; }

		private bool jumpflag = false;

		public float JumpWork = 15;
		public float AirJumpWork = 0;

		Vector3 jumpVector;

		internal void update(float time)
		{
			update(cling, time);
			update(traction, time);
			update(resilience, time);
			update(volume, time);

			#region Calculate Jump Vector
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
				jumpVector = Vector3.Normalize(jumpVector);
			}
			#endregion

			#region Jump
			if (jumpflag)
			{
				jumpflag = false;
				// Fake Fake Jump:
				foreach (PhysicsPoint p in playerBody.getPoints())
				{
					p.ForceThisFrame += Vector3.Up * (AirJumpWork / time);
					p.ForceThisFrame += jumpVector * (JumpWork / time);
				}
			}
			#endregion

			#region Movement
			if (moveflag)
			{
				moveflag = false;
				Vector3 CurrentPlayerCenter = playerBody.getCenter();

				Vector3 Up = Vector3.Up;
				if (DEBUG_MoveModeFlag)
				{
					foreach (PhysicsPoint p in playerBody.points)
					{
						if (p.LastCollision != null)
						{
							Up = p.LastCollision.Normal;
							break;
						}
					}
				}

				//Vector3 Horizontal = Vector3.Normalize(Vector3.Cross(theBlob.getCenter() - cameraPosition, Up));
				Vector3 Horizontal = Vector3.Normalize(Vector3.Cross(CurrentPlayerCenter - refPos, Up));
				Vector3 Run = Vector3.Normalize(Vector3.Cross(Horizontal, Up));

				Vector3 n = Vector3.Normalize((Horizontal * moveInput.Y) + (Run * moveInput.X));
				Vector3 n2 = Vector3.Normalize(Vector3.Cross(n, Up));
				float m = MathHelper.Clamp((float)Math.Sqrt((moveInput.X * moveInput.X) + (moveInput.Y * moveInput.Y)), 0, 1);

				float ClingEffect = MathHelper.Clamp(Cling.value * 0.5f, 2, 4);

				float force = m / time;

				if (jumpVector != Util.Zero) // touching something
				{
					foreach (PhysicsPoint p in playerBody.getPoints())
					{
						p.ForceThisFrame += Vector3.Normalize(Vector3.Cross(p.CurrentPosition - CurrentPlayerCenter, n)) * (force * Twist * ClingEffect);
						p.ForceThisFrame += n2 * (force * Drift);
					}
				}
				else
				{
					foreach (PhysicsPoint p in playerBody.getPoints())
					{
						p.ForceThisFrame += Vector3.Normalize(Vector3.Cross(p.CurrentPosition - CurrentPlayerCenter, n)) * (force * AirTwist * ClingEffect);
						p.ForceThisFrame += n2 * (force * AirDrift);
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
						p.ForceThisFrame -= p.LastCollision.Normal * (cling.value * p.LastCollision.getMaterial().getCling());
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
