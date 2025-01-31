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
    }

    public partial class IngredientGroup : ObservableObject
    {
        [ObservableProperty] public string? name;
        [ObservableProperty] public ObservableCollection<Ingredient>? ingredients;
    }

    public partial class Instruction : ObservableObject
    {
        [ObservableProperty] public int? step;
        [ObservableProperty] public string? description;
    }

    public partial class InstructionGroup : ObservableObject
    {
        [ObservableProperty] public string? name;
        [ObservableProperty] public ObservableCollection<Instruction>? instructions;
    }

    public partial class Nutrition : ObservableObject
    {
        [ObservableProperty] public string? combinedListOfNutriates;
        [ObservableProperty] public ObservableCollection<string>? listOfNutriates;

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
        [ObservableProperty] public Nutrition? nutrition;
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

    }
}