using Kinaru.ViewModels;

namespace Kinaru.Views;

public partial class AgentReservationsPage : ContentPage
{
    public AgentReservationsPage(AgentReservationsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        if (BindingContext is AgentReservationsViewModel viewModel)
        {
            await viewModel.LoadReservationsCommand.ExecuteAsync(null);
        }
    }
}
