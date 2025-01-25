using MyRecipeBookMaker.Common;
using MyRecipeBookMaker.Views.Forms;

using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Toolkit.Buttons;

namespace MyRecipeBookMaker
{
    public class ResetPasswordBehavior : Behavior<ResetPassword>
    {
        /// <summary>
        /// The data form.
        /// </summary>
        private SfDataForm? dataForm;

        /// <summary>
        /// The submit button.
        /// </summary>
        private SfButton? submitButton;

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
            ResetPassword? resetPasswordPage = bindable as ResetPassword;
            if (resetPasswordPage == null)
            {
                return;
            }

            this.dataForm = (SfDataForm)resetPasswordPage.Content.FindByName("dataForm");
            if (this.dataForm != null)
            {
                dataForm.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.submitButton = (SfButton)resetPasswordPage.Content.FindByName("submitButton");
            if (this.submitButton != null)
            {
                this.submitButton.Clicked += OnSubmitButtonClicked;
            }
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);
            ResetPassword? resetPasswordPage = bindable as ResetPassword;

            if (resetPasswordPage == null)
            {
                return;
            }

            this.dataForm = (SfDataForm)resetPasswordPage.Content.FindByName("dataForm");
            if (this.dataForm != null)
            {
                dataForm.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.submitButton = (SfButton)resetPasswordPage.Content.FindByName("submitButton");
            if (this.submitButton != null)
            {
                this.submitButton.Clicked -= OnSubmitButtonClicked;
            }
        }

        private void OnSubmitButtonClicked(object? sender, EventArgs e)
        {
            this.dataForm?.Validate();
        }
    }
}
