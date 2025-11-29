namespace Kinaru.Views;

public partial class MessagingPage : ContentPage
{
    public MessagingPage(ViewModels.MessagingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
