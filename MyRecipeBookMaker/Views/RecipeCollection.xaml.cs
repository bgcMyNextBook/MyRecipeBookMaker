using System.Collections;
using System.Diagnostics;

using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;

using DevExpress.Maui.CollectionView;
using DevExpress.Maui.Core;

using Microsoft.Maui.Controls.Internals;

using Syncfusion.Maui.RadialMenu;
using MyRecipeBookMaker.Models;
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
        WeakReferenceMessenger.Default.Register<RecipeAdded>(this, (recipient, message) =>
        {


            collectionView.ScrollTo(message.recipeAdded,DXScrollToPosition.MakeVisible);
        });
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

    private void CollectionView_SelectionChanged(object sender, DevExpress.Maui.CollectionView.CollectionViewSelectionChangedEventArgs e)
    {
        Debug.WriteLine("seledtion changed event.");
    }

    private void collectionView_ChildAdded(object sender, ElementEventArgs e)
    {
        // Get the added element
        var addedElement = e.Element;

        // Get the items source of the CollectionView
        var itemsSource = (sender as DXCollectionView)?.ItemsSource as IList;

        if (itemsSource != null)
        {
            // Find the index of the added element
            int index = itemsSource.IndexOf(addedElement.BindingContext);

            if (index >= 0)
            {
                // Item index found
                Console.WriteLine($"Item index: {index}");
            }
            else
            {
                // Item not found in the items source
                Console.WriteLine("Item not found");
            }
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