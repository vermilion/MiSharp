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
using System.Security;

namespace Linsft.FmodSharp
{
	
	/// <summary>
	/// Bit fields to use with <see cref="FmodSharp.Debug.Level"/> to
	/// control the level of tty debug output with logging versions of FMOD (fmodL).
	/// </summary>
	/// <platforms>
	/// Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2,
	/// PlayStation Portable, PlayStation 3, Wii, Android
	/// </platforms>
	[Flags]
	public enum DebugLevel : byte
	{
		None = 0x00,
		
		Log = 0x01,
		Error = 0x02,
		Warning = 0x04,
		Hint = 0x08,
		
		All = 0xFF,
	}
	
	[Flags]
	public enum DebugType : byte
	{
		None = 0x00,
		
		Memory = 0x01,
        Thread = 0x02,
        File = 0x04,
        Net = 0x08,
        Event = 0x10,
		
        All = 0xFF,
	}
	
	[Flags]
	public enum DebugDisplay : byte
	{
		None = 0x00,
		
		Timestamps = 0x01,
        LineNumbers = 0x02,
        Compress = 0x04,
        Thread = 0x08,
		
        All = 0x0F,
	}
	
	/// <platforms>
	/// Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2,
	/// PlayStation Portable, PlayStation 3, Wii
	/// </platforms>
	public static class Debug
	{
		public static DebugLevel Level {
			get { return (DebugLevel)(DebugValue & 0xFF); }
			set { DebugValue = (int)value | (int)(DebugValue & 0xFFFFFF00); }
		}
		
		public static DebugType Type {
			get { return (DebugType)((DebugValue >> 8) & 0xFF); }
			set { DebugValue = ((int)value << 8) | (int)(DebugValue & 0xFFFF00FF); }
		}
		
		public static DebugDisplay Display {
			get { return (DebugDisplay)((DebugValue >> 24) & 0x0F); }
			set { DebugValue = (((int)value & 0x0F) << 24) | (int)(DebugValue & 0xF0FFFFFF); }
		}
		
		private static int DebugValue {
			get {
				int Val = 0;
				Error.Code ReturnCode = GetLevel(ref Val);
				if(ReturnCode == Error.Code.Unsupported) {
					// On windows you need the Loggin version of Fmod to use Debugging.
					//Error.Code.Unsupported [82]
				} else {
					Error.Errors.ThrowError(ReturnCode);
				}
				
				return Val;
			}
			
			set {
				Error.Code ReturnCode = SetLevel(value);
				if(ReturnCode == Error.Code.Unsupported) {
					// On windows you need the Loggin version of Fmod to use Debugging.
					//Error.Code.Unsupported [82]
				} else {
					Error.Errors.ThrowError(ReturnCode);
				}
			}
		}
		
		[DllImport("fmodex", SetLastError = true, EntryPoint = "FMOD_Debug_SetLevel"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetLevel (int Level);
		
		[DllImport("fmodex", SetLastError = true, EntryPoint = "FMOD_Debug_GetLevel"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetLevel (ref int Level);
		
	}
}
