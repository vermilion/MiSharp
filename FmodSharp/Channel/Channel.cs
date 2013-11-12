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

namespace Linsft.FmodSharp.Channel
{
	public class Channel : Handle, iSpectrumWave
	{
		
		#region Create/Release
		
		private Channel ()
		{
		}
		internal Channel (IntPtr hnd) : base()
		{
			this.SetHandle(hnd);
		}
		
		protected override bool ReleaseHandle ()
		{
			if (this.IsInvalid)
				return true;
			
			Stop(this.handle);
			
			//TODO find if Channel need to be released before closing.
			//Release (this.handle);
			this.SetHandleAsInvalid ();
			
			return true;
		}
		
		#endregion
		
		#region Properties
		
		public bool Paused {
			get {
				bool pause = false;
				
				Error.Code ReturnCode = GetPaused(this.DangerousGetHandle(), ref pause);
				Error.Errors.ThrowError(ReturnCode);
				
				return pause;
			}
			set {
				Error.Code ReturnCode = SetPaused(this.DangerousGetHandle(), value);
				Error.Errors.ThrowError(ReturnCode);
			}
		}
		
		public float Volume {
			get {
				float Vol = 0.0f;
				
				Error.Code ReturnCode = GetVolume(this.DangerousGetHandle(), ref Vol);
				Error.Errors.ThrowError(ReturnCode);
				
				return Vol;
			}
			set {
				Error.Code ReturnCode = SetVolume(this.DangerousGetHandle(), value);
				Error.Errors.ThrowError(ReturnCode);
			}
		}
		
		public float Frequency {
			get {
				float Freq = 0.0f;
				
				Error.Code ReturnCode = GetFrequency(this.DangerousGetHandle(), ref Freq);
				Error.Errors.ThrowError(ReturnCode);
				
				return Freq;
			}
			set {
				Error.Code ReturnCode = SetFrequency(this.DangerousGetHandle(), value);
				Error.Errors.ThrowError(ReturnCode);
			}
		}
		
		public float Pan {
			get {
				float pan = 0.0f;
				
				Error.Code ReturnCode = GetPan(this.DangerousGetHandle(), ref pan);
				Error.Errors.ThrowError(ReturnCode);
				
				return pan;
			}
			set {
				Error.Code ReturnCode = SetPan(this.DangerousGetHandle(), value);
				Error.Errors.ThrowError(ReturnCode);
			}
		}
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetPaused"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetPaused (IntPtr channel, bool paused);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetPaused"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetPaused (IntPtr channel, ref bool paused);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetVolume"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetVolume (IntPtr channel, float volume);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetVolume"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetVolume (IntPtr channel, ref float volume);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetFrequency"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetFrequency (IntPtr channel, float frequency);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetFrequency"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetFrequency (IntPtr channel, ref float frequency);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetPan"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetPan (IntPtr channel, float pan);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetPan"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetPan (IntPtr channel, ref float pan);
		
		#endregion
		
		#region PlayPauseStop	
		
		public bool IsPlaying {
			get {
				bool playing = false;
				
				Error.Code ReturnCode = IsPlaying_External(this.DangerousGetHandle(), ref playing);
				Error.Errors.ThrowError(ReturnCode);
				
				return playing;
			}
		}
		
		public void Stop()
		{
			Error.Code ReturnCode = Stop(this.DangerousGetHandle());
			Error.Errors.ThrowError(ReturnCode);
		}
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Stop"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Stop (IntPtr channel);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_IsPlaying"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code IsPlaying_External (IntPtr channel, ref bool isplaying);

		#endregion
		
		public bool Mute {
			get {
				bool Val = false;
				Error.Code ReturnCode = GetMute(this.DangerousGetHandle(), ref Val);
				Error.Errors.ThrowError(ReturnCode);
				
				return Val;
			}
			
			set {
				Error.Code ReturnCode = SetMute(this.DangerousGetHandle(), value);
				Error.Errors.ThrowError(ReturnCode);
			}
		}
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetMute"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetMute (IntPtr channel, bool mute);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetMute"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetMute (IntPtr channel, ref bool mute);
		
		#region Spectrum/Wave
		
		public float[] GetSpectrum (int numvalues, int channeloffset, Dsp.FFTWindow windowtype)
		{
			float[] SpectrumArray = new float[numvalues];
			this.GetSpectrum (SpectrumArray, numvalues, channeloffset, windowtype);
			return SpectrumArray;
		}
		
		public void GetSpectrum (float[] spectrumarray, int numvalues, int channeloffset, Dsp.FFTWindow windowtype)
		{
			GetSpectrum(this.DangerousGetHandle(), spectrumarray, numvalues, channeloffset, windowtype);
		}
		
		public float[] GetWaveData (int numvalues, int channeloffset)
		{
			float[] WaveArray = new float[numvalues];
			this.GetWaveData (WaveArray, numvalues, channeloffset);
			return WaveArray;
		}
		
		public void GetWaveData (float[] wavearray, int numvalues, int channeloffset)
		{
			GetWaveData(this.DangerousGetHandle(), wavearray, numvalues, channeloffset);
		}
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetSpectrum"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetSpectrum (IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, Dsp.FFTWindow windowtype);

		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetWaveData"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetWaveData (IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
		
		#endregion
		
		//TODO Implement extern funcitons
		
