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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab_1C
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string plikArchiwizacji = "archiwum.txt";


        public MainWindow()
        {
            
            TextBoxWithErrorProvider.BrushForAll = Brushes.Red;
            InitializeComponent();
            textBoxWEPName.SetFocus();
            
           
        }
        //Test sprawdzający czy pole nie jest puste
        //jeśli tak to przy okazji zgłasza błąd w obrębie kontrolki
        private bool IsNotEmpty(TextBoxWithErrorProvider tb)
        {
            if (tb.Text.Trim() == "")
            {
                tb.SetError("Pole nie może być puste!");
                return false;
            }
            tb.SetError("");
            return true;
        }

        private void Clear()
        {
            //czyszczenie formularza i ustawienie stanu początkowego
            textBoxWEPName.Text = "";
            textBoxWEPSurname.Text = "";
            sliderWeight.Value = 75;
            sliderAge.Value = 25;
            buttonEdytuj.IsEnabled = false;
            buttonUsun.IsEnabled = false;
            //odznaczenie listy
            listBoxPlayers.SelectedIndex = -1;
            textBoxWEPName.SetFocus();
        }

        private void LoadPlayer(Player pilkarz)
        {
            textBoxWEPName.Text = pilkarz.Name;
            textBoxWEPSurname.Text = pilkarz.Surname;
            sliderWeight.Value = pilkarz.Weight;
            sliderAge.Value = pilkarz.Age;
            buttonEdytuj.IsEnabled = true;
            buttonUsun.IsEnabled = true;
            textBoxWEPName.SetFocus();
        }

        private void buttonEdytuj_Click(object sender, RoutedEventArgs e)
        {
            // operator logiczny & a nie podwójny operator warunkowy &&
            //w przypadku operatora warunkowego && nie doszłoby do sprawdzenia drugiego warunku gdy pierwszy byłby niespełniony
            if (IsNotEmpty(textBoxWEPName) & IsNotEmpty(textBoxWEPSurname))
            {
                var biezacyPilkarz = new Player(textBoxWEPName.Text.Trim(), textBoxWEPSurname.Text.Trim(), (uint)sliderAge.Value, (uint)sliderWeight.Value);
                var czyJuzJestNaLiscie = false;
                foreach (var p in listBoxPlayers.Items)
                {
                    var pilkarz = p as Player;
                    if (pilkarz.isTheSame(biezacyPilkarz))
                    {
                        czyJuzJestNaLiscie = true;
                        break;
                    }
                }
                if (!czyJuzJestNaLiscie)
                {
                    var dialogResult=MessageBox.Show($"Czy na pewno chcesz zmienić dane  {Environment.NewLine} {listBoxPlayers.SelectedItem}?", "Edycja", MessageBoxButton.YesNo);

                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        Player selectedPlayer = (Player)listBoxPlayers.SelectedItem;
                        selectedPlayer.Name = textBoxWEPName.Text.Trim();
                        selectedPlayer.Surname = textBoxWEPSurname.Text.Trim();
                        selectedPlayer.Age = (uint)sliderAge.Value;
                        selectedPlayer.Weight = (uint)sliderWeight.Value;

                        listBoxPlayers.Items.Refresh();
                    }
                    Clear();
                    listBoxPlayers.SelectedIndex = -1;
                    
                }
                else
                    MessageBox.Show($"{biezacyPilkarz.ToString()} już jest na liście.", "Uwaga");
                    
                    
                
            }
        }

        private void buttonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (IsNotEmpty(textBoxWEPName) & IsNotEmpty(textBoxWEPSurname))
            {
                var biezacyPilkarz = new Player(textBoxWEPName.Text.Trim(), textBoxWEPSurname.Text.Trim(), (uint)sliderAge.Value, (uint)sliderWeight.Value);
                var czyJuzJestNaLiscie = false;
                foreach (var p in listBoxPlayers.Items)
                {
                    var pilkarz = p as Player;
                    if (pilkarz.isTheSame(biezacyPilkarz))
                    {
                        czyJuzJestNaLiscie = true;
                        break;
                    }
                }
                if (!czyJuzJestNaLiscie)
                {
                    listBoxPlayers.Items.Add(biezacyPilkarz);
                    Clear();
                }
                else
                {
                    var dialog = MessageBox.Show($"{biezacyPilkarz.ToString()} już jest na liście {Environment.NewLine} Czy wyczyścić formularz?", "Uwaga", MessageBoxButton.OKCancel);
                    if (dialog == MessageBoxResult.OK)
                    {
                        Clear();
                    }

                }
            }
        }

        private void buttonUsun_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show($"Czy na pewno chcesz usunąć dane  {Environment.NewLine} {listBoxPlayers.SelectedItem}?", "Usuń", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                listBoxPlayers.Items.Remove(listBoxPlayers.SelectedItem);
            }
        }

        // zmiana zaznaczenia na liscie piłkarzy
        //uwaga brak zaznaczenia również wywołuje to zdarzenie i wówczas indeks zaznaczonwego
        //wynosi -1
        
        private void listBoxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //wtedy, gdy faktycznie coś jest zaznaczone
            if (listBoxPlayers.SelectedIndex > -1)
            {
                LoadPlayer((Player)listBoxPlayers.SelectedItem);
            }

        }
        //nadpisujemy plik archiwum przy zamknięciu okna
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int n = listBoxPlayers.Items.Count;
            Player[] players = null;
            if (n > 0)
            {
                players = new Player[n];
                int index = 0;
                foreach (var o in listBoxPlayers.Items)
                {
                    players[index++] = o as Player;
                }
                Archiving.ArchivePlayers(plikArchiwizacji,players);
            }
            
                
        }
        //metoda wykonana po załadowaniu okna
        //ładujemy zawartość pliku z zapisanymi piłkarzami jeśli tylko istnieje
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var players=Archiving.ReadPlayersFromFile(plikArchiwizacji);
            if(players!=null)
            foreach (var p in players)
            {
                listBoxPlayers.Items.Add(p);
            }

        }
    }
}
