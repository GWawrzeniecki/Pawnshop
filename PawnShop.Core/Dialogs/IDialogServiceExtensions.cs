using Prism.Services.Dialogs;
using System;

namespace PawnShop.Core.Dialogs
{
    public static class IDialogServiceExtensions
    {
        public static void ShowLoginDialog(this IDialogService dialogService, Action<IDialogResult> callBack)
        {
            dialogService.ShowDialog(DialogNames.LoginDialog, callBack);
        }

        public static void ShowNotificationDialog(this IDialogService dialogService, string title, string message, Action<IDialogResult> callBack)
        {
            var dialogParameters = new DialogParameters();
            dialogParameters.Add("title", title);
            dialogParameters.Add("message", message);
            dialogService.ShowDialog(DialogNames.NotificationDialog, dialogParameters, callBack);
        }
    }
}