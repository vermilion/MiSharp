namespace Linsft.FmodSharp.Dsp
{
    /*
    [ENUM]
    [  
        [DESCRIPTION]   
        Parameter types for the FMOD_DSP_TYPE_PARAMEQ filter.

        [REMARKS]
        Parametric EQ is a bandpass filter that attenuates or amplifies a selected frequency and its neighbouring frequencies.<br>
        <br>
        To create a multi-band EQ create multiple FMOD_DSP_TYPE_PARAMEQ units and set each unit to different frequencies, for example 1000hz, 2000hz, 4000hz, 8000hz, 16000hz with a range of 1 octave each.<br>
        <br>
        When a frequency has its gain set to 1.0, the sound will be unaffected and represents the original signal exactly.<br>

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        DSP::setParameter
        DSP::getParameter
        FMOD_DSP_TYPE
    ]
    */
    public enum DSPParameq
    {
        Center,     /* Frequency center.  20.0 to 22000.0.  Default = 8000.0. */
        Bandwidth,  /* Octave range around the center frequency to filter.  0.2 to 5.0.  Default = 1.0. */
        Gain        /* Frequency Gain.  0.05 to 3.0.  Default = 1.0.  */
    }
}