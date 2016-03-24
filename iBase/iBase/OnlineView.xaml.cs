using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Windows.Shapes;

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
                    
                    InfoBox.Items.Add(albums); ;
                    break;
                case "Artist":
                    string artists = spotify.SearchArtists(term);

                    InfoBox.Items.Add(artists); ;
                    break;
                case "Track":
                    string tracks = spotify.SearchTracks(term);

                    InfoBox.Items.Add(tracks); ;
                    break;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void InfoBox_Loaded(object sender, RoutedEventArgs e)
        {
            // ... Create a TreeViewItem.
            TreeViewItem item = new TreeViewItem();
            item.Header = "Album 1";
            item.ItemsSource = new string[] { "Title 1", "Title 2", "Title 3" };

            // ... Create a second TreeViewItem.
            TreeViewItem item2 = new TreeViewItem();
            item2.Header = "Album 2";
            item2.ItemsSource = new string[] { "Title 1", "Title 2", "Title 3" };

            // ... Get TreeView reference and add both items.
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
                //this.Title = "Selected header: " + item.Header.ToString();
            }
            else if (tree.SelectedItem is string)
            {
                // ... Handle a string.
                //this.Title = "Selected: " + tree.SelectedItem.ToString();
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
