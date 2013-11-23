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
using System.Runtime.InteropServices;

namespace Linsft.FmodSharp
{
	public delegate Error.Code File_OpenDelegate ([MarshalAs(UnmanagedType.LPWStr)]string name, int unicode, ref uint filesize, ref IntPtr handle, ref IntPtr userdata);
    public delegate Error.Code File_CloseDelegate (IntPtr handle, IntPtr userdata);
    public delegate Error.Code File_ReadDelegate (IntPtr handle, IntPtr buffer, uint sizebytes, ref uint bytesread, IntPtr userdata);
    public delegate Error.Code File_SeekDelegate (IntPtr handle, int pos, IntPtr userdata);
    public delegate Error.Code File_AsyncReadDelegate (IntPtr handle, IntPtr info, IntPtr userdata);
    public delegate Error.Code File_AsyncCancelDelegate (IntPtr handle, IntPtr userdata);

    public delegate float  CB_3D_RollOffDelegate (IntPtr channelraw, float distance);
	
	
}

