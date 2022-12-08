using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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
        private object temp;
        public int SelectionStart { get; set; }
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
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                using (var fs = File.Create(fl))
                {
                    range.Save(fs, System.Windows.DataFormats.Rtf);
                    fs.Close();
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
            try
            {
                string fl = $"{fullpath}{filename}";
                File.Delete(fl);
                var dir = new System.IO.DirectoryInfo(fullpath);
                FileInfo[] files = dir.GetFiles("*.*");
                listBox.ItemsSource = files;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
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

        private void rtbEditor_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateCursorPosition();

        }

        private void ChangedText()
        {
            //string text = "";
            //if (rtbEditor.TextChanged)
            //{
            //    text = "Требуется сохранение";
            //}
            //lblChangedText.Text = "";
        }

        private void UpdateCursorPosition()
        {
            TextPointer tp1 = rtbEditor.Selection.Start.GetLineStartPosition(0);
            TextPointer tp2 = rtbEditor.Selection.Start;

            int column = tp1.GetOffsetToPosition(tp2);

            int someBigNumber = int.MaxValue;
            int lineMoved, currentLineNumber;
            rtbEditor.Selection.Start.GetLineStartPosition(-someBigNumber, out lineMoved);
            currentLineNumber = -lineMoved;

            lblCursorPosition.Text = "Line: " + currentLineNumber.ToString() + " Column: " + column.ToString();
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));
        }
    }
}
