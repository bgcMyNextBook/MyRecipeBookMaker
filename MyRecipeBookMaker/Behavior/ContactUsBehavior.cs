using MyRecipeBookMaker.Common;
using MyRecipeBookMaker.Views.ContactUs;

using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Toolkit.Buttons;

namespace MyRecipeBookMaker
{
    public class ContactUsBehavior : Behavior<ContactUs>
    {
#if ANDROID || IOS
        /// <summary>
        /// The data form
        /// </summary>
        private SfDataForm? dataForm;

        /// <summary>
        /// The submit button
        /// </summary>
        private SfButton? submitButton;
#else
        /// <summary>
        /// The data form
        /// </summary>
        private SfDataForm? dataFormDesktop;

        /// <summary>
        /// The submit button
        /// </summary>
        private SfButton? submitButtonDesktop;
#endif

        protected override void OnAttachedTo(ContactUs bindable)
        {
            base.OnAttachedTo(bindable);
            ContactUs? contactUsPage = bindable as ContactUs;
            if (contactUsPage == null)
            {
                return;
            }

#if ANDROID || IOS
            this.dataForm = (SfDataForm)contactUsPage.Content.FindByName("dataForm");
            if (this.dataForm != null)
            {
                dataForm.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.submitButton = (SfButton)contactUsPage.Content.FindByName("submitButton");
            if (this.submitButton != null)
            {
                this.submitButton.Clicked += OnAddButtonClicked;
            }
#else
            this.dataFormDesktop = (SfDataForm)contactUsPage.Content.FindByName("dataFormDesktop");
            if (this.dataFormDesktop != null)
            {
                dataFormDesktop.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.submitButtonDesktop = (SfButton)contactUsPage.Content.FindByName("submitButtonDesktop");
            if (this.submitButtonDesktop != null)
            {
                this.submitButtonDesktop.Clicked += OnAddButtonClicked;
            }
#endif
        }

        protected override void OnDetachingFrom(ContactUs bindable)
        {
            base.OnDetachingFrom(bindable);
            ContactUs? contactUsPage = bindable as ContactUs;
            if (contactUsPage == null)
            {
                return;
            }

#if ANDROID || IOS
            this.dataForm = (SfDataForm)contactUsPage.Content.FindByName("dataForm");
            if (this.dataForm != null)
            {
                dataForm.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.submitButton = (SfButton)contactUsPage.Content.FindByName("submitButton");
            if (this.submitButton != null)
            {
                this.submitButton.Clicked -= OnAddButtonClicked;
            }
#else
            this.dataFormDesktop = (SfDataForm)contactUsPage.Content.FindByName("dataFormDesktop");
            if (this.dataFormDesktop != null)
            {
                dataFormDesktop.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.submitButtonDesktop = (SfButton)contactUsPage.Content.FindByName("submitButtonDesktop");
            if (this.submitButtonDesktop != null)
            {
                this.submitButtonDesktop.Clicked -= OnAddButtonClicked;
            }
#endif
        }

        private void OnAddButtonClicked(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            this.dataForm?.Validate();
#else
            this.dataFormDesktop?.Validate();
#endif
        }
    }
}
