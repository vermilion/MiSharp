// Author:
//       Marc-Andre Ferland <madrang@gmail.com>
// 
// Copyright (c) 2011 TheWarrentTeam
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Linsft.FmodSharp.SoundSystem
{
	public partial class SoundSystem
	{
		public Reverb.Reverb CreateReverb ()
		{
			IntPtr ReverbHandle = IntPtr.Zero;
			
			Error.Code ReturnCode = CreateReverb (this.DangerousGetHandle (), ref ReverbHandle);
			Error.Errors.ThrowError (ReturnCode);
			
			return new Reverb.Reverb (ReverbHandle);
		}
		
		public Reverb.Properties ReverbProperties {
			get {
				Reverb.Properties Val = Reverb.Properties.Generic;
				Error.Code ReturnCode = GetReverbProperties(this.DangerousGetHandle(), ref Val);
				Error.Errors.ThrowError(ReturnCode);
				
				return Val;
			}
			
			set {
				Error.Code ReturnCode = SetReverbProperties(this.DangerousGetHandle(), ref value);
				Error.Errors.ThrowError(ReturnCode);
			}
		}
		
		public Reverb.Properties ReverbAmbientProperties {
			get {
				Reverb.Properties Val = Reverb.Properties.Generic;
				
				Error.Code ReturnCode = GetReverbAmbientProperties(this.DangerousGetHandle(), ref Val);
				Error.Errors.ThrowError(ReturnCode);
				
				return Val;
			}
			
			set {
				Error.Code ReturnCode = SetReverbAmbientProperties(this.DangerousGetHandle(), ref value);
				Error.Errors.ThrowError(ReturnCode);
			}
		}
		
		[DllImport("fmodex", EntryPoint = "FMOD_System_CreateReverb"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code CreateReverb (IntPtr system, ref IntPtr reverb);
		
		[DllImport("fmodex", EntryPoint = "FMOD_System_SetReverbProperties"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetReverbProperties (IntPtr system, ref Reverb.Properties prop);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetReverbProperties"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetReverbProperties (IntPtr system, ref Reverb.Properties prop);

		[DllImport("fmodex", EntryPoint = "FMOD_System_SetReverbAmbientProperties"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetReverbAmbientProperties (IntPtr system, ref Reverb.Properties prop);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetReverbAmbientProperties"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetReverbAmbientProperties (IntPtr system, ref Reverb.Properties prop);
		
	}
}

