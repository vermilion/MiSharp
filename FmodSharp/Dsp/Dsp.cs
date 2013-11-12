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

namespace Linsft.FmodSharp.Dsp
{
	public class Dsp : Handle
	{
		internal Dsp (IntPtr hnd) : base()
		{
			this.SetHandle (hnd);
		}

		protected override bool ReleaseHandle ()
		{
			if (this.IsInvalid)
				return true;
			
			this.Remove ();
			Release (this.handle);
			this.SetHandleAsInvalid ();
			
			return true;
		}
		
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport ("fmodex", EntryPoint = "FMOD_DSP_Release")]
		private static extern Error.Code Release (IntPtr dsp);
		
		public void Remove()
		{
			Error.Code ReturnCode = Remove_External(this.DangerousGetHandle());
			Error.Errors.ThrowError (ReturnCode);
		}
		
		public void Reset()
		{
			Error.Code ReturnCode = Reset_External(this.DangerousGetHandle());
			Error.Errors.ThrowError (ReturnCode);
		}
		
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport ("fmodex", EntryPoint = "FMOD_DSP_Remove")]
        private static extern Error.Code Remove_External (IntPtr dsp);
		
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport ("fmodex", EntryPoint = "FMOD_DSP_Reset")]
        private static extern Error.Code Reset_External (IntPtr dsp);
		
		//TODO Implement extern funcitons
		/*

		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetSystemObject           (IntPtr dsp, ref IntPtr system);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]                   
        private static extern RESULT FMOD_DSP_AddInput                  (IntPtr dsp, IntPtr target, ref IntPtr connection);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_DisconnectFrom            (IntPtr dsp, IntPtr target);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_DisconnectAll             (IntPtr dsp, int inputs, int outputs);
        
      	[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetNumInputs              (IntPtr dsp, ref int numinputs);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetNumOutputs             (IntPtr dsp, ref int numoutputs);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetInput                  (IntPtr dsp, int index, ref IntPtr input, ref IntPtr inputconnection);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetOutput                 (IntPtr dsp, int index, ref IntPtr output, ref IntPtr outputconnection);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_SetActive                 (IntPtr dsp, int active);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetActive                 (IntPtr dsp, ref int active);    
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_SetBypass                 (IntPtr dsp, int bypass);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetBypass                 (IntPtr dsp, ref int bypass);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_SetSpeakerActive          (IntPtr dsp, SPEAKER speaker, int active);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetSpeakerActive          (IntPtr dsp, SPEAKER speaker, ref int active);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_SetParameter              (IntPtr dsp, int index, float value);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetParameter              (IntPtr dsp, int index, ref float value, StringBuilder valuestr, int valuestrlen);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetNumParameters          (IntPtr dsp, ref int numparams);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetParameterInfo          (IntPtr dsp, int index, ref IntPtr name, ref IntPtr label, StringBuilder description, int descriptionlen, ref float min, ref float max);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_ShowConfigDialog          (IntPtr dsp, IntPtr hwnd, int show);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetInfo                   (IntPtr dsp, ref IntPtr name, ref uint version, ref int channels, ref int configwidth, ref int configheight);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetType                   (IntPtr dsp, ref DSP_TYPE type);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_SetDefaults               (IntPtr dsp, float frequency, float volume, float pan, int priority);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetDefaults               (IntPtr dsp, ref float frequency, ref float volume, ref float pan, ref int priority);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]                   
        private static extern RESULT FMOD_DSP_SetUserData               (IntPtr dsp, IntPtr userdata);
        
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetUserData               (IntPtr dsp, ref IntPtr userdata);
        
		[DllImport(VERSION.dll), SuppressUnmanagedCodeSecurity]
        private static extern RESULT FMOD_DSP_GetMemoryInfo             (IntPtr dsp, uint memorybits, uint event_memorybits, ref uint memoryused, ref MEMORY_USAGE_DETAILS memoryused_details);
		*/
	}
}
