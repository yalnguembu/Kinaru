namespace Kinaru.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ViewModels.ProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void OnPageLoaded(object sender, EventArgs e)
    {
        if (BindingContext is ViewModels.ProfileViewModel viewModel)
        {
            viewModel.LoadProfileCommand.Execute(null);
        }
    }
}
