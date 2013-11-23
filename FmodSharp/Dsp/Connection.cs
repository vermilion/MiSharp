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
using Linsft.FmodSharp.Error;

namespace Linsft.FmodSharp.Dsp
{
    public class Connection : Handle
    {
        #region Create/Release

        internal Connection(IntPtr connPtr)
        {
            SetHandle(connPtr);
        }

        public float Mix
        {
            get
            {
                float val = 0;
                Code returnCode = GetMix(DangerousGetHandle(), ref val);
                Errors.ThrowError(returnCode);

                return val;
            }

            set
            {
                Code returnCode = SetMix(DangerousGetHandle(), value);
                Errors.ThrowError(returnCode);
            }
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return true;

            //TODO find if Connection need to be released before closing.
            //Release (this.handle);
            SetHandleAsInvalid();

            return true;
        }

        [DllImport("fmodex", EntryPoint = "FMOD_DSPConnection_SetMix"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetMix(IntPtr dspconnection, float volume);

        [DllImport("fmodex", EntryPoint = "FMOD_DSPConnection_GetMix"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetMix(IntPtr dspconnection, ref float volume);

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