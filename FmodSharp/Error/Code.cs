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

namespace Linsft.FmodSharp.Error
{
	public enum Code
	{
		/// <summary>
		/// No errors.
		/// </summary>
		OK,
		
		/// <summary>
		/// Tried to call lock a second time before unlock was called.
		/// </summary>
		AlreadyLocked,
		
		/// <summary>
		/// Tried to call a function on a data type that does not allow
		/// this type of functionality (ie calling Sound::lock on a streaming sound).
		/// </summary>
		BadCommand,
		
		/// <summary>
		/// Neither NTSCSI nor ASPI could be initialised.
		/// </summary>
		CDDA_Drivers,
		
		/// <summary>
		/// An error occurred while initialising the CDDA subsystem.
		/// </summary>
		CDDA_Init,
		
		/// <summary>
		/// Couldn't find the specified device.
		/// </summary>
		CDDA_InvalidDevice,
		
		/// <summary>
		/// No audio tracks on the specified disc.
		/// </summary>
		CDDA_NoAudio,
		
		/// <summary>
		/// No CD/DVD devices were found.
		/// </summary>
		CDDA_NoDevices,
		
		/// <summary>
		/// No disc present in the specified drive.
		/// </summary>
		CDDA_NoDisc,
		
		/// <summary>
		/// A CDDA read error occurred.
		/// </summary>
		CDDA_Read,
		
		/// <summary>
		/// Error trying to allocate a channel.
		/// </summary>
		Channel_Alloc,
		
		/// <summary>
		/// The specified channel has been reused to play another sound.
		/// </summary>
		Channel_Stolen,
		
		/// <summary>
		/// A Win32 COM related error occured. COM failed to initialize or a QueryInterface failed meaning a Windows codec or driver was not installed properly.
		/// </summary>
		COM,
		
		/// <summary>
		/// DMA Failure.  See debug output for more information.
		/// </summary>
		DMA,
		
		/// <summary>
		/// DSP connection error.  Connection possibly caused a cyclic dependancy.
		/// </summary>
		DSP_Connection,
		
		/// <summary>
		/// DSP Format error.  A DSP unit may have attempted to connect to this network with the wrong format.
		/// </summary>
		DSP_Format,
		
		/// <summary>
		/// DSP connection error.  Couldn't find the DSP unit specified.
		/// </summary>
		DSP_NotFound,
		
		/// <summary>
		/// DSP error.  Cannot perform this operation while the network is in the middle of running.  This will most likely happen if a connection or disconnection is attempted in a DSP callback.
		/// </summary>
		DSP_Running,
		
		/// <summary>
		/// DSP connection error.  The unit being connected to or disconnected should only have 1 input or output.
		/// </summary>
		DSP_TooManyConnections,
		
		/// <summary>
		/// Error loading file.
		/// </summary>
		File_Bad,
		
		/// <summary>
		/// Couldn't perform seek operation.  This is a limitation of the medium (ie netstreams) or the file format.
		/// </summary>
		File_CouldNotSeek,
		
		/// <summary>
		/// Media was ejected while reading.
		/// </summary>
		File_DiskEjected,
		
		/// <summary>
		/// End of file unexpectedly reached while trying to read essential data (truncated data?).
		/// </summary>
		File_EOF,
		
		/// <summary>
		/// File not found.
		/// </summary>
		File_NotFound,
		
		/// <summary>
		/// Unwanted file access occured.
		/// </summary>
		File_Unwanted,
		
		/// <summary>
		/// Unsupported file or audio format.
		/// </summary>
		Format,
		
		/// <summary>
		/// A HTTP error occurred. This is a catch-all for HTTP errors not listed elsewhere.
		/// </summary>
		HTTP,
		
		/// <summary>
		/// The specified resource requires authentication or is forbidden.
		/// </summary>
		HTTP_Access,
		
		/// <summary>
		/// Proxy authentication is required to access the specified resource.
		/// </summary>
		HTTP_ProxyAuth,
		
		/// <summary>
		/// A HTTP server error occurred.
		/// </summary>
		HTTP_ServerError,
		
		/// <summary>
		/// The HTTP request timed out.
		/// </summary>
		HTTP_Timeout,
		
		/// <summary>
		/// FMOD was not initialized correctly to support this function.
		/// </summary>
		Initialization,
		
		/// <summary>
		/// Cannot call this command after System::init.
		/// </summary>
		Initialized,
		
		/// <summary>
		/// An error occured that wasn't supposed to.  Contact support.
		/// </summary>
		Internal,
		
		/// <summary>
		/// On Xbox 360 this memory address passed to FMOD must be physical (ie allocated with XPhysicalAlloc.)
		/// </summary>
		InvalidAddress,
		
