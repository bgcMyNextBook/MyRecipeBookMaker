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
        [ObservableProperty] public ObservableCollection<Recipe>? listOfRecipes;
        [ObservableProperty] bool showAddMenu = false;
      
        
        double radialMenuWidth;
        double radialMenuHeight;
        [ObservableProperty] public Point addMenuPoint =new(0,0);
        

        public RecipeCollectionViewModel()
        {
            ListOfRecipes = new ObservableCollection<Recipe>();
            LoadData();
          
            AddMenuPoint = new Point(AppShell.Current.Window.Width - 80, -20);
          
        }
        
     
        
        private async Task LoadData()
        {
            string jsonData = await ListHelper.ReadDataFileWithDefault("Recipes.json", "Recipes.json");

            List<Recipe>? recipes = null;
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Error = (sender, args) =>
                    {
                        if (args.ErrorContext.Member.ToString() == "nutrition")
                        {
                            args.ErrorContext.Handled = true;
                        }
                    }
                };
                recipes = JsonConvert.DeserializeObject<List<Recipe>>(jsonData, settings);
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
        #region Commands
      
       
        [RelayCommand]
        public async Task AddNewRecipe()
        {

            ShowAddMenu = true;
        }
        [RelayCommand]
        public async Task ChangePhotoByAlbum()
        {
      
           
            ShowAddMenu = false;
        }
        [RelayCommand]
        public async Task ChangePhotoByCamera()
        {
            
      
            ShowAddMenu = false;
        }
        [RelayCommand]
        public async Task ChangePhotoByPdf()
        {
          
        
            ShowAddMenu = false;
        }
        [RelayCommand]
        public async Task ItemTapped(Recipe recipe)
        {
            Debug.WriteLine($"Item tapped: {recipe.Name}");
            // Navigate to RecipeDetails page with the selected recipe as a query parameter
            var navigationParameter = new Dictionary<string, object>
            {
                { "recipe", recipe }
            };
            await Shell.Current.GoToAsync("RecipeDetails", navigationParameter);
        }
        #endregion
    }

}







