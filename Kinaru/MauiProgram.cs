using Kinaru.Services;
using Kinaru.ViewModels;
using Kinaru.Views;
using Microsoft.Extensions.Logging;
using Refit;

namespace Kinaru;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Services
        // API URL configuration
        // For Android Emulator: 10.0.2.2 maps to host machine's localhost
        // For iOS Simulator: localhost works directly
        string baseUrl = DeviceInfo.Platform == DevicePlatform.Android 
            ? "http://10.0.2.2:5117" 
            : "http://localhost:5117";

        builder.Services.AddSingleton(sp =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            return RestService.For<IAuthApi>(httpClient);
        });

        builder.Services.AddSingleton(sp =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            return RestService.For<IPropertyService>(httpClient);
        });

        builder.Services.AddSingleton(sp =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            return RestService.For<IReservationService>(httpClient);
        });

        builder.Services.AddSingleton(sp =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            return RestService.For<IMessagingService>(httpClient);
        });

        builder.Services.AddSingleton(sp =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            return RestService.For<IFavoriteService>(httpClient);
        });

        builder.Services.AddSingleton(sp =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            return RestService.For<IUserService>(httpClient);
        });

        builder.Services.AddSingleton(sp =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            return RestService.For<IAgentService>(httpClient);
        });

        builder.Services.AddSingleton<AuthService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<PropertyDetailsViewModel>();
        builder.Services.AddTransient<PropertyReservationViewModel>();
        builder.Services.AddTransient<MessagingViewModel>();
        builder.Services.AddTransient<FilterViewModel>();
        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<AgentDashboardViewModel>();
        builder.Services.AddTransient<MyPropertiesViewModel>();
        builder.Services.AddTransient<AddPropertyViewModel>();
        builder.Services.AddTransient<AgentReservationsViewModel>();
        builder.Services.AddTransient<AgentProfileViewModel>();
        builder.Services.AddTransient<AgentCalendarViewModel>();

        // Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<PropertyDetailsPage>();
        builder.Services.AddTransient<PropertyReservationPage>();
        builder.Services.AddTransient<MessagingPage>();
        builder.Services.AddTransient<FilterPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<AgentDashboardPage>();
        builder.Services.AddTransient<MyPropertiesPage>();
        builder.Services.AddTransient<AddPropertyPage>();
        builder.Services.AddTransient<AgentReservationsPage>();
        builder.Services.AddTransient<AgentProfilePage>();
        builder.Services.AddTransient<AgentCalendarPage>();

        return builder.Build();
    }
}
