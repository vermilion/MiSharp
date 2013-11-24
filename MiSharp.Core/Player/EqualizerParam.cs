namespace MiSharp.Core.Player
{
    public class EqualizerParam
    {
        public float Center;
        public float Bandwidth;
        public float Gain;

        public EqualizerParam() {}

        public EqualizerParam(float center, float bandwidth, float gain)
        {
            Center = center;
            Bandwidth = bandwidth;
            Gain = gain;
        }
    }
}