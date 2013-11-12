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
using System.Collections.Generic;
using System.Security;

namespace Linsft.FmodSharp.SoundSystem
{
	public partial class SoundSystem
	{
		public IEnumerable<RecordDriver> RecordDrivers {
			get {
				int Numb = this.NumberRecordDrivers;
				for (int i = 0; i < Numb; i++) {
					yield return this.GetRecordDriver(i);
				}
			}
		}
		
		private int NumberRecordDrivers {
			get {
				int numdrivers;
				
				Error.Code ReturnCode = GetRecordNumDrivers (this.DangerousGetHandle (), out numdrivers);
				Error.Errors.ThrowError (ReturnCode);
				
				return numdrivers;
			}
		}
		
		private void GetRecordDriverInfo(int Id, out string Name, out Guid DriverGuid)
		{
			System.Text.StringBuilder str = new System.Text.StringBuilder(255);
			
			Error.Code ReturnCode = GetRecordDriverInfo (this.DangerousGetHandle (), Id, str, str.Capacity, out DriverGuid);
			Error.Errors.ThrowError (ReturnCode);
			
			Name = str.ToString();
		}
		
		private void GetRecordDriverCapabilities (int id, out Capabilities caps, out int minfrequency, out int maxfrequency)
		{
			Error.Code ReturnCode = GetRecordDriverCaps (this.DangerousGetHandle (), id, out caps, out minfrequency, out maxfrequency);
			Error.Errors.ThrowError (ReturnCode);
		}
		
		private RecordDriver GetRecordDriver (int Id)
		{
			Guid DriverGuid;
			string DriverName;
			this.GetRecordDriverInfo(Id, out DriverName, out DriverGuid);
			
			Capabilities caps;
			int minfrequency, maxfrequency;
			this.GetRecordDriverCapabilities(Id, out caps, out minfrequency, out maxfrequency);
			
			return new RecordDriver {
				Id = Id,
				Name = DriverName,
				Guid = DriverGuid,
				
				Capabilities = caps,
				MinimumFrequency = minfrequency,
				MaximumFrequency = maxfrequency,
			};
		}
		
		[DllImport("fmodex", EntryPoint = "FMOD_System_GetRecordNumDrivers"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetRecordNumDrivers (IntPtr system, out int numdrivers);
		
		[DllImport("fmodex", EntryPoint = "FMOD_System_GetRecordDriverInfo"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetRecordDriverInfo (IntPtr system, int id, System.Text.StringBuilder name, int namelen, out Guid guid);
		
		[DllImport("fmodex", EntryPoint = "FMOD_System_GetRecordDriverInfoW"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code FMOD_System_GetRecordDriverInfoW (IntPtr system, int id, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder name, int namelen, out Guid guid);
		
		[DllImport("fmodex", EntryPoint = "FMOD_System_GetRecordDriverCaps"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetRecordDriverCaps (IntPtr system, int id, out Capabilities caps, out int minfrequency, out int maxfrequency);
		
	}
}

