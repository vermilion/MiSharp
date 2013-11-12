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
	
    /*
    [DEFINE] 
    [
        [NAME] 
        REVERB_CHANNELFLAGS

        [DESCRIPTION]
        Values for the Flags member of the REVERB_CHANNELPROPERTIES structure.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        REVERB_CHANNELPROPERTIES
    ]
    */
	[Flags]
	public enum ChannelFlags : uint
	{
		DirectHFAuto  = 0x00000001,
		/* Automatic setting of 'Direct'  due to distance from listener */
        
		RoomAuto      = 0x00000002,
		/* Automatic setting of 'Room'  due to distance from listener */
        
		RoomHFAuto   = 0x00000004,
		/* Automatic setting of 'RoomHF' due to distance from listener */
        
		Instance0     = 0x00000010,
		/* SFX/Wii. Specify channel to target reverb instance 0.  Default target. */
        
		Instance1     = 0x00000020,
		/* SFX/Wii. Specify channel to target reverb instance 1. */
        
		Instance2     = 0x00000040,
		/* SFX/Wii. Specify channel to target reverb instance 2. */
        
		Instance3     = 0x00000080,
		/* SFX. Specify channel to target reverb instance 3. */
		
		Default = (DirectHFAuto | RoomAuto | RoomHFAuto | Instance0)
	}
}

