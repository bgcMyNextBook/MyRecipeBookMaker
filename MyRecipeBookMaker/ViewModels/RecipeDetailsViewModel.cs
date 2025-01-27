using System.Text.Json;
using MyRecipeBookMaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using MyRecipeBookMaker.Common;

namespace MyRecipeBookMaker
{
    public partial class RecipeDetailsViewModel : ObservableObject, IQueryAttributable
    {

        public Recipe? selectedRecipe { get; set; }
        public ObservableCollection<DropDownListObject> Cuisines { get; set; } = new();
        public ObservableCollection<DropDownListObject> Courses { get; set; } = new();
        public ObservableCollection<DropDownListObject> Units { get; set; } = new();
        public ObservableCollection<InstructionGroup> instructionGroups { get; set; } = new();
        public ObservableCollection<IngredientGroup> ingredientGroups { get; set; } = new();

        public RecipeDetailsViewModel()
        {
            LoadListsAsync();
        }

        private async void LoadListsAsync()
        {
            Cuisines = await LoadList("cuisine.json");
            Courses = await LoadList("course.json");
            Units = await LoadList("units.json");
            OnPropertyChanged(nameof(Cuisines));
            OnPropertyChanged(nameof(Courses));
            OnPropertyChanged(nameof(Units));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("recipe"))
            {
                selectedRecipe = query["recipe"] as Recipe;
                instructionGroups = selectedRecipe.instructionGroups;
                ingredientGroups = selectedRecipe.ingredientGroups;
                OnPropertyChanged(nameof(selectedRecipe));
            }


        }

        private async Task<ObservableCollection<DropDownListObject>> LoadList(string listName)
        {
            ObservableCollection<DropDownListObject> list = new();

            try
            {
                string jsonData = await ListHelper.ReadDataFileWithDefault(listName, listName);
                string listN = listName.Replace(".json", "");
                var cuisinesJson = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(jsonData);
                if (cuisinesJson != null)
                {
                    foreach (var c in cuisinesJson)
                    {
                        if (c.TryGetValue(listN, out var cuisine))
                        {
                            list.Add(new DropDownListObject(cuisine));
                        }
                    }

      
                }
                return list;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return list;
            }
        }


        private void SaveCuisines()
        {
            var json = JsonSerializer.Serialize(Cuisines.ToList());
            string appDataFilePath = Path.Combine(FileSystem.AppDataDirectory, "cuisine.json");

            File.WriteAllText(appDataFilePath, json);
        }

        /*
        [RelayCommand]
        private void AddCuisine(string newCuisine)
        {
            if (!string.IsNullOrWhiteSpace(newCuisine) && !Cuisines.Contains(newCuisine))
            {
                Cuisines.Add(newCuisine);
                SaveCuisines();
            }
        }
        */
    }



    public class Review
    {
        private string? customerImage;
        private List<string>? customerReviewImages;
        public string? ProfileImage { get; set; }
        public string? CustomerName { get; set; }
        public string? Comment { get; set; }
        public string? ReviewedDate { get; set; }
        public string CustomerImage
        {
            get { return App.ImageServerPath + this.customerImage; }
            set { this.customerImage = value; }
        }

        public List<string>? CustomerReviewImages
        {
            get
            {
                if (this.customerReviewImages != null)
                {
                    for (var i = 0; i < this.customerReviewImages.Count; i++)
                    {
                        this.customerReviewImages[i] = this.customerReviewImages[i].Contains(App.ImageServerPath) ? this.customerReviewImages[i] : App.ImageServerPath + this.customerReviewImages[i];
                    }
                }

                return this.customerReviewImages;
            }

            set
            {
                this.customerReviewImages = value;
            }
        }

        public int Rating { get; set; }
    }

}
