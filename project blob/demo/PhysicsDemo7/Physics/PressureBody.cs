namespace Physics
{
    public interface PressureBody : Body
    {

        float getVolume();

        float getIdealVolume();

        void setIdealVolume( float volume );

    }
}
