using Kinaru.ViewModels;

namespace Kinaru.Views;

public partial class AgentDashboardPage : ContentPage
{
    public AgentDashboardPage(AgentDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        if (BindingContext is AgentDashboardViewModel viewModel)
        {
            await viewModel.LoadStatisticsCommand.ExecuteAsync(null);
        }
    }
}
