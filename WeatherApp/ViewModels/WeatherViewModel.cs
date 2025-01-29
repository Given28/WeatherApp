using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        private readonly WeatherService _weatherService;
        private WeatherData _weatherData;
        private string _cityName = null;
        private bool _isLoading;
        private string _errorMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public WeatherViewModel(WeatherService weatherService)
        {
            _weatherService = weatherService;
            GetWeatherCommand = new Command(async () => await GetWeatherAsync());
            GetLocationWeatherCommand = new Command(async () => await GetLocationWeatherAsync());
        }

        public WeatherData WeatherData
        {
            get => _weatherData;
            set
            {
                if (_weatherData != value)
                {
                    _weatherData = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CityName
        {
            get => _cityName;
            set
            {
                if (_cityName != value)
                {
                    _cityName = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand GetWeatherCommand { get; }
        public ICommand GetLocationWeatherCommand { get; }

        private async Task GetWeatherAsync()
        {
            if (string.IsNullOrWhiteSpace(CityName))
                return;

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                WeatherData = await _weatherService.GetWeatherDataAsync(CityName);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetLocationWeatherAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status != PermissionStatus.Granted)
                    {
                        ErrorMessage = "Location permission is required to get weather for current location.";
                        return;
                    }
                }

                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(5)
                });

                if (location == null)
                {
                    ErrorMessage = "Unable to get current location.";
                    return;
                }

                WeatherData = await _weatherService.GetWeatherByLocationAsync(
                    location.Latitude,
                    location.Longitude);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error getting weather: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}