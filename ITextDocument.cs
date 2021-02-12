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
    [Guid("8CC497C0-A1DF-11ce-8098-00AA0047BE5D")]
    public interface ITextDocument
    {
        // IDispath methods (We never use them)
        int GetIDsOfNames(Guid riid, IntPtr rgszNames, uint cNames, uint lcid, ref int rgDispId);
        int GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);
        int GetTypeInfoCount(ref uint pctinfo);
        int Invoke(uint dispIdMember, Guid riid, uint lcid, uint wFlags, IntPtr pDispParams, IntPtr pvarResult, IntPtr pExcepInfo, ref uint puArgErr);

        // ITextDocument methods
        int GetName( /* [retval][out] BSTR* */ [In, Out, MarshalAs(UnmanagedType.BStr)] ref string pName);
        int GetSelection( /* [retval][out] ITextSelection** */ IntPtr ppSel);
        int GetStoryCount( /* [retval][out] */ ref int pCount);
        int GetStoryRanges( /* [retval][out] ITextStoryRanges** */ IntPtr ppStories);
        int GetSaved( /* [retval][out] */ ref int pValue);
        int SetSaved( /* [in] */ int Value);
        int GetDefaultTabStop( /* [retval][out] */ ref float pValue);
        int SetDefaultTabStop( /* [in] */ float Value);
        int New();
        int Open( /* [in] VARIANT **/ IntPtr pVar, /* [in] */ int Flags, /* [in] */ int CodePage);
        int Save( /* [in] VARIANT * */ IntPtr pVar, /* [in] */ int Flags, /* [in] */ int CodePage);
        int Freeze( /* [retval][out] */ ref int pCount);
        int Unfreeze( /* [retval][out] */ ref int pCount);
        int BeginEditCollection();
        int EndEditCollection();
        int Undo( /* [in] */ int Count, /* [retval][out] */ ref IntPtr prop);
        int Redo( /* [in] */ int Count, /* [retval][out] */ ref IntPtr prop);
        int Range( /* [in] */ int cp1, /* [in] */ int cp2, /* [retval][out] ITextRange** */ IntPtr ppRange);
        int RangeFromPoint( /* [in] */ int x, /* [in] */ int y, /* [retval][out] ITextRange** */ IntPtr ppRange);
    }
}
