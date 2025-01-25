using MyRecipeBookMaker.Common;
using MyRecipeBookMaker.Views.Forms;

using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Toolkit.Buttons;

namespace MyRecipeBookMaker
{
    public class ForgotPasswordBehavior : Behavior<ForgotPassword>
    {
        /// <summary>
        /// The data form.
        /// </summary>
        private SfDataForm? dataForm;

        /// <summary>
        /// The send button.
        /// </summary>
        private SfButton? sendButton;

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
            ForgotPassword? forgotPasswordPage = bindable as ForgotPassword;
            if (forgotPasswordPage == null)
            {
                return;
            }

            this.dataForm = (SfDataForm)forgotPasswordPage.Content.FindByName("dataForm");
            if (this.dataForm != null)
            {
                dataForm.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.sendButton = (SfButton)forgotPasswordPage.Content.FindByName("sendButton");
            if (this.sendButton != null)
            {
                this.sendButton.Clicked += OnSendButtonClicked;
            }
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);
            ForgotPassword? forgotPasswordPage = bindable as ForgotPassword;

            if (forgotPasswordPage == null)
            {
                return;
            }

            this.dataForm = (SfDataForm)forgotPasswordPage.Content.FindByName("dataForm");
            if (this.dataForm != null)
            {
                dataForm.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.sendButton = (SfButton)forgotPasswordPage.Content.FindByName("sendButton");
            if (this.sendButton != null)
            {
                this.sendButton.Clicked -= OnSendButtonClicked;
            }
        }

        private void OnSendButtonClicked(object? sender, EventArgs e)
        {
            this.dataForm?.Validate();
        }
    }
}
