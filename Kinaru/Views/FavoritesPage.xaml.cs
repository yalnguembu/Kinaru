namespace Kinaru.Views;

public partial class FavoritesPage : ContentPage
{
	public FavoritesPage(ViewModels.FavoritesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void OnPageLoaded(object sender, EventArgs e)
    {
        if (BindingContext is ViewModels.FavoritesViewModel viewModel)
        {
            viewModel.LoadFavoritesCommand.Execute(null);
        }
    }
}
