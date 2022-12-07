using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using Path = System.IO.Path;

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
            var dir = new System.IO.DirectoryInfo(fullpath);
            FileInfo[] files = dir.GetFiles("*.*");
            listBox.ItemsSource = files;
        }

        public void OpenFile(string filename)
        {
            try
            {
                string fl = $"{fullpath}{filename}";
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                using (var fs = File.OpenRead(fl))
                {
                    if (fl.ToLower().EndsWith(".rtf"))
                        range.Load(fs, System.Windows.DataFormats.Rtf);
                    else if (fl.ToLower().EndsWith(".txt"))
                        range.Load(fs, System.Windows.DataFormats.Text);
                    else
                        range.Load(fs, System.Windows.DataFormats.Xaml);
                    fs.Close();
                }
            }
            catch (Exception e) 
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CreateFile(string filename)
        {
            try 
            {
                string fl = $"{fullpath}{filename}.rtf";
                //File.Create(fl);
                using (FileStream fw = new FileStream(fl, FileMode.Create))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("");
                    fw.Write(info, 0, info.Length);
                    fw.Close();
                }
                var dir = new System.IO.DirectoryInfo(fullpath);
                FileInfo[] files = dir.GetFiles("*.*");
                listBox.ItemsSource = files;
                listBox.DisplayMemberPath = "Name";
                MessageBox.Show($"Файл {filename} был создан!\n{fl}");
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SaveFile(string filename)
        {
            try 
            {
                string fl = $"{fullpath}{filename}";
                //FileStream fileStream = new FileStream(fl, FileMode.OpenOrCreate);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                using (FileStream fs = new FileStream(fl, FileMode.OpenOrCreate))
                {
                    range.Save(fs, DataFormats.Rtf);
                    fs.Close();

                }
                    
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void DeleteFile(string filename)
        {
            string fl = $"{fullpath}{filename}";
            File.Delete(fl);
            //var dir = new System.IO.DirectoryInfo(fullpath);
            //FileInfo[] files = dir.GetFiles(filename);
            //listBox.Items.Remove(files);
            if (listBox.SelectedIndex >= 0)
            {
                int Ind = listBox.SelectedIndex;
                listBox.SelectedIndex = -1;
                listBox.Items.RemoveAt(Ind);
            }
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
        private void saveFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string itm = listBox.SelectedItem.ToString();

            SaveFile(itm);
        }
        private void delFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string itm = listBox.SelectedItem.ToString();

            DeleteFile(itm);
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string itm = listBox.SelectedItem.ToString();
            OpenFile(itm);
        }

        //private void listBox_Selected(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
