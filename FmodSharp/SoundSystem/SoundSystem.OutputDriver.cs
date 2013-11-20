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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Linsft.FmodSharp.Enums;
using Linsft.FmodSharp.Error;

namespace Linsft.FmodSharp.SoundSystem
{
    public partial class SoundSystem
    {
        public OutputDriver OutputDriver
        {
            get
            {
                int driver;
                Code returnCode = GetDriver(DangerousGetHandle(), out driver);
                Errors.ThrowError(returnCode);

                return GetOutputDriver(driver);
            }
            set
            {
                Code returnCode = SetDriver(DangerousGetHandle(), value.Id);
                Errors.ThrowError(returnCode);
            }
        }

        public IEnumerable<OutputDriver> OutputDrivers
        {
            get
            {
                int numb = NumberOutputDrivers;
                for (int i = 0; i < numb; i++)
                {
                    yield return GetOutputDriver(i);
                }
            }
        }

        private int NumberOutputDrivers
        {
            get
            {
                int numdrivers;
                Code returnCode = GetNumDrivers(DangerousGetHandle(), out numdrivers);
                Errors.ThrowError(returnCode);

                return numdrivers;
            }
        }

        private void GetOutputDriverInfo(int id, out string name, out Guid driverGuid)
        {
            var str = new StringBuilder(255);

            Code returnCode = GetDriverInfo(DangerousGetHandle(), id, str, str.Capacity, out driverGuid);
            Errors.ThrowError(returnCode);

            name = str.ToString();
        }

        private void GetOutputDriverCapabilities(int id, out Capabilities caps, out int minfrequency, out int maxfrequency, out SpeakerMode controlpanelspeakermode)
        {
            Code returnCode = GetDriverCaps(DangerousGetHandle(), id, out caps, out minfrequency, out maxfrequency, out controlpanelspeakermode);
            Errors.ThrowError(returnCode);
        }

        private OutputDriver GetOutputDriver(int id)
        {
            Guid driverGuid;
            string driverName;
            GetOutputDriverInfo(id, out driverName, out driverGuid);

            Capabilities caps;
            int minfrequency, maxfrequency;
            SpeakerMode controlpanelspeakermode;
            GetOutputDriverCapabilities(id, out caps, out minfrequency, out maxfrequency, out controlpanelspeakermode);

            return new OutputDriver
                {
                    Id = id,
                    Name = driverName,
                    Guid = driverGuid,
                    Capabilities = caps,
                    MinimumFrequency = minfrequency,
                    MaximumFrequency = maxfrequency,
                    SpeakerMode = controlpanelspeakermode
                };
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetNumDrivers"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetNumDrivers(IntPtr system, out int numdrivers);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetDriverInfo"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetDriverInfo(IntPtr system, int id, StringBuilder name, int namelen, out Guid guid);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetDriverInfoW"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetDriverInfoW(IntPtr system, int id, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder name, int namelen, out Guid guid);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetDriverCaps"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetDriverCaps(IntPtr system, int id, out Capabilities caps, out int minfrequency, out int maxfrequency, out SpeakerMode controlpanelspeakermode);

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetDriver"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetDriver(IntPtr system, int driver);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetDriver"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetDriver(IntPtr system, out int driver);
    }
}