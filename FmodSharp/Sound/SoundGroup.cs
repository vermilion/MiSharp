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

namespace Linsft.FmodSharp.Sound
{
	public class SoundGroup : Handle
	{
		
		#region Create/Release
		
		private SoundGroup ()
		{
		}
		internal SoundGroup (IntPtr hnd) : base()
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
		
		[DllImport("fmodex", EntryPoint = "FMOD_SoundGroup_Release"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Release (IntPtr soundgroup);

		#endregion
		
		//TODO Implement extern funcitons
		/*
	
		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetSystemObject (IntPtr soundgroup, ref IntPtr system);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_SetMaxAudible (IntPtr soundgroup, int maxaudible);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetMaxAudible (IntPtr soundgroup, ref int maxaudible);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_SetMaxAudibleBehavior (IntPtr soundgroup, SOUNDGROUP_BEHAVIOR behavior);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetMaxAudibleBehavior (IntPtr soundgroup, ref SOUNDGROUP_BEHAVIOR behavior);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_SetMuteFadeSpeed (IntPtr soundgroup, float speed);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetMuteFadeSpeed (IntPtr soundgroup, ref float speed);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_SetVolume (IntPtr soundgroup, float volume);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetVolume (IntPtr soundgroup, ref float volume);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_Stop (IntPtr soundgroup);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetName (IntPtr soundgroup, StringBuilder name, int namelen);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetNumSounds (IntPtr soundgroup, ref int numsounds);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetSound (IntPtr soundgroup, int index, ref IntPtr sound);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetNumPlaying (IntPtr soundgroup, ref int numplaying);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_SetUserData (IntPtr soundgroup, IntPtr userdata);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetUserData (IntPtr soundgroup, ref IntPtr userdata);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_SoundGroup_GetMemoryInfo (IntPtr soundgroup, uint memorybits, uint event_memorybits, ref uint memoryused, ref MEMORY_USAGE_DETAILS memoryused_details);
		*/
	}
}