		/*
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetSystemObject"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetSystemObject (IntPtr channel, ref IntPtr system);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetDelay"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetDelay (IntPtr channel, DELAYTYPE delaytype, uint delayhi, uint delaylo);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetDelay"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetDelay (IntPtr channel, DELAYTYPE delaytype, ref uint delayhi, ref uint delaylo);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetSpeakerMix"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetSpeakerMix (IntPtr channel, float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetSpeakerMix"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetSpeakerMix (IntPtr channel, ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetSpeakerLevels"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetSpeakerLevels (IntPtr channel, SPEAKER speaker, float[] levels, int numlevels);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetSpeakerLevels"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetSpeakerLevels (IntPtr channel, SPEAKER speaker, [MarshalAs(UnmanagedType.LPArray)] float[] levels, int numlevels);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetInputChannelMix"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetInputChannelMix (IntPtr channel, float[] levels, int numlevels);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetInputChannelMix"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetInputChannelMix (IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] levels, int numlevels);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetPriority"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetPriority (IntPtr channel, int priority);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetPriority"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetPriority (IntPtr channel, ref int priority);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Set3DAttributes"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DAttributes (IntPtr channel, ref Vector3 pos, ref Vector3 vel);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Get3DAttributes"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DAttributes (IntPtr channel, ref Vector3 pos, ref Vector3 vel);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Set3DMinMaxDistance"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DMinMaxDistance (IntPtr channel, float mindistance, float maxdistance);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Get3DMinMaxDistance"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DMinMaxDistance (IntPtr channel, ref float mindistance, ref float maxdistance);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Set3DConeSettings"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DConeSettings (IntPtr channel, float insideconeangle, float outsideconeangle, float outsidevolume);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Get3DConeSettings"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DConeSettings (IntPtr channel, ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Set3DConeOrientation"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DConeOrientation (IntPtr channel, ref Vector3 orientation);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Get3DConeOrientation"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DConeOrientation (IntPtr channel, ref Vector3 orientation);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Set3DCustomRolloff"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DCustomRolloff (IntPtr channel, ref Vector3 points, int numpoints);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Get3DCustomRolloff"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DCustomRolloff (IntPtr channel, ref IntPtr points, ref int numpoints);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Set3Docclusion"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3Docclusion (IntPtr channel, float directocclusion, float reverbocclusion);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Get3Docclusion"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3Docclusion (IntPtr channel, ref float directocclusion, ref float reverbocclusion);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Set3DOcclusion"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DOcclusion (IntPtr channel, float directocclusion, float reverbocclusion);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Get3DOcclusion"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DOcclusion (IntPtr channel, ref float directocclusion, ref float reverbocclusion);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Set3DSpread"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DSpread (IntPtr channel, float angle);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Get3DSpread"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DSpread (IntPtr channel, ref float angle);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Set3DPanLevel"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DPanLevel (IntPtr channel, float level);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Get3DPanLevel"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DPanLevel (IntPtr channel, ref float level);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Set3DDopplerLevel"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DDopplerLevel (IntPtr channel, float level);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_Get3DDopplerLevel"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DDopplerLevel (IntPtr channel, ref float level);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetReverbProperties"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetReverbProperties (IntPtr channel, ref REVERB_CHANNELPROPERTIES prop);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetReverbProperties"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetReverbProperties (IntPtr channel, ref REVERB_CHANNELPROPERTIES prop);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetLowPassGain"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetLowPassGain (IntPtr channel, float gain);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetLowPassGain"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetLowPassGain (IntPtr channel, ref float gain);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetChannelGroup"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetChannelGroup (IntPtr channel, IntPtr channelgroup);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetChannelGroup"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetChannelGroup (IntPtr channel, ref IntPtr channelgroup);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_IsVirtual"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code IsVirtual (IntPtr channel, ref int isvirtual);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetAudibility"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetAudibility (IntPtr channel, ref float audibility);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetCurrentSound"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetCurrentSound (IntPtr channel, ref IntPtr sound);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetIndex"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetIndex (IntPtr channel, ref int index);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetCallback"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetCallback (IntPtr channel, CHANNEL_CALLBACK callback);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetPosition"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetPosition (IntPtr channel, uint position, TimeUnit postype);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetPosition"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetPosition (IntPtr channel, ref uint position, TimeUnit postype);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetDSPHead"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetDSPHead (IntPtr channel, ref IntPtr dsp);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_AddDSP"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code AddDSP (IntPtr channel, IntPtr dsp, ref IntPtr connection);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetMode"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetMode (IntPtr channel, Mode mode);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetMode"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetMode (IntPtr channel, ref Mode mode);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetLoopCount"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetLoopCount (IntPtr channel, int loopcount);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetLoopCount"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetLoopCount (IntPtr channel, ref int loopcount);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetLoopPoints"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetLoopPoints (IntPtr channel, uint loopstart, TimeUnit loopstarttype, uint loopend, TimeUnit loopendtype);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetLoopPoints"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetLoopPoints (IntPtr channel, ref uint loopstart, TimeUnit loopstarttype, ref uint loopend, TimeUnit loopendtype);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_SetUserData"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetUserData (IntPtr channel, IntPtr userdata);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetUserData"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetUserData (IntPtr channel, ref IntPtr userdata);
		
		[DllImport("fmodex", EntryPoint = "FMOD_Channel_GetMemoryInfo"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetMemoryInfo (IntPtr channel, uint memorybits, uint event_memorybits, ref uint memoryused, ref MEMORY_USAGE_DETAILS memoryused_details);
		
		*/
	}
}
