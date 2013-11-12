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

namespace Linsft.FmodSharp.Dsp
{
	public class Connection : Handle
	{
		#region Create/Release
		
		private Connection ()
		{
		}
		
		internal Connection (IntPtr ConnPtr)
		{
			this.SetHandle(ConnPtr);
		}
		
		protected override bool ReleaseHandle ()
		{
			if (this.IsInvalid)
				return true;
			
			//TODO find if Connection need to be released before closing.
			//Release (this.handle);
			this.SetHandleAsInvalid ();
			
			return true;
		}
		
		public float Mix {
			get {
				float Val = 0;
				Error.Code ReturnCode = GetMix(this.DangerousGetHandle(), ref Val);
				Error.Errors.ThrowError(ReturnCode);
				
				return Val;
			}
			
			set {
				Error.Code ReturnCode = SetMix(this.DangerousGetHandle(), value);
				Error.Errors.ThrowError(ReturnCode);
			}
		}
		
		[DllImport("fmodex", EntryPoint = "FMOD_DSPConnection_SetMix"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetMix (IntPtr dspconnection, float volume);
		
		[DllImport("fmodex", EntryPoint = "FMOD_DSPConnection_GetMix"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetMix (IntPtr dspconnection, ref float volume);

		#endregion
		
		//TODO Implement extern funcitons
		
		/*
		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_DSPConnection_GetInput (IntPtr dspconnection, ref IntPtr input);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_DSPConnection_GetOutput (IntPtr dspconnection, ref IntPtr output);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_DSPConnection_SetLevels (IntPtr dspconnection, SPEAKER speaker, float[] levels, int numlevels);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_DSPConnection_GetLevels (IntPtr dspconnection, SPEAKER speaker, [MarshalAs(UnmanagedType.LPArray)] float[] levels, int numlevels);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_DSPConnection_SetUserData (IntPtr dspconnection, IntPtr userdata);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_DSPConnection_GetUserData (IntPtr dspconnection, ref IntPtr userdata);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_DSPConnection_GetMemoryInfo (IntPtr dspconnection, uint memorybits, uint event_memorybits, ref uint memoryused, ref MEMORY_USAGE_DETAILS memoryused_details);
		*/
	}
}
