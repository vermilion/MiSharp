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
using Linsft.FmodSharp.Error;

namespace Linsft.FmodSharp.Channel
{
	/// <summary>
	/// These callback types are used with Channel::setCallback.
	/// </summary>
	/// <remarks>
	/// Each callback has commanddata parameters passed int unique to the type of callback.
	/// See reference to FMOD_CHANNEL_CALLBACK to determine what they might mean for each type of callback.
	/// </remarks>
	public enum CallbackType : int
	{
		/// <summary>
		/// Called when a sound ends.
		/// </summary>
		End,

		/// <summary>
		/// Called when a voice is swapped out or swapped in.
		/// </summary>
		VirtualVoice,

		/// <summary>
		/// Called when a syncpoint is encountered.  Can be from wav file markers.
		/// </summary>
		SyncPoint,

		/// <summary>
		/// Called when the channel has its geometry occlusion value calculated.
		/// Can be used to clamp or change the value.
		/// </summary>
		Occlusion,

		Max
	}

	public delegate Code ChannelDelegate (IntPtr channelraw, CallbackType type, IntPtr commanddata1, IntPtr commanddata2);
	
	//TODO end submmary
	
	/*
    [ENUM]
    [
        [DESCRIPTION]   
        

        [REMARKS]


        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        Channel::setCallback
        FMOD_CHANNEL_CALLBACK
    ]
    */	
	
}

