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
        private void SaveButtonName_Click(object sender, RoutedEventArgs e)
        {
            //db.SaveChanges();
        }

        private void NewClick_Click(object sender, RoutedEventArgs e)
        {
            /*schueler s1 = new schueler();
            s1.S_Name = "NEW";
            s1.S_K_Klasse = ((klassen)(LeftListBoxName.SelectedItem)).K_ID;
            db.schuelers.Add(s1);
            ((klassen)(LeftListBoxName.SelectedItem)).schuelers.Add(s1);
            MiddleListBoxName.Items.Refresh();*/
            NewItem i = new NewItem();
            i.ShowDialog();
        }

        private void DeleteClick_Click(object sender, RoutedEventArgs e)
        {
            /*
            schueler s1 = MiddleListBoxName.SelectedItem as schueler;
            if (s1 != null)
            {
                //Would delete only the FK!
                //((klassen)LeftListBoxName.SelectedItem).schuelers.Remove(s1);

                db.schuelers.Remove(s1);
                MiddleListBoxName.Items.Refresh();
            }
            */
        }

        private void OfflineView_Loaded(object sender, RoutedEventArgs e)
        {
            LeftListBox.ItemsSource = iBase.AlbumTables.ToList();
            /*TO DO: Alphabetical sorting of LeftListBox*/
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (TableSelection.Text)
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
    }

}
