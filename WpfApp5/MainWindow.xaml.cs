using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string path = Convert.ToString($@"data/");
        public static string fullpath = System.IO.Path.GetFullPath(path);
        public MainWindow()
        {
            InitializeComponent();
            //string path = Convert.ToString($@"data/");
            //string fullpath = System.IO.Path.GetFullPath(path);
            var dir = new System.IO.DirectoryInfo(fullpath);
            FileInfo[] files = dir.GetFiles("*.*");
            listBox.ItemsSource = files;
        }

        public void OpenFile(string filename)
        {
            //string path = Convert.ToString($@"data/file1.rtf");
            //string fullpath = System.IO.Path.GetFullPath(path);
            string fl = $"{fullpath}";
            FileStream fileStream = new FileStream(fl, FileMode.Open);
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            range.Load(fileStream, DataFormats.Text);
        }

        public void CreateFile(string filename)
        {
            //string path = Convert.ToString($@"data/");
            //string fullpath = System.IO.Path.GetFullPath(path);
            string fl = $"{fullpath}{filename}.rtf";
            File.Create(fl);
            var dir = new System.IO.DirectoryInfo(fullpath);
            FileInfo[] files = dir.GetFiles("*.*");
            listBox.ItemsSource = files;
            listBox.DisplayMemberPath = "Name";
            //FileStream fileStream = new FileStream(System.IO.Path.GetFullPath(path), FileMode.Create);
            //TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            //range.Save(fileStream, DataFormats.Rtf);
        }

        public void SaveFile()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Plain Text File (*.txt)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.OpenOrCreate);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Text);
            }
        }

        public void DeleteFile(string filename)
        {
            //string path = Convert.ToString($@"data/");
            //string fullpath = System.IO.Path.GetFullPath(path);
            string fl = $"{fullpath}{filename}.rtf";
            File.Delete(fl);
        }

        private void newFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string filename;
            CreateFileWindow createFileWindow = new CreateFileWindow();
            if (createFileWindow.ShowDialog() == true)
            {
                filename = createFileWindow.FileName;
                CreateFile(filename);
            }
        }
        private void delFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //string filename;

            //DeleteFile(filename);
        }
    }
}
