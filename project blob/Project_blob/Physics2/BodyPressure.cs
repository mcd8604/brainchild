namespace Physics2
{
	public abstract class BodyPressure : Body
	{

        public abstract float getVolume();

        public abstract float getIdealVolume();

        public abstract float getPotientialVolume();

        public abstract void setIdealVolume(float volume);

	}
}
