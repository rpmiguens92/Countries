using Countries.Classes;
using Countries.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;

namespace Countries
{
   
    public partial class MainWindow : Window
    {
        List<Country> _countries;
        APIService _apiService;
        DialogService _dialogService;
        DataService _dataService;
        NetworkService _networkService;


        public MainWindow()
        {
            InitializeComponent();
            InitializeServices();
            InitializeMap();
            LoadCountries();
        }

        private void InitializeServices()
        {
            _networkService = new NetworkService();
            _apiService = new APIService();
            _dialogService = new DialogService();
            _dataService = new DataService();
        }

        private async Task LoadCountries()
        {
            bool load;

            lbl_progress.Content = "A carregar os países... "; //se a conexao falhar le da bdlocal se nao le da api

            var connection = _networkService.CheckConnection();
            if (!connection.IsSuccess)
            {
                LoadLocalCountries();
                load = false;       
            }
            else
            {
                await LoadApiCountries();
                load = true;
                if(_countries != null && _countries.Count > 0)
                {
                    _dataService.SaveData(_countries);
                    
                }
                
            }

            if (_countries == null || _countries.Count == 0)
            {
                lbl_progress.Content = "Sem ligação à internet, os Países não foram carregados. Garanta acesso à internet. ";

                img_flag.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/No_flag.svg.png", UriKind.RelativeOrAbsolute));
               

                return;       
            }
            ProgressBar1.Value = 50;

            country_list.DataContext = null; //ordenar alfabeticamente
            country_list.ItemsSource = _countries.OrderBy(x => x.Nome.commonName).ToList();
            country_list.DisplayMemberPath = "Nome.commonName";
            

            if(load)
            {
                lbl_progress.Content =string.Format( "Carregado da API a {0:F}", DateTime.Now);
            }
            else
            {
                lbl_progress.Content = string.Format("Carregado a partir da Base de Dados");
            }

            ProgressBar1.Value = 100;
        }
        

        private async Task LoadApiCountries()
        {
            try
            {
                ProgressBar1.Value = 0;
                 
                _countries?.Clear();
                var response = await _apiService.GetCountries("https://restcountries.com/", "/v3.1/all");

                _countries = (List<Country>)response.Result;
               
                _dataService.SaveData(_countries);
            }
            catch (Exception ex)
            {
                lbl_progress.Content = $"Erro: {ex.Message}";
                LoadLocalCountries(); //carrega localmente de api falha
            }
        }

        private void LoadLocalCountries()
        {
            _countries = _dataService.GetData();
            img_flag.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/No_flag.svg.png", UriKind.RelativeOrAbsolute));
           
        }

        private void CountryDetails()
        {
            if (country_list.SelectedItem == null)
            {
                lbl_progress.Content = " ";

                img_flag.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/No_flag.svg.png", UriKind.RelativeOrAbsolute));
                
                return;
            }
          
            Country country = (Country)country_list.SelectedItem;

            txt_box_country.Text = !string.IsNullOrEmpty(country.Nome.commonName) ? country.Nome.commonName : "n/a";

            if (country.Capital != null && country.Capital.Count > 0)
            {
                capital_list.ItemsSource = country.Capital;
            
            }
            else
            {
                // se não há...
                capital_list.ItemsSource = new List<string> { "n/a" };
            }

            txt_box_region.Text = !string.IsNullOrEmpty(country.Region) ? country.Region : "n/a";
            txt_box_subregion.Text = !string.IsNullOrEmpty(country.Subregion) ? country.Subregion : "n/a";
            //txt_box_currency.Text = !string.IsNullOrEmpty(country.Currency) ? country.Currency : "n/a";
            txt_box_popul.Text = country.Population > 0 ? country.Population.ToString() : "n/a";
            if (country.Gini != null && country.Gini.Count > 0)
            {
                
                float giniValue = country.Gini.Values.First(); 
                txt_box_gini.Text = giniValue > 0 ? giniValue.ToString() : "n/a";
            }
            else
            { txt_box_gini.Text = "n/a";  }

            if (!string.IsNullOrEmpty(country.Flags.Png))
            {
                img_flag.Source = new BitmapImage(new Uri(country.Flags.Png));
            }
            else
            {
                img_flag.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/No_flag.svg.png", UriKind.RelativeOrAbsolute));
            }

        }
        /// <summary>
        /// correcto
        /// </summary>
        private async void InitializeMap()
        {
            img_maps.Visibility = Visibility.Collapsed;
            await img_maps.EnsureCoreWebView2Async(null);
            img_maps.Visibility = Visibility.Visible;
        }
        private void Maps(Country country)
        {
            if (country == null || string.IsNullOrEmpty(country.Nome?.commonName))
            {
                img_maps.Visibility = Visibility.Collapsed;

                return;
            }
            var connection = _networkService.CheckConnection();
            if (!connection.IsSuccess)
            {
                img_maps.CoreWebView2.NavigateToString("<html><body><h2>Sem conexão à Internet</h2><p>Não foi possível carregar o mapa.</p></body></html>");
                img_maps.Visibility = Visibility.Visible;
                return;
            }
            string countryName = country.Nome.commonName;
            string query = $"{countryName}";

            string url = $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(query)}";
            img_maps.CoreWebView2.Navigate(url);
            img_maps.Visibility = Visibility.Visible;

        }

        private void country_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(country_list.SelectedIndex != -1)
            {
                CountryDetails();
                var selectedCountry = (Country)country_list.SelectedItem;
                Maps(selectedCountry); //atualiza o mapa
            }
        }
    }
}