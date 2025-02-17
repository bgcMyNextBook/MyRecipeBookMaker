using Syncfusion.Maui.RadialMenu;
using DevExpress.Maui.Core;
using CommunityToolkit.Maui.Core;

namespace MyRecipeBookMaker.Views;

public partial class RecipeCollection : ContentPage
{
    RecipeCollectionViewModel vm;// l => BindingContext as RecipeCollectionViewModel;
    public RecipeCollection(RecipeCollectionViewModel _vm)
    {
        InitializeComponent();
        BindingContext = _vm;
        vm = _vm;
       
        //BindingContext = new RecipeCollectionViewModel();
        ON.OrientationChanged(this, OnOrientationChanged);
        OnOrientationChanged(this);
    }
    void OnOrientationChanged(ContentPage view)
    {
        UpdateColumnsCount();
    }
    void UpdateColumnsCount()
    {
        vm.ColumnsCount = ON.Idiom<int>(ON.Orientation<int>(1, 2), ON.Orientation<int>(2, Height < 600 ? 2 : 4));
    }

    
    private void addMenu_Opening(object sender, Syncfusion.Maui.RadialMenu.OpeningEventArgs e)
    {
        var radialMenu = sender as Syncfusion.Maui.RadialMenu.SfRadialMenu;
        if (radialMenu != null)
        {
            radialMenu.Point = new Point(AppShell.Current.Window.Width - 135, 60);

            // Add your logic here
        }
    }
    
    private void addMenu_Closing(object sender, Syncfusion.Maui.RadialMenu.ClosingEventArgs e)
    {
        var radialMenu = sender as Syncfusion.Maui.RadialMenu.SfRadialMenu;
        if (radialMenu != null)
        {
            radialMenu.Point = new Point(AppShell.Current.Window.Width - 80, -5);
        }
    }

    


    private void Grid_SizeChanged(object sender, EventArgs e)
    {
        if (AppShell.Current?.Window != null)
        {
            addMenu.Point = new Point(AppShell.Current.Window.Width - 80, -5);

        }
    }
    /*
    private void actionMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
       
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var itemContainer = (Border) sender; //(Border)((TapGestureRecognizer)sender).Parent;

        // Set the PlacementTarget to the item container
        actionMenu.PlacementTarget = itemContainer;
    }
    */
}