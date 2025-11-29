using Kinaru.Views;

namespace Kinaru;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("home", typeof(Views.HomePage));
        Routing.RegisterRoute("propertydetails", typeof(Views.PropertyDetailsPage));
        Routing.RegisterRoute("reservation", typeof(Views.PropertyReservationPage));
        Routing.RegisterRoute("messaging", typeof(Views.MessagingPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}
