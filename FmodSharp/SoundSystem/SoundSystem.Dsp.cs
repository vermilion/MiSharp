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
		
		public Dsp.Dsp CreateDSP(ref Dsp.Description description)
		{
			IntPtr DspHandle = IntPtr.Zero;
			
			Error.Code ReturnCode = CreateDSP (this.DangerousGetHandle (), ref description, ref DspHandle);
			Error.Errors.ThrowError (ReturnCode);
			
			return new Dsp.Dsp (DspHandle);
		}
		
		public Dsp.Dsp CreateDspByType(Dsp.Type type)
		{
			IntPtr DspHandle = IntPtr.Zero;
			
			Error.Code ReturnCode = CreateDspByType (this.DangerousGetHandle (), type, ref DspHandle);
			Error.Errors.ThrowError (ReturnCode);
			
			return new Dsp.Dsp (DspHandle);
		}
		
		public Channel.Channel PlayDsp (Dsp.Dsp dsp)
		{
			return PlayDsp (dsp, false);
		}

		public Channel.Channel PlayDsp (Dsp.Dsp dsp, bool paused)
		{
			IntPtr ChannelHandle = IntPtr.Zero;
			
			Error.Code ReturnCode = PlayDsp (this.DangerousGetHandle (), Channel.Index.Free, dsp.DangerousGetHandle (), paused, ref ChannelHandle);
			Error.Errors.ThrowError (ReturnCode);
			
			return new Channel.Channel (ChannelHandle);
		}

		public void PlayDsp (Dsp.Dsp dsp, bool paused, Channel.Channel chn)
		{
			IntPtr channel = chn.DangerousGetHandle ();
			
			Error.Code ReturnCode = PlayDsp (this.DangerousGetHandle (), Channel.Index.Reuse, dsp.DangerousGetHandle (), paused, ref channel);
			Error.Errors.ThrowError (ReturnCode);
			
			//This can't really happend.
			//Check just in case...
			if(chn.DangerousGetHandle () != channel)
				throw new Exception("Channel handle got changed by Fmod.");
		}
		
		public Dsp.Connection AddDsp (Dsp.Dsp dsp)
		{
			IntPtr ConnectionHandle = IntPtr.Zero;
			
			Error.Code ReturnCode = AddDSP (this.DangerousGetHandle (), dsp.DangerousGetHandle (), ref ConnectionHandle);
			Error.Errors.ThrowError (ReturnCode);
			
			return new Dsp.Connection (ConnectionHandle);
		}
		
		public void LockDSP ()
		{
			Error.Code ReturnCode = LockDSP (this.DangerousGetHandle ());
			Error.Errors.ThrowError (ReturnCode);
		}
		
		public void UnlockDSP ()
		{
			Error.Code ReturnCode = UnlockDSP (this.DangerousGetHandle ());
			Error.Errors.ThrowError (ReturnCode);
		}
		
		public ulong DSPClock {
			get {
				uint hi = 0, low = 0;
				Error.Code ReturnCode = GetDSPClock (this.DangerousGetHandle (), ref hi, ref low);
				Error.Errors.ThrowError (ReturnCode);
				return (hi << 32) | low;
			}
		}

		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport ("fmodex", EntryPoint = "FMOD_System_CreateDSP")]
		private static extern Error.Code CreateDSP (IntPtr system, ref Dsp.Description description, ref IntPtr dsp);

		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport ("fmodex", EntryPoint = "FMOD_System_CreateDSPByType")]
		private static extern Error.Code CreateDspByType (IntPtr system, Dsp.Type type, ref IntPtr dsp);

		[DllImport("fmodex", EntryPoint = "FMOD_System_PlayDSP"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code PlayDsp (IntPtr system, Channel.Index channelid, IntPtr dsp, bool paused, ref IntPtr channel);
		
		[DllImport("fmodex", EntryPoint = "FMOD_System_AddDSP"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code AddDSP (IntPtr system, IntPtr dsp, ref IntPtr connection);
		
		[DllImport("fmodex", EntryPoint = "FMOD_System_LockDSP"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code LockDSP (IntPtr system);

		[DllImport("fmodex", EntryPoint = "FMOD_System_UnlockDSP"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code UnlockDSP (IntPtr system);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetDSPClock"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetDSPClock (IntPtr system, ref uint hi, ref uint lo);
	}
}

