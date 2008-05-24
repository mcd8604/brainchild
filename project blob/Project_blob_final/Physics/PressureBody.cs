namespace Physics
{
	public abstract class PressureBody : Body
    {

		public abstract float getVolume();

		public abstract float getNextVolume();

		public abstract float getIdealVolume();

		public abstract void setIdealVolume(float volume);

    }
}
