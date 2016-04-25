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
    /// Interaction logic for NewItem.xaml
    /// </summary>
    public partial class NewItem : Window
    {
        public NewItem()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            iBaseDB db = new iBaseDB();
            ArtistTable at = new ArtistTable();
            at.Id = RandomString(16);
            while (db.ArtistTables.Any(x => x.Id == at.Id)) at.Id = RandomString(16);
            at.Name = itemname.Text;
            at.Genre = genrecontent.Text;
            at.Href = itemhref.Text;
            at.Type = itemtype.Text;
            at.Popularity = (long)Int32.Parse(itempopularity.Text);

            db.ArtistTables.Add(at);
            db.SaveChanges();
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
