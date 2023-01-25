﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private void ShowAlbumArt()
        {
            var pic = currentFile.Tag.Pictures[0];
            if (pic != null)
            { //Found code for this on 
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);

                // ImageSource for System.Windows.Controls.Image
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();

                // Create a System.Windows.Controls.Image control
                albumImage.Source = bitmap;
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();

            //fileDlg.Filter = "MP3 files (*.mp3)|*.mp3 | All files (*.*)|*.*";
            //ShowDialog() shows onscreen for user.
            //By default it returns ture if the user selects a file and hits open.
            if (fileDlg.ShowDialog() == true)
            {
                //Create tag lib file object, for accessing mp3 meta data.
                currentFile = TagLib.File.Create(fileDlg.FileName);

                myMediaPlayer.Source = new Uri(fileDlg.FileName);
                myMediaPlayer.Play();

                tagButton.IsEnabled = true;
                menuPauseButton.IsEnabled = true;
                pauseButton.IsEnabled = true;
                menuStopButton.IsEnabled = true;
                stopButton.IsEnabled = true;

                ShowAlbumArt();

                infoTitle.Text = currentFile.Tag.Title;
                infoArtist.Text = currentFile.Tag.Artists.Any() ? currentFile.Tag.Artists[0] : "Unknown Artist.";
                infoAlbum.Text = currentFile.Tag.Album + " (" + currentFile.Tag.Year + ")"; 
            }
        }

        private void ShowTags_Click(object sender, RoutedEventArgs e)
        {
            //Examples of reading tag data from currently selected file
            if (currentFile != null)
            {
                var year = currentFile.Tag.Year;
                var title = currentFile.Tag.Title;
            }
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            myMediaPlayer.Play();
            pauseButton.IsEnabled = true;
            menuPauseButton.IsEnabled = true;
            stopButton.IsEnabled = true;
            menuStopButton.IsEnabled = true;
            playButton.IsEnabled = false;
            menuPlayButton.IsEnabled = false;
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            myMediaPlayer.Pause();
            playButton.IsEnabled = true;
            menuPlayButton.IsEnabled = true;
            pauseButton.IsEnabled = false;
            menuPauseButton.IsEnabled = false;
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            myMediaPlayer.Stop();
            playButton.IsEnabled = true;
            menuPlayButton.IsEnabled = true;
            stopButton.IsEnabled = false;
            menuStopButton.IsEnabled = false;
            pauseButton.IsEnabled = false;
            menuPauseButton.IsEnabled = false;
        }
    }
}
