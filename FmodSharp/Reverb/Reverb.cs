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

namespace Linsft.FmodSharp.Reverb
{
    public class Reverb : Handle
    {
        #region Create/Release

        internal Reverb(IntPtr hnd)
        {
            SetHandle(hnd);
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return true;

            Release(handle);
            SetHandleAsInvalid();

            return true;
        }

        [DllImport("fmodex", EntryPoint = "FMOD_Reverb_Release"), SuppressUnmanagedCodeSecurity]
        private static extern Code Release(IntPtr reverb);

        #endregion

        public Properties Properties
        {
            get
            {
                Properties val = Properties.Generic;
                Code returnCode = GetProperties(DangerousGetHandle(), ref val);
                Errors.ThrowError(returnCode);

                return val;
            }

            set
            {
                Code returnCode = SetProperties(DangerousGetHandle(), ref value);
                Errors.ThrowError(returnCode);
            }
        }

        [DllImport("fmodex", EntryPoint = "FMOD_Reverb_SetProperties"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetProperties(IntPtr reverb, ref Properties properties);

        [DllImport("fmodex", EntryPoint = "FMOD_Reverb_GetProperties"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetProperties(IntPtr reverb, ref Properties properties);


        //TODO Implement extern funcitons

        /*

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_Reverb_Set3DAttributes (IntPtr reverb, ref VECTOR position, float mindistance, float maxdistance);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_Reverb_Get3DAttributes (IntPtr reverb, ref VECTOR position, ref float mindistance, ref float maxdistance);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_Reverb_SetActive (IntPtr reverb, int active);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_Reverb_GetActive (IntPtr reverb, ref int active);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_Reverb_SetUserData (IntPtr reverb, IntPtr userdata);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_Reverb_GetUserData (IntPtr reverb, ref IntPtr userdata);

		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
		private static extern RESULT FMOD_Reverb_GetMemoryInfo (IntPtr reverb, uint memorybits, uint event_memorybits, ref uint memoryused, ref MEMORY_USAGE_DETAILS memoryused_details);
		*/
    }
}