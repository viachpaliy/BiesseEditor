using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

namespace BiesseEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IntPtr hWnd = IntPtr.Zero;
        IntPtr editHwnd = IntPtr.Zero;
        string prgName = "BiesseWorks";
        string winName = "RichEdit20W";
        protected IRichEditOle IRichEditOleValue = null;
        protected IntPtr IRichEditOlePtr = IntPtr.Zero;
        protected ITextDocument ITextDocumentValue = null;
        protected IntPtr ITextDocumentPtr = IntPtr.Zero;



        public MainWindow()
        {
            InitializeComponent();
            SetPrgHWND();
            SetEditHWND();
            bppCode.Text = GetText();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bppCode.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("VB");
            bppCode.ShowLineNumbers = true;
        }

        private void MenuItem_GetCode(object sender, RoutedEventArgs e)
        {
            bppCode.Text = GetText();
        }

        private void MenuItem_SendCode(object sender, RoutedEventArgs e)
        {
            SendText(bppCode.Text);
        }

        private void MenuItem_OpenFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "*.*";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                bppCode.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void MenuItem_Save(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "VBScript Files (*.vbs)|*.vbs";

            if (saveFileDialog.ShowDialog() == true)
            {
                bppCode.Save(saveFileDialog.FileName);
            }
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Cut(object sender, RoutedEventArgs e)
        {
            bppCode.Cut();
        }

        private void MenuItem_Copy(object sender, RoutedEventArgs e)
        {
            bppCode.Copy();
        }

        private void MenuItem_Paste(object sender, RoutedEventArgs e)
        {
            bppCode.Paste();
        }

        private static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }

        void SetPrgHWND()
        {
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(prgName))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
        }

        void SetEditHWND()
        {
            if (!hWnd.Equals(IntPtr.Zero))
            {
                List<IntPtr> childHandles = new List<IntPtr>();

                GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
                IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

                try
                {
                    API.EnumWindowProc childProc = new API.EnumWindowProc(EnumWindow);
                    API.EnumChildWindows(hWnd, childProc, pointerChildHandlesList);
                }
                finally
                {
                    gcChildhandlesList.Free();
                }
                foreach (IntPtr child in childHandles)
                {
                    const int nChars = 256;
                    StringBuilder Buff = new StringBuilder(nChars);
                    if (API.GetClassName(child, Buff, nChars) > 0)
                    {
                        if (Buff.ToString() == winName)
                        {
                            editHwnd = child;
                            
                        }
                    }

                }

            }

        }
        public void SendText(string text)
        {
            IntPtr textPtr = Marshal.StringToHGlobalAnsi(text);
            API.SendMessage(editHwnd, Messages.WM_SETTEXT, IntPtr.Zero, textPtr);
        }

        public string GetText()
        {            
            int length;
            IntPtr p;

            var result = API.SendMessageTimeout(editHwnd, Messages.WM_GETTEXTLENGTH, 0, 0, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 5, out length);

            if (result != 1 || length <= 0)
                return string.Empty;

            var sb = new StringBuilder(length + 1);

            return API.SendMessageTimeoutText(editHwnd, Messages.WM_GETTEXT, sb.Capacity, sb, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 5, out p) != 0 ?
                    sb.ToString() :
                    string.Empty;
        }
        public IRichEditOle GetRichEditOleInterface()
        {
            if (this.IRichEditOleValue == null)
            {
                // Allocate the ptr that EM_GETOLEINTERFACE will fill in
                IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(IntPtr)));  // Alloc the ptr.
                Marshal.WriteIntPtr(ptr, IntPtr.Zero);                                    // Clear it.
                try
                {
                    if (0 != API.SendMessage(editHwnd, Messages.EM_GETOLEINTERFACE, IntPtr.Zero, ptr))
                    {
                        // Read the returned pointer
                        IntPtr pRichEdit = Marshal.ReadIntPtr(ptr);
                        try
                        {
                            if (pRichEdit != IntPtr.Zero)
                            {
                                // Query for the IRichEditOle interface
                                Guid guid = new Guid("00020D00-0000-0000-c000-000000000046");
                                Marshal.QueryInterface(pRichEdit, ref guid, out this.IRichEditOlePtr);

                                // Wrap it in the C# interface for IRichEditOle
                                this.IRichEditOleValue = (IRichEditOle)Marshal.GetTypedObjectForIUnknown(this.IRichEditOlePtr, typeof(IRichEditOle));

                                if (this.IRichEditOleValue == null)
                                {
                                    throw new Exception("Failed to get the object wrapper for the IRichEditOle interface.");
                                }

                                // IID_ITextDocument
                                guid = new Guid("8CC497C0-A1DF-11CE-8098-00AA0047BE5D");
                                Marshal.QueryInterface(pRichEdit, ref guid, out this.ITextDocumentPtr);

                                // Wrap it in the C# interface for IRichEditOle
                                this.ITextDocumentValue = (ITextDocument)Marshal.GetTypedObjectForIUnknown(this.ITextDocumentPtr, typeof(ITextDocument));

                                if (this.ITextDocumentValue == null)
                                {
                                    throw new Exception("Failed to get the object wrapper for the ITextDocument interface.");
                                }
                            }
                            else
                            {
                                throw new Exception("Failed to get the pointer.");
                            }
                        }
                        finally
                        {
                            Marshal.Release(pRichEdit);
                        }
                    }
                    else
                    {
                        throw new Exception("EM_GETOLEINTERFACE failed.");
                    }
                }
                catch (Exception err)
                {
                    Trace.WriteLine(err.ToString());
                    this.ReleaseRichEditOleInterface();
                }
                finally
                {
                    // Free the ptr memory
                    Marshal.FreeCoTaskMem(ptr);
                }
            }

            return this.IRichEditOleValue;
        }
        public void ReleaseRichEditOleInterface()
        {
            if (this.IRichEditOlePtr != IntPtr.Zero)
            {
                Marshal.Release(this.IRichEditOlePtr);
            }

            this.IRichEditOlePtr = IntPtr.Zero;
            this.IRichEditOleValue = null;
        }

        public void EnableUndo(bool enable)
        {
            if (IRichEditOleValue == null)
            {
                GetRichEditOleInterface();
            }

            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(IntPtr)));  // Alloc the ptr.
            Marshal.WriteIntPtr(ptr, IntPtr.Zero);

            ITextDocumentValue.Undo((enable == true) ? TomConstants.tomResume : TomConstants.tomSuspend, ptr);
        }


    }
      
   
}

