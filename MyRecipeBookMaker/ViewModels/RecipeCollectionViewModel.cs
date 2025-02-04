using System.Collections.ObjectModel;

using Newtonsoft.Json;
using MyRecipeBookMaker.Models;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using MyRecipeBookMaker.Common;
using MyRecipeBookMaker.AI;
namespace MyRecipeBookMaker
{
    public partial class RecipeCollectionViewModel : ObservableObject
    {
        [ObservableProperty] public ObservableCollection<Recipe>? listOfRecipes;
        [ObservableProperty] bool showAddMenu = false;
        [ObservableProperty] int gridRow = 0;
        [ObservableProperty] int gridColumn = 1;
        
        double radialMenuWidth;
        double radialMenuHeight;
        [ObservableProperty] public Point addMenuPoint =new(0,0);
        

        public RecipeCollectionViewModel()
        {
            ListOfRecipes = new ObservableCollection<Recipe>();
            LoadData();
          
            AddMenuPoint = new Point(AppShell.Current.Window.Width - 80, -5);
            OnPropertyChanged(nameof(AddMenuPoint));
            AppShell.Current.Window.SizeChanged += MainWindow_SizeChanged;
        }
        
        private void MainWindow_SizeChanged(object sender, EventArgs e)

        {
       
      
            AddMenuPoint = new Point(AppShell.Current.Window.Width-80, -5);
            OnPropertyChanged(nameof(AddMenuPoint));
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
        public async Task AddRecipeByPhotoByAlbum()
        {
            AddMenuPoint = new Point(AppShell.Current.Window.Width - 80, -20);
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult? photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null)
                {

                    using Stream sourceStream = await photo.OpenReadAsync();

                    // Convert to base64
                    sourceStream.Position = 0;
                    using MemoryStream memoryStream = new();
                    await sourceStream.CopyToAsync(memoryStream);
                    byte[] photoBytes = memoryStream.ToArray();
                    AIReadRecipe ai = new AIReadRecipe();
                    Recipe r = await ai.ReadRecipeFromImage(photoBytes);

             
                }
            }
            ShowAddMenu = false;
        }
        [RelayCommand]
        public async Task AddRecipeByCamera()
        {
            AddMenuPoint = new Point(AppShell.Current.Window.Width - 80, -20);
      
            ShowAddMenu = false;
        }
        [RelayCommand]
        public async Task AddRecipeoByPdf()
        {
            AddMenuPoint = new Point(AppShell.Current.Window.Width - 80, -20);
        
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







