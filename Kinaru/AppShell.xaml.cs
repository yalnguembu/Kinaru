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
        Routing.RegisterRoute("filters", typeof(Views.FilterPage));
        Routing.RegisterRoute("agentdashboard", typeof(Views.AgentDashboardPage));
        Routing.RegisterRoute("myproperties", typeof(Views.MyPropertiesPage));
        Routing.RegisterRoute("addproperty", typeof(Views.AddPropertyPage));
        Routing.RegisterRoute("agentreservations", typeof(Views.AgentReservationsPage));
        Routing.RegisterRoute("agentprofile", typeof(Views.AgentProfilePage));
        Routing.RegisterRoute("agentcalendar", typeof(Views.AgentCalendarPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}
