using Microsoft.Win32;
using Phonebook1.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Phonebook1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        

        public MainWindow()
        {

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded); //sta je ovo
            InitializeComponent();            
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LB2.ItemsSource = kontakti;
            LV1.ItemsSource = kontakti; // ovo radi za toString
            //LB2.DisplayMemberPath = "Ime"; // TO DO kako da ovde lepo ispise
        }

        ObservableCollection<Kontakt> kontakti = new();








        private void btnUnosNovog_Click(object sender, RoutedEventArgs e)
        {
            //Window1 win1 = new Window1();
            //win1.Show();
            var dialog = new Window1();

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                kontakti.Add(dialog.Odgovor);
            }
            else
            {
                MessageBox.Show("Greska");
            }

        }

        private void btwExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //CP
        private void btnEditKontakt_Click(object sender, RoutedEventArgs e)
        {
            Kontakt? sel = LB2.SelectedItem as Kontakt;

            if (sel != null)
            {
                Window1 kontakti = new Window1(sel);
                kontakti.ShowDialog();
            }
        }// sta je ovo

        /// <summary>
        /// Metod za pretrazivanje Kontakata.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrikaz_Click(object sender, RoutedEventArgs e)
        { 
            Trace.WriteLine("################ PRETRAGA ####################");

            ObservableCollection<Kontakt> pretrazena;

            var dialog = new Pretrazivanje();

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string imeT;
                string prezimeT;
                string telefonT;
                
                imeT = dialog.Odgovor.Ime;
                prezimeT = dialog.Odgovor.Prezime;
                telefonT = dialog.Odgovor.Telefon;

                int opcija = 0;

                if (imeT.Length > 0) opcija = opcija + 1;
                if (prezimeT.Length > 0) opcija = opcija + 2;
                if (telefonT.Length > 0) opcija = opcija + 5;

                //IEnumerable<Kontakt> filteringQuery;


                switch (opcija)
                {  
                    //imamo samo ime
                    case 1:
                        IEnumerable<Kontakt> filteringQuery = 
                        from kon in kontakti
                        where String.Equals(imeT, kon.Ime, StringComparison.OrdinalIgnoreCase) // String.Equals(root, root2, StringComparison.OrdinalIgnoreCase);
                        select kon;

                        //where imeT.Equals(kon.Ime)

                        Trace.WriteLine(" - - - - - - Ovo je iz IEnumerable");
                        foreach (Kontakt kon in filteringQuery) {
                            Trace.WriteLine(kon);
                        }

                        pretrazena = new ObservableCollection<Kontakt>(filteringQuery.Cast<Kontakt>());

                        Trace.WriteLine(" - - - - - - Ovo je iz Observable coll");
                        foreach (Kontakt kon in pretrazena)
                        {
                            Trace.WriteLine(kon);
                        }
                       break;


                    //imamo samo perzime
                    case 2:
                        break;

                    //imamo ime i prezime
                    case 3:
                        break;

                    //imamo telefon
                    case 5:
                        break;


                    // imamo ime i telefon
                    case 6:
                        break;

                    //imamo prezime i telefon 
                    case 7:
                        break;

                    //imamo sve
                    case 8:
                        break;

                    default:
                        MessageBox.Show("Greska!!! usli smo u default");
                        break;
                }


                

            }
            else{
                MessageBox.Show("Greska!!! - dijalog nije dobro vratio-");
            }

           

        }

        private void btnUvozCSV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new();
                bool? result = ofd.ShowDialog();

                if (result == true)
                {
                    string fileName = ofd.FileName;
                    using (StreamReader sr = new(ofd.FileName))
                    {
                        string? line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] tokens = line.Split(",");
                            string ime = tokens[0].Trim();
                            string prezime = tokens[1].Trim();
                            string telefon = tokens[2].Trim();
                            kontakti.Add(new(ime, prezime, telefon));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }//uvoz CSV

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new();
                bool? result = sfd.ShowDialog();

                if (result == true)
                {
                    string fileName = sfd.FileName;
                    using (StreamWriter sw = new(fileName))
                    {
                        foreach (Kontakt kontakt in kontakti)
                        {
                            sw.WriteLine(kontakt);
                        }
                        MessageBox.Show("Uspesno snimljen imenik!", "Success", MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        
        private void btnExportJSON_Click(object sender, RoutedEventArgs e)
        {


            //string json = JsonSerializer.Serialize(imenik);
            //File.WriteAllText(@"C:\Users\grani\OneDrive\Desktop\fakultet\5 semestar\Seminarski C C# i .NET\domaci 2 Telefonski imenik\program\test 1\Phonebook1\Phonebook1\fajlovi\path.json", json);


            try
            {
                SaveFileDialog sfd = new();
                bool? result = sfd.ShowDialog();

                if (result == true)
                {
                    string fileName = sfd.FileName;

                   

                    string jsonZaExport = Newtonsoft.Json.JsonConvert.SerializeObject(kontakti);
    

                    using (StreamWriter sw = new(fileName))
                    {
                        
                        //sw.WriteLine(jsonZaExport);
                        sw.Write(jsonZaExport);
                        MessageBox.Show("Uspesno snimljen imenik!", "Success", MessageBoxButton.OK);
                    }



                 


                    //Trace.WriteLine("Nas JSON je: " + jsonZaExport);

                    Trace.WriteLine("USPESO SMO NAPRAVILI JSON FILE!!!!------------ ");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



        }//export
         //JSON

        private void btnInportJSON_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new();
                bool? result = ofd.ShowDialog();

                if (result == true)
                {


                    string fileName = ofd.FileName;
                    string? line = "";
                    List<Kontakt> novi;
                    using (StreamReader sr = new(ofd.FileName))
                    {
                        line = line + sr.ReadLine();

                        novi = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Kontakt>>(line);

                        foreach (Kontakt k in novi)

                        {
                            kontakti.Add(k);  

                        }

                        //kontakti = JsonConvert.DeserializeObject<ObservableCollection<Kontakt>>(line);
                    }

                    


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }// import JSON

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Kontakt? sel = LB2.SelectedItem as Kontakt;

            if (sel != null)
            {
                Window1 kontakti = new Window1(sel);
                kontakti.ShowDialog();
            }
            else
            {
                sel = LV1.SelectedItem as Kontakt;
                if (sel != null)
                {
                    Window1 kontakti = new Window1(sel);
                    kontakti.ShowDialog();
                }

            }
        }


    }// main window
}



//Console.WriteLine("WH!!1");

//Trace.WriteLine("--------------------------------------------------------------");
//Trace.WriteLine("--------------------------------------------------------------");
//Trace.WriteLine("--------------------------------------------------------------");

//Ovo je nas glavni imenik
//Imenik imenik = new Imenik();
//LB2.ItemsSource = (System.Collections.IEnumerable)imenik;


//Kontakt kontakt = new Kontakt("Petar", "Peric", "123 456 789");
//Kontakt kontakt2 = new Kontakt("Marko", "MArkovic", "954 654 321");

//imenik.Add(kontakt);
//imenik.Add(kontakt2);

//foreach (Kontakt item in imenik)
//{
//    Trace.WriteLine(item.ToString());
//}

//= String[imenik.getSize()]
//String[] imenikStr = new string[imenik.getSize()];

//for (int i = 0; i < imenik.getSize(); i++)
//{
//    imenikStr[i] = imenik.getByIndex(i).ToString();
//}


//Trace.WriteLine("Ovo je sa array-a!!!");
//foreach (var item in imenikStr)
//{
//    Trace.WriteLine(item);
//}

//LB2 = new ListBox();
//(System.Collections.IEnumerable)
//LB2.ItemsSource = imenikStr;//Ovde je greska

//Trace.WriteLine("--------------------------------------------------------------");
//Trace.WriteLine("--------------------------------------------------------------");
//Trace.WriteLine("--------------------------------------------------------------");
