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
using Linsft.FmodSharp.Channel;
using Linsft.FmodSharp.Dsp;
using Linsft.FmodSharp.Error;
using Type = Linsft.FmodSharp.Dsp.Type;

namespace Linsft.FmodSharp.SoundSystem
{
    public partial class SoundSystem
    {
        public ulong DSPClock
        {
            get
            {
                uint hi = 0, low = 0;
                Code returnCode = GetDSPClock(DangerousGetHandle(), ref hi, ref low);
                Errors.ThrowError(returnCode);
                return (hi << 32) | low;
            }
        }

        public Dsp.Dsp CreateDSP(ref Description description)
        {
            IntPtr dspHandle = IntPtr.Zero;

            Code returnCode = CreateDSP(DangerousGetHandle(), ref description, ref dspHandle);
            Errors.ThrowError(returnCode);

            return new Dsp.Dsp(dspHandle);
        }

        public Dsp.Dsp CreateDspByType(Type type)
        {
            IntPtr dspHandle = IntPtr.Zero;

            Code returnCode = CreateDspByType(DangerousGetHandle(), type, ref dspHandle);
            Errors.ThrowError(returnCode);

            return new Dsp.Dsp(dspHandle);
        }

        public Channel.Channel PlayDsp(Dsp.Dsp dsp)
        {
            return PlayDsp(dsp, false);
        }

        public Channel.Channel PlayDsp(Dsp.Dsp dsp, bool paused)
        {
            IntPtr channelHandle = IntPtr.Zero;

            Code returnCode = PlayDsp(DangerousGetHandle(), Index.Free, dsp.DangerousGetHandle(), paused, ref channelHandle);
            Errors.ThrowError(returnCode);

            return new Channel.Channel(channelHandle);
        }

        public void PlayDsp(Dsp.Dsp dsp, bool paused, Channel.Channel chn)
        {
            IntPtr channel = chn.DangerousGetHandle();

            Code returnCode = PlayDsp(DangerousGetHandle(), Index.Reuse, dsp.DangerousGetHandle(), paused, ref channel);
            Errors.ThrowError(returnCode);

            //This can't really happend.
            //Check just in case...
            if (chn.DangerousGetHandle() != channel)
                throw new Exception("Channel handle got changed by Fmod.");
        }

        public Connection AddDsp(Dsp.Dsp dsp)
        {
            IntPtr connectionHandle = IntPtr.Zero;

            Code returnCode = AddDSP(DangerousGetHandle(), dsp.DangerousGetHandle(), ref connectionHandle);
            Errors.ThrowError(returnCode);

            return new Connection(connectionHandle);
        }

        public void LockDSP()
        {
            Code returnCode = LockDSP(DangerousGetHandle());
            Errors.ThrowError(returnCode);
        }

        public void UnlockDSP()
        {
            Code returnCode = UnlockDSP(DangerousGetHandle());
            Errors.ThrowError(returnCode);
        }

        [SuppressUnmanagedCodeSecurity]
        [DllImport("fmodex", EntryPoint = "FMOD_System_CreateDSP")]
        private static extern Code CreateDSP(IntPtr system, ref Description description, ref IntPtr dsp);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("fmodex", EntryPoint = "FMOD_System_CreateDSPByType")]
        private static extern Code CreateDspByType(IntPtr system, Type type, ref IntPtr dsp);

        [DllImport("fmodex", EntryPoint = "FMOD_System_PlayDSP"), SuppressUnmanagedCodeSecurity]
        private static extern Code PlayDsp(IntPtr system, Index channelid, IntPtr dsp, bool paused, ref IntPtr channel);

        [DllImport("fmodex", EntryPoint = "FMOD_System_AddDSP"), SuppressUnmanagedCodeSecurity]
        private static extern Code AddDSP(IntPtr system, IntPtr dsp, ref IntPtr connection);

        [DllImport("fmodex", EntryPoint = "FMOD_System_LockDSP"), SuppressUnmanagedCodeSecurity]
        private static extern Code LockDSP(IntPtr system);

        [DllImport("fmodex", EntryPoint = "FMOD_System_UnlockDSP"), SuppressUnmanagedCodeSecurity]
        private static extern Code UnlockDSP(IntPtr system);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetDSPClock"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetDSPClock(IntPtr system, ref uint hi, ref uint lo);
    }
}