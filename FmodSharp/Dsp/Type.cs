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
	/// <summary>
	/// These definitions can be used for creating FMOD defined special effects or DSP units.
	/// </summary>
	/// <remarks>
	/// To get them to be active, first create the unit,
	/// then add it somewhere into the DSP network,
	/// either at the front of the network near the soundcard unit to affect
	/// the global output (by using System::getDSPHead),
	/// or on a single channel (using Channel::getDSPHead).
	/// </remarks>
	/// <platforms>
	/// Win32, Win64, Linux, Linux64, Macintosh,
	/// Xbox, Xbox360, PlayStation 2, GameCube,
	/// PlayStation Portable, PlayStation 3, Wii
	/// </platforms>
	/// <seealso cref="FmodSharp.System.System.CreateDspByType"/>
	public enum Type : int
	{
		/// <summary>
		/// This unit was created via a non FMOD plugin so has an unknown purpose.
		/// </summary>
		Unknown,

		/// <summary>
		/// This unit does nothing but take inputs and mix them together
		/// then feed the result to the soundcard unit.
		/// </summary>
		Mixer,

		/// <summary>
		/// This unit generates sine/square/saw/triangle or noise tones.
		/// </summary>
		Oscillator,

		/// <summary>
		/// This unit filters sound using a high quality,
		/// resonant lowpass filter algorithm but consumes more CPU time.
		/// </summary>
		LowPass,

		/// <summary>
		/// This unit filters sound using a resonant lowpass filter
		/// algorithm that is used in Impulse Tracker,
		/// but with limited cutoff range (0 to 8060hz).
		/// </summary>
		ImpulseTrackerLowPass,

		/// <summary>
		/// This unit filters sound using a resonant highpass filter algorithm.
		/// </summary>
		HighPass,

		/// <summary>
		/// This unit produces an echo on the sound and fades out at the desired rate.
		/// </summary>
		Echo,

		/// <summary>
		/// This unit produces a flange effect on the sound.
		/// </summary>
		Flange,

		/// <summary>
		/// This unit distorts the sound.
		/// </summary>
		Distortion,

		/// <summary>
		/// This unit normalizes or amplifies the sound to a certain level.
		/// </summary>
		Normalize,

		/// <summary>
		/// This unit attenuates or amplifies a selected frequency range.
		/// </summary>
		ParameQ,

		/// <summary>
		/// This unit bends the pitch of a sound without changing the speed of playback.
		/// </summary>
		PitchShift,

		/// <summary>
		/// This unit produces a chorus effect on the sound.
		/// </summary>
		Chorus,

		/// <summary>
		/// This unit produces a reverb effect on the sound.
		/// </summary>
		Reverb,

		/// <summary>
		/// This unit allows the use of Steinberg VST plugins
		/// </summary>
		VSTPlugin,

		/// <summary>
		/// This unit allows the use of Nullsoft Winamp plugins.
		/// </summary>
		WinampPlugin,

		/// <summary>
		/// This unit produces an echo on the sound and fades out
		/// at the desired rate as is used in Impulse Tracker.
		/// </summary>
		ImpulseTrackerEcho,

		/// <summary>
		/// This unit implements dynamic compression.
		/// (linked multichannel, wideband)
		/// </summary>
		Compressor,

		/// <summary>
		/// This unit implements SFX reverb.
		/// </summary>
		SFXReverb,

		/// <summary>
		/// This unit filters sound using a simple lowpass with no resonance, but has flexible cutoff and is fast.
		/// </summary>
		LowPassSimple,

		/// <summary>
		/// This unit produces different delays on individual channels of the sound.
		/// </summary>
		Delay,

		/// <summary>
		/// This unit produces a tremolo / chopper effect on the sound.
		/// </summary>
		Tremolo,

		/// <summary>
		/// This unit allows the use of LADSPA standard plugins.
		/// </summary>
		LADSPAPlugin
	}
	
}
