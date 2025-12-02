namespace Kinaru.Views;

public partial class HomePage : ContentPage
{
    public HomePage(ViewModels.HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    private void OnPageLoaded(object sender, EventArgs e)
    {
        if (BindingContext is ViewModels.HomeViewModel viewModel)
        {
            viewModel.LoadDataCommand.Execute(null);
        }
    }
}
