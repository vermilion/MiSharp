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

namespace Linsft.FmodSharp.Memory
{
	//TODO complete submmary
	
	    /*
    [DEFINE] 
    [
        [NAME]
        FMOD_MEMORY_TYPE

        [DESCRIPTION]   
        Bit fields for memory allocation type being passed into FMOD memory callbacks.

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        FMOD_MEMORY_ALLOCCALLBACK
        FMOD_MEMORY_REALLOCCALLBACK
        FMOD_MEMORY_FREECALLBACK
        Memory_Initialize
    
    ]
    */
    public enum Type
    {
        /// <summary>
		/// Standard memory.
		/// </summary>
		Normal = 0x00000000,
		
		/// <summary>
		/// Stream file buffer, size controllable with System::setStreamBufferSize.
		/// </summary>
		StreamFile = 0x00000001,
		
		/// <summary>
		/// Stream decode buffer, size controllable with FMOD_CREATESOUNDEXINFO::decodebuffersize.
		/// </summary>
		StreamDecode = 0x00000002,
		
		/// <summary>
		/// Requires XPhysicalAlloc / XPhysicalFree.
		/// </summary>
		Xbox360_Physical = 0x00100000,
		
		/// <summary>
		/// Persistent memory. Memory will be freed when System::release is called.
		/// </summary>
		Persistent = 0x00200000,
		
		/// <summary>
		/// Secondary memory. Allocation should be in secondary memory. For example RSX on the PS3.
		/// </summary>
		Secondary = 0x00400000
	}
}

