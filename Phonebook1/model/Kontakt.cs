using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;


namespace Phonebook1.model
{
    public class Kontakt : INotifyPropertyChanged
    {
        private string ime;
        private string prezime;
        private string telefon;

        /// <summary>
        /// Ime kontakta
        /// </summary>
        public string Ime
        {
            get { return ime; }
            set
            {
                ime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Prezime kontakta
        /// </summary>
        public string Prezime
        {
            get { return prezime; }
            set
            {
                prezime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Telefon kontakta
        /// </summary>
        public string Telefon
        {
            get { return telefon; }
            set
            {
                telefon = value;
                RaisePropertyChanged();
            }
        }











        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="ime"></param>
        /// <param name="prezime"></param>
        /// <param name="telefon"></param>
        public Kontakt(string ime, string prezime, string telefon)
        {
            this.Ime = ime;
            this.Prezime = prezime;
            this.Telefon = telefon;
        }// konstruktor


        /// <summary>
        /// Prazan konstruktor
        /// </summary>
        public Kontakt()
        {
        }




        /// <summary>
        /// Dogadjaj za izmenu property values nad objektom
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;


        //PropertyChanged metod
        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public override string ToString()
        {
            return new string($"{Ime}, {Prezime}, {Telefon}");
        }



    }
}
