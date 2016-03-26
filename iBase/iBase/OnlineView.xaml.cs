using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace iBase
{
    /// <summary>
    /// Interaction logic for OnlineView.xaml
    /// </summary>
    public partial class OnlineView : UserControl
    {
        Track currentTrack { get; set; }
        WaveOut waveOut { get; set; }
        BackgroundWorker bgWorker;
        AutoResetEvent doneEvent = new AutoResetEvent(false);
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public OnlineView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Search()
        {
            string term = SearchTerm.Text;
            string type = SearchType.Text;
            SpotifyAPI spotify = new SpotifyAPI();
            switch (type)
            {
                case "Album":
                    string albums = spotify.SearchAlbums(term);

                    InfoBox.Items.Add(albums);
                    break;
                case "Artist":
                    InfoBox.Items.Clear();
                    List<Artist> artists = spotify.SearchArtists(term);

                    foreach (Artist a in artists)
                    {
                        TreeViewItem newChild = new TreeViewItem();
                        newChild.Header = a.name;
                        newChild.Tag = a.id;
                        newChild.ItemsSource = new string[] { "Follower: " + a.followers_total, "Link: " + a.href, "Typ: " + a.type, "Popularity: " + a.popularity + "/100" };
                        InfoBox.Items.Add(newChild);
                    }
                    break;
                case "Track":
                    InfoBox.Items.Clear();
                    List<Track> tracks = spotify.SearchTracks(term);

                    foreach (Track a in tracks)
                    {
                        TreeViewItem newChild = new TreeViewItem();
                        newChild.Header = a.name;
                        newChild.Tag = a.id;
                        TimeSpan t = TimeSpan.FromMilliseconds(a.duration_ms);
                        newChild.ItemsSource = new string[] { "Disk number: " + a.disc_number, "Track number: " + a.track_number, "Explicity: " + a.explicity, "Duration: " + String.Format("{0}:{1:D2}", t.Minutes, t.Seconds), "Album:" + a.album, "Link: " + a.href, "Typ: " + a.type, "Popularity: " + a.popularity + "/100" };
                        InfoBox.Items.Add(newChild);
                    }
                    break;
            }
        }
        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            SpotifyAPI spotify = new SpotifyAPI();
            if (sender is TreeViewItem && !((TreeViewItem)sender).IsSelected)
                return;

            Trace.WriteLine(((TreeViewItem)sender).Tag + " Doubleclicked!");
            if (SearchType.SelectedValue.Equals("Track"))
            {
                currentTrack = spotify.GetTrackFromID(((TreeViewItem)sender).Tag + "");
                var image = new Image();
                var fullFilePath = currentTrack.imageurl;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();

                image.Source = bitmap;
                CoverImage.Children.Add(image);

                if (bgWorker != null && bgWorker.IsBusy)
                {
                    bgWorker.CancelAsync();
                }
                if(waveOut != null)
                    waveOut.Stop();
                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler
                        (bgWorker_ProgressChanged);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                        (bgWorker_RunWorkerCompleted);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.WorkerSupportsCancellation = true;

                bgWorker.RunWorkerAsync(currentTrack.preview_url);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void InfoBox_Loaded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = "Doubleclick an entry";
            TreeViewItem item2 = new TreeViewItem();
            item2.Header = "to get more information!";
            var tree = sender as TreeView;
            tree.Items.Add(item);
            tree.Items.Add(item2);
        }

        private void InfoBox_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;

            // ... Determine type of SelectedItem.
            if (tree.SelectedItem is TreeViewItem)
            {
                // ... Handle a TreeViewItem.
                var item = tree.SelectedItem as TreeViewItem;
                //this.Title = "Selected header: " + item.Header;
                Trace.WriteLine("User selected " + item.Header);
            }
            else if (tree.SelectedItem is string)
            {
                // ... Handle a string.
                //this.Title = "Selected: " + tree.SelectedItem;
                Trace.WriteLine("Selected: " + tree.SelectedItem);
            }
        }

        private void SearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchType != null && SearchTerm != null)
                SearchTypeLabel.Content = "Search result for " + SearchType.SelectedValue + ":";
            else
                if (SearchTypeLabel != null)
                SearchTypeLabel.Content = "Search result for Album:";
        }

        void timer_Tick(object sender, EventArgs e)
        {
            //if (mediaPlayer.Source != null)
            //lblStatus.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            //else
            //lblStatus.Content = "No file selected...";
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (bgWorker.IsBusy)
            {
                bgWorker.CancelAsync();
            }
        }

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The background process is complete. We need to inspect
            // our response to see if an error occurred, a cancel was
            // requested or if we completed successfully.  
            if (e.Cancelled)
            {
                lblStatus.Content = "Task Cancelled.";
            }
            else if (e.Error != null)
            {
                lblStatus.Content = "Error while performing background operation.";
            }
            else
            {
                // Everything completed normally.
            }
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string url = e.Argument + "";
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            //NOTE : Never play with the UI thread here...
            Stream ms = new MemoryStream();
            Stream stream = WebRequest.Create(url)
                    .GetResponse().GetResponseStream();
            byte[] buffer = new byte[32768];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            ms.Position = 0;
            WaveStream blockAlignedStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(ms)));
            waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
            waveOut.Init(blockAlignedStream);
            waveOut.Play();

            while (waveOut.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(100);
            }


            // Periodically report progress to the main thread so that it can
            // update the UI.  In most cases you'll just need to send an
            // integer that will update a ProgressBar                    
            //bgWorker.ReportProgress(i);
            // Periodically check if a cancellation request is pending.
            // If the user clicks cancel the line
            // m_AsyncWorker.CancelAsync(); if ran above.  This
            // sets the CancellationPending to true.
            // You must check this flag in here and react to it.
            // We react to it by setting e.Cancel to true and leaving
            if (bgWorker.CancellationPending)
            {
                // Set the e.Cancel flag so that the WorkerCompleted event
                // knows that the process was cancelled.
                e.Cancel = true;
                bgWorker.ReportProgress(0);
                waveOut.Stop();

                return;
            }
            bgWorker.ReportProgress(100);
        }
    }
    public class ActionCommand : ICommand
    {
        private readonly Action _action;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
