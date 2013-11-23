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

    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]
        Structure defining the properties for a reverb source, related to a FMOD channel.

        For more indepth descriptions of the reverb properties under win32, please see the EAX3
        documentation at http://developer.creative.com/ under the 'downloads' section.
        If they do not have the EAX3 documentation, then most information can be attained from
        the EAX2 documentation, as EAX3 only adds some more parameters and functionality on top of 
        EAX2.

        Note the default reverb properties are the same as the PRESET_GENERIC preset.
        Note that integer values that typically range from -10,000 to 1000 are represented in 
        decibels, and are of a logarithmic scale, not linear, wheras FLOAT values are typically linear.
        PORTABILITY: Each member has the platform it supports in braces ie (win32/xbox).  
        Some reverb parameters are only supported in win32 and some only on xbox. If all parameters are set then
        the reverb should product a similar effect on either platform.
        Linux and FMODCE do not support the reverb api.

        The numerical values listed below are the maximum, minimum and default values for each variable respectively.

        [REMARKS]
        For EAX4 support with multiple reverb environments, set FMOD_REVERB_CHANNELFLAGS_ENVIRONMENT0,
        FMOD_REVERB_CHANNELFLAGS_ENVIRONMENT1 or/and FMOD_REVERB_CHANNELFLAGS_ENVIRONMENT2 in the flags member 
        of FMOD_REVERB_CHANNELPROPERTIES to specify which environment instance(s) to target. 
        Only up to 2 environments to target can be specified at once. Specifying three will result in an error.
        If the sound card does not support EAX4, the environment flag is ignored.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        Channel::setReverbProperties
        Channel::getReverbProperties
        REVERB_CHANNELFLAGS
    ]
    */
	
	
    [StructLayout(LayoutKind.Sequential)]
    public struct ChannelProperties  
	{
		/*          MIN     MAX    DEFAULT  DESCRIPTION */
		
		public int       Direct;
		/* [in/out] -10000, 1000,  0,       direct path level (at low and mid frequencies) (win32/xbox) */
		
		public int       DirectHF;
		/* [in/out] -10000, 0,     0,       relative direct path level at high frequencies (win32/xbox) */
		
		public int       Room;
		/* [in/out] -10000, 1000,  0,       room effect level (at low and mid frequencies) (win32/xbox) */
		
		public int       RoomHF;
		/* [in/out] -10000, 0,     0,       relative room effect level at high frequencies (win32/xbox) */
		
		public int       Obstruction;
		/* [in/out] -10000, 0,     0,       main obstruction control (attenuation at high frequencies)  (win32/xbox) */
		
		public float     ObstructionLFRatio;
		/* [in/out] 0.0,    1.0,   0.0,     obstruction low-frequency level re. main control (win32/xbox) */
		
		public int       Occlusion;
		/* [in/out] -10000, 0,     0,       main occlusion control (attenuation at high frequencies) (win32/xbox) */
		
		public float     OcclusionLFRatio;
		/* [in/out] 0.0,    1.0,   0.25,    occlusion low-frequency level re. main control (win32/xbox) */
		
		public float     OcclusionRoomRatio;
		/* [in/out] 0.0,    10.0,  1.5,     relative occlusion control for room effect (win32) */
		
		public float     OcclusionDirectRatio;
		/* [in/out] 0.0,    10.0,  1.0,     relative occlusion control for direct path (win32) */
		
		public int       Exclusion;
		/* [in/out] -10000, 0,     0,       main exlusion control (attenuation at high frequencies) (win32) */
		
		public float     ExclusionLFRatio;
		/* [in/out] 0.0,    1.0,   1.0,     exclusion low-frequency level re. main control (win32) */
		
		public int       OutsideVolumeHF;
		/* [in/out] -10000, 0,     0,       outside sound cone level at high frequencies (win32) */
		
		public float     DopplerFactor;
		/* [in/out] 0.0,    10.0,  0.0,     like DS3D flDopplerFactor but per source (win32) */
		
		public float     RolloffFactor;
		/* [in/out] 0.0,    10.0,  0.0,     like DS3D flRolloffFactor but per source (win32) */
		
		public float     RoomRolloffFactor;
		/* [in/out] 0.0,    10.0,  0.0,     like DS3D flRolloffFactor but for room effect (win32/xbox) */
		
		public float     AirAbsorptionFactor;
		/* [in/out] 0.0,    10.0,  1.0,     multiplies AirAbsorptionHF member of REVERB_PROPERTIES (win32) */
		
		public ChannelFlags      Flags;
		/* [in/out] REVERB_CHANNELFLAGS - modifies the behavior of properties (win32) */
		
		public IntPtr    ConnectionPoint;
		/* [in/out] See remarks.            DSP network location to connect reverb for this channel.    (SUPPORTED:SFX only).*/
		
	}
}

