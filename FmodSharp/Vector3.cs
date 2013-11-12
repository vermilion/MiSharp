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

namespace Linsft.FmodSharp
{
	
	/// <summary>
	/// Structure describing a point in 3D space.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3
	{
		public float X;
		public float Y;
		public float Z;
	}
	
	//[REMARKS]
	//FMOD uses a left handed co-ordinate system by default.<br>
	//To use a right handed co-ordinate system specify FMOD_INIT_3D_RIGHTHANDED from FMOD_INITFLAGS in System::init.
	
	//[PLATFORMS]
	//Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii
	
	//[SEE_ALSO]
	//FMOD_System_Set3DListenerAttributes
	//FMOD_System_Get3DListenerAttributes
	//FMOD_Channel_Set3DAttributes
	//FMOD_Channel_Get3DAttributes
	//FMOD_Geometry_AddPolygon
	//FMOD_Geometry_SetPolygonVertex
	//FMOD_Geometry_GetPolygonVertex
	//FMOD_Geometry_SetRotation
	//FMOD_Geometry_GetRotation
	//FMOD_Geometry_SetPosition
	//FMOD_Geometry_GetPosition
	//FMOD_Geometry_SetScale
	//FMOD_Geometry_GetScale
	//FMOD_INITFLAGS
	//]
	//
}
