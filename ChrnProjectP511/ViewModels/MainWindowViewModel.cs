using ChrnProjectP511.Models;
using ChrnProjectP511.service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;



namespace ChrnProjectP511.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly WeatherApi _weatherApi = new WeatherApi();

        [ObservableProperty]
        private string _cityName;

        [ObservableProperty]
        private string _cityNameText;

        [ObservableProperty]
        private string _iconText = "❓";

        [ObservableProperty]
        private string _tempText;

        [ObservableProperty]
        private string _descriptionText;

        [ObservableProperty]
        private string _windText;

        [ObservableProperty]
        private string _humidityText;

        [ObservableProperty]
        private bool _isWeatherCardVisible;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _errorText;

        [ObservableProperty]
        private bool _isErrorVisible;

        [ObservableProperty]
        private string _defaultCityText = "*Город по умолчанию: не выбран*";

        [ObservableProperty]
        private ObservableCollection<string> _historyItems = new ObservableCollection<string>();

        
        [RelayCommand]
        private async Task SearchWeather()
        {
            System.Diagnostics.Debug.WriteLine($"Поиск города: {CityName}");

            if (string.IsNullOrWhiteSpace(CityName))
            {
                ShowError("Введите название города");
                return;
            }

            IsLoading = true;
            IsErrorVisible = false;
            IsWeatherCardVisible = false;

            try
            {
                System.Diagnostics.Debug.WriteLine("Начинаем запрос к API...");

                var weather = await _weatherApi.GetWeatherByCityAsync(CityName);

                System.Diagnostics.Debug.WriteLine($"Результат: {(weather == null ? "null" : "OK")}");

                if (weather == null)
                {
                    ShowError("Город не найден. Проверьте название");
                    return;
                }

                CityNameText = weather.CityName;
                IconText = weather.Icon;
                TempText = $"{weather.Temperature:0}°C";
                DescriptionText = weather.WeatherDiscription;
                WindText = $"{weather.WindSpeed:0} км/ч";
                HumidityText = $"{weather.Humidity}%";
                IsWeatherCardVisible = true;

                System.Diagnostics.Debug.WriteLine("Погода отображена!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ОШИБКА: {ex.Message}");

                ShowError($"Ошибка: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        [RelayCommand]
        private async Task SaveDefaultCity()
        {
            if (string.IsNullOrWhiteSpace(CityName))
            {
                ShowError("Сначала найдите город");
                return;
            }
            
            try
            {
              
                var coordinates = await GetCoordinatesForSave(CityName);
                if (coordinates == null)
                {
                    ShowError("Не удалось найти координаты города");
                    return;
                }
                
                var defaultCity = new DefaultCity
                {
                    Name = coordinates.Name,
                    Latitude = coordinates.Latitude,
                    Longitude = coordinates.Longitude
                };
                
                string json = JsonConvert.SerializeObject(defaultCity);
                File.WriteAllText("default_city.json", json);
                
                DefaultCityText = $" Город по умолчанию: {coordinates.Name}";
                ShowError("Город сохранён!");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка сохранения: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ClearHistory()
        {
            HistoryItems.Clear();
        }



        private async Task<CityCoordinates?> GetCoordinatesForSave(string cityName)
        {
  
            try
            {
                using var httpClient = new System.Net.Http.HttpClient();
                string url = $"https://geocoding-api.open-meteo.com/v1/search?name={cityName}&count=1";
                string json = await httpClient.GetStringAsync(url);
                JObject data = JObject.Parse(json);

                var firstResult = data["results"]?[0];
                if (firstResult == null) return null;

                return new CityCoordinates
                {
                    Name = firstResult["name"]?.ToString() ?? cityName,
                    Latitude = (double)firstResult["latitude"],
                    Longitude = (double)firstResult["longitude"],
                    Country = firstResult["country"]?.ToString() ?? ""
                };
            }
            catch
            {
                return null;
            }
        }

        private void ShowError(string message)
        {
            _errorText = message;
            _isErrorVisible = true;

            Task.Delay(3000).ContinueWith(_ =>
            {
                _isErrorVisible = false;
            });
        }
       
        public async Task LoadDefaultCityOnStart()
        {
            try
            {
                if (File.Exists("default_city.json"))
                {
                    string json = File.ReadAllText("default_city.json");
                    var defaultCity = JsonConvert.DeserializeObject<DefaultCity>(json);

                    if (defaultCity != null)
                    {
                        DefaultCityText = $" Город по умолчанию: {defaultCity.Name}";
                        CityName = defaultCity.Name;
                        await SearchWeather();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки города по умолчанию: {ex.Message}");
            }
        }
    }
}
