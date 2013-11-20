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

namespace Linsft.FmodSharp.Reverb
{
    //TODO end submmary

    /*
    [PLATFORMS]
    Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii

    [SEE_ALSO]
    System::setReverbProperties
    ]
	 */


    /// <summary>
    ///     A set of predefined environment PARAMETERS, created by Creative Labs
    ///     These are used to initialize an FMOD_REVERB_PROPERTIES structure statically.
    ///     ie
    ///     FMOD_REVERB_PROPERTIES prop = FMOD_PRESET_GENERIC;
    /// </summary>
    public static class Presets
    {
        public static readonly Properties Off = new Properties
            {
                Instance = 0,
                Environment = -1,
                EnvironmentSize = 7.5f,
                EnvironmentDiffusion = 1.00f,
                Room = -10000,
                RoomHighFrequencies = -10000,
                RoomLowFrequencies = 0,
                DecayTime = 1.00f,
                DecayHighFrequencyRatio = 1.00f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -2602,
                ReflectionsDelay = 0.007f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 200,
                ReverbDelay = 0.011f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 0.0f,
                Density = 0.0f,
                //TODO Find documentation about the Flag 0x33F.
                Flags = (Flags) 0x33F
            };

        public static readonly Properties Generic = new Properties
            {
                Instance = 0,
                Environment = 0,
                EnvironmentSize = 7.5f,
                EnvironmentDiffusion = 1.00f,
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
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties PaddedCell = new Properties
            {
                Instance = 0,
                Environment = 1,
                EnvironmentSize = 1.4f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -6000,
                RoomLowFrequencies = 0,
                DecayTime = 0.17f,
                DecayHighFrequencyRatio = 0.10f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1204,
                ReflectionsDelay = 0.001f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 207,
                ReverbDelay = 0.002f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Room = new Properties
            {
                Instance = 0,
                Environment = 2,
                EnvironmentSize = 1.9f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -454,
                RoomLowFrequencies = 0,
                DecayTime = 0.40f,
                DecayHighFrequencyRatio = 0.83f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1646,
                ReflectionsDelay = 0.002f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 53,
                ReverbDelay = 0.003f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Bathroom = new Properties
            {
                Instance = 0,
                Environment = 3,
                EnvironmentSize = 1.4f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -1200,
                RoomLowFrequencies = 0,
                DecayTime = 1.49f,
                DecayHighFrequencyRatio = 0.54f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -370,
                ReflectionsDelay = 0.007f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 1030,
                ReverbDelay = 0.011f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 60.0f,
                Flags = Flags.Default
            };

        public static readonly Properties LivingRoom = new Properties
            {
                Instance = 0,
                Environment = 4,
                EnvironmentSize = 2.5f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -6000,
                RoomLowFrequencies = 0,
                DecayTime = 0.50f,
                DecayHighFrequencyRatio = 0.10f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1376,
                ReflectionsDelay = 0.003f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -1104,
                ReverbDelay = 0.004f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties StoneRoom = new Properties
            {
                Instance = 0,
                Environment = 5,
                EnvironmentSize = 11.6f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -300,
                RoomLowFrequencies = 0,
                DecayTime = 2.31f,
                DecayHighFrequencyRatio = 0.64f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -711,
                ReflectionsDelay = 0.012f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 83,
                ReverbDelay = 0.017f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Auditorium = new Properties
            {
                Instance = 0,
                Environment = 6,
                EnvironmentSize = 21.6f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -476,
                RoomLowFrequencies = 0,
                DecayTime = 4.32f,
                DecayHighFrequencyRatio = 0.59f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -789,
                ReflectionsDelay = 0.020f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -289,
                ReverbDelay = 0.030f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties ConcertHall = new Properties
            {
                Instance = 0,
                Environment = 7,
                EnvironmentSize = 19.6f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -500,
                RoomLowFrequencies = 0,
                DecayTime = 3.92f,
                DecayHighFrequencyRatio = 0.70f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1230,
                ReflectionsDelay = 0.020f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -2,
                ReverbDelay = 0.029f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Cave = new Properties
            {
                Instance = 0,
                Environment = 8,
                EnvironmentSize = 14.6f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = 0,
                RoomLowFrequencies = 0,
                DecayTime = 2.91f,
                DecayHighFrequencyRatio = 1.30f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -602,
                ReflectionsDelay = 0.015f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -302,
                ReverbDelay = 0.022f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.DecayTimeScale | Flags.ReflectionsScale |
                        Flags.ReflectionsDelayScale | Flags.ReverbScale | Flags.ReverbDelayScale
            };

        public static readonly Properties Arena = new Properties
            {
                Instance = 0,
                Environment = 9,
                EnvironmentSize = 36.2f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -698,
                RoomLowFrequencies = 0,
                DecayTime = 7.24f,
                DecayHighFrequencyRatio = 0.33f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1166,
                ReflectionsDelay = 0.020f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 16,
                ReverbDelay = 0.030f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Hangar = new Properties
            {
                Instance = 0,
                Environment = 10,
                EnvironmentSize = 50.3f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -1000,
                RoomLowFrequencies = 0,
                DecayTime = 10.05f,
                DecayHighFrequencyRatio = 0.23f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -602,
                ReflectionsDelay = 0.020f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 198,
                ReverbDelay = 0.030f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties CarpettedHallway = new Properties
            {
                Instance = 0,
                Environment = 11,
                EnvironmentSize = 1.9f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -4000,
                RoomLowFrequencies = 0,
                DecayTime = 0.30f,
                DecayHighFrequencyRatio = 0.10f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1831,
                ReflectionsDelay = 0.002f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -1630,
                ReverbDelay = 0.030f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Hallway = new Properties
            {
                Instance = 0,
                Environment = 12,
                EnvironmentSize = 1.8f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -300,
                RoomLowFrequencies = 0,
                DecayTime = 1.49f,
                DecayHighFrequencyRatio = 0.59f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1219,
                ReflectionsDelay = 0.007f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 441,
                ReverbDelay = 0.011f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties StoneCorridor = new Properties
            {
                Instance = 0,
                Environment = 13,
                EnvironmentSize = 13.5f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -237,
                RoomLowFrequencies = 0,
                DecayTime = 2.70f,
                DecayHighFrequencyRatio = 0.79f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1214,
                ReflectionsDelay = 0.013f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 395,
                ReverbDelay = 0.020f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Alley = new Properties
            {
                Instance = 0,
                Environment = 14,
                EnvironmentSize = 7.5f,
                EnvironmentDiffusion = 0.30f,
                Room = -1000,
                RoomHighFrequencies = -270,
                RoomLowFrequencies = 0,
                DecayTime = 1.49f,
                DecayHighFrequencyRatio = 0.86f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1204,
                ReflectionsDelay = 0.007f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -4,
                ReverbDelay = 0.011f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.125f,
                EchoDepth = 0.95f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Forest = new Properties
            {
                Instance = 0,
                Environment = 15,
                EnvironmentSize = 38.0f,
                EnvironmentDiffusion = 0.30f,
                Room = -1000,
                RoomHighFrequencies = -3300,
                RoomLowFrequencies = 0,
                DecayTime = 1.49f,
                DecayHighFrequencyRatio = 0.54f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -2560,
                ReflectionsDelay = 0.162f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -229,
                ReverbDelay = 0.088f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.125f,
                EchoDepth = 1.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 79.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties City = new Properties
            {
                Instance = 0,
                Environment = 16,
                EnvironmentSize = 7.5f,
                EnvironmentDiffusion = 0.50f,
                Room = -1000,
                RoomHighFrequencies = -800,
                RoomLowFrequencies = 0,
                DecayTime = 1.49f,
                DecayHighFrequencyRatio = 0.67f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -2273,
                ReflectionsDelay = 0.007f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -1691,
                ReverbDelay = 0.011f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 50.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Mountains = new Properties
            {
                Instance = 0,
                Environment = 17,
                EnvironmentSize = 100.0f,
                EnvironmentDiffusion = 0.27f,
                Room = -1000,
                RoomHighFrequencies = -2500,
                RoomLowFrequencies = 0,
                DecayTime = 1.49f,
                DecayHighFrequencyRatio = 0.21f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -2780,
                ReflectionsDelay = 0.299f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -1434,
                ReverbDelay = 0.0999f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 1.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 27.0f,
                Density = 100.0f,
                Flags = Flags.DecayTimeScale | Flags.ReflectionsScale |
                        Flags.ReflectionsDelayScale | Flags.ReverbScale | Flags.ReverbDelayScale
            };

        public static readonly Properties Quarry = new Properties
            {
                Instance = 0,
                Environment = 18,
                EnvironmentSize = 17.5f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -1000,
                RoomLowFrequencies = 0,
                DecayTime = 1.49f,
                DecayHighFrequencyRatio = 0.83f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -10000,
                ReflectionsDelay = 0.061f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 500,
                ReverbDelay = 0.025f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.125f,
                EchoDepth = 0.70f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Plain = new Properties
            {
                Instance = 0,
                Environment = 19,
                EnvironmentSize = 42.5f,
                EnvironmentDiffusion = 0.21f,
                Room = -1000,
                RoomHighFrequencies = -2000,
                RoomLowFrequencies = 0,
                DecayTime = 1.49f,
                DecayHighFrequencyRatio = 0.50f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -2466,
                ReflectionsDelay = 0.179f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -1926,
                ReverbDelay = 0.0999f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 1.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 21.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Parkinglot = new Properties
            {
                Instance = 0,
                Environment = 20,
                EnvironmentSize = 8.3f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = 0,
                RoomLowFrequencies = 0,
                DecayTime = 1.65f,
                DecayHighFrequencyRatio = 1.50f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1363,
                ReflectionsDelay = 0.008f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -1153,
                ReverbDelay = 0.012f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.DecayTimeScale | Flags.ReflectionsScale |
                        Flags.ReflectionsDelayScale | Flags.ReverbScale | Flags.ReverbDelayScale
            };

        public static readonly Properties SewerPipe = new Properties
            {
                Instance = 0,
                Environment = 21,
                EnvironmentSize = 1.7f,
                EnvironmentDiffusion = 0.80f,
                Room = -1000,
                RoomHighFrequencies = -1000,
                RoomLowFrequencies = 0,
                DecayTime = 2.81f,
                DecayHighFrequencyRatio = 0.14f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = 429,
                ReflectionsDelay = 0.014f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 1023,
                ReverbDelay = 0.021f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 0.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 80.0f,
                Density = 60.0f,
                Flags = Flags.Default
            };

        public static readonly Properties Underwater = new Properties
            {
                Instance = 0,
                Environment = 22,
                EnvironmentSize = 1.8f,
                EnvironmentDiffusion = 1.00f,
                Room = -1000,
                RoomHighFrequencies = -4000,
                RoomLowFrequencies = 0,
                DecayTime = 1.49f,
                DecayHighFrequencyRatio = 0.10f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -449,
                ReflectionsDelay = 0.007f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 1700,
                ReverbDelay = 0.011f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 1.18f,
                ModulationDepth = 0.348f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.Default
            };

        #region Non I3DL2 presets

        public static readonly Properties Drugged = new Properties
            {
                Instance = 0,
                Environment = 23,
                EnvironmentSize = 1.9f,
                EnvironmentDiffusion = 0.50f,
                Room = -1000,
                RoomHighFrequencies = 0,
                RoomLowFrequencies = 0,
                DecayTime = 8.39f,
                DecayHighFrequencyRatio = 1.39f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -115,
                ReflectionsDelay = 0.002f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 985,
                ReverbDelay = 0.030f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 0.25f,
                ModulationDepth = 1.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.DecayTimeScale | Flags.ReflectionsScale |
                        Flags.ReflectionsDelayScale | Flags.ReverbScale | Flags.ReverbDelayScale
            };

        public static readonly Properties Dizzy = new Properties
            {
                Instance = 0,
                Environment = 24,
                EnvironmentSize = 1.8f,
                EnvironmentDiffusion = 0.60f,
                Room = -1000,
                RoomHighFrequencies = -400,
                RoomLowFrequencies = 0,
                DecayTime = 17.23f,
                DecayHighFrequencyRatio = 0.56f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -1713,
                ReflectionsDelay = 0.020f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = -613,
                ReverbDelay = 0.030f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 1.00f,
                ModulationTime = 0.81f,
                ModulationDepth = 0.310f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.DecayTimeScale | Flags.ReflectionsScale |
                        Flags.ReflectionsDelayScale | Flags.ReverbScale | Flags.ReverbDelayScale
            };

        public static readonly Properties Psychotic = new Properties
            {
                Instance = 0,
                Environment = 25,
                EnvironmentSize = 1.0f,
                EnvironmentDiffusion = 0.50f,
                Room = -1000,
                RoomHighFrequencies = -151,
                RoomLowFrequencies = 0,
                DecayTime = 7.56f,
                DecayHighFrequencyRatio = 0.91f,
                DecayLowFrequencyRatio = 1.0f,
                Reflections = -626,
                ReflectionsDelay = 0.020f,
                ReflectionsPan = new[] {0.0f, 0.0f, 0.0f},
                Reverb = 774,
                ReverbDelay = 0.030f,
                ReverbPan = new[] {0.0f, 0.0f, 0.0f},
                EchoTime = 0.250f,
                EchoDepth = 0.00f,
                ModulationTime = 4.00f,
                ModulationDepth = 1.000f,
                AirAbsorptionHighFrequencies = -5.0f,
                HighFrequencyReference = 5000.0f,
                LowFrequencyReference = 250.0f,
                RoomRolloffFactor = 0.0f,
                Diffusion = 100.0f,
                Density = 100.0f,
                Flags = Flags.DecayTimeScale | Flags.ReflectionsScale |
                        Flags.ReflectionsDelayScale | Flags.ReverbScale | Flags.ReverbDelayScale
            };

        #endregion

        #region PlayStation 2 Only presets

        public static readonly Properties PS2_Room = new Properties
            {
                Environment = 1,
                Flags = (Flags) 0x31f
            };

        public static readonly Properties PS2_Studio_A = new Properties
            {
                Environment = 2,
                Flags = (Flags) 0x31f
            };

        public static readonly Properties PS2_Studio_B = new Properties
            {
                Environment = 3,
                Flags = (Flags) 0x31f
            };

        public static readonly Properties PS2_Studio_C = new Properties
            {
                Environment = 4,
                Flags = (Flags) 0x31f
            };

        public static readonly Properties PS2_Hall = new Properties
            {
                Environment = 5,
                Flags = (Flags) 0x31f
            };

        public static readonly Properties PS2_Space = new Properties
            {
                Environment = 6,
                Flags = (Flags) 0x31f
            };

        public static readonly Properties PS2_Echo = new Properties
            {
                Environment = 7,
                Flags = (Flags) 0x31f
            };

        public static readonly Properties PS2_Delay = new Properties
            {
                Environment = 8,
                Flags = (Flags) 0x31f
            };

        public static readonly Properties PS2_Pipe = new Properties
            {
                Environment = 9,
                Flags = (Flags) 0x31f
            };

        #endregion
    }
}