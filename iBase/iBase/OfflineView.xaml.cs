using System;
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
using System.Windows.Shapes;

namespace iBase
{
    /// <summary>
    /// Interaction logic for OfflineView.xaml
    /// </summary>
    public partial class OfflineView : UserControl
    {
        iBaseDB iBase = new iBaseDB();
        public OfflineView()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            NewItem i = new NewItem();
            i.ShowDialog();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            iBase.SaveChanges();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string type = Sep(LeftListBox.SelectedItem.GetType().Name);
            switch (type)
            {
                case "ArtistTable":
                    ArtistTable at = LeftListBox.SelectedItem as ArtistTable;
                    if (at != null)
                    {
                        iBase.ArtistTables.Remove(at);
                        LeftListBox.Items.Refresh();
                    }
                    break;
                case "AlbumTable":
                    AlbumTable al = LeftListBox.SelectedItem as AlbumTable;
                    if (al != null)
                    {
                        iBase.AlbumTables.Remove(al);
                        LeftListBox.Items.Refresh();
                    }
                    break;
                case "TrackTable":
                    TrackTable tb = LeftListBox.SelectedItem as TrackTable;
                    if (tb != null)
                    {
                        iBase.TrackTables.Remove(tb);
                        LeftListBox.Items.Refresh();
                    }
                    break;
                default:
                    break;
            }
        }

        private void OfflineView_Loaded(object sender, RoutedEventArgs e)
        {
            TableSelection.SelectedIndex = 0;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (TableSelection.SelectedValue.ToString())
            {
                case "Artist":
                    LeftListBox.ItemsSource = iBase.ArtistTables.ToList();
                    break;
                case "Album":
                    LeftListBox.ItemsSource = iBase.AlbumTables.ToList();
                    break;
                case "Track":
                    LeftListBox.ItemsSource = iBase.TrackTables.ToList();
                    break;

            }
        }
        public static string Sep(string s)
        {
            int l = s.IndexOf("_");
            if (l > 0)
            {
                return s.Substring(0, l);
            }
            return "";

        }
    }

}
