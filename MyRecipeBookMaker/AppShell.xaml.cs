﻿using MyRecipeBookMaker.Views;
using MyRecipeBookMaker.Common;
using MyRecipeBookMaker.Models;
namespace MyRecipeBookMaker
{
    public partial class AppShell : Shell
    {
       
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();

         //   GetSecretsHelper.GetSecrets();
        }
        private void RegisterRoutes()
        {
            //Routing.RegisterRoute(nameof(RecipeList), typeof(RecipeDetailPage));
            Routing.RegisterRoute(nameof(RecipeCollection), typeof(RecipeCollection));
            Routing.RegisterRoute(nameof(RecipeDetails), typeof(RecipeDetails));
            Routing.RegisterRoute(nameof(ChangeRecipePhoto), typeof(ChangeRecipePhoto));
            // Add more routes as needed
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);


            //pageTitle.Text = Current.CurrentPage.Title;
        }
   
    }
}
