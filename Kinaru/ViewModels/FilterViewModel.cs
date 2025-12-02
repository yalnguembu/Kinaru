using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Shared.DTOs.Properties;
using Kinaru.Shared.Enums;

namespace Kinaru.ViewModels;

public partial class FilterViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    private PropertyFilterDto filter = new();

    [ObservableProperty]
    private List<string> propertyTypes;

    [ObservableProperty]
    private string selectedTypeStr;

    public FilterViewModel()
    {
        PropertyTypes = Enum.GetNames(typeof(PropertyType)).ToList();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Filter") && query["Filter"] is PropertyFilterDto existingFilter)
        {
            Filter = existingFilter;
            if (Filter.Type.HasValue)
            {
                SelectedTypeStr = Filter.Type.Value.ToString();
            }
        }
    }

    [RelayCommand]
    private async Task ApplyFiltersAsync()
    {
        if (!string.IsNullOrEmpty(SelectedTypeStr) && Enum.TryParse<PropertyType>(SelectedTypeStr, out var type))
        {
            Filter.Type = type;
        }
        else
        {
            Filter.Type = null;
        }

        var navigationParameter = new Dictionary<string, object>
        {
            { "FilterResult", Filter }
        };

        await Shell.Current.GoToAsync("..", navigationParameter);
    }

    [RelayCommand]
    private async Task ResetFiltersAsync()
    {
        Filter = new PropertyFilterDto();
        SelectedTypeStr = string.Empty;
        await ApplyFiltersAsync();
    }

    [RelayCommand]
    private async Task CloseAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
