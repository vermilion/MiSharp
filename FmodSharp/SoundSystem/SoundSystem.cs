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
using System.Text;
using Linsft.FmodSharp.Channel;
using Linsft.FmodSharp.Dsp;
using Linsft.FmodSharp.Enums;
using Linsft.FmodSharp.Error;
using Linsft.FmodSharp.Sound;

namespace Linsft.FmodSharp.SoundSystem
{
    public partial class SoundSystem : Handle, IISpectrumWave
    {
        /// <summary>
        ///     Used to check against <see cref="FmodSharp.System.Version" /> FMOD::System::getVersion.
        /// </summary>
        public const uint FmodVersion = 0x43202;

        #region Create/Release

        public SoundSystem()
        {
            IntPtr soundSystemHandle = IntPtr.Zero;

            Code returnCode = Create(ref soundSystemHandle);
            Errors.ThrowError(returnCode);

            SetHandle(soundSystemHandle);

            if (Version < FmodVersion)
            {
                Release(handle);
                SetHandleAsInvalid();
                throw new NotSupportedException("The current version of Fmod isnt supported.");
            }
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return true;

            CloseSystem(handle);
            Release(handle);
            SetHandleAsInvalid();

            return true;
        }

        public void Init()
        {
            Init(32, InitFlags.Normal | InitFlags.RightHanded3D);
        }

        public void Init(int maxchannels, InitFlags flags)
        {
            Init(maxchannels, flags, IntPtr.Zero);
        }

        public void Init(int maxchannels, InitFlags flags, IntPtr extradriverdata)
        {
            Code returnCode = Init(handle, maxchannels, flags, extradriverdata);
            Errors.ThrowError(returnCode);
        }

