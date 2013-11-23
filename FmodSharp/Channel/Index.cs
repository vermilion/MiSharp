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

namespace Linsft.FmodSharp.Channel
{
	/// <summary>
	/// 
	/// </summary>
	/// <platforms>
	/// Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii
	/// </platforms>
	/// <seealso cref="FmodSharp.System.PlaySound"/>
	/// <seealso cref="FmodSharp.System.PlayDSP"/>
	/// <seealso cref="FmodSharp.System.GetChannel"/>
	public enum Index
	{
		/// <summary>
		/// For a channel index, FMOD chooses a free voice using the priority system.
		/// </summary>
		Free = -1,
		
		/// <summary>
		/// For a channel index, re-use the channel handle that was passed in.
		/// </summary>
		Reuse = -2
	}
}
