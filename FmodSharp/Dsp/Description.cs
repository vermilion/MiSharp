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
	/*
        <br>
        There are 2 different ways to change a parameter in this architecture.<br>
        One is to use DSP::setParameter / DSP::getParameter.  This is platform independant and is dynamic, so new unknown plugins can have their parameters enumerated and used.<br>
        The other is to use DSP::showConfigDialog.  This is platform specific and requires a GUI, and will display a dialog box to configure the plugin.<br>
        
        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        System::createDSP
        System::getDSP
    ]
    */
	
	//TODO end submmary

	/// <summary>
	/// Strcture to define the parameters for a DSP unit.
	/// </summary>
	public struct Description
	{
		/// <summary>
		/// Name of the unit to be displayed in the network.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public char[] Name;

		/// <summary>
		/// Plugin writer's version number.
		/// </summary>
		public uint Version;

		/// <summary>
		/// Number of channels.
		/// Use 0 to process whatever number of channels is currently in the network.
		/// Value other than 0 would be mostly used if the unit is a unit that only generates sound.
		/// </summary>
		public int Channels;

		/// <summary>
		/// Create callback.
		/// This is called when DSP unit is created.
		/// Can be null.
		/// </summary>
		public DspDelegate Create;

		/// <summary>
		/// Release callback.
		/// This is called just before the unit is freed so the user can do any cleanup needed for the unit.
		/// Can be null.
		/// </summary>
		public DspDelegate Release;

		/// <summary>
		/// Reset callback.
		/// This is called by the user to reset any history buffers that may need resetting for a filter,
		/// when it is to be used or re-used for the first time to its initial clean state.
		/// Use to avoid clicks or artifacts.
		/// </summary>
		public DspDelegate Reset;

		/// <summary>
		/// Read callback.
		/// Processing is done here.
		/// Can be null.
		/// </summary>
		public ReadDelegate Read;

		/// <summary>
		/// SetPosition callback.
		/// This is called if the unit wants to update its position info but not process data.
		/// Can be null.
		/// </summary>
		public SetPositionDelegate SetPosition;

		/// <summary>
		/// Number of parameters used in this filter.
		/// The user finds this with DSP::getNumParameters
		/// </summary>
		public int NumberParameters;

		/// <summary>
		/// Variable number of parameter structures.
		/// </summary>
		public ParameterDescription[] ParameterDesc;

		/// <summary>
		/// This is called when the user calls DSP::setParameter.
		/// Can be null.
		/// </summary>
		public SetParamDelegate SetParameter;

		/// <summary>
		/// This is called when the user calls DSP::getParameter.
		/// Can be null.
		/// </summary>
		public GetParamDelegate GetParameter;

		/// <summary>
		/// This is called when the user calls DSP::showConfigDialog.
		/// Can be used to display a dialog to configure the filter.
		/// Can be null.
		/// </summary>
		public DialogDelegate Config;

		/// <summary>
		/// Width of config dialog graphic if there is one.
		/// 0 otherwise.
		/// </summary>
		public int ConfigWidth;

		/// <summary>
		/// Height of config dialog graphic if there is one.
		/// 0 otherwise.
		/// </summary>
		public int ConfigHeight;

		/// <summary>
		/// Optional.
		/// Specify 0 to ignore.
		/// This is user data to be attached to the DSP unit during creation.
		/// Access via DSP::getUserData.
		/// </summary>
		public IntPtr UserData;
	}
}
