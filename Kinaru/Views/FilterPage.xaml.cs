namespace Kinaru.Views;

public partial class FilterPage : ContentPage
{
	public FilterPage(ViewModels.FilterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
