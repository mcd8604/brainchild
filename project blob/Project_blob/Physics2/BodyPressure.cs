using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public abstract class BodyPressure : Body
	{

		public BodyPressure() : base() { }

		public BodyPressure(Body ParentBody, IList<PhysicsPoint> p_points, IList<Collidable> p_collidables, IList<Spring> p_springs, IList<Task> p_tasks)
			: base(ParentBody, p_points, p_collidables, p_springs, p_tasks)
		{ }

		public abstract float Volume
		{
			get;
		}

		public abstract float IdealVolume
		{
			get;
			set;
		}

		public abstract float PotentialVolume
		{
			get;
		}

		public override void update(float TotalElapsedSeconds)
		{
			base.update(TotalElapsedSeconds);

			// Volumes
			Vector3 currentCenter = getCenter();
			Vector3 nextCenter = getPotentialCenter();
			float currentVolume = Volume;
			float nextVolume = PotentialVolume;
			float idealVolume = IdealVolume;
			foreach (PhysicsPoint p in getPoints())
			{
				p.ForceThisFrame += ((Vector3.Normalize(currentCenter - p.CurrentPosition) * (currentVolume - idealVolume)) + (Vector3.Normalize(currentCenter - p.PotentialPosition) * (nextVolume - idealVolume)) * 0.5f);
			}
		}
	}
}
