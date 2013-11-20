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

using System.Runtime.InteropServices;

namespace Linsft.FmodSharp.Memory
{
	//TODO end subbmary
	
	/*
    [STRUCTURE]
    [
        [DESCRIPTION]
        
        [REMARKS]
        Every public readonly FMOD class has a getMemoryInfo function which can be used to get detailed information on what memory resources are associated with the object in question. 
        On return from getMemoryInfo, each member of this structure will hold the amount of memory used for its type in bytes.<br>
        <br>

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3

        [SEE_ALSO]
        System::getMemoryInfo
        EventSystem::getMemoryInfo
        FMOD_MEMBITS    
        FMOD_EVENT_MEMBITS
    ]
    */
	
	/// <summary>
	/// Structure to be filled with detailed memory usage information of an FMOD object
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    public struct UsageDetails
    {
		/// <summary>
		/// Memory not accounted for by other types.
		/// </summary>
        public readonly uint Other;
		
		/// <summary>
		/// String data
		/// </summary>
        public readonly uint StringData;
        
		/// <summary>
		/// System object and various internals.
		/// </summary>
        public readonly uint System;
        
		/// <summary>
		/// Plugin objects and internals
		/// </summary>
        public readonly uint Plugin;
        
		/// <summary>
		/// Output module object and internals
		/// </summary>
        public readonly uint Output;
        
		/// <summary>
		/// Channel related memory
		/// </summary>
        public readonly uint Channel;
        
		/// <summary>
		/// ChannelGroup objects and internals
		/// </summary>
        public readonly uint ChannelGroup;
        
		/// <summary>
		/// Codecs allocated for streaming
		/// </summary>
        public readonly uint Codec;
        
		/// <summary>
		/// File buffers and structures
		/// </summary>
        public readonly uint File;
        
		/// <summary>
		/// Sound objects and internals
		/// </summary>
        public readonly uint Sound;
        
		/// <summary>
		/// Sound data stored in secondary RAM
		/// </summary>
        public readonly uint SecondaryRam;
        
		/// <summary>
		/// SoundGroup objects and internals
		/// </summary>
        public readonly uint SoundGroup;
        
		/// <summary>
		/// Stream buffer memory
		/// </summary>
        public readonly uint StreamBuffer;
        
		/// <summary>
		/// DSPConnection objects and internals
		/// </summary>
        public readonly uint DspConnection;
        
		/// <summary>
		/// DSP implementation objects
		/// </summary>
        public readonly uint Dsp;
        
		/// <summary>
		/// Realtime file format decoding DSP objects
		/// </summary>
        public readonly uint DspCodec;
        
		/// <summary>
		/// Profiler memory footprint.
		/// </summary>
        public readonly uint Profile;
        
		/// <summary>
		/// Buffer used to store recorded data from microphone
		/// </summary>
        public readonly uint RecordBuffer;
        
		/// <summary>
		/// Reverb implementation objects
		/// </summary>
        public readonly uint Reverb;
        
		/// <summary>
		/// Reverb channel properties structs
		/// </summary>
        public readonly uint ReverbChannelProperties;
        
		/// <summary>
		/// Geometry objects and internals
		/// </summary>
        public readonly uint Geometry;
        
		/// <summary>
		/// Sync point memory.
		/// </summary>
        public readonly uint SyncPoint;
        
		/// <summary>
		/// EventSystem and various internals
		/// </summary>
        public readonly uint EventSystem;
        
		/// <summary>
		/// MusicSystem and various internals
		/// </summary>
        public readonly uint MusicSystem;
        
		/// <summary>
		/// Definition of objects contained in all loaded projects.
		/// e.g. events, groups, categories
		/// </summary>
        public readonly uint FEV;
        
		/// <summary>
		/// Data loaded with registerMemoryFSB
		/// </summary>
        public readonly uint MemoryFSB;
        
		/// <summary>
		/// EventProject objects and internals
		/// </summary>
        public readonly uint EventProject;
        
		/// <summary>
		/// EventGroup objects and internals
		/// </summary>
        public readonly uint EventGroup;
        
		/// <summary>
		/// Objects used to manage wave banks
		/// </summary>
        public readonly uint SoundBankClass;
        
		/// <summary>
		/// Data used to manage lists of wave bank usage
		/// </summary>
        public readonly uint SoundBankList;
        
		/// <summary>
		/// Stream objects and internals
		/// </summary>
        public readonly uint StreamInstance;
        
		/// <summary>
		/// Sound definition objects
		/// </summary>
        public readonly uint SoundDefinitionClass;
        
		/// <summary>
		/// Sound definition static data objects
		/// </summary>
        public readonly uint SoundDefinitionStaticClass;
        
		/// <summary>
		/// Sound definition pool data
		/// </summary>
        public readonly uint SoundDefinitionPool;
        
		/// <summary>
		/// Reverb definition objects
		/// </summary>
        public readonly uint ReverbDefinition;
        
		/// <summary>
		/// Reverb objects
		/// </summary>
        public readonly uint EventReverb;
        
		/// <summary>
		/// User property objects
		/// </summary>
        public readonly uint UserProperty;
        
		/// <summary>
		/// Event instance base objects
		/// </summary>
        public readonly uint EventInstance;
        
		/// <summary>
		/// Complex event instance objects
		/// </summary>
        public readonly uint EventInstanceComplex;
        
		/// <summary>
		/// Simple event instance objects
		/// </summary>
        public readonly uint EventInstanceSimple;
        
		/// <summary>
		/// Event layer instance objects
		/// </summary>
        public readonly uint EventInstanceLayer;
        
		/// <summary>
		/// Event sound instance objects
		/// </summary>
        public readonly uint EventInstanceSound;
        
		/// <summary>
		/// Event envelope objects
		/// </summary>
        public readonly uint EventEnvelope;
        
		/// <summary>
		/// Event envelope definition objects
		/// </summary>
        public readonly uint EventEnvelopeDefinition;
        
		/// <summary>
		/// Event parameter objects
		/// </summary>
        public readonly uint EventParameter;
        
		/// <summary>
		/// Event category objects
		/// </summary>
        public readonly uint EventCategory;
        
		/// <summary>
		/// Event envelope point objects
		/// </summary>
        public readonly uint EventEnvelopePoint;
        
		/// <summary>
		/// Event instance pool memory
		/// </summary>
        public readonly uint EventInstancePool;
		
	}
}
