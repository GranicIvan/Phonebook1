using Phonebook1.model;
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

namespace Phonebook1
{
    /// <summary>
    /// Interaction logic for Pretrazivanje.xaml
    /// </summary>
    public partial class Pretrazivanje : Window
    {
        public Pretrazivanje()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnUnos_Click(object sender, RoutedEventArgs e)
        {

            string ime = txtbIme.Text.Trim();
            string prezime = txtbPrezime.Text.Trim();
            string telefon = txtbBroj.Text.Trim();

            Kon = new Kontakt(ime, prezime, telefon);

            this.DialogResult = true;

        }








        public Kontakt? Kon { get; set; }


        public Kontakt Odgovor
        {
            get { return Kon; }
        }




    }
}
