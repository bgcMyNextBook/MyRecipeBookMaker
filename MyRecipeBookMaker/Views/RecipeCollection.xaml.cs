using Syncfusion.Maui.RadialMenu;

namespace MyRecipeBookMaker.Views;

public partial class RecipeCollection : ContentPage
{
	public RecipeCollection()
	{
		InitializeComponent();
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
            radialMenu.Point = new Point(AppShell.Current.Window.Width - 80, -20);
        }
    }

    private void addMenu_SizeChanged(object sender, EventArgs e)
    {

    }

    private void Grid_SizeChanged(object sender, EventArgs e)
    {
        addMenu.Point = new Point(AppShell.Current.Window.Width - 80, -20);
    }
}