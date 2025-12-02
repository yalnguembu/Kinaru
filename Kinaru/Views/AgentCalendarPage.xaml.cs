using Kinaru.ViewModels;

namespace Kinaru.Views;

public partial class AgentCalendarPage : ContentPage
{
    public AgentCalendarPage(AgentCalendarViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        if (BindingContext is AgentCalendarViewModel viewModel)
        {
            await viewModel.LoadAvailabilitiesCommand.ExecuteAsync(null);
        }
    }
}
