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

namespace Linsft.FmodSharp.Channel
{
	public class ChannelGroup : Handle
	{
		
		#region Create/Release
		
		private ChannelGroup ()
		{
		}
		internal ChannelGroup (IntPtr hnd) : base()
		{
			this.SetHandle(hnd);
		}
		
		protected override bool ReleaseHandle ()
		{
			if (this.IsInvalid)
				return true;
			
			Release (this.handle);
			this.SetHandleAsInvalid ();
			
			return true;
		}
		
		[DllImport("fmodex", EntryPoint = "FMOD_ChannelGroup_Release"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Release (IntPtr channelgroup);
		
		#endregion
		
		
		//TODO Implement extern funcitons
		/*
	
		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetSystemObject (IntPtr channelgroup, ref IntPtr system);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_SetVolume (IntPtr channelgroup, float volume);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetVolume (IntPtr channelgroup, ref float volume);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_SetPitch (IntPtr channelgroup, float pitch);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetPitch (IntPtr channelgroup, ref float pitch);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_Set3DOcclusion (IntPtr channelgroup, float directocclusion, float reverbocclusion);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_Get3DOcclusion (IntPtr channelgroup, ref float directocclusion, ref float reverbocclusion);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_SetPaused (IntPtr channelgroup, int paused);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetPaused (IntPtr channelgroup, ref int paused);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_SetMute (IntPtr channelgroup, int mute);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetMute (IntPtr channelgroup, ref int mute);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_Stop (IntPtr channelgroup);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_OverridePaused (IntPtr channelgroup, int paused);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_OverrideVolume (IntPtr channelgroup, float volume);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_OverrideFrequency (IntPtr channelgroup, float frequency);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_OverridePan (IntPtr channelgroup, float pan);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_OverrideMute (IntPtr channelgroup, int mute);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_OverrideReverbProperties (IntPtr channelgroup, ref REVERB_CHANNELPROPERTIES prop);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_Override3DAttributes (IntPtr channelgroup, ref VECTOR pos, ref VECTOR vel);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_OverrideSpeakerMix (IntPtr channelgroup, float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_AddGroup (IntPtr channelgroup, IntPtr @group);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetNumGroups (IntPtr channelgroup, ref int numgroups);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetGroup (IntPtr channelgroup, int index, ref IntPtr @group);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetParentGroup (IntPtr channelgroup, ref IntPtr @group);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetDSPHead (IntPtr channelgroup, ref IntPtr dsp);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_AddDSP (IntPtr channelgroup, IntPtr dsp, ref IntPtr connection);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetName (IntPtr channelgroup, StringBuilder name, int namelen);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetNumChannels (IntPtr channelgroup, ref int numchannels);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetChannel (IntPtr channelgroup, int index, ref IntPtr channel);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetSpectrum (IntPtr channelgroup, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetWaveData (IntPtr channelgroup, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_SetUserData (IntPtr channelgroup, IntPtr userdata);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetUserData (IntPtr channelgroup, ref IntPtr userdata);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_ChannelGroup_GetMemoryInfo (IntPtr channelgroup, uint memorybits, uint event_memorybits, ref uint memoryused, ref MEMORY_USAGE_DETAILS memoryused_details);
		*/
	}
}
