using Kinaru.ViewModels;

namespace Kinaru.Views;

public partial class AgentProfilePage : ContentPage
{
    public AgentProfilePage(AgentProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        if (BindingContext is AgentProfileViewModel viewModel)
        {
            await viewModel.LoadProfileCommand.ExecuteAsync(null);
        }
    }
}
