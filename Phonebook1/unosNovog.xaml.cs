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
using static Phonebook1.model.Kontakt;
using static Phonebook1.MainWindow;
using System.Security.Cryptography.X509Certificates;

namespace Phonebook1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        public Window1(Kontakt? kon) : this() 
        {
            Kon = kon;
        }

        public Kontakt? Kon { get; set; }
       

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnUnos_Click(object sender, RoutedEventArgs e)
        {
            //String ime = txtbIme.Text;
            //String prezime = txtbPrezime.Text;
            //String broj = txtbBroj.Text;

            //Kontakt novi = new Kontakt(ime, prezime, broj);

            //Window.primi


            string ime = txtbIme.Text.Trim();
            string prezime = txtbPrezime.Text.Trim();
            string telefon = txtbBroj.Text.Trim();

            Kon = new Kontakt(ime, prezime, telefon);

            this.DialogResult = true;
        }   

        public Kontakt Odgovor
        {
            get { return Kon;  }
        }



        //Ovo se desava kada se prozor napravi
        // ako je Kon null onda ga 
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (Kon == null)
        //        Kon = new Kontakt();
        //    DataContext = Kon;
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Kon == null)
                Kon = new Kontakt();
            DataContext = Kon; // Odakle nam DataContext???
        }


    }
}
