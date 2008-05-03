using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public abstract class BodyPressure : Body
	{
		[Obsolete ("The fully qualified constructor is recommended.")]
		public BodyPressure() { }

		public BodyPressure(Body ParentBody, List<PhysicsPoint> p_points, List<Collidable> p_collidables, List<Spring> p_springs, List<Task> p_tasks)
			:base(ParentBody, p_points, p_collidables,p_springs, p_tasks)
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
