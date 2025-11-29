namespace Kinaru.Views;

public partial class PropertyDetailsPage : ContentPage
{
    public PropertyDetailsPage(ViewModels.PropertyDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
