using System;
using Microsoft.Xna.Framework;
using Project_blob.GameState;

namespace Project_blob
{
	[Serializable]
	class DeltaEvent : EventTrigger
	{

		private Vector3 DeltaPosition;
		private Vector3 DeltaVelocity;
		private Vector3 DeltaForce;

		public DeltaEvent(Vector3 Force)
		{
			DeltaForce = Force;
			DeltaVelocity = Vector3.Zero;
			DeltaPosition = Vector3.Zero;
		}

		public DeltaEvent(Vector3 Force, Vector3 Velocity)
		{
			DeltaForce = Force;
			DeltaVelocity = Velocity;
			DeltaPosition = Vector3.Zero;
		}

		public DeltaEvent(Vector3 Force, Vector3 Velocity, Vector3 Position)
		{
			DeltaForce = Force;
			DeltaVelocity = Velocity;
			DeltaPosition = Position;
		}

		public void PerformEvent(GameplayScreen gameRef)
		{

			foreach (Physics.Point p in gameRef.Player.getPoints())
			{
				p.NextPosition += DeltaPosition;
				p.NextVelocity += DeltaVelocity;
				p.ForceNextFrame += DeltaForce;
			}

		}

	}
}
