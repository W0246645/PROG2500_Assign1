using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace MediaTagger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TagLib.File currentFile;
        string fileName;

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

        private void SetInfo()
        {
            infoTitle.Text = currentFile.Tag.Title;
            infoArtist.Text = currentFile.Tag.Performers.Any() ? currentFile.Tag.Performers[0] : "Unknown Artist.";
            infoAlbum.Text = currentFile.Tag.Album + " (" + currentFile.Tag.Year + ")";
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();

            //fileDlg.Filter = "MP3 files (*.mp3)|*.mp3 | All files (*.*)|*.*";
            //ShowDialog() shows onscreen for user.
            //By default it returns ture if the user selects a file and hits open.
            if (fileDlg.ShowDialog() == true)
            {
                fileName = fileDlg.FileName;
                //Create tag lib file object, for accessing mp3 meta data.
                currentFile = TagLib.File.Create(fileName);
                currentFile.Dispose();

                myMediaPlayer.Source = new Uri(fileName);
                myMediaPlayer.Play();

                tagButton.IsEnabled = true;
                menuPauseButton.IsEnabled = true;
                pauseButton.IsEnabled = true;
                menuStopButton.IsEnabled = true;
                stopButton.IsEnabled = true;

                ShowAlbumArt();
                SetInfo();
                
            }
        }

        private void ShowTags_Click(object sender, RoutedEventArgs e)
        {
            //Examples of reading tag data from currently selected file
            if (currentFile != null)
            {
                editYear.Text = currentFile.Tag.Year.ToString();
                editTitle.Text = currentFile.Tag.Title;
                editArtist.Text = currentFile.Tag.Performers[0];
                editAlbum.Text = currentFile.Tag.Album;

                tagEditorPanel.Visibility = Visibility.Visible;
                infoPanel.Visibility = Visibility.Hidden;
            }
        }

        private void SaveTags_Click(object sender, RoutedEventArgs e)
        {
            currentFile.Tag.Year = (uint)int.Parse(editYear.Text);
            currentFile.Tag.Title = editTitle.Text;
            currentFile.Tag.Album= editAlbum.Text;

            var position = myMediaPlayer.Position;
            myMediaPlayer.Stop();
            myMediaPlayer.Source = null;
            Thread.Sleep(50);//The thread was setting null and saving too quicky causing an overlap and crashing. Sleep for 50ms to give a buffer.
            currentFile.Save();
            currentFile.Dispose();
            myMediaPlayer.Source = new Uri(fileName);
            myMediaPlayer.Play();
            myMediaPlayer.Position = position;
            if (playButton.IsEnabled) //Set song mode back to what it was before the save.
            {
                myMediaPlayer.Pause();
            }

            

            tagEditorPanel.Visibility = Visibility.Hidden;
            infoPanel.Visibility = Visibility.Visible;
            SetInfo();
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

        // When the media opens, initialize the "Seek To" slider maximum value
        // to the total number of miliseconds in the length of the media clip.
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            timelineSlider.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void MediaTimeChanged(object sender, EventArgs e)
        {
            timelineSlider.Value = myMediaElement.Position.TotalMilliseconds;
        }
    }
}
