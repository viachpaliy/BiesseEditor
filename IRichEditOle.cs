using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BiesseEditor
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00020D00-0000-0000-c000-000000000046")]
    public interface IRichEditOle
    {
        int GetClientSite(IntPtr lplpolesite);
        int GetObjectCount();
        int GetLinkCount();
        int GetObject(int iob, REOBJECT lpreobject, [MarshalAs(UnmanagedType.U4)] GetObjectOptions flags);
        int InsertObject(REOBJECT lpreobject);
        int ConvertObject(int iob, CLSID rclsidNew, string lpstrUserTypeNew);
        int ActivateAs(CLSID rclsid, CLSID rclsidAs);
        int SetHostNames(string lpstrContainerApp, string lpstrContainerObj);
        int SetLinkAvailable(int iob, int fAvailable);
        int SetDvaspect(int iob, uint dvaspect);
        int HandsOffStorage(int iob);
        int SaveCompleted(int iob, IntPtr lpstg);
        int InPlaceDeactivate();
        int ContextSensitiveHelp(int fEnterMode);
    }
}
