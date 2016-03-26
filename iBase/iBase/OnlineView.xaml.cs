using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace iBase
{
    public partial class OnlineView : UserControl
    {
        private Track currentTrack { get; set; }
        public WaveOut waveOut { get; set; }
        public BackgroundWorker bgWorker;
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public OnlineView()
        {
            InitializeComponent();
            Stop.IsEnabled = false;
            Unloaded += ExitEvent;
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
                    InfoBox.Items.Clear();
                    List<Album> albums = spotify.SearchAlbums(term);

                    foreach (Album a in albums)
                    {
                        TreeViewItem newChild = new TreeViewItem();
                        newChild.Header = a.name;
                        newChild.Tag = a.id;
                        newChild.ItemsSource = new string[] { "Link: " + a.href};
                        InfoBox.Items.Add(newChild);
                    }
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
            if (SearchType.SelectedValue.Equals("Track") && InfoBox.SelectedItem != null)
            {
                SpotifyAPI spotify = new SpotifyAPI();

                TreeViewItem item = InfoBox.SelectedItem as TreeViewItem;

                currentTrack = spotify.GetTrackFromID(item.Tag + "");
                Icon.Source = new BitmapImage(new Uri(currentTrack.imageurl, UriKind.Absolute));

                StopPlayBack();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler
                        (bgWorker_ProgressChanged);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                        (bgWorker_RunWorkerCompleted);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.WorkerSupportsCancellation = true;

                Stop.IsEnabled = true;
                bgWorker.RunWorkerAsync(currentTrack.preview_url);
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

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopPlayBack();
        }

        public void StopPlayBack()
        {
            if (bgWorker != null && bgWorker.IsBusy)
            {
                bgWorker.CancelAsync();
            }
            if (waveOut != null)
                waveOut.Stop();
            Stop.IsEnabled = false;
        }

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                lblStatus.Content = "Task Cancelled.";
                StopPlayBack();
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
                bgWorker.ReportProgress(5);
            }
            if (bgWorker.CancellationPending)
            {
                // Set the e.Cancel flag so that the WorkerCompleted event
                // knows that the process was cancelled.
                e.Cancel = true;
                bgWorker.ReportProgress(0);
                return;
            }
            bgWorker.ReportProgress(100);
        }
        void ExitEvent(Object sender, RoutedEventArgs e)
        {
            Loaded -= ExitEvent;
            StopPlayBack();
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
