namespace RTS.Core
{
    public interface ITickReceiver
    {
        public void SetTickSpeed(float oldSpeed, float newSpeed);
    }
}
