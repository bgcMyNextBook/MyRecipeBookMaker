using System.Collections.ObjectModel;

using Newtonsoft.Json;
using MyRecipeBookMaker.Models;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using MyRecipeBookMaker.Common;
using MyRecipeBookMaker.AI;
using CommunityToolkit.Mvvm.Messaging;
namespace MyRecipeBookMaker
{

    public partial class RecipeCollectionViewModel : ObservableObject, IRecipient<ReadRecipeMessage>, IRecipient<RecipeListUpdatedMessage>

    {
        [ObservableProperty]  public ObservableCollection<Recipe>? listOfRecipes;
        [ObservableProperty] bool showAddMenu = false;
        [ObservableProperty] int columnsCount = 1;
        [ObservableProperty] public Point addMenuPoint = new(0, 0);

        [ObservableProperty] public bool showItemMenu = false;
        public RecipeCollectionViewModel()
        {
            // Register the ViewModel to receive messages
            WeakReferenceMessenger.Default.Register<ReadRecipeMessage>(this);
            WeakReferenceMessenger.Default.Register<RecipeListUpdatedMessage>(this);
            //todo: research if this is better done with DI 
            ListOfRecipes = new();


            AddMenuPoint = new Point(AppShell.Current.Window.Width - 80, -5);
            OnPropertyChanged(nameof(AddMenuPoint));
            AppShell.Current.Window.SizeChanged += MainWindow_SizeChanged;
        }
        public void Receive(RecipeListUpdatedMessage updated)
        {
            if (updated.Updated)
            {
                ListOfRecipes = App.currentSessionData.ListOfRecipes;
                OnPropertyChanged(nameof(ListOfRecipes));
            }
        }
        public void Receive(ReadRecipeMessage message)
        {
            // Handle the message here
            if (message.r.Status == true)
            {
                SetErrorMessageOnRecipeCard(message);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var existingRecipe = ListOfRecipes.FirstOrDefault(r => r.Uid == message.r.Uid);
                    if (existingRecipe != null)
                    {
                        var index = ListOfRecipes.IndexOf(existingRecipe);
                        ListOfRecipes[index] = message.r;
                        OnPropertyChanged(nameof(ListOfRecipes));
                    }
                    else
                    {
                        ListOfRecipes.Add(message.r);
                        OnPropertyChanged(nameof(ListOfRecipes));

                    }
                });
            }
        }
        void SetErrorMessageOnRecipeCard(ReadRecipeMessage message)
        {
            Recipe r = ListOfRecipes.Where(e => e.uid == message.uid).FirstOrDefault();
            if (r != null)
            {
                r.Name = "Error Reading Recipe.";
                r.Description = message.processingMessage;
            }
            else
            {
                r = message.r;
            }

        }
        private void MainWindow_SizeChanged(object sender, EventArgs e)

        {


            AddMenuPoint = new Point(AppShell.Current.Window.Width - 80, -5);
            OnPropertyChanged(nameof(AddMenuPoint));
        }


        #region Commands
        [RelayCommand]
        public async Task DeleteRecipe()
        {
            ShowAddMenu = !ShowAddMenu;
        }
        [RelayCommand]
        public async Task CreateRecipeWordDocument(Recipe recipe)
        {
            Debug.WriteLine($"CreateRecipeWordDocument: {recipe.Name}");
            // Navigate to RecipeDetails page with the selected recipe as a query parameter
            var navigationParameter = new Dictionary<string, object>
            {
                { "recipe", recipe }
            };
            await Shell.Current.GoToAsync("RecipeDetails", navigationParameter);
        }
        [RelayCommand]
        public async Task ShowRecipeDetail(Recipe theRecipe)
        {
            Debug.WriteLine($"ShowShowRecipeDetailCommand: {theRecipe.Name}");
            theRecipe.ShowItemMenu = false;
            // Navigate to RecipeDetails page with the selected recipe as a query parameter
            var navigationParameter = new Dictionary<string, object>
            {
                { "recipe", theRecipe }
            };
            await Shell.Current.GoToAsync("RecipeDetails", navigationParameter);
        }

        [RelayCommand]
        public async Task AddNewRecipe()
        {

            ShowAddMenu = true;
        }
        [RelayCommand]
        public async Task AddRecipeByPhotoByAlbum()
        {
            AddMenuPoint = new Point(AppShell.Current.Window.Width - 80, -20);
            ShowAddMenu = false;
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult? photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null)
                {

                    using Stream sourceStream = await photo.OpenReadAsync();
                    sourceStream.Position = 0;
                    using MemoryStream memoryStream = new();
                    await sourceStream.CopyToAsync(memoryStream);
                    byte[] photoBytes = memoryStream.ToArray();

                    ReadRecipeFromAI(photoBytes);
                }
            }
            ShowAddMenu = false;
        }
        async Task DoWorkReadRecipeFromAI(Recipe r, byte[] imageData)
        {
            ReadRecipeMessage r1 = new();
            try
            {
                AIReadRecipe ai = await AIReadRecipe.CreateAsync();
                Debug.WriteLine($"r: {r.Uid}");

                r1.uid = r.Uid;
                r1.r = ai.ReadRecipeFromImage(imageData).Result;
                if (r1.r != null)
                {
                    r1.r.Uid = r.Uid;
                    r1.r.Status = true;
                    r1.r.processingMessage = "";
                }
                else
                {
                    r1.r = new Recipe();
                    r1.r.Uid = r.Uid;
                    r1.r.processingMessage = "Recipe read failed.";
                }
                r1.r.Status = false;
                r1.processingMessage = "Recipe read failed";
                WeakReferenceMessenger.Default.Send(r1);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (r1.r == null)
                {
                    r1.r = new Recipe();
                    r1.r.Uid = r.Uid;
                }
                r1.r.processingMessage = $"oops something went wrong...{ex.Message}";
                WeakReferenceMessenger.Default.Send(r1);
            }
        }
        void ReadRecipeFromAI(Byte[] imageData)
        {

            Recipe? r = new Recipe();
            r.Uid = Guid.NewGuid();
            r.ImageBASE64 = Convert.ToBase64String(imageData);
            r.processingMessage = "Using AI to read the recipe this could take a few minutes.";
            r.Name = "Reading recipe";
            ListOfRecipes.Add(r);
            Task.Run(async () =>
                    await DoWorkReadRecipeFromAI(r, imageData));
        }
        [RelayCommand]
        public async Task AddRecipeByCamera()
        {
            AddMenuPoint = new Point(AppShell.Current.Window.Width - 80, -20);
            ShowAddMenu = false;
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {

                        using Stream sourceStream = await photo.OpenReadAsync();

                   
                        sourceStream.Position = 0;
                        using MemoryStream memoryStream = new();
                        await sourceStream.CopyToAsync(memoryStream);
                        byte[] photoBytes = memoryStream.ToArray();
                        ReadRecipeFromAI(photoBytes);
                    }
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                Debug.WriteLine("// Handle not supported on device exception");
            }
            catch (PermissionException)
            {
                Debug.WriteLine("// Handle permission exception");
            }
            catch (Exception)
            {
                Debug.WriteLine("// Unable to take photo");
            }

        }
        [RelayCommand]
        public async Task AddRecipeoByPdf()
        {
            AddMenuPoint = new Point(AppShell.Current.Window.Width - 80, -20);
            ShowAddMenu = false;

            var options = new PickOptions
            {
                PickerTitle = "Please select a PDF file",
                FileTypes = FilePickerFileType.Pdf
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                using Stream sourceStream = await result.OpenReadAsync();
                using MemoryStream memoryStream = new();
                await sourceStream.CopyToAsync(memoryStream);
                byte[] pdfBytes = memoryStream.ToArray();
                ReadRecipeFromAI(pdfBytes);

            }

        }
        [RelayCommand]
        public async Task ItemPopup (Recipe recipe)
        {
              ShowItemMenu = true;
        }
        [RelayCommand]
        public async Task ShowItemPopupMenu(Recipe recipe)
        {
           // var itemContainer = (Border) ((TapGestureRecognizer) sender).Parent;
            //actionMenu.PlacementTarget = itemContainer;
            ShowItemMenu = true;
            /*
            Debug.WriteLine($"Item tapped: {recipe.Name}");
            // Navigate to RecipeDetails page with the selected recipe as a query parameter
            var navigationParameter = new Dictionary<string, object>
            {
                { "recipe", recipe }
            };
            await Shell.Current.GoToAsync("RecipeDetails", navigationParameter);
            */
        }
        #endregion
    }

}







