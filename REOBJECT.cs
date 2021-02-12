using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BiesseEditor
{
     public class REOBJECT
    {
        public REOBJECT()
        {
        }

        public int cbStruct = Marshal.SizeOf(typeof(REOBJECT));     // Size of structure
        public int cp = 0;                                        // Character position of object
        public CLSID clsid = new CLSID();                              // Class ID of object
        public IntPtr poleobj = IntPtr.Zero;                              // OLE object interface
        public IntPtr pstg = IntPtr.Zero;                              // Associated storage interface
        public IntPtr polesite = IntPtr.Zero;                              // Associated client site interface
        public SIZEL sizel = new SIZEL();                              // Size of object (may be 0,0)
        public uint dvaspect = 0;                                        // Display aspect to use
        public uint dwFlags = 0;                                        // Object status flags
        public uint dwUser = 0;                                        // Dword for user's use
    }
}
