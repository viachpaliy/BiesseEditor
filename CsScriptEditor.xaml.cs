using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace BiesseEditor
{
    /// <summary>
    /// Логика взаимодействия для CsScriptEditor.xaml
    /// </summary>
    public partial class CsScriptEditor : Window
    {
        string filename;
        public CsScriptEditor()
        {
            filename = "";
            InitializeComponent();
            
        }

        private void MenuItem_New(object sender, RoutedEventArgs e)
        {
            filename = "Noname";
            Title = filename;
            scriptCode.Clear();
        }

        public void OpenFile()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.DefaultExt = "*.*";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;
                Title = filename;
                scriptCode.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }
        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.DefaultExt = "*.*";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;
                Title = filename;
                scriptCode.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void MenuItem_SaveAs(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "C# Script Files (*.cs)|*.cs";

            if (saveFileDialog.ShowDialog() == true)
            {
                filename = saveFileDialog.FileName;
                Title = filename;
                scriptCode.Save(saveFileDialog.FileName);
            }
        }

        private void MenuItem_Save(object sender, RoutedEventArgs e)
        {
            if ((filename != "")&&(filename != "Noname"))
            {
                scriptCode.Save(filename);
            } else
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "C# Script Files (*.cs)|*.cs";

                if (saveFileDialog.ShowDialog() == true)
                {
                    filename = saveFileDialog.FileName;
                    Title = filename;
                    scriptCode.Save(saveFileDialog.FileName);
                }
            }
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
            private void MenuItem_Cut(object sender, RoutedEventArgs e)
        {
            scriptCode.Cut();
        }

        private void MenuItem_Copy(object sender, RoutedEventArgs e)
        {
            scriptCode.Copy();
        }

        private void MenuItem_Paste(object sender, RoutedEventArgs e)
        {
            scriptCode.Paste();
        }
    }
}
