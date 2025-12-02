namespace Kinaru.Views;

public partial class MyPropertiesPage : ContentPage
{
	public MyPropertiesPage(ViewModels.MyPropertiesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void OnPageLoaded(object sender, EventArgs e)
    {
        if (BindingContext is ViewModels.MyPropertiesViewModel viewModel)
        {
            viewModel.LoadPropertiesCommand.Execute(null);
        }
    }
}
