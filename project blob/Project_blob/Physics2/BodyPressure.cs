using Microsoft.Xna.Framework;

namespace Physics2
{
	public abstract class BodyPressure : Body
	{

        public abstract float getVolume();

        public abstract float getIdealVolume();

        public abstract float getPotientialVolume();

        public abstract void setIdealVolume(float volume);

        public override void update(float TotalElapsedSeconds)
        {

            // Volumes
                    Vector3 CurrentCenter = getCenter();
                    Vector3 NextCenter = getPotientialCenter();
                    float CurrentVolume = getVolume();
                    float NextVolume = getPotientialVolume();
                    float IdealVolume = getIdealVolume();
                    foreach (Point p in getPoints())
                    {
                        p.ForceThisFrame += ((Vector3.Normalize(CurrentCenter - p.CurrentPosition) * (CurrentVolume - IdealVolume)) + (Vector3.Normalize(CurrentCenter - p.PotientialPosition) * (NextVolume - IdealVolume)) / 2f);
                    }
            

            base.update(TotalElapsedSeconds);
            
        }

	}
}
