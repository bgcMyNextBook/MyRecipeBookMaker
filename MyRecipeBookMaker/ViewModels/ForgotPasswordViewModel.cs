using System.ComponentModel.DataAnnotations;

using Syncfusion.Maui.DataForm;

namespace MyRecipeBookMaker
{
    internal class ForgotPasswordViewModel
    {
        public ForgotPasswordInfo ContactsInfo { get; set; }

        #region Constructor
        public ForgotPasswordViewModel()
        {
            this.ContactsInfo = new ForgotPasswordInfo();
        }

        #endregion
    }

    public class ForgotPasswordInfo
    {
        public ForgotPasswordInfo()
        {
            this.Email = string.Empty;
        }

        [Display(Name = "Enter mail id")]
        [DataFormDisplayOptions(ColumnSpan = 3)]
        [EmailAddress(ErrorMessage = "Enter your email")]
        public string Email { get; set; }
    }
}
