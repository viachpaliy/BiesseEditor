﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiesseEditor
{
    public enum GetObjectOptions
    {
        REO_GETOBJ_NO_INTERFACES = 0x00000000,
        REO_GETOBJ_POLEOBJ = 0x00000001,
        REO_GETOBJ_PSTG = 0x00000002,
        REO_GETOBJ_POLESITE = 0x00000004,
        REO_GETOBJ_ALL_INTERFACES = 0x00000007,
    }
}
