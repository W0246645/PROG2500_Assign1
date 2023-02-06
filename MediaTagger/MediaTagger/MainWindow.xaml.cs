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
        DispatcherTimer timer;
        bool isPlaying = false;
        bool isSeeking = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowAlbumArt()
        {
            var pic = currentFile.Tag.Pictures[0];
            if (pic != null)
            {
                try
                {
                    //Found code for this on stackoverflow
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
                catch (Exception ex) { Console.WriteLine(ex); }
            }
        }

        private void SetInfo()
        {
            infoPanel.infoTitle.Text = currentFile.Tag.Title;
            infoPanel.infoArtist.Text = currentFile.Tag.Performers.Any() ? currentFile.Tag.Performers[0] : "Unknown Artist.";
            infoPanel.infoAlbum.Text = currentFile.Tag.Album + " (" + currentFile.Tag.Year + ")";
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();

            //fileDlg.Filter = "MP3 files (*.mp3)|*.mp3 | All files (*.*)|*.*";
            //ShowDialog() shows onscreen for user.
            //By default it returns ture if the user selects a file and hits open.
            if (fileDlg.ShowDialog() == true)
            {
                try
                {
                    fileName = fileDlg.FileName;
                    //Create tag lib file object, for accessing mp3 meta data.
                    currentFile = TagLib.File.Create(fileName);
                    currentFile.Dispose();

                    myMediaPlayer.Source = new Uri(fileName);
                    myMediaPlayer.Play();
                    isPlaying = true;


                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(300);
                    timer.Tick += TimerTick;
                    timer.Start();


                    tagButton.IsEnabled = true;
                    menuPauseButton.IsEnabled = true;
                    pauseButton.IsEnabled = true;
                    menuStopButton.IsEnabled = true;
                    stopButton.IsEnabled = true;

                    ShowAlbumArt();
                    SetInfo();
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            if ((myMediaPlayer.Source != null) && (myMediaPlayer.NaturalDuration.HasTimeSpan) && (!isSeeking))
            {
                seekBar.Minimum = 0;
                seekBar.Maximum = myMediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                seekBar.Value = myMediaPlayer.Position.TotalMilliseconds;
                timerTextBlock.Text = TimeSpan.FromMilliseconds(seekBar.Value).ToString(@"hh\:mm\:ss");
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
            currentFile.Tag.Album = editAlbum.Text;

            var position = myMediaPlayer.Position;
            try
            {
                myMediaPlayer.Stop();
                myMediaPlayer.Source = null;
                Thread.Sleep(50);//The thread was setting null and saving too quicky causing an overlap and crashing. Sleep for 50ms to give a buffer.
                currentFile.Save();
                currentFile.Dispose();
                myMediaPlayer.Source = new Uri(fileName);
                myMediaPlayer.Play();
                myMediaPlayer.Position = position;
                if (!isPlaying) //Set song mode back to what it was before the save.
                {
                    myMediaPlayer.Pause();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }



            tagEditorPanel.Visibility = Visibility.Hidden;
            infoPanel.Visibility = Visibility.Visible;
            SetInfo();
        }

        // When the media opens, initialize the "Seek To" slider maximum value
        // to the total number of miliseconds in the length of the media clip.
        private void SeekToMediaPosition(object sender, EventArgs e)
        {
            int sliderValue = (int)seekBar.Value;

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, sliderValue);
            myMediaPlayer.Position = ts;
        }

        private void CanExecuteCloseCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExecutedCloseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = myMediaPlayer.Source != null && !isPlaying;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isPlaying;
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isPlaying;
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaPlayer.Play();
            isPlaying = true;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaPlayer.Pause();
            isPlaying = false;
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaPlayer.Stop();
            isPlaying = false;
        }
    }
}
