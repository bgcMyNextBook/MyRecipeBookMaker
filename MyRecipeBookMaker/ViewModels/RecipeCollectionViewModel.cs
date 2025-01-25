using System.Collections.ObjectModel;

using Newtonsoft.Json;
using MyRecipeBookMaker.Models;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using MyRecipeBookMaker.Common;
namespace MyRecipeBookMaker
{
    public partial class RecipeCollectionViewModel : ObservableObject
    {
        public ObservableCollection<Recipe>? ListOfRecipes { get; set; }

        public RecipeCollectionViewModel()
        {
            ListOfRecipes = new ObservableCollection<Recipe>();
            LoadData();
        }

        private async Task LoadData()
        {
            string jsonData = await ListHelper.ReadDataFileWithDefault("Recipes.json", "Recipes.json");

            List<Recipe>? recipes = null;
            try
            {
                recipes = JsonConvert.DeserializeObject<List<Recipe>>(jsonData);
            }
            catch (JsonException)
            {
                // Handle JSON deserialization error if needed
                return;
            }

            if (recipes != null)
            {
                ListOfRecipes?.Clear();
                foreach (var recipe in recipes)
                {
                    ListOfRecipes?.Add(recipe);
                }
            }
        }

        [RelayCommand]
        public async Task ItemTapped(Recipe recipe)
        {
            Debug.WriteLine($"Item tapped: {recipe.name}");
            // Navigate to RecipeDetails page with the selected recipe as a query parameter
            var navigationParameter = new Dictionary<string, object>
            {
                { "recipe", recipe }
            };
            await Shell.Current.GoToAsync("RecipeDetails", navigationParameter);
        }
    }

}







