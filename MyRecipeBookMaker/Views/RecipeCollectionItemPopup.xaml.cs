using CommunityToolkit.Maui.Views;
using MyRecipeBookMaker.ViewModels;
namespace MyRecipeBookMaker.Views;

public partial class RecipeCollectionItemPopup : Popup
{
    public RecipeCollectionItemPopup(RecipeCollectionItemPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
   
   
}
