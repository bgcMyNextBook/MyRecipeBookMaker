using System.Text.Json;
using MyRecipeBookMaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using MyRecipeBookMaker.Common;
using System.Diagnostics;

using System.ComponentModel;
using System.Reflection;
using System.Collections;
using CommunityToolkit.Mvvm.Messaging;



namespace MyRecipeBookMaker
{
    public partial class RecipeDetailsViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty] bool? showChangeImageMenu;

        [ObservableProperty] public ObservableCollection<DropDownListObject>? cuisines;
        [ObservableProperty] public ObservableCollection<DropDownListObject>? courses;
        [ObservableProperty] public ObservableCollection<DropDownListObject>? units;
        [ObservableProperty] private Recipe? selectedRecipe;

        public RecipeDetailsViewModel()
        {
            LoadListsAsync();
            ShowChangeImageMenu = false;
           
        }
     
        private async void LoadListsAsync()
        {
            Cuisines = await LoadList("cuisine.json");
            Courses = await LoadList("course.json");
            Units = await LoadList("units.json");
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("recipe"))
            {
                SelectedRecipe = query["recipe"] as Recipe;
              
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
    }
}



