using MyRecipeBookMaker.Common;
using MyRecipeBookMaker.Views.Forms;

using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Toolkit.Buttons;

namespace MyRecipeBookMaker
{
    public class SignupBehavior : Behavior<Signup>
    {
        /// <summary>
        /// The data form.
        /// </summary>
        private SfDataForm? dataForm;

        /// <summary>
        /// The register button.
        /// </summary>
        private SfButton? registerButton;

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
            Signup? signupPage = bindable as Signup;
            if (signupPage == null)
            {
                return;
            }

            this.dataForm = (SfDataForm)signupPage.Content.FindByName("dataForm");
            if (this.dataForm != null)
            {
                dataForm.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.registerButton = (SfButton)signupPage.Content.FindByName("registerButton");
            if (this.registerButton != null)
            {
                this.registerButton.Clicked += OnRegisterButtonClicked;
            }
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);
            Signup? signupPage = bindable as Signup;
            if (signupPage == null)
            {
                return;
            }

            this.dataForm = (SfDataForm)signupPage.Content.FindByName("dataForm");
            if (this.dataForm != null)
            {
                dataForm.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.registerButton = (SfButton)signupPage.Content.FindByName("registerButton");
            if (this.registerButton != null)
            {
                this.registerButton.Clicked -= OnRegisterButtonClicked;
            }
        }

        private void OnRegisterButtonClicked(object? sender, EventArgs e)
        {
            this.dataForm?.Validate();
        }
    }
}
