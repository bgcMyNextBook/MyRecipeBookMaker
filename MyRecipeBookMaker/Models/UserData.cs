using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyRecipeBookMaker.AI;
using MyRecipeBookMaker.Common;

using Newtonsoft.Json;

namespace MyRecipeBookMaker.Models
{
    public class UserData
    {
        public ObservableCollection<Recipe> ListOfRecipes = new ObservableCollection<Recipe>();
        public UserData()
        {
        
        }
        public static async Task<UserData> CreateAsync()
        {
            var result = new UserData();
            await result.InitializeAsync();
            return result;
        }
        public async Task InitializeAsync()
        {
            await LoadData();
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
    }
}
