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

namespace Linsft.FmodSharp.SoundSystem
{
	/// <summary>
	/// //Initialization flags.
	/// Use them with <see cref="FmodSharp.System.System.Init"/> in the flags parameter to change various behaviour.
	/// </summary>
	/// <platforms>
	/// Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii
	/// </platforms>
	[Flags]
	public enum InitFlags : int
	{
		/// <summary>
		/// Initialize normally.
		/// </summary>
		Normal = 0x0,
		
		/// <summary>
		/// No stream thread is created internally.
		/// Mainly used with non-realtime outputs.
		/// </summary>
		StreamFromUpdate = 0x1,
		
		/// <summary>
		/// FMOD will treat +X as left, +Y as up and +Z as forwards.
		/// </summary>
		RightHanded3D = 0x2,
		
		//TODO verify if it fails with or without SoftwareDisable.
		
		/// <summary>
		/// Disable software mixer to save memory.
		/// Anything created with SoftwareDisable will fail and DSP will not work.
		/// </summary>
		SoftwareDisable = 0x4,
		
		//TODO replace Channel::set3DOcclusion by real setting.
		
		/// <summary>
		/// All FMOD_SOFTWARE with FMOD_3D based voices will add a
		/// software lowpass filter effect into the DSP chain which is automatically
		/// used when Channel::set3DOcclusion is used or the geometry API.
		/// </summary>
		SoftwareOcclusion = 0x8,
		
		/// <summary>
		/// All Software with 3D based voices will add a
		/// software lowpass filter effect into the DSP chain which causes
		/// sounds to sound duller when the sound goes behind the listener.
		/// </summary>
		SoftwareHRTF = 0x10,
		
		/// <summary>
		/// SFX reverb is run using 22/24khz delay buffers, halving the memory required.
		/// </summary>
		SoftwareReverbLowMem = 0x40,
		
		/// <summary>
		/// Enable TCP/IP based host which allows "DSPNet Listener.exe" to connect to it,
		/// and view the DSP dataflow network graph in real-time.
		/// </summary>
		EnableProfile = 0x20,
		
		//TODO replace System::setAdvancedSettings
		
		/// <summary>
		/// Any sounds that are 0 volume will go virtual and not be processed except
		/// for having their positions updated virtually.
		/// Use System::setAdvancedSettings to adjust what volume besides zero to switch
		/// to virtual at.
		/// </summary>
		Vol0BecomesVirtual = 0x80,
		
		
		
		WASAPI_EXCLUSIVE = 0x100,
		// Win32 Vista only - for WASAPI output - Enable exclusive access to hardware, lower latency at the expense of excluding other applications from accessing the audio hardware.
		
		DSOUND_HRTFNONE = 0x200,
		// Win32 only - for DirectSound output - FMOD_HARDWARE | FMOD_3D buffers use simple stereo panning/doppler/attenuation when 3D hardware acceleration is not present.
		
		DSOUND_HRTFLIGHT = 0x400,
		// Win32 only - for DirectSound output - FMOD_HARDWARE | FMOD_3D buffers use a slightly higher quality algorithm when 3D hardware acceleration is not present.
		
		DSOUND_HRTFFULL = 0x800,
		// Win32 only - for DirectSound output - FMOD_HARDWARE | FMOD_3D buffers use full quality 3D playback when 3d hardware acceleration is not present.
		
		PS2_DISABLECORE0REVERB = 0x10000,
		// PS2 only - Disable reverb on CORE 0 to regain SRAM.
		
		PS2_DISABLECORE1REVERB = 0x20000,
		// PS2 only - Disable reverb on CORE 1 to regain SRAM.
		
		PS2_DONTUSESCRATCHPAD = 0x40000,
		// PS2 only - Disable FMOD's usage of the scratchpad.
		
		PS2_SWAPDMACHANNELS = 0x80000,
		// PS2 only - Changes FMOD from using SPU DMA channel 0 for software mixing, and 1 for sound data upload/file streaming, to 1 and 0 respectively.
		
		XBOX_REMOVEHEADROOM = 0x100000,
		// XBox only - By default DirectSound attenuates all sound by 6db to avoid clipping/distortion.  CAUTION.  If you use this flag you are responsible for the final mix to make sure clipping / distortion doesn't happen.
		
		Xbox360_MUSICMUTENOTPAUSE = 0x200000,
		// Xbox 360 only - The "music" channelgroup which by default pauses when custom 360 dashboard music is played, can be changed to mute (therefore continues playing) instead of pausing, by using this flag.
		
		SYNCMIXERWITHUPDATE = 0x400000,
		// Win32/Wii/PS3/Xbox/Xbox 360 - FMOD Mixer thread is woken up to do a mix when System::update is called rather than waking periodically on its own timer.
		
		DTS_NEURALSURROUND      = 0x02000000,
		/* Win32/Mac/Linux - Use DTS Neural surround downmixing from 7.1 if speakermode set to FMOD_SPEAKERMODE_STEREO or FMOD_SPEAKERMODE_5POINT1.  Internal DSP structure will be set to 7.1. */
        
		GEOMETRY_USECLOSEST     = 0x04000000,
		/* All platforms - With the geometry engine, only process the closest polygon rather than accumulating all polygons the sound to listener line intersects. */
        
		DISABLE_MYEARS          = 0x08000000
		/* Win32 - Disables MyEars HRTF 7.1 downmixing.  MyEars will otherwise be disbaled if speakermode is not set to FMOD_SPEAKERMODE_STEREO or the data file is missing. */
	}
	
	//TODO complete conversion.
}

