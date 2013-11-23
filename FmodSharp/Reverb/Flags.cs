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

namespace Linsft.FmodSharp.Reverb
{
	
	/*
    [DEFINE] 
    [
        [NAME] 
        REVERB_FLAGS

        [DESCRIPTION]
        

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        REVERB_PROPERTIES
    ]
    */
	
	/// <summary>
	/// Values for the Flags member of the REVERB_PROPERTIES structure.
	/// </summary>
    [Flags]
    public enum Flags : uint
	{
		/// <summary>
		/// 'EnvSize' affects reverberation decay time
		/// </summary>
		DecayTimeScale = 0x00000001,
        
		/// <summary>
		/// 'EnvSize' affects reflection level
		/// </summary>
		ReflectionsScale = 0x00000002,
        
		/// <summary>
		/// 'EnvSize' affects initial reflection delay time
		/// </summary>
		ReflectionsDelayScale = 0x00000004,
        
		/// <summary>
		/// 'EnvSize' affects reflections level
		/// </summary>
		ReverbScale = 0x00000008,
        
		/// <summary>
		/// 'EnvSize' affects late reverberation delay time
		/// </summary>
		ReverbDelayScale = 0x00000010,
        
		/// <summary>
		/// AirAbsorptionHF affects DecayHFRatio
		/// </summary>
		DecayHFLimit = 0x00000020,
        
		/// <summary>
		/// 'EnvSize' affects echo time
		/// </summary>
		EchoTimeScale = 0x00000040,
        
		/// <summary>
		/// 'EnvSize' affects modulation time
		/// </summary>
		ModulationTimeScale = 0x00000080,
        
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// Is equal to 0x3F
		/// </remarks>
		Default = DecayTimeScale | ReflectionsScale | 
		ReflectionsDelayScale | ReverbScale | 
		ReverbDelayScale | DecayHFLimit
	}
}

//TODO complete submmary.
