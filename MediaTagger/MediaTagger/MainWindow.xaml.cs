using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MediaTagger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TagLib.File currentFile;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();

            //fileDlg.Filter = "MP3 files (*.mp3)|*.mp3 | All files (*.*)|*.*";
            //ShowDialog() shows onscreen for user.
            //By default it returns ture if the user selects a file and hits open.
            if (fileDlg.ShowDialog() == true)
            {
                fileNameBox.Text = fileDlg.FileName;

                //Create tag lib file object, for accessing mp3 meta data.
                currentFile = TagLib.File.Create(fileDlg.FileName);

                myMediaPlayer.Source = new Uri(fileDlg.FileName);
            }
        }

        private void ShowTags_Click(object sender, RoutedEventArgs e)
        {
            //Examples of reading tag data from currently selected file
            if (currentFile != null)
            {
                var year = currentFile.Tag.Year;
                var title = currentFile.Tag.Title;

                tagNameBox.Text = title + " : " + year
            }
        }
    }
}
