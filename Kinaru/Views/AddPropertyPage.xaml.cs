namespace Kinaru.Views;

public partial class AddPropertyPage : ContentPage
{
	public AddPropertyPage(ViewModels.AddPropertyViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
