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
using Linsft.FmodSharp.Error;

namespace Linsft.FmodSharp.SoundSystem
{
    public partial class SoundSystem
    {
        public IEnumerable<RecordDriver> RecordDrivers
        {
            get
            {
                int numb = NumberRecordDrivers;
                for (int i = 0; i < numb; i++)
                {
                    yield return GetRecordDriver(i);
                }
            }
        }

        private int NumberRecordDrivers
        {
            get
            {
                int numdrivers;

                Code returnCode = GetRecordNumDrivers(DangerousGetHandle(), out numdrivers);
                Errors.ThrowError(returnCode);

                return numdrivers;
            }
        }

        private void GetRecordDriverInfo(int id, out string name, out Guid driverGuid)
        {
            var str = new StringBuilder(255);

            Code returnCode = GetRecordDriverInfo(DangerousGetHandle(), id, str, str.Capacity, out driverGuid);
            Errors.ThrowError(returnCode);

            name = str.ToString();
        }

        private void GetRecordDriverCapabilities(int id, out Capabilities caps, out int minfrequency, out int maxfrequency)
        {
            Code returnCode = GetRecordDriverCaps(DangerousGetHandle(), id, out caps, out minfrequency, out maxfrequency);
            Errors.ThrowError(returnCode);
        }

        private RecordDriver GetRecordDriver(int id)
        {
            Guid driverGuid;
            string driverName;
            GetRecordDriverInfo(id, out driverName, out driverGuid);

            Capabilities caps;
            int minfrequency, maxfrequency;
            GetRecordDriverCapabilities(id, out caps, out minfrequency, out maxfrequency);

            return new RecordDriver
                {
                    Id = id,
                    Name = driverName,
                    Guid = driverGuid,
                    Capabilities = caps,
                    MinimumFrequency = minfrequency,
                    MaximumFrequency = maxfrequency,
                };
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetRecordNumDrivers"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetRecordNumDrivers(IntPtr system, out int numdrivers);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetRecordDriverInfo"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetRecordDriverInfo(IntPtr system, int id, StringBuilder name, int namelen, out Guid guid);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetRecordDriverInfoW"), SuppressUnmanagedCodeSecurity]
        private static extern Code FMOD_System_GetRecordDriverInfoW(IntPtr system, int id, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder name, int namelen, out Guid guid);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetRecordDriverCaps"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetRecordDriverCaps(IntPtr system, int id, out Capabilities caps, out int minfrequency, out int maxfrequency);
    }
}