using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BiesseEditor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CLSID
    {
        public int a;
        public short b;
        public short c;
        public byte d;
        public byte e;
        public byte f;
        public byte g;
        public byte h;
        public byte i;
        public byte j;
        public byte k;
    }

}
