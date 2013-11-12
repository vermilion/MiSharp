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

namespace Linsft.FmodSharp
{
	
	/// <summary>
	/// Sound description bitfields, bitwise OR them together for loading and describing sounds.
	/// </summary>
	/// <platforms>
	/// Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2,
	/// GameCube, PlayStation Portable, PlayStation 3, Wii
	/// </platforms>
	/// <remarks>
	/// By default a sound will open as a static sound that is decompressed fully into memory.<br>
	/// To have a sound stream instead, use FMOD_CREATESTREAM.<br>
	/// Some opening modes (ie FMOD_OPENUSER, FMOD_OPENMEMORY, FMOD_OPENRAW) will need extra information.<br>
	/// This can be provided using the FMOD_CREATESOUNDEXINFO structure.
	/// </remarks>
	[Flags]
	public enum Mode : uint
	{
		/// <summary>
		/// FMOD_DEFAULT is a default sound type.
		/// Equivalent to all the defaults listed below.
		/// FMOD_LOOP_OFF, FMOD_2D, FMOD_HARDWARE.
		/// </summary>
		Default = 0x0,
		
		/// <summary>
		/// For non looping sounds. (default).  Overrides FMOD_LOOP_NORMAL / FMOD_LOOP_BIDI.
		/// </summary>
		LoopOff = 0x1,
		
		/// <summary>
		/// For forward looping sounds.
		/// </summary>
		LoopNormal = 0x2,
		
		/// <summary>
		/// For bidirectional looping sounds. (only works on software mixed static sounds).
		/// </summary>
		LoopBidi = 0x4,
		
		/// <summary>
		/// Ignores any 3d processing. (default).
		/// </summary>
		_2D = 0x8,
		
		/// <summary>
		/// Makes the sound positionable in 3D.
		/// Overrides FMOD_2D.
		/// </summary>
		_3D = 0x10,
		
		/// <summary>
		/// Attempts to make sounds use hardware acceleration. (default).
		/// </summary>
		Hardware = 0x20,
		
		/// <summary>
		/// Makes sound reside in software.
		/// Overrides FMOD_HARDWARE.
		/// Use this for FFT,
		/// DSP, 2D multi speaker support and other software related features.
		/// </summary>
		Software = 0x40,
		
		/// <summary>
		/// Decompress at runtime, streaming from the source provided (standard stream).
		/// Overrides FMOD_CREATESAMPLE.
		/// </summary>
		CreateStream = 0x80,
		
		/// <summary>
		/// Decompress at loadtime,
		/// decompressing or decoding whole file into memory as the target sample format.
		/// (standard sample).
		/// </summary>
		CreateSample = 0x100,
		
		/// <summary>
		/// Load MP2, MP3, IMAADPCM or XMA into memory and leave it compressed.
		/// During playback the FMOD software mixer will decode it in realtime as a 'compressed sample'.
		/// Can only be used in combination with FMOD_SOFTWARE.
		/// </summary>
		CreateCompressedSample = 0x200,
		
		/// <summary>
		/// Opens a user created static sample or stream.
		/// Use FMOD_CREATESOUNDEXINFO to specify format and/or read callbacks.
		/// If a user created 'sample' is created with no read callback, the sample will be empty.
		/// Use FMOD_Sound_Lock and FMOD_Sound_Unlock to place sound data into the sound if this is the case.
		/// </summary>
		OpenUser = 0x400,
		
		/// <summary>
		/// "name_or_data" will be interpreted as a pointer to memory instead of filename for creating sounds.
		/// </summary>
		OpenMemory = 0x800,
		
		/// <summary>
		/// "name_or_data" will be interpreted as a pointer to memory instead of filename for creating sounds.
		/// Use FMOD_CREATESOUNDEXINFO to specify length.
		/// This differs to FMOD_OPENMEMORY in that it uses the memory as is, without duplicating the memory into its own buffers.
		/// FMOD_SOFTWARE only.
		/// Doesn't work with FMOD_HARDWARE, as sound hardware cannot access main ram on a lot of platforms.
		/// Cannot be freed after open, only after Sound::release.
		/// Will not work if the data is compressed and FMOD_CREATECOMPRESSEDSAMPLE is not used.
		/// </summary>
		OpenMemoryPoint = 0x10000000,
		
