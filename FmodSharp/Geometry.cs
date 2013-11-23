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
	public class Geometry : Handle
	{
		private Geometry()
		{
		}
		
		internal Geometry (IntPtr hnd) : base()
		{
			this.SetHandle (hnd);
		}

		protected override bool ReleaseHandle ()
		{
			if (this.IsInvalid)
				return true;
			
			Release (this.handle);
			this.SetHandleAsInvalid ();
			
			return true;
		}
		
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("fmodex", EntryPoint = "FMOD_Geometry_Release")]
		private static extern Error.Code Release (IntPtr geometry);
		
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("fmodex", EntryPoint = "FMOD_Geometry_Flush")]
        private static extern Error.Code Flush_External (IntPtr geometry);
    
		
		//TODO Implement extern funcitons
		
		/*
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_AddPolygon           (IntPtr geometry, float directocclusion, float reverbocclusion, int doublesided, int numvertices, VECTOR[] vertices, ref int polygonindex);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetNumPolygons       (IntPtr geometry, ref int numpolygons);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetMaxPolygons       (IntPtr geometry, ref int maxpolygons, ref int maxvertices);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetPolygonNumVertices(IntPtr geometry, int index, ref int numvertices);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetPolygonVertex     (IntPtr geometry, int index, int vertexindex, ref VECTOR vertex);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetPolygonVertex     (IntPtr geometry, int index, int vertexindex, ref VECTOR vertex);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetPolygonAttributes (IntPtr geometry, int index, float directocclusion, float reverbocclusion, int doublesided);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetPolygonAttributes (IntPtr geometry, int index, ref float directocclusion, ref float reverbocclusion, ref int doublesided);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetActive            (IntPtr geometry, int active);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetActive            (IntPtr geometry, ref int active);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetRotation          (IntPtr geometry, ref VECTOR forward, ref VECTOR up);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetRotation          (IntPtr geometry, ref VECTOR forward, ref VECTOR up);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetPosition          (IntPtr geometry, ref VECTOR position);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetPosition          (IntPtr geometry, ref VECTOR position);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetScale             (IntPtr geometry, ref VECTOR scale);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetScale             (IntPtr geometry, ref VECTOR scale);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_Save                 (IntPtr geometry, IntPtr data, ref int datasize);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]                   
        private static extern RESULT FMOD_Geometry_SetUserData          (IntPtr geometry, IntPtr userdata);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetUserData          (IntPtr geometry, ref IntPtr userdata);
        
        [System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport(VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetMemoryInfo        (IntPtr geometry, uint memorybits, uint event_memorybits, ref uint memoryused, ref MEMORY_USAGE_DETAILS memoryused_details);
		
		*/
	}
}

