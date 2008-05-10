using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public abstract class BodyPressure : Body
	{

		public BodyPressure() { }

		public BodyPressure(Body ParentBody, IList<PhysicsPoint> p_points, IList<Collidable> p_collidables, IList<Spring> p_springs, IList<Task> p_tasks, string p_collisionSound)
			:base(ParentBody, p_points, p_collidables,p_springs, p_tasks, p_collisionSound)
		{ }

		public abstract float getVolume();

		public abstract float getIdealVolume();

		public abstract float getPotentialVolume();

		public abstract void setIdealVolume(float volume);

		public override void update(float TotalElapsedSeconds)
		{

			base.update(TotalElapsedSeconds);

			// Volumes
			Vector3 CurrentCenter = getCenter();
			Vector3 NextCenter = getPotentialCenter();
			float CurrentVolume = getVolume();
			float NextVolume = getPotentialVolume();
			float IdealVolume = getIdealVolume();
			foreach (PhysicsPoint p in getPoints())
			{
				p.ForceThisFrame += ((Vector3.Normalize(CurrentCenter - p.CurrentPosition) * (CurrentVolume - IdealVolume)) + (Vector3.Normalize(CurrentCenter - p.PotentialPosition) * (NextVolume - IdealVolume)) / 2f);
			}


		}

	}
}
