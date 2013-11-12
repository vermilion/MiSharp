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

namespace Linsft.FmodSharp.Plugin
{
	//TODO write Summary
	
	//These are plugin types defined for use with the System::getNumPlugins,
	//System::getPluginInfo and System::unloadPlugin functions.
	
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="FMOD_System_GetNumPlugins"/>
	/// <seealso cref="FMOD_System_GetPluginInfo"/>
	/// <seealso cref="FMOD_System_UnloadPlugin"/>
	/// <platforms>
	/// Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii
	/// </platforms>
	public enum Type
	{
		
		/// <summary>
		/// The plugin type is an output module.
		/// FMOD mixed audio will play through one of these devices.
		/// </summary>
		Output,
		
		/// <summary>
		/// The plugin type is a file format codec.
		/// FMOD will use these codecs to load file formats for playback.
		/// </summary>
		Codec,
		
		/// <summary>
		/// The plugin type is a DSP unit.
		/// FMOD will use these plugins as part of its DSP network to
		/// apply effects to output or generate sound in realtime.
		/// </summary>
		DSP
	}
}
