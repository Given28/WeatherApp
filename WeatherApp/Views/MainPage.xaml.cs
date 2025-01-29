using WeatherApp.ViewModels;
using WeatherApp.Services;

namespace WeatherApp.Views;

public partial class MainPage : ContentPage
{
    private readonly WeatherViewModel _viewModel;

    public MainPage(WeatherViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}