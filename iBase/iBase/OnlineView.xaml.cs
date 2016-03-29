using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        SpotifyAPI spotify = new SpotifyAPI();
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

        private void Search()
        {
            string term = SearchTerm.Text;
            string type = SearchType.Text;
            switch (type)
            {
                case "Album":
                    InfoBox.Items.Clear();
                    List<AlbumTable> albums = spotify.SearchAlbums(term);

                    if (albums != null)
                        foreach (AlbumTable a in albums)
                        {
                            TreeViewItem newChild = new TreeViewItem();
                            string header = a.name;
                            newChild.Tag = a.id;

                            List<string> albumitems = new List<string>();

                            List<string> tracksList = new List<string>(a.tracks.Keys);
                            if (tracksList.Count > 0)
                            {
                                albumitems.Add("Tracks:");
                                foreach (string trackname in tracksList.ToList())
                                {
                                    albumitems.Add("- " + trackname);
                                }
                            }
                            string[] artistsList = (new List<string>(a.artists.Keys)).Select(i => i.ToString()).ToArray();

                            if (artistsList.Length == 1)
                            {
                                header += " by " + artistsList[0];
                            }
                            else if (artistsList.Length > 1)
                            {
                                albumitems.Add("Artists:");
                                foreach (string artistname in artistsList)
                                {
                                    albumitems.Add("- " + artistname);
                                }
                            }
                            newChild.Header = header;

                            newChild.ItemsSource = albumitems;
                            InfoBox.Items.Add(newChild);
                        }
                    else
                    {
                        TreeViewItem newChild = new TreeViewItem();
                        newChild.Header = "No internet connection!";
                        InfoBox.Items.Add(newChild);
                    }
                    break;
                case "Artist":
                    InfoBox.Items.Clear();
                    List<Artist> artists = spotify.SearchArtists(term);

                    if (artists != null)
                        foreach (Artist a in artists)
                        {
                            TreeViewItem newChild = new TreeViewItem();
                            newChild.Header = a.name;
                            newChild.Tag = a.id;
                            newChild.ItemsSource = new string[] { "Follower: " + a.followers_total, "Link: " + a.href, "Typ: " + a.type, "Popularity: " + a.popularity + "/100" };
                            InfoBox.Items.Add(newChild);
                        }
                    else
                    {
                        TreeViewItem newChild = new TreeViewItem();
                        newChild.Header = "Artists null!";
                        InfoBox.Items.Add(newChild);
                    }
                    break;
                case "Track":
                    InfoBox.Items.Clear();
                    List<Track> tracks = spotify.SearchTracks(term);
                    if (tracks != null)
                        foreach (Track track in tracks)
                        {
                            TreeViewItem newChild = new TreeViewItem();
                            newChild.Header = track.name;
                            newChild.Tag = track.id;
                            TimeSpan t = TimeSpan.FromMilliseconds(track.duration_ms);
                            newChild.ItemsSource = new string[] { "Disk number: " + track.disc_number, "Track number: " + track.track_number, "Explicity: " + track.explicity, "Duration: " + String.Format("{0}:{1:D2}", t.Minutes, t.Seconds), "Album:" + track.album, "Link: " + track.href, "Typ: " + track.type, "Popularity: " + track.popularity + "/100" };
                            InfoBox.Items.Add(newChild);
                        }
                    else
                    {
                        TreeViewItem newChild = new TreeViewItem();
                        newChild.Header = "No internet connection!";
                        InfoBox.Items.Add(newChild);
                    }
                    break;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void InfoBox_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SearchType.SelectedValue.Equals("Track") && InfoBox.SelectedItem != null)
            {
                TreeViewItem item = InfoBox.SelectedItem as TreeViewItem;

                currentTrack = spotify.GetTrackFromID(item.Tag + "");

                PlayCurrentTrack();
            }
        }

        public void PlayCurrentTrack()
        {
            StopPlayBack();
            Icon.Source = new BitmapImage(new Uri(currentTrack.imageurl, UriKind.Absolute));

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;

            Stop.IsEnabled = true;
            bgWorker.RunWorkerAsync(currentTrack.preview_url);
        }

        private void SearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchType != null && SearchTerm != null)
                SearchTypeLabel.Content = "Search result for " + SearchType.SelectedValue + ":";
            else if (SearchTypeLabel != null)
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
                PlayingStatus.Content = "Nothing playing.";
                StopPlayBack();
            }
            else if (e.Error != null)
                PlayingStatus.Content = "Could not play track. Try again.";
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PlayingStatus.Content = "Playing: " + currentTrack.name;
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
            }
            if (bgWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }
        void ExitEvent(Object sender, RoutedEventArgs e)
        {
            Loaded -= ExitEvent;
            StopPlayBack();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (currentTrack != null)
            {
                PlayCurrentTrack();
            }
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
