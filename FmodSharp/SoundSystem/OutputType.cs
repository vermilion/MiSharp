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
	//TODO Replace System::setOutput/System::getOutput by real name.
	//TODO Convert Remarks with real names.
	
	/// <summary>
	/// These output types are used with System::setOutput/System::getOutput,
	/// to choose which output method to use.
	/// </summary>
	/// <remarks>
	/// To drive the output synchronously, and to disable FMOD's timing thread, use the FMOD_INIT_NONREALTIME flag.
	/// To pass information to the driver when initializing fmod use the extradriverdata parameter for the following reasons.
	/// <li>FMOD_OUTPUTTYPE_WAVWRITER - extradriverdata is a pointer to a char * filename that the wav writer will output to.
	/// <li>FMOD_OUTPUTTYPE_WAVWRITER_NRT - extradriverdata is a pointer to a char * filename that the wav writer will output to.
	/// <li>FMOD_OUTPUTTYPE_DSOUND - extradriverdata is a pointer to a HWND so that FMOD can set the focus on the audio for a particular window.
	/// <li>FMOD_OUTPUTTYPE_GC - extradriverdata is a pointer to a FMOD_ARAMBLOCK_INFO struct. This can be found in fmodgc.h.
	/// Currently these are the only FMOD drivers that take extra information.  Other unknown plugins may have different requirements.
	/// </remarks>
	public enum OutputType : int
	{
		/// <summary>
		/// Picks the best output mode for the platform.
		/// This is the default.
		/// </summary>
		AutoDetect,

        /// <summary>
        /// 3rd party plugin, unknown.  This is for use with System::getOutput only.
        /// </summary>
		Unknown,
        
		/// <summary>
		/// All calls in this mode succeed but make no sound.
		/// </summary>
		NoSound,
		
		//TODO Replace System::setSoftwareFormat by real name.
		
		/// <summary>
		/// Writes output to fmodout.wav by default.
		/// Use System::setSoftwareFormat to set the filename.
		/// </summary>
		WavWriter,
		
		/// <summary>
		/// Non-realtime version of FMOD_OUTPUTTYPE_NOSOUND.
		/// User can drive mixer with System::update at whatever rate they want.
		/// </summary>
		NoSound_NRT,
     
		/// <summary>
		/// Non-realtime version of FMOD_OUTPUTTYPE_WAVWRITER.
		/// User can drive mixer with System::update at whatever rate they want.
		/// </summary>
		WavWriter_NRT,
		
        /// <summary>
		/// Win32/Win64 - DirectSound output.
		/// Use this to get hardware accelerated 3d audio and EAX Reverb support.
		/// (Default on Windows)
		/// </summary>
		DirectSound,
		
		/// <summary>
		/// Win32/Win64 - Windows Multimedia output.
		/// </summary>
		WindowsMultimedia,
		
		/// <summary>
		/// Win32 - Windows Audio Session API. (Default on Windows Vista)
		/// </summary>
		WASAPI,
		
		/// <summary>
		/// Win32 - Low latency ASIO driver.
		/// </summary>
		ASIO,
		
		/// <summary>
		/// Linux - Open Sound System output.
		/// </summary>
		OSS,
		
		/// <summary>
		/// Linux - Advanced Linux Sound Architecture output.
		/// </summary>
		ALSA,
		
		/// <summary>
		/// Linux - Enlightment Sound Daemon output.
		/// </summary>
		ESD,
		
		/// <summary>
		/// Linux/Linux64 - PulseAudio output.
		/// </summary>
		PulseAudio,
		
		/// <summary>
		/// Mac - Macintosh SoundManager output.
		/// </summary>
		SoundManager,
		
		/// <summary>
		/// Mac - Macintosh CoreAudio output.
		/// </summary>
		CoreAudio,
		
		/// <summary>
		/// Xbox - Native hardware output.
		/// </summary>
		Xbox,
		
		/// <summary>
		/// PS2 - Native hardware output.
		/// </summary>
		PS2,
		
		/// <summary>
		/// PS3 - Native hardware output. (Default on PS3)
		/// </summary>
		PS3,
		
		/// <summary>
		/// GameCube - Native hardware output.
		/// </summary>
		GameCube,
		
		/// <summary>
		/// Xbox 360 - Native hardware output.
		/// </summary>
		Xbox360,
		
		/// <summary>
		/// PSP - Native hardware output.
		/// </summary>
		PSP,
		
		/// <summary>
		/// Native hardware output. (Default on Wii)
		/// </summary>
		Wii,
        
		/// <summary>
		/// Native android output.
		/// </summary>
		Android,
		
		/// <summary>
		/// Maximum number of output types supported.
		/// </summary>
        Max
	}
	
	//TODO complete submmary
	
	    /*
 
        [REMARKS]


        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii, Android

        [SEE_ALSO]      
        System::setOutput
        System::getOutput
        System::setSoftwareFormat
        System::getSoftwareFormat
        System::init
    ]
    */
}

