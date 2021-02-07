using Prism.Services.Dialogs;
using System;

namespace PawnShop.Core.Dialogs
{
    public static class IDialogServiceExtensions
    {
        public static void ShowLogInDialog(this IDialogService dialogService, Action<IDialogResult> callBack)
        {
            dialogService.ShowDialog("LoginDialog", callBack);
        }
    }
}