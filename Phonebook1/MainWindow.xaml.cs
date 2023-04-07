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
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text.Json.Nodes;
using System.Runtime.InteropServices;
using Refit;
using System.Threading.Tasks;

namespace Phonebook1
{




    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {


        private static readonly HttpClient client = new HttpClient();


        /// <summary>
        /// Konstruktor
        /// </summary>
        public MainWindow()
        {

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded); //sta je ovo
            InitializeComponent();
            kontakti = new();
        }


        /// <summary>
        /// kontakti 
        /// </summary>
        /// <param name="kontakti">Podaci koji se prikazuju</param>
        public MainWindow(ObservableCollection<Kontakt> kontakti)
        {

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded); //sta je ovo
            InitializeComponent();
            this.kontakti = kontakti;
        }





        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LB2.ItemsSource = kontakti;
            LV1.ItemsSource = kontakti; // ovo radi za toString
            //LB2.DisplayMemberPath = "Ime"; // TO DO kako da ovde lepo ispise
        }

        ObservableCollection<Kontakt> kontakti;








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

            //ObservableCollection<Kontakt> pretrazena;

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

                //int opcija = 0;

                //if (imeT.Length > 0) opcija = opcija + 1;
                //if (prezimeT.Length > 0) opcija = opcija + 2;
                //if (telefonT.Length > 0) opcija = opcija + 5;

                //IEnumerable<Kontakt> filteringQuery;


                // TEST
                IEnumerable<Kontakt> trazena = kontakti.Where( x =>
                                                        ( String.Equals(imeT, x.Ime, StringComparison.OrdinalIgnoreCase) || imeT.Equals("") ) &&
                                                        ( String.Equals(prezimeT, x.Prezime, StringComparison.OrdinalIgnoreCase) || prezimeT.Equals("") ) &&
                                                        (String.Equals(telefonT, x.Telefon, StringComparison.OrdinalIgnoreCase) || telefonT.Equals(""))
                                                        );
                Trace.WriteLine("Trazeni kontakti su: ");
                foreach (Kontakt kon in trazena)
                {
                    Trace.WriteLine(kon);
                }


                ObservableCollection<Kontakt> trazenaOC = new ObservableCollection<Kontakt>(trazena);
                //trazenaOC = (ObservableCollection<Kontakt>)trazena;

                MainWindow trazeni = new MainWindow(trazenaOC); 
                trazeni.Show();




                //    switch (opcija)
                //    {  
                //        //imamo samo ime
                //        case 1:
                //            IEnumerable<Kontakt> filteringQuery = 
                //            from kon in kontakti
                //            where String.Equals(imeT, kon.Ime, StringComparison.OrdinalIgnoreCase) // String.Equals(root, root2, StringComparison.OrdinalIgnoreCase);
                //            select kon;

                //            //where imeT.Equals(kon.Ime)

                //            Trace.WriteLine(" - - - - - - Ovo je iz IEnumerable");
                //            foreach (Kontakt kon in filteringQuery) {
                //                Trace.WriteLine(kon);
                //            }

                //            pretrazena = new ObservableCollection<Kontakt>(filteringQuery.Cast<Kontakt>());

                //            Trace.WriteLine(" - - - - - - Ovo je iz Observable coll");
                //            foreach (Kontakt kon in pretrazena)
                //            {
                //                Trace.WriteLine(kon);
                //            }
                //           break;


                //        //imamo samo perzime
                //        case 2:
                //            break;

                //        //imamo ime i prezime
                //        case 3:
                //            break;

                //        //imamo telefon
                //        case 5:
                //            break;


                //        // imamo ime i telefon
                //        case 6:
                //            break;

                //        //imamo prezime i telefon 
                //        case 7:
                //            break;

                //        //imamo sve
                //        case 8:
                //            break;

                //        default:
                //            MessageBox.Show("Greska!!! usli smo u default");
                //            break;
                //    }




            }
            else
            {
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
                Trace.WriteLine(ex.Message);
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
                Trace.WriteLine(ex.Message);
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
                Trace.WriteLine(ex.Message);
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
                Trace.WriteLine(ex.Message);
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


        private async  void btnAzure_Click(object sender, RoutedEventArgs e)
        //private void btnAzure_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Kliknuli smo Azure funk");

            

            try
            {
                
                string jsonZaBody = Newtonsoft.Json.JsonConvert.SerializeObject(kontakti);

                //Trace.WriteLine(jsonZaBody);


                try
                {



                    string functionUrl = "https://contactlistig.azurewebsites.net/api/VratiListuKontakata?code=U0Jo6mfgn8igMFDEYPHfoQ4Sdo_pSmmj0cQfBsVQE3yUAzFuwjkCpg==";
                    var response = await CallAzureFunctionAsync(functionUrl, jsonZaBody);
                    //var response = await CallAzureFunctionAsync(functionUrl, "Ivan");


                    // ovo treba var response = CallAzureFunctionAsync(functionUrl, jsonZaBody).Result;
                    //var response = CallAzureFunctionAsync(functionUrl, "Ivan").Result;

                    Trace.WriteLine($"Odgovor funkcije: {response}");

                    List<string>  dobijenaImena = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(response);

                    Trace.WriteLine("ispisujemo dobijena imena: ");
                    foreach (string s in dobijenaImena) {
                        Trace.WriteLine(s);

                    }


                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error: {ex.Message}");
                }

                // mozda ne treba async ovde
                async Task<string> CallAzureFunctionAsync(string url, string? dobijeniKon)
                {
                    if (!String.IsNullOrEmpty(dobijeniKon))
                    {
                        url += ("&name=" + dobijeniKon);

                    }

                    //HttpResponseMessage response = await client.GetAsync(url);
                    var content = new StringContent(jsonZaBody, Encoding.UTF8, "application/json");
                    //Trace.WriteLine("content je: " + content);
                    //Trace.WriteLine("content to str: " + content.ToString);
                    HttpResponseMessage response = client.PostAsync(url, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception($"Error u pozivu Azure funkcije. err: {response.StatusCode}");
                    }
                }




            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }




            //try
            //{
            //    string functionUrl = "https://phonebookivan.azurewebsites.net/api/HttpTriggerTest1?code=uEZgqfXnyqPD-jC5P82E4UP-X6VGQAK_79XYPJnA459nAzFuZfVypA==";
            //    var response = await CallAzureFunctionAsync(functionUrl, jsonZaBody);
            //    var response = CallAzureFunctionAsync(functionUrl, jsonZaBody).Result;
            //    Trace.WriteLine($"Odgovor funkcije: {response}");
            //}
            //catch (Exception ex)
            //{
            //    Trace.WriteLine($"Error: {ex.Message}");
            //}

            //public static async Task<string> CallAzureFunctionAsync(string url, string? ime)
            //{
            //    if (!String.IsNullOrEmpty(ime))
            //    {
            //        url += ("&name=" + ime);

            //    }

            //    if (response.IsSuccessStatusCode)
            //    {
            //        return await response.Content.ReadAsStringAsync();
            //    }
            //    else
            //    {
            //        throw new Exception($"Error u pozivu Azure funkcije. err: {response.StatusCode}");
            //    }
            //}



        }





        public interface IAzureFunctionService
        {
            [Post("/api/VratiListuKontakata")]
            Task<string> CallAzureFunction([AliasAs("code")] string code, [Body] ObservableCollection<Kontakt> kon);
        }

        private async void btnAzureRefit_Click(object sender, RoutedEventArgs e)
        {

            Trace.WriteLine("Poceli smo Azure Refit -----");

            try
            {
                string baseUrl = "https://contactlistig.azurewebsites.net";                
                string functionCode = "U0Jo6mfgn8igMFDEYPHfoQ4Sdo_pSmmj0cQfBsVQE3yUAzFuwjkCpg==";
                //https://contactlistig.azurewebsites.net/api/VratiListuKontakata?code=U0Jo6mfgn8igMFDEYPHfoQ4Sdo_pSmmj0cQfBsVQE3yUAzFuwjkCpg==

                var azureFunctionService = RestService.For<IAzureFunctionService>(baseUrl);


                var kont = kontakti;

                var response = await azureFunctionService.CallAzureFunction(functionCode, kont);
                Trace.WriteLine($"Response from Azure Function: {response}");
                //Console.WriteLine($"Response from Azure Function: {response}");

                List<string> dobijenaImena = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(response);

                Trace.WriteLine("ispisujemo dobijena imena: ");
                foreach (string s in dobijenaImena)
                {
                    Trace.WriteLine(s);

                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error: {ex.Message}");
                Trace.WriteLine($"Error str: {ex.ToString}");
            }


            

        }
    }// main window
}





//Trace.WriteLine("WH!!1");

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
