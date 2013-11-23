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

namespace Linsft.FmodSharp.Sound
{	
	/// <summary>
	/// These definitions describe the native format of the hardware or software buffer that will be used.
	/// </summary>
	public enum Format
	{
		
		/// <summary>
		/// Unitialized / unknown.
		/// </summary>
		None,
		
		/// <summary>
		/// 8bit integer PCM data.
		/// </summary>
		PCM8,
		
		/// <summary>
		/// 16bit integer PCM data.
		/// </summary>
		PCM16,
		
		/// <summary>
		/// 24bit integer PCM data.
		/// </summary>
		PCM24,
		
		/// <summary>
		/// 32bit integer PCM data.
		/// </summary>
		PCM32,
		
		/// <summary>
		/// 32bit floating point PCM data.
		/// </summary>
		PCMFLOAT,
		
		/// <summary>
		/// Compressed GameCube DSP data.
		/// </summary>
		GCADPCM,
		
		/// <summary>
		/// Compressed XBox ADPCM data.
		/// </summary>
		IMAADPCM,
		
		/// <summary>
		/// Compressed PlayStation 2 ADPCM data.
		/// </summary>
		VAG,
		
		/// <summary>
		/// Compressed Xbox360 data.
		/// </summary>
		XMA,
		
		/// <summary>
		/// Compressed MPEG layer 2 or 3 data.
		/// </summary>
		MPEG,
		
		/// <summary>
		/// Maximum number of sound formats supported.
		/// </summary>
		Max,
		
		/// <summary>
		/// Compressed CELT data.
		/// </summary>
		CELT,
	}
	
	//TODO end submary
	
	
	//[REMARKS]
	//This is the format the native hardware or software buffer will be or is created in.
	
	//[PLATFORMS]
	//Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii
	
	//[SEE_ALSO]
	//System::createSound
	//Sound::GetFormat
	//]
	//
}