        public void CloseSystem()
        {
            CloseSystem(DangerousGetHandle());
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_Create"), SuppressUnmanagedCodeSecurity]
        private static extern Code Create(ref IntPtr system);

        [DllImport("fmodex", EntryPoint = "FMOD_System_Release"), SuppressUnmanagedCodeSecurity]
        private static extern Code Release(IntPtr system);

        [DllImport("fmodex", EntryPoint = "FMOD_System_Init"), SuppressUnmanagedCodeSecurity]
        private static extern Code Init(IntPtr system, int maxchannels, InitFlags flags, IntPtr extradriverdata);

        [DllImport("fmodex", EntryPoint = "FMOD_System_Close"), SuppressUnmanagedCodeSecurity]
        private static extern Code CloseSystem(IntPtr system);

        #endregion

        #region Events

        //TODO Implement SoundSystem Events.

        public delegate void SystemDelegate(SoundSystem sys);

        /// <summary>
        ///     Called when the enumerated list of devices has changed.
        /// </summary>
        public event SystemDelegate DeviceListChanged;

        /// <summary>
        ///     Called from System::update when an output device has been lost
        ///     due to control panel parameter changes and FMOD cannot automatically recover.
        /// </summary>
        public event SystemDelegate DeviceLost;

        /// <summary>
        ///     Called directly when a memory allocation fails somewhere in FMOD.
        /// </summary>
        public event SystemDelegate MemoryAllocationFailed;

        /// <summary>
        ///     Called directly when a thread is created.
        /// </summary>
        public event SystemDelegate ThreadCreated;

        /// <summary>
        ///     Called when a bad connection was made with DSP::addInput.
        ///     Usually called from mixer thread because that is where the connections are made.
        /// </summary>
        public event SystemDelegate BadDspConnection;

        /// <summary>
        ///     Called when too many effects were added exceeding the maximum tree depth of 128.
        ///     This is most likely caused by accidentally adding too many DSP effects.
        ///     Usually called from mixer thread because that is where the connections are made.
        /// </summary>
        public event SystemDelegate BadDspLevel;

        private Code HandleCallback(IntPtr systemraw, CallbackType type, IntPtr commanddata1, IntPtr commanddata2)
        {
            return Code.OK;
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetCallback"), SuppressUnmanagedCodeSecurity]
        private static extern Code FMOD_System_SetCallback(IntPtr system, SystemDelegate callback);

        #endregion

        #region Network

        public string NetworkProxy
        {
            get
            {
                var str = new StringBuilder(255);

                Code returnCode = GetNetworkProxy(DangerousGetHandle(), str, str.Capacity);
                Errors.ThrowError(returnCode);

                return str.ToString();
            }
            set
            {
                Code returnCode = SetNetworkProxy(DangerousGetHandle(), value);
                Errors.ThrowError(returnCode);
            }
        }

        public int NetworkTimeout
        {
            get
            {
                int time = 0;

                Code returnCode = GetNetworkTimeout(DangerousGetHandle(), ref time);
                Errors.ThrowError(returnCode);

                return time;
            }
            set
            {
                Code returnCode = SetNetworkTimeout(DangerousGetHandle(), value);
                Errors.ThrowError(returnCode);
            }
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetNetworkProxy"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetNetworkProxy(IntPtr system, string proxy);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetNetworkProxy"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetNetworkProxy(IntPtr system, StringBuilder proxy, int proxylen);

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetNetworkTimeout"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetNetworkTimeout(IntPtr system, int timeout);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetNetworkTimeout"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetNetworkTimeout(IntPtr system, ref int timeout);

        #endregion

        #region Others

        public uint Version
        {
            get
            {
                uint ver = 0;

                Code returnCode = GetVersion(DangerousGetHandle(), ref ver);
                Errors.ThrowError(returnCode);

                return ver;
            }
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetVersion"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetVersion(IntPtr system, ref uint version);

        #endregion

        #region Output

        public OutputType Output
        {
            get
            {
                var output = OutputType.Unknown;

                Code returnCode = GetOutput(DangerousGetHandle(), ref output);
                Errors.ThrowError(returnCode);

                return output;
            }
            set
            {
                Code returnCode = SetOutput(DangerousGetHandle(), value);
                Errors.ThrowError(returnCode);
            }
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetOutput"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetOutput(IntPtr system, OutputType output);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetOutput"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetOutput(IntPtr system, ref OutputType output);

        #region Channels

        public int ChannelsPlaying
        {
            get
            {
                int playing = 0;

                Code returnCode = GetChannelsPlaying(DangerousGetHandle(), ref playing);
                Errors.ThrowError(returnCode);

                return playing;
            }
        }

        public bool IsPlaying
        {
            get { return ChannelsPlaying <= 0; }
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetChannelsPlaying"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetChannelsPlaying(IntPtr system, ref int channels);

        #endregion

        #region Group

        [DllImport("fmodex", CharSet = CharSet.Ansi, EntryPoint = "FMOD_System_CreateChannelGroup"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateChannelGroup(IntPtr system, string name, ref IntPtr channelgroup);

        [DllImport("fmodex", CharSet = CharSet.Ansi, EntryPoint = "FMOD_System_CreateSoundGroup"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateSoundGroup(IntPtr system, string name, ref IntPtr soundgroup);

        #endregion

        #region Sound

        public Sound.Sound CreateSound(string path)
        {
            return CreateSound(path, Mode.Default | Mode.CreateStream);
        }

        public Sound.Sound CreateSound(string path, Mode mode)
        {
            IntPtr soundHandle = IntPtr.Zero;

            Code returnCode = CreateSound(DangerousGetHandle(), path, mode, 0, ref soundHandle);
            Errors.ThrowError(returnCode);

            return new Sound.Sound(soundHandle);
        }

        public Sound.Sound CreateSound(string path, Mode mode, Info exinfo)
        {
            IntPtr soundHandle = IntPtr.Zero;

            Code returnCode = CreateSound(DangerousGetHandle(), path, mode, ref exinfo, ref soundHandle);
            Errors.ThrowError(returnCode);

            return new Sound.Sound(soundHandle);
        }

        public Sound.Sound CreateSound(byte[] data)
        {
            return CreateSound(data, Mode.Default);
        }

        public Sound.Sound CreateSound(byte[] data, Mode mode)
        {
            IntPtr soundHandle = IntPtr.Zero;

            Code returnCode = CreateSound(DangerousGetHandle(), data, mode, 0, ref soundHandle);
            Errors.ThrowError(returnCode);

            return new Sound.Sound(soundHandle);
        }

        public Sound.Sound CreateSound(byte[] data, Mode mode, Info exinfo)
        {
            IntPtr soundHandle = IntPtr.Zero;

            Code returnCode = CreateSound(DangerousGetHandle(), data, mode, ref exinfo, ref soundHandle);
            Errors.ThrowError(returnCode);

            return new Sound.Sound(soundHandle);
        }

        public Channel.Channel PlaySound(Sound.Sound snd)
        {
            return PlaySound(snd, false);
        }

        public Channel.Channel PlaySound(Sound.Sound snd, bool paused)
        {
            IntPtr channelHandle = IntPtr.Zero;

            Code returnCode = PlaySound(DangerousGetHandle(), Index.Free, snd.DangerousGetHandle(), paused, ref channelHandle);
            Errors.ThrowError(returnCode);

            return new Channel.Channel(channelHandle);
        }

        private void PlaySound(Sound.Sound snd, bool paused, Channel.Channel chn)
        {
            //FIXME The handle is changed most of the time on some system.
            //Only use the other metods to be safe.

            IntPtr channel = chn.DangerousGetHandle();

            Code returnCode = PlaySound(DangerousGetHandle(), Index.Reuse, snd.DangerousGetHandle(), paused, ref channel);
            Errors.ThrowError(returnCode);

            //This can't really happend.
            //Check just in case...
            if (chn.DangerousGetHandle() == channel)
                throw new Exception("Channel handle got changed by Fmod.");
        }

        [DllImport("fmodex", CharSet = CharSet.Ansi, EntryPoint = "FMOD_System_CreateSound"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateSound(IntPtr system, string name, Mode mode, ref Info exinfo, ref IntPtr Sound);

        [DllImport("fmodex", CharSet = CharSet.Ansi, EntryPoint = "FMOD_System_CreateSound"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateSound(IntPtr system, string name, Mode mode, int exinfo, ref IntPtr sound);

        [DllImport("fmodex", EntryPoint = "FMOD_System_CreateSound"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateSound(IntPtr system, byte[] data, Mode mode, ref Info exinfo, ref IntPtr sound);

        [DllImport("fmodex", EntryPoint = "FMOD_System_CreateSound"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateSound(IntPtr system, byte[] data, Mode mode, int exinfo, ref IntPtr sound);

        [DllImport("fmodex", EntryPoint = "FMOD_System_PlaySound"), SuppressUnmanagedCodeSecurity]
        private static extern Code PlaySound(IntPtr system, Index channelid, IntPtr Sound, bool paused, ref IntPtr channel);

        #endregion

        #region Stream

        public Sound.Sound CreateStream(string path, Mode mode)
        {
            IntPtr soundHandle = IntPtr.Zero;

            Code returnCode = CreateStream(DangerousGetHandle(), path, mode, 0, ref soundHandle);
            Errors.ThrowError(returnCode);

            return new Sound.Sound(soundHandle);
        }

        public Sound.Sound CreateStream(string path, Mode mode, Info exinfo)
        {
            IntPtr soundHandle = IntPtr.Zero;

            Code returnCode = CreateStream(DangerousGetHandle(), path, mode, ref exinfo, ref soundHandle);
            Errors.ThrowError(returnCode);

            return new Sound.Sound(soundHandle);
        }

        public Sound.Sound CreateStream(byte[] data, Mode mode)
        {
            IntPtr soundHandle = IntPtr.Zero;

            Code returnCode = CreateStream(DangerousGetHandle(), data, mode, 0, ref soundHandle);
            Errors.ThrowError(returnCode);

            return new Sound.Sound(soundHandle);
        }

        public Sound.Sound CreateStream(byte[] data, Mode mode, Info exinfo)
        {
            IntPtr soundHandle = IntPtr.Zero;

            Code returnCode = CreateStream(DangerousGetHandle(), data, mode, ref exinfo, ref soundHandle);
            Errors.ThrowError(returnCode);

            return new Sound.Sound(soundHandle);
        }

        [DllImport("fmodex", CharSet = CharSet.Ansi, EntryPoint = "FMOD_System_CreateStream"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateStream(IntPtr system, string name, Mode mode, ref Info exinfo, ref IntPtr sound);

        [DllImport("fmodex", CharSet = CharSet.Ansi, EntryPoint = "FMOD_System_CreateStream"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateStream(IntPtr system, string name, Mode mode, int exinfo, ref IntPtr sound);

        [DllImport("fmodex", EntryPoint = "FMOD_System_CreateStream"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateStream(IntPtr system, byte[] data, Mode mode, ref Info exinfo, ref IntPtr sound);

        [DllImport("fmodex", EntryPoint = "FMOD_System_CreateStream"), SuppressUnmanagedCodeSecurity]
        private static extern Code CreateStream(IntPtr system, byte[] data, Mode mode, int exinfo, ref IntPtr sound);

        #endregion

        #endregion

        #region Recording

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetRecordPosition"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetRecordPosition(IntPtr system, int id, ref uint position);

        [DllImport("fmodex", EntryPoint = "FMOD_System_RecordStart"), SuppressUnmanagedCodeSecurity]
        private static extern Code RecordStart(IntPtr system, int id, IntPtr sound, int loop);

        [DllImport("fmodex", EntryPoint = "FMOD_System_RecordStop"), SuppressUnmanagedCodeSecurity]
        private static extern Code RecordStop(IntPtr system, int id);

        [DllImport("fmodex", EntryPoint = "FMOD_System_IsRecording"), SuppressUnmanagedCodeSecurity]
        private static extern Code IsRecording(IntPtr system, int id, ref int recording);

        #endregion

        #region Spectrum/Wave

        public float[] GetSpectrum(int numvalues, int channeloffset, FFTWindow windowtype)
        {
            var spectrumArray = new float[numvalues];
            GetSpectrum(spectrumArray, numvalues, channeloffset, windowtype);
            return spectrumArray;
        }

        public void GetSpectrum(float[] spectrumarray, int numvalues, int channeloffset, FFTWindow windowtype)
        {
            GetSpectrum(DangerousGetHandle(), spectrumarray, numvalues, channeloffset, windowtype);
        }

        public float[] GetWaveData(int numvalues, int channeloffset)
        {
            var waveArray = new float[numvalues];
            GetWaveData(waveArray, numvalues, channeloffset);
            return waveArray;
        }

        public void GetWaveData(float[] wavearray, int numvalues, int channeloffset)
        {
            GetWaveData(DangerousGetHandle(), wavearray, numvalues, channeloffset);
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetSpectrum"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetSpectrum(IntPtr system, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, FFTWindow windowtype);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetWaveData"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetWaveData(IntPtr system, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);

        #endregion

        #region Update

        public void Update()
        {
            Update(DangerousGetHandle());
        }

        public void UpdateFinished()
        {
            UpdateFinished(DangerousGetHandle());
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_Update"), SuppressUnmanagedCodeSecurity]
        private static extern Code Update(IntPtr system);

        [DllImport("fmodex", EntryPoint = "FMOD_System_UpdateFinished"), SuppressUnmanagedCodeSecurity]
        private static extern Code UpdateFinished(IntPtr system);

        #endregion

        #region Speaker mode

        public SpeakerMode SpeakerMode
        {
            get
            {
                var speakerMode = SpeakerMode.Max;

                Code returnCode = GetSpeakerMode(DangerousGetHandle(), ref speakerMode);
                Errors.ThrowError(returnCode);

                return speakerMode;
            }
            set
            {
                Code returnCode = SetSpeakerMode(DangerousGetHandle(), value);
                Errors.ThrowError(returnCode);
            }
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetSpeakerMode"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetSpeakerMode(IntPtr system, SpeakerMode speakermode);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetSpeakerMode"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetSpeakerMode(IntPtr system, ref SpeakerMode speakermode);

        #endregion

        #region Buffer

        public int FileBufferSize
        {
            get
            {
                var sizebytes = 0;
                Code returnCode = GetFileBufferSize(DangerousGetHandle(), ref sizebytes);
                Errors.ThrowError(returnCode);

                return sizebytes;
            }
            set
            {
                Code returnCode = SetFileBufferSize(DangerousGetHandle(), value);
                Errors.ThrowError(returnCode);
            }
        }

        public void SetStreamBufferSize(uint filebuffersize, TimeUnit filebuffersizetype)
        {
            Code returnCode = SetStreamBufferSize(DangerousGetHandle(), filebuffersize, filebuffersizetype);
            Errors.ThrowError(returnCode);
        }

        public void GetStreamBufferSize(ref uint filebuffersize, ref TimeUnit filebuffersizetype)
        {
            Code returnCode = GetStreamBufferSize(DangerousGetHandle(), ref filebuffersize, ref filebuffersizetype);
            Errors.ThrowError(returnCode);
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetFileBufferSize"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetFileBufferSize(IntPtr system, int sizebytes);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetFileBufferSize"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetFileBufferSize(IntPtr system, ref int sizebytes);

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetStreamBufferSize"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetStreamBufferSize(IntPtr system, uint filebuffersize, TimeUnit filebuffersizetype);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetStreamBufferSize"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetStreamBufferSize(IntPtr system, ref uint filebuffersize, ref TimeUnit filebuffersizetype);

        #endregion

        #region Software Format

        public void GetSoftwareFormat(ref int samplerate, ref Format format, ref int numoutputchannels,
            ref int maxinputchannels, ref Resampler resamplemethod, ref int bits)
        {
            Code returnCode = GetSoftwareFormat(DangerousGetHandle(), ref samplerate, ref format, ref numoutputchannels,
                ref maxinputchannels, ref resamplemethod, ref bits);
            Errors.ThrowError(returnCode);
        }

        public void SetSoftwareFormat(int samplerate, Format format, int numoutputchannels, int maxinputchannels,
            Resampler resamplemethod)
        {
            Code returnCode = SetSoftwareFormat(DangerousGetHandle(), samplerate, format, numoutputchannels,
                maxinputchannels, resamplemethod);
            Errors.ThrowError(returnCode);
        }

        [DllImport("fmodex", EntryPoint = "FMOD_System_SetSoftwareFormat"), SuppressUnmanagedCodeSecurity]
        private static extern Code SetSoftwareFormat(IntPtr system, int samplerate, Format format, int numoutputchannels, int maxinputchannels, Resampler resamplemethod);

        [DllImport("fmodex", EntryPoint = "FMOD_System_GetSoftwareFormat"), SuppressUnmanagedCodeSecurity]
        private static extern Code GetSoftwareFormat(IntPtr system, ref int samplerate, ref Format format, ref int numoutputchannels, ref int maxinputchannels, ref Resampler resamplemethod, ref int bits);

        #endregion
        //TODO Implement extern funcitons

        /*
		
		[DllImport("fmodex", EntryPoint = "FMOD_System_GetDSPHead"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetDSPHead (IntPtr system, ref IntPtr dsp);

		[DllImport("fmodex", EntryPoint = "FMOD_System_SetHardwareChannels"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetHardwareChannels (IntPtr system, int min2d, int max2d, int min3d, int max3d);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetHardwareChannels"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetHardwareChannels (IntPtr system, ref int num2d, ref int num3d, ref int total);

		[DllImport("fmodex", EntryPoint = "FMOD_System_SetSoftwareChannels"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetSoftwareChannels (IntPtr system, int numsoftwarechannels);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetSoftwareChannels"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetSoftwareChannels (IntPtr system, ref int numsoftwarechannels);

		[DllImport("fmodex", EntryPoint = "FMOD_System_SetDSPBufferSize"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetDSPBufferSize (IntPtr system, uint bufferlength, int numbuffers);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetDSPBufferSize"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetDSPBufferSize (IntPtr system, ref uint bufferlength, ref int numbuffers);

		[DllImport("fmodex", EntryPoint = "FMOD_System_SetFileSystem"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetFileSystem (IntPtr system, File_OpenDelegate useropen, File_CloseDelegate userclose, File_ReadDelegate userread, File_SeekDelegate userseek, File_AsyncReadDelegate userasyncread, File_AsyncCancelDelegate userasynccancel, int blockalign);

		[DllImport("fmodex", EntryPoint = "FMOD_System_AttachFileSystem"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code AttachFileSystem (IntPtr system, File_OpenDelegate useropen, File_CloseDelegate userclose, File_ReadDelegate userread, File_SeekDelegate userseek);

		[DllImport("fmodex", EntryPoint = "FMOD_System_SetPluginPath"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetPluginPath (IntPtr system, string path);

		[DllImport("fmodex", EntryPoint = "FMOD_System_LoadPlugin"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code LoadPlugin (IntPtr system, string filename, ref uint handle, uint priority);

		[DllImport("fmodex", EntryPoint = "FMOD_System_UnloadPlugin"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code UnloadPlugin (IntPtr system, uint handle);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetNumPlugins"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetNumPlugins (IntPtr system, Plugin.Type plugintype, ref int numplugins);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetPluginHandle"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetPluginHandle (IntPtr system, Plugin.Type plugintype, int index, ref uint handle);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetPluginInfo"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetPluginInfo (IntPtr system, uint handle, ref PLUGINTYPE plugintype, StringBuilder name, int namelen, ref uint version);

		[DllImport("fmodex", EntryPoint = "FMOD_System_CreateDSPByPlugin"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code CreateDSPByPlugin (IntPtr system, uint handle, ref IntPtr dsp);

		[DllImport("fmodex", EntryPoint = "FMOD_System_CreateCodec"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code CreateCodec (IntPtr system, IntPtr description, uint priority);

		[DllImport("fmodex", EntryPoint = "FMOD_System_SetOutputByPlugin"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetOutputByPlugin (IntPtr system, uint handle);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetOutputByPlugin"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetOutputByPlugin (IntPtr system, ref uint handle);

		[DllImport("fmodex", EntryPoint = "FMOD_System_SetAdvancedSettings"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetAdvancedSettings (IntPtr system, ref ADVANCEDSETTINGS settings);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetAdvancedSettings"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetAdvancedSettings (IntPtr system, ref ADVANCEDSETTINGS settings);

		[DllImport("fmodex", EntryPoint = "FMOD_System_Set3DRolloffCallback"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DRolloffCallback (IntPtr system, CB_3D_RollOffDelegate callback);

		[DllImport("fmodex", EntryPoint = "FMOD_System_Set3DSpeakerPosition"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DSpeakerPosition (IntPtr system, Speaker speaker, float x, float y, int active);

		[DllImport("fmodex", EntryPoint = "FMOD_System_Get3DSpeakerPosition"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DSpeakerPosition (IntPtr system, Speaker speaker, ref float x, ref float y, ref int active);

		[DllImport("fmodex", EntryPoint = "FMOD_System_Set3DSettings"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DSettings (IntPtr system, float dopplerscale, float distancefactor, float rolloffscale);

		[DllImport("fmodex", EntryPoint = "FMOD_System_Get3DSettings"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DSettings (IntPtr system, ref float dopplerscale, ref float distancefactor, ref float rolloffscale);

		[DllImport("fmodex", EntryPoint = "FMOD_System_Set3DNumListeners"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DNumListeners (IntPtr system, int numlisteners);

		[DllImport("fmodex", EntryPoint = "FMOD_System_Get3DNumListeners"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DNumListeners (IntPtr system, ref int numlisteners);

		[DllImport("fmodex", EntryPoint = "FMOD_System_Set3DListenerAttributes"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Set3DListenerAttributes (IntPtr system, int listener, ref Vector3 pos, ref Vector3 vel, ref Vector3 forward, ref Vector3 up);

		[DllImport("fmodex", EntryPoint = "FMOD_System_Get3DListenerAttributes"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code Get3DListenerAttributes (IntPtr system, int listener, ref Vector3 pos, ref Vector3 vel, ref Vector3 forward, ref Vector3 up);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetOutputHandle"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetOutputHandle (IntPtr system, ref IntPtr handle);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetCPUUsage"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetCPUUsage (IntPtr system, ref float dsp, ref float stream, ref float geometry, ref float update, ref float total);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetSoundRAM"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetSoundRAM (IntPtr system, ref int currentalloced, ref int maxalloced, ref int total);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetNumCDROMDrives"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetNumCDROMDrives (IntPtr system, ref int numdrives);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetCDROMDriveName"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetCDROMDriveName (IntPtr system, int drive, StringBuilder drivename, int drivenamelen, StringBuilder scsiname, int scsinamelen, StringBuilder devicename, int devicenamelen);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetChannel"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetChannel (IntPtr system, int channelid, ref IntPtr channel);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetMasterChannelGroup"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetMasterChannelGroup (IntPtr system, ref IntPtr channelgroup);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetMasterSoundGroup"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetMasterSoundGroup (IntPtr system, ref IntPtr soundgroup);

		[DllImport("fmodex", EntryPoint = "FMOD_System_CreateGeometry"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code CreateGeometry (IntPtr system, int maxpolygons, int maxvertices, ref IntPtr geometry);

		[DllImport("fmodex", EntryPoint = "FMOD_System_SetGeometrySettings"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetGeometrySettings (IntPtr system, float maxworldsize);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetGeometrySettings"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetGeometrySettings (IntPtr system, ref float maxworldsize);

		[DllImport("fmodex", EntryPoint = "FMOD_System_LoadGeometry"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code LoadGeometry (IntPtr system, IntPtr data, int datasize, ref IntPtr geometry);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetGeometryOcclusion"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetGeometryOcclusion (IntPtr system, ref Vector3 listener, ref Vector3 source, ref float direct, ref float reverb);

		[DllImport("fmodex", EntryPoint = "FMOD_System_SetUserData"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code SetUserData (IntPtr system, IntPtr userdata);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetUserData"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetUserData (IntPtr system, ref IntPtr userdata);

		[DllImport("fmodex", EntryPoint = "FMOD_System_GetMemoryInfo"), SuppressUnmanagedCodeSecurity]
		private static extern Error.Code GetMemoryInfo (IntPtr system, uint memorybits, uint event_memorybits, ref uint memoryused, ref Memory.UsageDetails memoryused_details);

		
		 //Old Methods
		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetHardwareChannels"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetHardwareChannels (IntPtr system, int Min2d, int Max2d, int Min3d, int Max3d);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetSoftwareChannels"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetSoftwareChannels (IntPtr system, int Numsoftwarechannels);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetSoftwareChannels"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetSoftwareChannels (IntPtr system, ref int Numsoftwarechannels);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetSoftwareFormat"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetSoftwareFormat (IntPtr system, int Samplerate, Sound.Format format, int Numoutputchannels, int Maxinputchannels, DSP.Resampler Resamplemethod);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetSoftwareFormat"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetSoftwareFormat (IntPtr system, ref int Samplerate, ref Sound.Format format, ref int Numoutputchannels, ref int Maxinputchannels, ref DSP.Resampler Resamplemethod, ref int Bits);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetDSPBufferSize"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetDSPBufferSize (IntPtr system, int Bufferlength, int Numbuffers);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetDSPBufferSize"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetDSPBufferSize (IntPtr system, ref int Bufferlength, ref int Numbuffers);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetFileSystem"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetFileSystem (IntPtr system, int useropen, int userclose, int userread, int userseek, int Buffersize);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_AttachFileSystem"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code AttachFileSystem (IntPtr system, int useropen, int userclose, int userread, int userseek);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetAdvancedSettings"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetAdvancedSettings (IntPtr system, ref FMOD_ADVANCEDSETTINGS settings);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetAdvancedSettings"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetAdvancedSettings (IntPtr system, ref FMOD_ADVANCEDSETTINGS settings);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetSpeakerMode"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetSpeakerMode (IntPtr system, SpeakerMode Speakermode);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetSpeakerMode"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetSpeakerMode (IntPtr system, ref SpeakerMode Speakermode);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetPluginPath"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetPluginPath (IntPtr system, string Path);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_LoadPlugin"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code LoadPlugin (IntPtr system, string Filename, ref int Handle, int Priority);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_UnloadPlugin"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code UnloadPlugin (IntPtr system, int Handle);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetNumPlugins"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetNumPlugins (IntPtr system, Plugin.Type Plugintype, ref int Numplugins);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetPluginHandle"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetPluginHandle (IntPtr system, Plugin.Type Plugintype, int Index, ref int Handle);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetPluginInfo"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetPluginInfo (IntPtr system, int Handle, ref Plugin.Type Plugintype, ref byte name, int namelen, ref int version);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetOutputByPlugin"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetOutputByPlugin (IntPtr system, int Handle);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetOutputByPlugin"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetOutputByPlugin (IntPtr system, ref int Handle);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_CreateDSPByPlugin"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code CreateDSPByPlugin (IntPtr system, int Handle, ref int DSP);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_CreateCodec"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code CreateCodec (IntPtr system, int CodecDescription);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_Set3DSettings"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code Set3DSettings (IntPtr system, float Dopplerscale, float Distancefactor, float Rolloffscale);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_Get3DSettings"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code Get3DSettings (IntPtr system, ref float Dopplerscale, ref float Distancefactor, ref float Rolloffscale);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_Set3DNumListeners"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code Set3DNumListeners (IntPtr system, int Numlisteners);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_Get3DNumListeners"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code Get3DNumListeners (IntPtr system, ref int Numlisteners);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_Set3DListenerAttributes"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code Set3DListenerAttributes (IntPtr system, int Listener, ref Vector3 Pos, ref Vector3 Vel, ref Vector3 Forward, ref Vector3 Up);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_Get3DListenerAttributes"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code Get3DListenerAttributes (IntPtr system, int Listener, ref Vector3 Pos, ref Vector3 Vel, ref Vector3 Forward, ref Vector3 Up);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_Set3DRolloffCallback"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code Set3DRolloffCallback (IntPtr system, int Callback);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_Set3DSpeakerPosition"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code Set3DSpeakerPosition (IntPtr system, Speaker speaker, float X, float Y, int active);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_Get3DSpeakerPosition"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code Get3DSpeakerPosition (IntPtr system, ref Speaker speaker, ref float X, ref float Y, ref int active);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetStreamBufferSize"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetStreamBufferSize (IntPtr system, int Filebuffersize, TimeUnit Filebuffersizetype);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetStreamBufferSize"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetStreamBufferSize (IntPtr system, ref int Filebuffersize, ref TimeUnit Filebuffersizetype);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetOutputHandle"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetOutputHandle (IntPtr system, ref int Handle);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetChannelsPlaying"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetChannelsPlaying (IntPtr system, ref int Channels);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetHardwareChannels"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetHardwareChannels (IntPtr system, ref int Num2d, ref int Num3d, ref int total);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetCPUUsage"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetCPUUsage (IntPtr system, ref float DSP, ref float Stream, ref float Geometry, ref float Update, ref float total);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetSoundRAM"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetSoundRAM (IntPtr system, ref int currentalloced, ref int maxalloced, ref int total);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetNumCDROMDrives"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetNumCDROMDrives (IntPtr system, ref int Numdrives);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetCDROMDriveName"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetCDROMDriveName (IntPtr system, int Drive, ref byte Drivename, int Drivenamelen, ref byte Scsiname, int Scsinamelen, ref byte Devicename, int Devicenamelen);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_CreateDSP"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code CreateDSP (IntPtr system, ref FMOD_DSP_DESCRIPTION description, ref int DSP);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_CreateDSPByType"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code CreateDSPByType (IntPtr system, DSP.Type dsptype, ref int DSP);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_CreateDSPByIndex"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code CreateDSPByIndex (IntPtr system, int Index, ref int DSP);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_PlayDSP"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code PlayDSP (IntPtr system, Channel.Index channelid, int DSP, int paused, ref int channel);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetChannel"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetChannel (IntPtr system, int channelid, ref int channel);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetMasterChannelGroup"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetMasterChannelGroup (IntPtr system, ref int Channelgroup);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetMasterSoundGroup"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetMasterSoundGroup (IntPtr system, ref int soundgroup);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetReverbProperties"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetReverbProperties (IntPtr system, ref Reverb.Properties Prop);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetReverbProperties"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetReverbProperties (IntPtr system, ref Reverb.Properties Prop);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetReverbAmbientProperties"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetReverbAmbientProperties (IntPtr system, ref Reverb.Properties Prop);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetReverbAmbientProperties"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetReverbAmbientProperties (IntPtr system, ref Reverb.Properties Prop);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_CreateGeometry"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code CreateGeometry (IntPtr system, int MaxPolygons, int MaxVertices, ref int Geometryf);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetGeometrySettings"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetGeometrySettings (IntPtr system, float Maxworldsize);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetGeometrySettings"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetGeometrySettings (IntPtr system, ref float Maxworldsize);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_LoadGeometry"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code LoadGeometry (IntPtr system, int Data, int DataSize, ref int Geometry);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_SetUserData"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code SetUserData (IntPtr system, int userdata);

		[DllImport("fmodex", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "FMOD_System_GetUserData"), SuppressUnmanagedCodeSecurity]
		public static extern Error.Code GetUserData (IntPtr system, ref int userdata);
		
		*/
    }
}