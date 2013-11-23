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

namespace Linsft.FmodSharp.Dsp
{
	//TODO end subbmary
	
	/*
    [ENUM]
    [
        [REMARKS]
        The default resampler type is FMOD_DSP_RESAMPLER_LINEAR.<br>
        Use System::setSoftwareFormat to tell FMOD the resampling quality you require for FMOD_SOFTWARE based sounds.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        System::setSoftwareFormat
        System::getSoftwareFormat
    ]
    */
	
	/// <summary>
	/// List of interpolation types that the FMOD Ex software mixer supports.
	/// </summary>
    public enum Resampler : int
    {
		/// <summary>
		/// High frequency aliasing hiss will be audible depending on the sample rate of the sound.
		/// </summary>
		NoInterpolation,
        
		/// <summary>
		/// Fast and good quality, causes very slight lowpass effect on low frequency sounds.
		/// (default method)
		/// </summary>
		Linear,
		
		/// <summary>
		/// Slower than linear interpolation but better quality.
		/// </summary>
		Cubic,
		
		/// <summary>
		/// 5 point spline interpolation.
		/// Slowest resampling method but best quality.
		/// </summary>
		Spline,
		
		/// <summary>
		/// Maximum number of resample methods supported.
		/// </summary>
		Max,
	}
}
