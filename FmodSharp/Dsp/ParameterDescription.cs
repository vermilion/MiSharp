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
	//TODO end submmary
	
	    /*
        <br>
        The step parameter tells the gui or application that the parameter has a certain granularity.<br>
        For example in the example of cutoff frequency with a range from 100.0 to 22050.0 you might only want the selection to be in 10hz increments.  For this you would simply use 10.0 as the step value.<br>
        For a boolean, you can use min = 0.0, max = 1.0, step = 1.0.  This way the only possible values are 0.0 and 1.0.<br>
        Some applications may detect min = 0.0, max = 1.0, step = 1.0 and replace a graphical slider bar with a checkbox instead.<br>
        A step value of 1.0 would simulate integer values only.<br>
        A step value of 0.0 would mean the full floating point range is accessable.<br>

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]    
        System::createDSP
        System::getDSP
    ]
    */
    public struct ParameterDescription
    {
		/// <summary>
		/// Minimum value of the parameter.
		/// (ie 100.0)
		/// </summary>
        public float Min;
        
		/// <summary>
		/// Maximum value of the parameter.
		/// (ie 22050.0)
		/// </summary>
        public float Max;
        
		/// <summary>
		/// Default value of parameter.
		/// </summary>
        public float Defaultval;
        
		/// <summary>
		/// Name of the parameter to be displayed
		/// (ie "Cutoff frequency").
		/// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public char[] Name;
        
		/// <summary>
		/// Short string to be put next to value to denote the unit type.
		/// (ie "hz")
		/// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public char[] Label;
        
		/// <summary>
		/// Description of the parameter to be displayed as a help item / tooltip for this parameter.
		/// </summary>
        public string Description;
    }
}

