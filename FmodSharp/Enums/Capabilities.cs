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

namespace Linsft.FmodSharp
{
	//TODO End subbmary
	
	[Flags]
	public enum Capabilities : uint
	{
		/// <summary>
		/// Device has no special capabilities.
		/// </summary>
		None = 0x00000000,

		/// <summary>
		/// Device supports hardware mixing.
		/// </summary>
		Hardware = 0x00000001,

		/// <summary>
		/// User has device set to 'Hardware acceleration = off' in control panel,
		/// and now extra 200ms latency is incurred.
		/// </summary>
		HardwareEmulated = 0x00000002,

		/// <summary>
		/// Device can do multichannel output, ie greater than 2 channels.
		/// </summary>
		OutputMultichannel = 0x00000004,

		/// <summary>
		/// Device can output to 8bit integer PCM.
		/// </summary>
		OutputFormatPCM8 = 0x00000008,

		/// <summary>
		/// Device can output to 16bit integer PCM.
		/// </summary>
		OutputFormatPCM16 = 0x00000010,

		/// <summary>
		/// Device can output to 24bit integer PCM.
		/// </summary>
		OutputFormatPCM24 = 0x00000020,

		/// <summary>
		/// Device can output to 32bit integer PCM.
		/// </summary>
		OutputFormatPCM32 = 0x00000040,

		/// <summary>
		/// Device can output to 32bit floating point PCM.
		/// </summary>
		OutputFormatPCMFloat = 0x00000080,

		/// <summary>
		/// Device supports EAX2 reverb.
		/// </summary>
		ReverbEAX2 = 0x00000100,

		/// <summary>
		/// Device supports EAX3 reverb.
		/// </summary>
		ReverbEAX3 = 0x00000200,

		/// <summary>
		/// Device supports EAX4 reverb.
		/// </summary>
		ReverbEAX4 = 0x00000400,

		/// <summary>
		/// Device supports EAX5 reverb.
		/// </summary>
		ReverbEAX5 = 0x00000800,

		/// <summary>
		/// Device supports I3DL2 reverb.
		/// </summary>
		ReverbI3DL2 = 0x00001000,

		/// <summary>
		/// Device supports some form of limited hardware reverb, maybe parameterless and only selectable by environment.
		/// </summary>
		ReverbLimited = 0x00002000
	}


	/*
        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii
	 */
}
