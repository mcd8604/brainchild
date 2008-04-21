using Microsoft.Xna.Framework;

namespace Physics2
{
	public abstract class BodyPressure : Body
	{

		public abstract float getVolume();

		public abstract float getIdealVolume();

		public abstract float getpotentialVolume();

		public abstract void setIdealVolume(float volume);

		public override void update(float TotalElapsedSeconds)
		{

			base.update(TotalElapsedSeconds);

			// Volumes
			Vector3 CurrentCenter = getCenter();
			Vector3 NextCenter = getpotentialCenter();
			float CurrentVolume = getVolume();
			float NextVolume = getpotentialVolume();
			float IdealVolume = getIdealVolume();
			foreach (Point p in getPoints())
			{
				p.ForceThisFrame += ((Vector3.Normalize(CurrentCenter - p.CurrentPosition) * (CurrentVolume - IdealVolume)) + (Vector3.Normalize(CurrentCenter - p.potentialPosition) * (NextVolume - IdealVolume)) / 2f);
			}


		}

	}
}