		/// <summary>
		/// Will ignore file format and treat as raw pcm.
		/// User may need to declare if data is FMOD_SIGNED or FMOD_UNSIGNED
		/// </summary>
		OpenRaw = 0x1000,
		
		/// <summary>
		/// Just open the file, dont prebuffer or read.
		/// Good for fast opens for info, or when FMOD_Sound_ReadData is to be used.
		/// </summary>
		OpenOnly = 0x2000,
		
		/// <summary>
		/// For FMOD_System_CreateSound - for accurate FMOD_Sound_GetLength / FMOD_Channel_SetPosition on VBR MP3, AAC and MOD/S3M/XM/IT/MIDI files.
		/// Scans file first, so takes longer to open.
		/// FMOD_OPENONLY does not affect this.
		/// </summary>
		AccurateTime = 0x4000,
		
		/// <summary>
		/// For corrupted / bad MP3 files.
		/// This will search all the way through the file until it hits a valid MPEG header.
		/// Normally only searches for 4k.
		/// </summary>
		MpegSearch = 0x8000,
		
		/// <summary>
		/// For opening sounds and getting streamed subsounds (seeking) asyncronously.
		/// Use Sound::getOpenState to poll the state of the sound as it opens or retrieves the subsound in the background.
		/// </summary>
		NonBlocking = 0x10000,
		
		/// <summary>
		/// Unique sound, can only be played one at a time.
		/// </summary>
		Unique = 0x20000,
		
		/// <summary>
		/// Make the sound's position, velocity and orientation relative to the listener.
		/// </summary>
		_3D_HeadRelative = 0x40000,
		
		/// <summary>
		/// Make the sound's position, velocity and orientation absolute (relative to the world). (DEFAULT)
		/// </summary>
		_3D_WorldRelative = 0x80000,
		
		/// <summary>
		/// This sound will follow the standard logarithmic rolloff model where mindistance = full volume,
		/// maxdistance = where sound stops attenuating,
		/// and rolloff is fixed according to the global rolloff factor.  (default)
		/// </summary>
		_3D_LogRolloff = 0x100000,
		
		/// <summary>
		/// This sound will follow a linear rolloff model where mindistance = full volume, maxdistance = silence.
		/// </summary>
		_3D_LinearRolloff = 0x200000,
		
		/// <summary>
		/// This sound will follow a rolloff model defined by FMOD_Sound_Set3DCustomRolloff / FMOD_Channel_Set3DCustomRolloff.
		/// </summary>
		_3D_CustomRolloff = 0x4000000,
		
		/// <summary>
		/// For CDDA sounds only - use ASPI instead of NTSCSI to access the specified CD/DVD device.
		/// </summary>
		CDDA_ForceASPI = 0x400000,
		
		/// <summary>
		/// For CDDA sounds only - perform jitter correction.
		/// Jitter correction helps produce a more accurate CDDA stream at the cost of more CPU time.
		/// </summary>
		CDDA_JitterCorrect = 0x800000,
		
		/// <summary>
		/// Filename is double-byte unicode.
		/// </summary>
		Unicode = 0x1000000,
		
		/// <summary>
		/// Skips id3v2/asf/etc tag checks when opening a sound,
		/// to reduce seek/read overhead when opening files (helps with CD performance).
		/// </summary>
		IgnoreTags = 0x2000000,
		
		/// <summary>
		/// Removes some features from samples to give a lower memory overhead, like FMOD_Sound_GetName.
		/// </summary>
		LowMemory = 0x8000000,
		
		/// <summary>
		/// Load sound into the secondary RAM of supported platform.  On PS3, sounds will be loaded into RSX/VRAM.
		/// </summary>
		LoadSecondaryRam = 0x20000000,
		
		/// <summary>
		/// For sounds that start virtual (due to being quiet or low importance),
		/// instead of swapping back to audible,
		/// and playing at the correct offset according to time,
		/// this flag makes the sound play from the start.
		/// </summary>
		Virtual_PlayFromStart = 0x80000000
		
	}
	//TODO complete SellAlso

	//[SEE_ALSO]
	//System::createSound
	//System::createStream
	//Sound::setMode
	//Sound::getMode
	//Channel::setMode
	//Channel::getMode
}

