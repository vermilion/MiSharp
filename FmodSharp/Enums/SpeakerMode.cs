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
	/// These are speaker types defined for use with the System::setSpeakerMode or System::getSpeakerMode command.
	/// </summary>
	public enum SpeakerMode : int
	{
		/// <summary>
		/// There is no specific speakermode.  Sound channels are mapped in order of input to output.  See remarks for more information.
		/// </summary>
		Raw,
        
		/// <summary>
		/// The speakers are monaural.
		/// </summary>
		Mono,
        
		/// <summary>
		/// The speakers are stereo (DEFAULT).
		/// </summary>
		Stereo,
        
		/// <summary>
		/// 4 speaker setup.  This includes front left, front right, rear left, rear right.
		/// </summary>
		Quad,
        
		/// <summary>
		/// 4 speaker setup.  This includes front left, front right, center, rear center (rear left/rear right are averaged).
		/// </summary>
		Surround,
        
		/// <summary>
		/// 5.1 speaker setup.  This includes front left, front right, center, rear left, rear right and a subwoofer.
		/// </summary>
		_5Point1,
        
		/// <summary>
		///7.1 speaker setup.  This includes front left, front right, center, rear left, rear right, side left, side right and a subwoofer. 
		/// </summary>
		_7Point1,
        
		/// <summary>
		/// Stereo output, but data is encoded in a way that is picked up by a Prologic/Prologic2 decoder and split into a 5.1 speaker setup.
		/// </summary>
		Prologic,

        /// <summary>
		/// Maximum number of speaker modes supported.
		/// </summary>
		Max,
	}
	
	//TODO end submmary
	
	    /*
    [ENUM]
    [
        [DESCRIPTION]   
        

        [REMARKS]
        These are important notes on speaker modes in regards to sounds created with FMOD_SOFTWARE.<br>
        Note below the phrase 'sound channels' is used.  These are the subchannels inside a sound, they are not related and 
        have nothing to do with the FMOD class "Channel".<br>
        For example a mono sound has 1 sound channel, a stereo sound has 2 sound channels, and an AC3 or 6 channel wav file have 6 "sound channels".<br>
        <br>
        FMOD_SPEAKERMODE_RAW<br>
        ---------------------<br>
        This mode is for output devices that are not specifically mono/stereo/quad/surround/5.1 or 7.1, but are multichannel.<br>
        Sound channels map to speakers sequentially, so a mono sound maps to output speaker 0, stereo sound maps to output speaker 0 & 1.<br>
        The user assumes knowledge of the speaker order.  FMOD_SPEAKER enumerations may not apply, so raw channel indicies should be used.<br>
        Multichannel sounds map input channels to output channels 1:1. <br>
        Channel::setPan and Channel::setSpeakerMix do not work.<br>
        Speaker levels must be manually set with Channel::setSpeakerLevels.<br>
        <br>
        FMOD_SPEAKERMODE_MONO<br>
        ---------------------<br>
        This mode is for a 1 speaker arrangement.<br>
        Panning does not work in this speaker mode.<br>
        Mono, stereo and multichannel sounds have each sound channel played on the one speaker unity.<br>
        Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels.<br>
        Channel::setSpeakerMix does not work.<br>
        <br>
        FMOD_SPEAKERMODE_STEREO<br>
        -----------------------<br>
        This mode is for 2 speaker arrangements that have a left and right speaker.<br>
        <li>Mono sounds default to an even distribution between left and right.  They can be panned with Channel::setPan.<br>
        <li>Stereo sounds default to the middle, or full left in the left speaker and full right in the right speaker.  
        <li>They can be cross faded with Channel::setPan.<br>
        <li>Multichannel sounds have each sound channel played on each speaker at unity.<br>
        <li>Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels.<br>
        <li>Channel::setSpeakerMix works but only front left and right parameters are used, the rest are ignored.<br>
        <br>
        FMOD_SPEAKERMODE_QUAD<br>
        ------------------------<br>
        This mode is for 4 speaker arrangements that have a front left, front right, rear left and a rear right speaker.<br>
        <li>Mono sounds default to an even distribution between front left and front right.  They can be panned with Channel::setPan.<br>
        <li>Stereo sounds default to the left sound channel played on the front left, and the right sound channel played on the front right.<br>
        <li>They can be cross faded with Channel::setPan.<br>
        <li>Multichannel sounds default to all of their sound channels being played on each speaker in order of input.<br>
        <li>Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels.<br>
        <li>Channel::setSpeakerMix works but side left, side right, center and lfe are ignored.<br>
        <br>
        FMOD_SPEAKERMODE_SURROUND<br>
        ------------------------<br>
        This mode is for 4 speaker arrangements that have a front left, front right, front center and a rear center.<br>
        <li>Mono sounds default to the center speaker.  They can be panned with Channel::setPan.<br>
        <li>Stereo sounds default to the left sound channel played on the front left, and the right sound channel played on the front right.  
        <li>They can be cross faded with Channel::setPan.<br>
        <li>Multichannel sounds default to all of their sound channels being played on each speaker in order of input.<br>
        <li>Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels.<br>
        <li>Channel::setSpeakerMix works but side left, side right and lfe are ignored, and rear left / rear right are averaged into the rear speaker.<br>
        <br>
        FMOD_SPEAKERMODE_5POINT1<br>
        ------------------------<br>
        This mode is for 5.1 speaker arrangements that have a left/right/center/rear left/rear right and a subwoofer speaker.<br>
        <li>Mono sounds default to the center speaker.  They can be panned with Channel::setPan.<br>
        <li>Stereo sounds default to the left sound channel played on the front left, and the right sound channel played on the front right.  
        <li>They can be cross faded with Channel::setPan.<br>
        <li>Multichannel sounds default to all of their sound channels being played on each speaker in order of input.  
        <li>Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels.<br>
        <li>Channel::setSpeakerMix works but side left / side right are ignored.<br>
        <br>
        FMOD_SPEAKERMODE_7POINT1<br>
        ------------------------<br>
        This mode is for 7.1 speaker arrangements that have a left/right/center/rear left/rear right/side left/side right 
        and a subwoofer speaker.<br>
        <li>Mono sounds default to the center speaker.  They can be panned with Channel::setPan.<br>
        <li>Stereo sounds default to the left sound channel played on the front left, and the right sound channel played on the front right.  
        <li>They can be cross faded with Channel::setPan.<br>
        <li>Multichannel sounds default to all of their sound channels being played on each speaker in order of input.  
        <li>Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels.<br>
        <li>Channel::setSpeakerMix works and every parameter is used to set the balance of a sound in any speaker.<br>
        <br>
        FMOD_SPEAKERMODE_PROLOGIC<br>
        ------------------------------------------------------<br>
        This mode is for mono, stereo, 5.1 and 7.1 speaker arrangements, as it is backwards and forwards compatible with stereo, 
        but to get a surround effect a Dolby Prologic or Prologic 2 hardware decoder / amplifier is needed.<br>
        Pan behaviour is the same as FMOD_SPEAKERMODE_5POINT1.<br>
        <br>
        If this function is called the numoutputchannels setting in System::setSoftwareFormat is overwritten.<br>
        <br>
        For 3D sounds, panning is determined at runtime by the 3D subsystem based on the speaker mode to determine which speaker the 
        sound should be placed in.<br>

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox360, PlayStation 2, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        System::setSpeakerMode
        System::getSpeakerMode
        System::getDriverCaps
        Channel::setSpeakerLevels
    ]
    */
}