		/// <summary>
		/// Value passed in was a NaN, Inf or denormalized float.
		/// </summary>
		InvalidFloat,
		
		/// <summary>
		/// An invalid object handle was used.
		/// </summary>
		InvalidHandle,
		
		/// <summary>
		/// An invalid parameter was passed to this function.
		/// </summary>
		InvalidParam,
		
		/// <summary>
		/// An invalid seek position was passed to this function.
		/// </summary>
		InvalidPosition,
		
		/// <summary>
		/// An invalid speaker was passed to this function based on the current speaker mode.
		/// </summary>
		InvalidSpeaker,
		
		/// <summary>
		/// The syncpoint did not come from this sound handle.
		/// </summary>
		InvalidSyncpoint,
		
		/// <summary>
		/// The vectors passed in are not unit length, or perpendicular.
		/// </summary>
		InvalidVector,
		
		/// <summary>
		/// PS2 only.  fmodex.irx failed to initialize.  This is most likely because you forgot to load it.
		/// </summary>
		IRX,
		
		/// <summary>
		/// Reached maximum audible playback count for this sound's soundgroup.
		/// </summary>
		MaxAudible,
		
		/// <summary>
		/// Not enough memory or resources.
		/// </summary>
		Memory,
		
		/// <summary>
		/// Can't use FMOD_OPENMEMORY_POINT on non PCM source data or non mp3/xma/adpcm data if FMOD_CREATECOMPRESSEDSAMPLE was used.
		/// </summary>
		Memory_CantPoint,
		
		/// <summary>
		/// PS2 only.  Not enough memory or resources on PlayStation 2 IOP ram.
		/// </summary>
		Memory_IOP,
		
		/// <summary>
		/// Not enough memory or resources on console sound ram.
		/// </summary>
		Memory_SRAM,
		
		/// <summary>
		/// Tried to call a command on a 3d sound when the command was meant for 2d sound.
		/// </summary>
		Needs2D,
		
		/// <summary>
		/// Tried to call a command on a 2d sound when the command was meant for 3d sound.
		/// </summary>
		Needs3D,
		
		/// <summary>
		/// Tried to use a feature that requires hardware support.  (ie trying to play a VAG compressed sound in software on PS2).
		/// </summary>
		NeedsHardware,
		
		/// <summary>
		/// Tried to use a feature that requires the software engine.  Software engine has either been turned off, or command was executed on a hardware channel which does not support this feature.
		/// </summary>
		NeedsSoftware,
		
		/// <summary>
		/// Couldn't connect to the specified host.
		/// </summary>
		Net_Connect,
		
		/// <summary>
		/// A socket error occurred.  This is a catch-all for socket-related errors not listed elsewhere.
		/// </summary>
		Net_SocketError,
		
		/// <summary>
		/// The specified URL couldn't be resolved.
		/// </summary>
		Net_Url,
		
		/// <summary>
		/// Operation on a non-blocking socket could not complete immediately.
		/// </summary>
		Net_WouldBlock,
		
		/// <summary>
		/// Operation could not be performed because specified sound is not ready.
		/// </summary>
		NotReady,
		
		/// <summary>
		/// Error initializing output device, but more specifically, the output device is already in use and cannot be reused.
		/// </summary>
		Output_Allocated,
		
		/// <summary>
		/// Error creating hardware sound buffer.
		/// </summary>
		Output_CreateBuffer,
		
		/// <summary>
		/// A call to a standard soundcard driver failed, which could possibly mean a bug in the driver or resources were missing or exhausted.
		/// </summary>
		Output_DriverCall,
		
		/// <summary>
		/// Error enumerating the available driver list. List may be inconsistent due to a recent device addition or removal.
		/// </summary>
		Output_Enumeration,
		
		/// <summary>
		/// Soundcard does not support the minimum features needed for this soundsystem (16bit stereo output).
		/// </summary>
		Output_Format,
		
		/// <summary>
		/// Error initializing output device.
		/// </summary>
		Output_Init,
		
		/// <summary>
		/// FMOD_HARDWARE was specified but the sound card does not have the resources nescessary to play it.
		/// </summary>
		Output_NoHardware,
		
		/// <summary>
		/// Attempted to create a software sound but no software channels were specified in System::init.
		/// </summary>
		Output_NoSoftware,
		
		/// <summary>
		/// Panning only works with mono or stereo sound sources.
		/// </summary>
		Pan,
		
		/// <summary>
		/// An unspecified error has been returned from a 3rd party plugin.
		/// </summary>
		Plugin,
		
