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
	/*
    [ENUM]
    [
        [DESCRIPTION]   
        

        [REMARKS]
        Each callback has commanddata parameters passed as int unique to the type of callback.<br>
        See reference to FMOD_SYSTEM_CALLBACK to determine what they might mean for each type of callback.<br>
        <br>
        <b>Note!</b>  Currently the user must call System::update for these callbacks to trigger!

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        System::setCallback
        FMOD_SYSTEM_CALLBACK
        System::update
    ]
    */

	/// <summary>
	/// These callback types are used with System::setCallback.
	/// </summary>
	public enum CallbackType : int
	{
		/// <summary>
		/// Called when the enumerated list of devices has changed.
		/// </summary>
		DeviceListChanged,

		/// <summary>
		/// Called from System::update when an output device has been lost
		/// due to control panel parameter changes and FMOD cannot automatically recover.
		/// </summary>
		DeviceLost,

		/// <summary>
		/// Called directly when a memory allocation fails somewhere in FMOD.
		/// </summary>
		MemoryAllocationFailed,

		/// <summary>
		/// Called directly when a thread is created.
		/// </summary>
		ThreadCreated,

		/// <summary>
		/// Called when a bad connection was made with DSP::addInput.
		/// Usually called from mixer thread because that is where the connections are made.
		/// </summary>
		BadDspConnection,

		/// <summary>
		/// Called when too many effects were added exceeding the maximum tree depth of 128.
		/// This is most likely caused by accidentally adding too many DSP effects.
		/// Usually called from mixer thread because that is where the connections are made.
		/// </summary>
		BadDspLevel,

		/// <summary>
		/// Maximum number of callback types supported.
		/// </summary>
		Max
		
	}

	public delegate Error.Code SystemDelegate (IntPtr systemraw, CallbackType type, IntPtr commanddata1, IntPtr commanddata2);
	
	//TODO complete submary
}
