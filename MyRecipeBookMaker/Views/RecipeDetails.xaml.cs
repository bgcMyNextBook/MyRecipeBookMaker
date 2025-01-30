using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

using Syncfusion.Maui.DataGrid;
using Syncfusion.Maui.DataGrid.Helper;

namespace MyRecipeBookMaker.Views
{
    public partial class RecipeDetails : ContentPage
    {
        
        public RecipeDetails()
        {
            InitializeComponent();
     
        }
       
 




        private void ingredientsGrid_SizeChanged(object sender, DataGridQueryRowHeightEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                e.Height = e.GetIntrinsicRowHeight(e.RowIndex, false, new List<string> { "Description" });
                e.Handled = true;

                // Calculate the total height of all rows
                var dataGrid = sender as SfDataGrid;
                double totalHeight = 0;
                for (int i = 0; i < dataGrid.View.Records.Count; i++)
                {

                    totalHeight += dataGrid.GetRowHeight(i);
                }
                
                // Set the DataGrid height with additional padding based on the font size
                //double padding = dataGrid.FontSize * 1.5; // Adjust the multiplier as needed
                dataGrid.HeightRequest = totalHeight + dataGrid.HeaderRowHeight+20;
            }




        }
        private void instructionsGrid_SizeChanged(object sender, DataGridQueryRowHeightEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                e.Height = e.GetIntrinsicRowHeight(e.RowIndex, false, new List<string> { "Description" });
                e.Handled = true;

                // Calculate the total height of all rows
                var dataGrid = sender as SfDataGrid;
                double totalHeight = 0;
                for (int i = 0; i < dataGrid.View.Records.Count; i++)
                {

                    totalHeight += (dataGrid.GetRowHeight(i));
                    totalHeight += 7;
                }

                // Set the DataGrid height with additional padding based on the font size
                //double padding = dataGrid.FontSize * 1.5; // Adjust the multiplier as needed
                dataGrid.HeightRequest = totalHeight + dataGrid.HeaderRowHeight +60;
            }




        }

        private void ingredientsGrid_SizeChanged_1(object sender, DataGridQueryRowHeightEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                e.Height = e.GetIntrinsicRowHeight(e.RowIndex, false, new List<string> { "Description" });
                e.Handled = true;

                // Calculate the total height of all rows
                var dataGrid = sender as SfDataGrid;
                double totalHeight = 0;
                for (int i = 0; i < dataGrid.View.Records.Count; i++)
                {

                    totalHeight += dataGrid.GetRowHeight(i);
                }

                // Set the DataGrid height with additional padding based on the font size
                //double padding = dataGrid.FontSize * 1.5; // Adjust the multiplier as needed
                dataGrid.HeightRequest = totalHeight + dataGrid.HeaderRowHeight + 20;
            }
        }
    }
}