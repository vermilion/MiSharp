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
using Linsft.FmodSharp.Enums;
using Linsft.FmodSharp.Error;

namespace Linsft.FmodSharp.Sound
{
    public class Sound : Handle
    {
        #region Create/Release

        internal Sound(IntPtr hnd)
        {
            SetHandle(hnd);
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return true;

            //TODO: this sometimes throws exceptions when handle changes
            //Release(handle);
            SetHandleAsInvalid();

            return true;
        }

        [DllImport("fmodex", EntryPoint = "FMOD_Sound_Release"), SuppressUnmanagedCodeSecurity]
        private static extern Code Release(IntPtr sound);

        #endregion

        #region Length

        public uint LengthMs
        {
            get
            {
                uint val = 0;
                Code returnCode = FMOD_Sound_GetLength(DangerousGetHandle(), ref val, TimeUnit.Milliseconds);
                Errors.ThrowError(returnCode);

                return val;
            }
        }

        [DllImport("fmodex", EntryPoint = "FMOD_Sound_GetLength"), SuppressUnmanagedCodeSecurity]
        private static extern Code FMOD_Sound_GetLength(IntPtr sound, ref uint length, TimeUnit lengthtype);

        #endregion

        #region Sound Mode

        public Mode SoundMode
        {
            get
            {
                var mode = Mode.Default;
                Code returnCode = FMOD_Sound_GetMode(DangerousGetHandle(), ref mode);
                Errors.ThrowError(returnCode);

                return mode;
            }
            set
            {
                Code returnCode = FMOD_Sound_SetMode(DangerousGetHandle(), value);
                Errors.ThrowError(returnCode);
            }
        }

