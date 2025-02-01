using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using CommunityToolkit.Mvvm.ComponentModel;
namespace MyRecipeBookMaker.Models
{


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public partial class Ingredient : ObservableObject
    {
        [ObservableProperty] public string? name;
        [ObservableProperty] public double? quantity;
        [ObservableProperty] public string? unit;

        partial void OnNameChanged(string? oldValue, string? newValue) => Recipe.PropertyHasChanged("Ingredient/Name", oldValue, newValue);
        partial void OnQuantityChanged(double? oldValue, double? newValue) => Recipe.PropertyHasChanged("Ingredient/Quantity", oldValue?.ToString(), newValue?.ToString());
        partial void OnUnitChanged(string? oldValue, string? newValue) => Recipe.PropertyHasChanged("Ingredient/Unit", oldValue, newValue);

      
    }

    public partial class IngredientGroup : ObservableObject
    {
        [ObservableProperty] public string? name;
        [ObservableProperty] public ObservableCollection<Ingredient>? ingredients;

        partial void OnNameChanged(string? oldValue, string? newValue) => Recipe.PropertyHasChanged("IngredientGroup/Name", oldValue, newValue);
        partial void OnIngredientsChanged(ObservableCollection<Ingredient>? oldValue, ObservableCollection<Ingredient>? newValue) => Recipe.PropertyHasChanged("IngredientGroup/Ingredients", oldValue?.ToString(), newValue?.ToString());

   
    }

    public partial class Instruction : ObservableObject
    {
        [ObservableProperty] public int? step;
        [ObservableProperty] public string? description;

        partial void OnStepChanged(int? oldValue, int? newValue) => Recipe.PropertyHasChanged("Instruction/Step", oldValue?.ToString(), newValue?.ToString());
        partial void OnDescriptionChanged(string? oldValue, string? newValue) => Recipe.PropertyHasChanged("Instruction/Description", oldValue, newValue);

       
    }

    public partial class InstructionGroup : ObservableObject
    {
        [ObservableProperty] public string? name;
        [ObservableProperty] public ObservableCollection<Instruction>? instructions;

        partial void OnNameChanged(string? oldValue, string? newValue) => Recipe.PropertyHasChanged("InstructionGroup/Name", oldValue, newValue);
        partial void OnInstructionsChanged(ObservableCollection<Instruction>? oldValue, ObservableCollection<Instruction>? newValue) => Recipe.PropertyHasChanged("InstructionGroup/Instructions", oldValue?.ToString(), newValue?.ToString());

   
    }

    public partial class Nutrition : ObservableObject
    {
        [ObservableProperty] public string? name;
        [ObservableProperty] public string? value;

        partial void OnNameChanged(string? oldValue, string? newValue) => Recipe.PropertyHasChanged("Nutrition/Name", oldValue, newValue);
        partial void OnValueChanged(string? oldValue, string? newValue) => Recipe.PropertyHasChanged("Nutrition/Value", oldValue, newValue);

       
    }

    public partial class Recipe : ObservableObject
    {
        [ObservableProperty] public int id;
        [ObservableProperty] public string? name;
        [ObservableProperty] public string? description;
        [ObservableProperty] public string? servings;
        [ObservableProperty] public string? cookTime;
        [ObservableProperty] public string? prepTime;
        [ObservableProperty] public string? totalTime;
        [ObservableProperty] public string? author;
        [ObservableProperty] public string? cuisine;
        [ObservableProperty] public string? course;
        [ObservableProperty] public string? imageBASE64;
        [ObservableProperty] public string? imageURL;
        [ObservableProperty] public object? calories;
        [ObservableProperty] public ObservableCollection<Nutrition>? nutrition;
        [ObservableProperty] public ObservableCollection<IngredientGroup>? ingredientGroups;
        [ObservableProperty] public ObservableCollection<InstructionGroup>? instructionGroups;
        [ObservableProperty] public ObservableCollection<string>? tags;
        [ObservableProperty] public string? notes;

        public ImageSource? ImageData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(imageBASE64))
                {
                    if (!string.IsNullOrWhiteSpace(imageURL))
                    {
                        return ImageSource.FromUri(new Uri(imageURL));
                    }
                    return null;
                }
                else
                {
                    byte[] imageBytes = Convert.FromBase64String(imageBASE64);
                    return ImageSource.FromStream(() => new MemoryStream(imageBytes));
                }
            }
        }

        partial void OnIdChanged(int oldValue, int newValue) => PropertyHasChanged("Recipe/Id", oldValue.ToString(), newValue.ToString());
        partial void OnNameChanged(string? oldValue, string? newValue) =>       PropertyHasChanged("Recipe/Name", oldValue, newValue);
        partial void OnDescriptionChanged(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/Description", oldValue, newValue);
        partial void OnServingsChanged(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/Servings", oldValue, newValue);
        partial void OnCookTimeChanged(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/CookTime", oldValue, newValue);
        partial void OnPrepTimeChanged(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/PrepTime", oldValue, newValue);
        partial void OnTotalTimeChanged(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/TotalTime", oldValue, newValue);
        partial void OnAuthorChanged(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/Author", oldValue, newValue);
        partial void OnCuisineChanged(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/Cuisine", oldValue, newValue);
        partial void OnCourseChanged(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/Course", oldValue, newValue);
        partial void OnImageBASE64Changed(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/ImageBASE64", oldValue, newValue);
        partial void OnImageURLChanged(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/ImageURL", oldValue, newValue);
        partial void OnCaloriesChanged(object? oldValue, object? newValue) => PropertyHasChanged("Recipe/Calories", oldValue?.ToString(), newValue?.ToString());
        partial void OnNutritionChanged(ObservableCollection<Nutrition>? oldValue, ObservableCollection<Nutrition>? newValue) => PropertyHasChanged("Recipe/Nutrition", oldValue?.ToString(), newValue?.ToString());
        partial void OnIngredientGroupsChanged(ObservableCollection<IngredientGroup>? oldValue, ObservableCollection<IngredientGroup>? newValue) => PropertyHasChanged("Recipe/IngredientGroups", oldValue?.ToString(), newValue?.ToString());
        partial void OnInstructionGroupsChanged(ObservableCollection<InstructionGroup>? oldValue, ObservableCollection<InstructionGroup>? newValue) => PropertyHasChanged("Recipe/InstructionGroups", oldValue?.ToString(), newValue?.ToString());
        partial void OnTagsChanged(ObservableCollection<string>? oldValue, ObservableCollection<string>? newValue) => PropertyHasChanged("Recipe/Tags", oldValue?.ToString(), newValue?.ToString());
        partial void OnNotesChanged(string? oldValue, string? newValue) => PropertyHasChanged("Recipe/Notes", oldValue, newValue);

        static public void PropertyHasChanged(string propertyPath, string? oldValue, string? newValue)
        {
            Debug.WriteLine($"Property {propertyPath} changed from {oldValue} to {newValue}");
            // Add additional logic here if needed
        }
    }
}