		/// <summary>
		/// The number of allowed instances of a plugin has been exceeded.
		/// </summary>
		Plugin_Instances,
		
		/// <summary>
		/// A requested output dsp unit type or codec was not available.
		/// </summary>
		Plugin_Missing,
		
		/// <summary>
		/// A resource that the plugin requires cannot be found. (ie the DLS file for MIDI playback)
		/// </summary>
		Plugin_Resource,
		
		/// <summary>
		/// The specified sound is still in use by the event system,
		/// call EventSystem::unloadFSB before trying to release it.
		/// </summary>
		Preloaded,
		
		/// <summary>
		/// The specified sound is still in use by the event system,
		/// wait for the event which is using it finish with it.
		/// </summary>
		ProgrammerSound,
        
		/// <summary>
		/// An error occured trying to initialize the recording device.
		/// </summary>
		Record,
		
		/// <summary>
		/// Specified Instance in FMOD_REVERB_PROPERTIES couldn't be set. Most likely because another application has locked the EAX4 FX slot.
		/// </summary>
		Reverb_Instance,
		
		/// <summary>
		/// This subsound is already being used by another sound, you cannot have more than one parent to a sound.  Null out the other parent's entry first.
		/// </summary>
		SubSound_Allocated,
		
		/// <summary>
		/// Shared subsounds cannot be replaced or moved from their parent stream, such as when the parent stream is an FSB file.
		/// </summary>
		SubSound_CantMove,
		
		/// <summary>
		/// The subsound's mode bits do not match with the parent sound's mode bits.
		/// See documentation for function that it was called with.
		/// </summary>
		SubSound_Mode,
		
		/// <summary>
		/// The error occured because the sound referenced contains subsounds.
		/// (ie you cannot play the parent sound as a static sample, only its subsounds.)
		/// </summary>
		SubSounds,
		
		/// <summary>
		/// The specified tag could not be found or there are no tags.
		/// </summary>
		TagNotFound,
		
		/// <summary>
		/// The sound created exceeds the allowable input channel count.
		/// This can be increased using the maxinputchannels parameter in System::setSoftwareFormat.
		/// </summary>
		TooManyChannels,
		
		/// <summary>
		/// Something in FMOD hasn't been implemented when it should be! contact support!
		/// </summary>
		Unimplemented,
		
		/// <summary>
		/// This command failed because System::init or System::setDriver was not called.
		/// </summary>
		Uninitialized,
		
		/// <summary>
		/// A command issued was not supported by this object.
		/// Possibly a plugin without certain callbacks specified.
		/// </summary>
		Unsupported,
		
		/// <summary>
		/// An error caused by System::update occured.
		/// </summary>
		Update,
		
		/// <summary>
		/// The version number of this file format is not supported.
		/// </summary>
		Version,
		
		
		
		
		/// <summary>
		/// An Event failed to be retrieved, most likely due to 'just fail' being specified as the max playbacks behavior.
		/// </summary>
		Event_Failed,
		
		/// <summary>
		/// Can't execute this command on an EVENT_INFOONLY event.
		/// </summary>
		Event_InfoOnly,
		
		/// <summary>
		/// An error occured that wasn't supposed to.  See debug log for reason.
		/// </summary>
		Event_Internal,
		
		/// <summary>
		/// Event failed because 'Max streams' was hit when FMOD_INIT_FAIL_ON_MAXSTREAMS was specified.
		/// </summary>
		Event_MaxStreams,
		
		/// <summary>
		/// FSB mis-matches the FEV it was compiled with.
		/// </summary>
		Event_Mismatch,
		
		/// <summary>
		/// A category with the same name already exists.
		/// </summary>
		Event_NameConflict,
		
		/// <summary>
		/// The requested event, event group, event category or event property could not be found.
		/// </summary>
		Event_NotFound,
		
		/// <summary>
		/// Tried to call a function on a complex event that's only supported by simple events.
		/// </summary>
		Event_NeedsSimple,
		
		/// <summary>
		/// An event with the same GUID already exists.
		/// </summary>
		Event_GuidConflict,
		
		/// <summary>
		/// The specified project has already been loaded.
		/// Having multiple copies of the same project loaded simultaneously is forbidden.
		/// </summary>
		Event_AlreadyLoaded,
		
		
		
		
		/// <summary>
		/// Music system is not initialized probably because no music data is loaded.
		/// </summary>
		Music_Uninitialized,
		
		/// <summary>
		/// The requested music entity could not be found.
		/// </summary>
		Music_NotFound,
		
		/// <summary>
		/// The music callback is required, but it has not been set.
		/// </summary>
		Music_NoCallback,
		
	}
}