        [DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
        private static extern Code FMOD_Sound_SetMode(IntPtr sound, Mode mode);

        [DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
        private static extern Code FMOD_Sound_GetMode(IntPtr sound, ref Mode mode);

        #endregion

        #region Music speed

        public float MusicSpeed
        {
            get
            {
                float speed = 1.0f;
                Code returnCode = FMOD_Sound_GetMusicSpeed(DangerousGetHandle(), ref speed);
                Errors.ThrowError(returnCode);

                return speed;
            }
            set
            {
                Code returnCode = FMOD_Sound_SetMusicSpeed(DangerousGetHandle(), value);
                Errors.ThrowError(returnCode);
            }
        }

        [DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
        private static extern Code FMOD_Sound_SetMusicSpeed(IntPtr sound, float speed);

        [DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
        private static extern Code FMOD_Sound_GetMusicSpeed(IntPtr sound, ref float speed);

        #endregion

        #region Music Channels

        public int MusicChannelsNum
        {
            get
            {
                var numchannels = 0;
                Code returnCode = GetMusicNumChannels(DangerousGetHandle(), ref numchannels);
                Errors.ThrowError(returnCode);

                return numchannels;
            }
        }

        public void SetMusicChannelVolume(int channel, float volume)
        {
            Code returnCode = SetMusicChannelVolume(DangerousGetHandle(), channel, volume);
            Errors.ThrowError(returnCode);
        }

        public float GetMusicChannelVolume(int channel)
        {
            float volume = 1.0f;
            Code returnCode = GetMusicChannelVolume(DangerousGetHandle(), channel, ref volume);
            Errors.ThrowError(returnCode);

            return volume;
        }

        [DllImport("fmodex", EntryPoint = "FMOD_Sound_GetMusicNumChannels"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetMusicNumChannels(IntPtr sound, ref int numchannels);

        [DllImport("fmodex", EntryPoint = "FMOD_Sound_SetMusicChannelVolume"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetMusicChannelVolume(IntPtr sound, int channel, float volume);

        [DllImport("fmodex", EntryPoint = "FMOD_Sound_GetMusicChannelVolume"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetMusicChannelVolume(IntPtr sound, int channel, ref float volume);

        #endregion

        #region Open State

        public void GetOpenState(ref OpenState openstate, ref uint percentbuffered, ref int starving, ref int diskbusy)
        {
            Code returnCode = GetOpenState(DangerousGetHandle(), ref openstate, ref percentbuffered, ref starving, ref diskbusy);
            Errors.ThrowError(returnCode);
        }

        [DllImport("fmodex", EntryPoint = "FMOD_Sound_GetOpenState"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetOpenState(IntPtr sound, ref OpenState openstate, ref uint percentbuffered, ref int starving, ref int diskbusy);

        #endregion

        //TODO Implement extern functions

        /*
		
		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetSystemObject (IntPtr sound, ref IntPtr system);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_Lock (IntPtr sound, uint offset, uint length, ref IntPtr ptr1, ref IntPtr ptr2, ref uint len1, ref uint len2);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_Unlock (IntPtr sound, IntPtr ptr1, IntPtr ptr2, uint len1, uint len2);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_SetDefaults (IntPtr sound, float frequency, float volume, float pan, int priority);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetDefaults (IntPtr sound, ref float frequency, ref float volume, ref float pan, ref int priority);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_SetVariations (IntPtr sound, float frequencyvar, float volumevar, float panvar);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetVariations (IntPtr sound, ref float frequencyvar, ref float volumevar, ref float panvar);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_Set3DMinMaxDistance (IntPtr sound, float min, float max);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_Get3DMinMaxDistance (IntPtr sound, ref float min, ref float max);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_Set3DConeSettings (IntPtr sound, float insideconeangle, float outsideconeangle, float outsidevolume);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_Get3DConeSettings (IntPtr sound, ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_Set3DCustomRolloff (IntPtr sound, ref Vector3 points, int numpoints);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_Get3DCustomRolloff (IntPtr sound, ref IntPtr points, ref int numpoints);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_SetSubSound (IntPtr sound, int index, IntPtr subsound);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetSubSound (IntPtr sound, int index, ref IntPtr subsound);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_SetSubSoundSentence (IntPtr sound, int[] subsoundlist, int numsubsounds);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetName (IntPtr sound, StringBuilder name, int namelen);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetFormat (IntPtr sound, ref FmodSharp.Sound.Type type, ref FmodSharp.Sound.Format format, ref int channels, ref int bits);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetNumSubSounds (IntPtr sound, ref int numsubsounds);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetNumTags (IntPtr sound, ref int numtags, ref int numtagsupdated);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetTag (IntPtr sound, string name, int index, ref TAG tag);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_ReadData (IntPtr sound, IntPtr buffer, uint lenbytes, ref uint read);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_SeekData (IntPtr sound, uint pcm);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_SetSoundGroup (IntPtr sound, IntPtr soundgroup);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetSoundGroup (IntPtr sound, ref IntPtr soundgroup);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetNumSyncPoints (IntPtr sound, ref int numsyncpoints);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetSyncPoint (IntPtr sound, int index, ref IntPtr point);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetSyncPointInfo (IntPtr sound, IntPtr point, StringBuilder name, int namelen, ref uint offset, TIMEUNIT offsettype);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_AddSyncPoint (IntPtr sound, uint offset, TimeUnit offsettype, string name, ref IntPtr point);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_DeleteSyncPoint (IntPtr sound, IntPtr point);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_SetLoopCount (IntPtr sound, int loopcount);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetLoopCount (IntPtr sound, ref int loopcount);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_SetLoopPoints (IntPtr sound, uint loopstart, TimeUnit loopstarttype, uint loopend, TimeUnit loopendtype);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetLoopPoints (IntPtr sound, ref uint loopstart, TimeUnit loopstarttype, ref uint loopend, TimeUnit loopendtype);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_SetUserData (IntPtr sound, IntPtr userdata);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetUserData (IntPtr sound, ref IntPtr userdata);

		[DllImport("fmodex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_Sound_GetMemoryInfo (IntPtr sound, uint memorybits, uint event_memorybits, ref uint memoryused, ref MEMORY_USAGE_DETAILS memoryused_details);
		
		*/
    }
}