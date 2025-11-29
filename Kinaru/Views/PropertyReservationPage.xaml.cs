namespace Kinaru.Views;

public partial class PropertyReservationPage : ContentPage
{
    public PropertyReservationPage(ViewModels.PropertyReservationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
