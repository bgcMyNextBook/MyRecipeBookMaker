namespace MyRecipeBookMaker.Views.Settings
{
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            this.profileImage.Source = App.ImageServerPath + "ProfileImage1.png";
        }
    }
}