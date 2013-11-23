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
using Linsft.FmodSharp.Error;
using Linsft.FmodSharp.Reverb;

namespace Linsft.FmodSharp.SoundSystem
{
    public partial class SoundSystem
    {
        public Properties ReverbProperties
        {
            get
            {
                Properties val = Properties.Generic;
                Code returnCode = GetReverbProperties(DangerousGetHandle(), ref val);
                Errors.ThrowError(returnCode);

                return val;
            }

            set
            {
                Code returnCode = SetReverbProperties(DangerousGetHandle(), ref value);
                Errors.ThrowError(returnCode);
            }
        }

        public Properties ReverbAmbientProperties
        {
            get
            {
                Properties val = Properties.Generic;

                Code returnCode = GetReverbAmbientProperties(DangerousGetHandle(), ref val);
                Errors.ThrowError(returnCode);

                return val;
            }

            set
            {
                Code returnCode = SetReverbAmbientProperties(DangerousGetHandle(), ref value);
                Errors.ThrowError(returnCode);
            }
        }

        public Reverb.Reverb CreateReverb()
        {
            IntPtr reverbHandle = IntPtr.Zero;

            Code returnCode = CreateReverb(DangerousGetHandle(), ref reverbHandle);
            Errors.ThrowError(returnCode);

            return new Reverb.Reverb(reverbHandle);
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_CreateReverb"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateReverb(IntPtr system, ref IntPtr reverb);

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetReverbProperties"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetReverbProperties(IntPtr system, ref Properties prop);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetReverbProperties"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetReverbProperties(IntPtr system, ref Properties prop);

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetReverbAmbientProperties"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetReverbAmbientProperties(IntPtr system, ref Properties prop);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetReverbAmbientProperties"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetReverbAmbientProperties(IntPtr system, ref Properties prop);
    }
}