using System.Text.Json;
using MyRecipeBookMaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using MyRecipeBookMaker.Common;
using System.Diagnostics;

using System.ComponentModel;
using System.Reflection;


namespace MyRecipeBookMaker
{
    public partial class RecipeDetailsViewModel : ObservableObject, IQueryAttributable
    {




        [ObservableProperty] bool showChangeImageMenu = false;


        [ObservableProperty] public ObservableCollection<DropDownListObject>? cuisines;
        [ObservableProperty] public ObservableCollection<DropDownListObject>? courses;
        [ObservableProperty] public ObservableCollection<DropDownListObject>? units;
        //[ObservableProperty]  public ObservableCollection<InstructionGroup> instructionGroups { get; set; } = new();
        //public ObservableCollection<IngredientGroup> ingredientGroups { get; set; } = new();

        public RecipeDetailsViewModel()
        {
            LoadListsAsync();

            // PropertyChanged += RecipeDetailsViewModel_PropertyChanged;
            // Subscribe to all property changes with Debug.WriteLine
            this.SubscribeRecursive((propertyName, sender, args) =>
            {
                // Handle property changes here
                Debug.WriteLine($"Property changed: {propertyName}, Value: {args.PropertyName}");
            });
        }
        [ObservableProperty] public Recipe? selectedRecipe;




        private static string GetFullPath(object obj)
        {
            var fullPath = new List<string>();
            var current = obj;

            while (current != null)
            {
                if (current.GetType().GetProperty("name")?.GetValue(current) is string name)
                    fullPath.Add(name);
                current = current.GetType().GetProperty("parent")?.GetValue(current);
            }

            return string.Join(".", fullPath.Reverse<string>());
        }


   

    




        private async void LoadListsAsync()
        {
            Cuisines = await LoadList("cuisine.json");
            Courses = await LoadList("course.json");
            Units = await LoadList("units.json");
            //PropertyChanged += ViewModelPropertyChanged;
            //OnPropertyChanged(nameof(Cuisines));
            //OnPropertyChanged(nameof(Courses));
            //OnPropertyChanged(nameof(Units));
        }


        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("recipe"))
            {
                SelectedRecipe = query["recipe"] as Recipe;
                //instructionGroups = SelectedRecipe.InstructionGroups;
                //ingredientGroups = SelectedRecipe.IngredientGroups;

                try
                {
                    // Delay the subscription to PropertyChanged event until SelectedRecipe is fully initialized
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        SelectedRecipe?.SubscribeRecursive((propertyName, sender, args) =>
                        {
                            //Debug.WriteLine($"Property changed: {propertyName}");

                            // Optional: Show full path including parent object
                            var fullPath = GetFullPath(sender);
                            Debug.WriteLine($"Full path: {fullPath}.{propertyName}");
                        });
                    });
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during execution
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
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
        


        #region Commands
        [RelayCommand]
        public Task ChangePhoto()
        {
            ShowChangeImageMenu = !ShowChangeImageMenu;
            return Task.CompletedTask;

        }
        [RelayCommand]
        public async Task ChangePhotoByAlbum(object recipe)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult? photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();

                    // Convert to base64
                    sourceStream.Position = 0;
                    using MemoryStream memoryStream = new();
                    await sourceStream.CopyToAsync(memoryStream);
                    byte[] photoBytes = memoryStream.ToArray();
                    SelectedRecipe.ImageBASE64 = Convert.ToBase64String(photoBytes);

                    OnPropertyChanged(nameof(SelectedRecipe));
                }
            }
        }

        [RelayCommand]
        public async Task ChangePhotoByCamera()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {

                FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();

                    // Convert to base64
                    sourceStream.Position = 0;
                    using MemoryStream memoryStream = new();
                    await sourceStream.CopyToAsync(memoryStream);
                    byte[] photoBytes = memoryStream.ToArray();
                    SelectedRecipe.ImageBASE64 = Convert.ToBase64String(photoBytes);

                    OnPropertyChanged(nameof(SelectedRecipe));
                }
            }
        }
        #endregion

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
