using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using MyRecipeBookMaker.Common;
using AsyncAwaitBestPractices;
using Newtonsoft.Json;

namespace MyRecipeBookMaker.Models
{
    public partial class UserSessionData : ObservableObject
    {
        [ObservableProperty] public ObservableCollection<Recipe>? listOfRecipes = new();
        public UserSessionData()
        {
            //LoadData().SafeFireAndForget();
        }

        public async Task LoadData()
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
                WeakReferenceMessenger.Default.Send(new RecipeListUpdatedMessage(true));
            }
        }
    }
}
