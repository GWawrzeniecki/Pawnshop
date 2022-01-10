using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PawnShop.Modules.Sale.Dialogs.ViewModels
{
    public class PreviewSaleDialogViewModel : BindableBase, IDialogAware
    {
        #region PrivateMembers

        private string _title;
        private Business.Models.Sale _sale;

        #endregion

        #region Constructor

        public PreviewSaleDialogViewModel()
        {

        }

        #endregion

        #region PublicProperties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public Business.Models.Sale Sale
        {
            get => _sale;
            set => SetProperty(ref _sale, value);
        }

        #endregion

        #region IDialogAware

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            Sale = parameters.GetValue<Business.Models.Sale>("sale");
        }

#pragma warning disable CS0067 // The event 'PreviewSaleDialogViewModel.RequestClose' is never used
        public event Action<IDialogResult> RequestClose;
#pragma warning restore CS0067 // The event 'PreviewSaleDialogViewModel.RequestClose' is never used

        #endregion IDialogAware
    }
}
