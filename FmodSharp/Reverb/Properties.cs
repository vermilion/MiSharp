//Author:
//      Marc-Andre Ferland <madrang@gmail.com>
//
//Copyright (c) 2011 TheWarrentTeam
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;
using System.Runtime.InteropServices;

namespace Linsft.FmodSharp.Reverb
{
    //TODO end subbmary

    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]
        Structure defining a reverb environment.<br>
        <br>
        For more indepth descriptions of the reverb properties under win32, please see the EAX2 and EAX3
        documentation at http://developer.creative.com/ under the 'downloads' section.<br>
        If they do not have the EAX3 documentation, then most information can be attained from
        the EAX2 documentation, as EAX3 only adds some more parameters and functionality on top of 
        EAX2.

        [REMARKS]
        Note the default reverb properties are the same as the FMOD_PRESET_GENERIC preset.<br>
        Note that integer values that typically range from -10,000 to 1000 are represented in 
        decibels, and are of a logarithmic scale, not linear, wheras float values are always linear.<br>
        <br>
        The numerical values listed below are the maximum, minimum and default values for each variable respectively.<br>
        <br>
        <b>SUPPORTED</b> next to each parameter means the platform the parameter can be set on.  Some platforms support all parameters and some don't.<br>
        EAX   means hardware reverb on FMOD_OUTPUTTYPE_DSOUND on windows only (must use FMOD_HARDWARE), on soundcards that support EAX 1 to 4.<br>
        EAX4  means hardware reverb on FMOD_OUTPUTTYPE_DSOUND on windows only (must use FMOD_HARDWARE), on soundcards that support EAX 4.<br>
        I3DL2 means hardware reverb on FMOD_OUTPUTTYPE_DSOUND on windows only (must use FMOD_HARDWARE), on soundcards that support I3DL2 non EAX native reverb.<br>
        GC    means Nintendo Gamecube hardware reverb (must use FMOD_HARDWARE).<br>
        WII   means Nintendo Wii hardware reverb (must use FMOD_HARDWARE).<br>
        PS2   means Playstation 2 hardware reverb (must use FMOD_HARDWARE).<br>
        SFX   means FMOD SFX software reverb.  This works on any platform that uses FMOD_SOFTWARE for loading sounds.<br>
        <br>
        Members marked with [in] mean the user sets the value before passing it to the function.<br>
        Members marked with [out] mean FMOD sets the value to be used after the function exits.<br>

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        System::setReverbProperties
        System::getReverbProperties
        FMOD_REVERB_PRESETS
        FMOD_REVERB_FLAGS
    ]
    */

    [StructLayout(LayoutKind.Sequential)]
    public struct Properties
    {
        private int instance;
        private int environment;
        private float envSize;
        private float envDiffusion;
        private int room;
        private int roomHF;
        private int roomLF;
        private float decayTime;
        private float decayHFRatio;
        private float decayLFRatio;
        private int reflections;
        private float reflectionsDelay;

        //TODO replace by Vector3
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        private float[] reflectionsPan;

        private int reverb;
        private float reverbDelay;

        //TODO replace by Vector3
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        private float[] reverbPan;

        private float echoTime;
        private float echoDepth;
        private float modulationTime;
        private float modulationDepth;
        private float airAbsorptionHF;
        private float hFReference;
        private float lFReference;
        private float roomRolloffFactor;
        private float diffusion;
        private float density;

        /// <summary>
        ///     EAX4 only. Environment Instance.
        ///     3 seperate reverbs simultaneously are possible.
        ///     This specifies which one to set. (win32 only)
        /// </summary>
        public int Instance
        {
            get { return instance; }
            set
            {
                CheckRange(value, 0, 3, "Instance");
                instance = value;
            }
        }

        /// <summary>
        ///     sets all listener properties (win32/ps2)
        /// </summary>
        public int Environment
        {
            get { return environment; }
            set
            {
                CheckRange(value, -1, 25, "Environment");
                environment = value;
            }
        }

        /// <summary>
        ///     environment size in meters (win32 only)
        /// </summary>
        public float EnvironmentSize
        {
            get { return envSize; }
            set
            {
                CheckRange(value, 1.0, 100.0, "EnvironmentSize");
                envSize = value;
            }
        }

        /// <summary>
        ///     environment diffusion (win32/xbox)
        /// </summary>
        public float EnvironmentDiffusion
        {
            get { return envDiffusion; }
            set
            {
                CheckRange(value, 0.0, 1.0, "EnvironmentDiffusion");
                envDiffusion = value;
            }
        }

        /// <summary>
        ///     room effect level (at mid frequencies) (win32/xbox)
        /// </summary>
        public int Room
        {
            get { return room; }
            set
            {
                CheckRange(value, -10000, 0, "Room");
                room = value;
            }
        }

        /// <summary>
        ///     relative room effect level at high frequencies (win32/xbox)
        /// </summary>
        public int RoomHighFrequencies
        {
            get { return roomHF; }
            set
            {
                CheckRange(value, -10000, 0, "RoomHighFrequencies");
                roomHF = value;
            }
        }

        /// <summary>
        ///     relative room effect level at low frequencies (win32 only)
        /// </summary>
        public int RoomLowFrequencies
        {
            get { return roomLF; }
            set
            {
                CheckRange(value, -10000, 0, "RoomLowFrequencies");
                roomLF = value;
            }
        }

        /// <summary>
        ///     reverberation decay time at mid frequencies (win32/xbox)
        /// </summary>
        public float DecayTime
        {
            get { return decayTime; }
            set
            {
                CheckRange(value, 0.1, 20.0, "DecayTime");
                decayTime = value;
            }
        }

        /// <summary>
        ///     high-frequency to mid-frequency decay time ratio (win32/xbox)
        /// </summary>
        public float DecayHighFrequencyRatio
        {
            get { return decayHFRatio; }
            set
            {
                CheckRange(value, 0.1, 2.0, "DecayHighFrequencyRatio");
                decayHFRatio = value;
            }
        }

        /// <summary>
        ///     low-frequency to mid-frequency decay time ratio (win32 only)
        /// </summary>
        public float DecayLowFrequencyRatio
        {
            get { return decayLFRatio; }
            set
            {
                CheckRange(value, 0.1, 2.0, "DecayLowFrequencyRatio");
                decayLFRatio = value;
            }
        }

        /// <summary>
        ///     early reflections level relative to room effect (win32/xbox)
        /// </summary>
        public int Reflections
        {
            get { return reflections; }
            set
            {
                CheckRange(value, -10000, 1000, "Reflections");
                reflections = value;
            }
        }

        /// <summary>
        ///     initial reflection delay time (win32/xbox)
        /// </summary>
        public float ReflectionsDelay
        {
            get { return reflectionsDelay; }
            set
            {
                CheckRange(value, 0.0, 0.3, "ReflectionsDelay");
                reflectionsDelay = value;
            }
        }

        //TODO replace by Vector3

        /// <summary>
        ///     early reflections panning vector (win32 only)
        /// </summary>
        public float[] ReflectionsPan
        {
            get { return reflectionsPan; }
            set { reflectionsPan = value; }
        }

        /// <summary>
        ///     late reverberation level relative to room effect (win32/xbox)
        /// </summary>
        public int Reverb
        {
            get { return reverb; }
            set
            {
                CheckRange(value, -10000, 2000, "Reverb");
                reverb = value;
            }
        }

        /// <summary>
        ///     late reverberation delay time relative to initial reflection (win32/xbox)
        /// </summary>
        public float ReverbDelay
        {
            get { return reverbDelay; }
            set
            {
                CheckRange(value, 0.0, 0.1, "ReverbDelay");
                reverbDelay = value;
            }
        }

        //TODO replace by Vector3

        /// <summary>
        ///     late reverberation panning vector (win32 only)
        /// </summary>
        public float[] ReverbPan
        {
            get { return reverbPan; }
            set { reverbPan = value; }
        }

        /// <summary>
        ///     echo time (win32 only)
        /// </summary>
        public float EchoTime
        {
            get { return echoTime; }
            set
            {
                CheckRange(value, 0.075, 0.25, "EchoTime");
                echoTime = value;
            }
        }

        /// <summary>
        ///     echo depth (win32 only)
        /// </summary>
        public float EchoDepth
        {
            get { return echoDepth; }
            set
            {
                CheckRange(value, 0.0, 1.0, "EchoDepth");
                echoDepth = value;
            }
        }

        /// <summary>
        ///     modulation time (win32 only)
        /// </summary>
        public float ModulationTime
        {
            get { return modulationTime; }
            set
            {
                CheckRange(value, 0.04, 4.0, "ModulationTime");
                modulationTime = value;
            }
        }

        /// <summary>
        ///     modulation depth (win32 only)
        /// </summary>
        public float ModulationDepth
        {
            get { return modulationDepth; }
            set
            {
                CheckRange(value, 0.0, 1.0, "ModulationDepth");
                modulationDepth = value;
            }
        }

        /// <summary>
        ///     change in level per meter at high frequencies (win32 only)
        /// </summary>
        public float AirAbsorptionHighFrequencies
        {
            get { return airAbsorptionHF; }
            set
            {
                CheckRange(value, -100.0, 0.0, "AirAbsorptionHighFrequencies");
                airAbsorptionHF = value;
            }
        }

        /// <summary>
        ///     reference high frequency (hz) (win32/xbox)
        /// </summary>
        public float HighFrequencyReference
        {
            get { return hFReference; }
            set
            {
                CheckRange(value, 1000.0, 20000.0, "HighFrequencyReference");
                hFReference = value;
            }
        }

        /// <summary>
        ///     reference low frequency (hz) (win32 only)
        /// </summary>
        public float LowFrequencyReference
        {
            get { return lFReference; }
            set
            {
                CheckRange(value, 20.0, 1000.0, "LowFrequencyReference");
                lFReference = value;
            }
        }

        /// <summary>
        ///     like rolloffscale in System::set3DSettings but for reverb room size effect (win32)
        /// </summary>
        public float RoomRolloffFactor
        {
            get { return roomRolloffFactor; }
            set
            {
                CheckRange(value, 0.0, 10.0, "RoomRolloffFactor");
                roomRolloffFactor = value;
            }
        }

        /// <summary>
        ///     Value that controls the echo density in the late reverberation decay. (xbox only)
        /// </summary>
        public float Diffusion
        {
            get { return diffusion; }
            set
            {
                CheckRange(value, 0.0, 100.0, "Diffusion");
                diffusion = value;
            }
        }

        /// <summary>
        ///     Value that controls the modal density in the late reverberation decay (xbox only)
        /// </summary>
        public float Density
        {
            get { return density; }
            set
            {
                CheckRange(value, 0.0, 100.0, "Environment");
                density = value;
            }
        }

        /// <summary>
        ///     Modifies the behavior of above properties.
        ///     (win32/ps2)
        /// </summary>
        public Flags Flags { get; set; }

        private void CheckRange(double Value, double Min, double Max, string Param)
        {
            if (Value < Min || Value > Max)
                throw new ArgumentOutOfRangeException(Param, Value, string.Format("{0}: Tried to set the value [{1}], but only accept [{2} to {3}].", Param, Value, Min, Max));
        }

        #region Default Preset

        public static readonly Properties Generic = new Properties
            {
                Instance = 0,
                Environment = -1,
                EnvironmentSize = 7.5f,
                EnvironmentDiffusion = 1.0f,
                Room = -1000,
                RoomHighFrequencies = -100,
                RoomLowFrequencies = 0,
                DecayTime = 1.49f,
                DecayHighFrequencyRatio = 0.83f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -2602,
                ReflectionsDelay = 0.007f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 200,
                ReverbDelay = 0.011f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.25f,
                EchoDepth = 0.0f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.0f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        #endregion
    }
}