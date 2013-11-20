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

namespace Linsft.FmodSharp.Enums
{
	// TODO fix summary method ref
	
	/// <summary>
	/// List of time types that can be returned by Sound::getLength and used with Channel::setPosition or Channel::getPosition.
	/// </summary>
	public enum TimeUnit
	{
		Milliseconds = 0x1,
		
		/// <summary>
		/// PCM Samples, related to milliseconds * samplerate / 1000.
		/// </summary>
		PCM = 0x2,
		
		/// <summary>
		/// Bytes, related to PCM samples * channels * datawidth (ie 16bit = 2 bytes).
		/// </summary>
		PCMBytes = 0x4,
		
		RawBytes = 0x8,
		// Raw file bytes of (compressed) sound data (does not include headers).  Only used by Sound::getLength and Channel::getPosition.
		
		ModOrder = 0x100,
		// MOD/S3M/XM/IT.  Order in a sequenced module format.  Use Sound::getFormat to determine the format.
		
		ModRow = 0x200,
		// MOD/S3M/XM/IT.  Current row in a sequenced module format.  Sound::getLength will return the number if rows in the currently playing or seeked to pattern.
		
		ModPattern = 0x400,
		// MOD/S3M/XM/IT.  Current pattern in a sequenced module format.  Sound::getLength will return the number of patterns in the song and Channel::getPosition will return the currently playing pattern.
		
		Sentence_Milliseconds = 0x10000,
		// Currently playing subsound in a sentence time in milliseconds.
		
		Sentence_PCM = 0x20000,
		// Currently playing subsound in a sentence time in PCM Samples, related to milliseconds * samplerate / 1000.
		
		Sentence_PCMBytes = 0x40000,
		// Currently playing subsound in a sentence time in bytes, related to PCM samples * channels * datawidth (ie 16bit = 2 bytes).
		
		Sentence = 0x80000,
		// Currently playing sentence index according to the channel.
		
		Sentence_SubSound = 0x100000,
		// Currently playing subsound index in a sentence.
		
		Buffered = 0x10000000
		// Time value as seen by buffered stream.  This is always ahead of audible time, and is only used for processing.
		
	}

	//TODO end submary
	
		//[PLATFORMS]
		//Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

		//[SEE_ALSO]
		//Sound:: getLength
		//FMOD_Channel_SetPosition
		//FMOD_Channel_GetPosition
		//]
		//
}
