using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Properties;
using Kinaru.Shared.Enums;
using System.Collections.ObjectModel;

namespace Kinaru.ViewModels;

public partial class AddPropertyViewModel : ObservableObject, IQueryAttributable
{
    private readonly IPropertyService _propertyService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private Guid? propertyId;

    [ObservableProperty]
    private string titre = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private decimal prix;

    [ObservableProperty]
    private PropertyType selectedType;

    [ObservableProperty]
    private string adresse = string.Empty;

    [ObservableProperty]
    private string ville = string.Empty;

    [ObservableProperty]
    private string quartier = string.Empty;

    [ObservableProperty]
    private string codePostal = string.Empty;

    [ObservableProperty]
    private float superficie;

    [ObservableProperty]
    private int nombreChambres;

    [ObservableProperty]
    private int nombreSallesBain;

    [ObservableProperty]
    private int nombrePieces;

    public ObservableCollection<string> PropertyTypes { get; } = new();
    public ObservableCollection<string> SelectedImages { get; } = new();

    public AddPropertyViewModel(IPropertyService propertyService)
    {
        _propertyService = propertyService;
        
        foreach (var type in Enum.GetNames(typeof(PropertyType)))
        {
            PropertyTypes.Add(type);
        }
        SelectedType = PropertyType.Appartement;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id") && query["id"] is string idString && Guid.TryParse(idString, out var id))
        {
            PropertyId = id;
            IsEditing = true;
            _ = LoadPropertyAsync(id);
        }
    }

    private async Task LoadPropertyAsync(Guid id)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var property = await _propertyService.GetPropertyByIdAsync(id);

            Titre = property.Titre;
            Description = property.Description;
            Prix = property.Prix;
            SelectedType = property.Type;
            Adresse = property.Adresse;
            Ville = property.Ville;
            Quartier = property.Quartier;
            CodePostal = property.CodePostal ?? string.Empty;
            Superficie = property.Superficie;
            NombreChambres = property.NombreChambres;
            NombreSallesBain = property.NombreSallesBain;
            NombrePieces = property.NombrePieces;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading property: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task PickImageAsync()
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Sélectionner une image"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                var imagePath = Path.Combine(FileSystem.CacheDirectory, result.FileName);
                
                using (var fileStream = File.Create(imagePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                SelectedImages.Add(imagePath);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", $"Échec de la sélection: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task TakePhotoAsync()
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var result = await MediaPicker.Default.CapturePhotoAsync();

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    var imagePath = Path.Combine(FileSystem.CacheDirectory, result.FileName);
                    
                    using (var fileStream = File.Create(imagePath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    SelectedImages.Add(imagePath);
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Non supporté", "La caméra n'est pas disponible sur cet appareil", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", $"Échec de la capture: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private void RemoveImage(string imagePath)
    {
        SelectedImages.Remove(imagePath);
    }

    [RelayCommand]
    private async Task SavePropertyAsync()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(Titre) || Prix <= 0)
        {
            await Shell.Current.DisplayAlert("Erreur", "Veuillez remplir tous les champs requis", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            if (IsEditing && PropertyId.HasValue)
            {
                var dto = new UpdatePropertyDto
                {
                    Titre = Titre,
                    Description = Description,
                    Prix = Prix,
                    Type = SelectedType,
                    Adresse = Adresse,
                    Ville = Ville,
                    Quartier = Quartier,
                    CodePostal = CodePostal,
                    Superficie = Superficie,
                    NombreChambres = NombreChambres,
                    NombreSallesBain = NombreSallesBain,
                    NombrePieces = NombrePieces
                };

                await _propertyService.UpdatePropertyAsync(PropertyId.Value, dto);
                await Shell.Current.DisplayAlert("Succès", "Propriété mise à jour", "OK");
            }
            else
            {
                var dto = new CreatePropertyDto
                {
                    Titre = Titre,
                    Description = Description,
                    Prix = Prix,
                    Type = SelectedType,
                    Adresse = Adresse,
                    Ville = Ville,
                    Quartier = Quartier,
                    CodePostal = CodePostal,
                    Superficie = Superficie,
                    NombreChambres = NombreChambres,
                    NombreSallesBain = NombreSallesBain,
                    NombrePieces = NombrePieces
                };

                await _propertyService.CreatePropertyAsync(dto);
                await Shell.Current.DisplayAlert("Succès", "Propriété créée", "OK");
            }

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error saving property: {ex.Message}";
            await Shell.Current.DisplayAlert("Erreur", ErrorMessage, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
