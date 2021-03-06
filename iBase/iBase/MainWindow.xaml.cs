﻿using System;
using System.Collections.Generic;
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

namespace iBase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowSpace.Children.Add(new OnlineView());
        }
        private void OnlineMenuItemClick(object sender, RoutedEventArgs e)
        {
            MainWindowSpace.Children.Clear();
            var nc = new OnlineView();
            MainWindowSpace.Children.Add(nc);
        }

        private void OfflineMenuItemClick(object sender, RoutedEventArgs e)
        {
            MainWindowSpace.Children.Clear();
            var nc = new OfflineView();
            MainWindowSpace.Children.Add(nc);
        }

        private void AllArtistsClick (object sender, RoutedEventArgs e) {
            MainWindowSpace.Children.Clear();
            var nc = new AllArtists();
            MainWindowSpace.Children.Add(nc);
        }

        private void AllAlbumsClick (object sender, RoutedEventArgs e) {
            MainWindowSpace.Children.Clear();
            var nc = new AllAlbums();
            MainWindowSpace.Children.Add(nc);
        }

        private void AllTracksClick (object sender, RoutedEventArgs e) {
            MainWindowSpace.Children.Clear();
            var nc = new AllTracks();
            MainWindowSpace.Children.Add(nc);
        }
    }
}
