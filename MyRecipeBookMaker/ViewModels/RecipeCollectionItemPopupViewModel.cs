using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyRecipeBookMaker.ViewModels
{
    public partial  class RecipeCollectionItemPopupViewModel : ObservableObject
    {
        readonly IPopupService popupService;
        [ObservableProperty] public string name = "";
        public object callingControl { get; set; }

        public RecipeCollectionItemPopupViewModel(IPopupService popupService)
        {
            this.popupService = popupService;
        }
    }
}
