using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
        private ICommand someCommand;
        public ICommand SomeCommand
        {
            get
            {
                return someCommand
                    ?? (someCommand = new ActionCommand(() =>
                    {
                        Search();
                    }));
            }
        }
        public OnlineView()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //LeftListBoxName.ItemsSource = db.klassens.ToList();

        }

        private void LeftListBoxName_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            if(SearchType.SelectedValue.Equals("Track"))
            {
                PlayTrack(spotify.GetTrackFromID(((TreeViewItem)sender).Tag + "").preview_url);
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

        private MediaPlayer mediaPlayer = new MediaPlayer();

        public void PlayTrack(string url)
        {
            //WebClient webClient = new WebClient();
            //string path = System.IO.Path.GetTempPath() + "\\" + RandomString() + ".mp3";
            //webClient.DownloadFile(url, path);

            //mediaPlayer.Open(new Uri(path));

            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += timer_Tick;
            //timer.Start();
            PlayMp3FromUrl(url);
        }
        public static void PlayMp3FromUrl(string url)
        {
            using (Stream ms = new MemoryStream())
            {
                using (Stream stream = WebRequest.Create(url)
                    .GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                ms.Position = 0;
                using (WaveStream blockAlignedStream =
                    new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(ms))))
                {
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
                lblStatus.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            else
                lblStatus.Content = "No file selected...";
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();

        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();

        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();

        }
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 11)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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
